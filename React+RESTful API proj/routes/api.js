const User = require('../models/user');
const Photo = require('../models/photos');
const Album = require('../models/albums');
const express = require('express');
const router = express.Router();
const authRouter = require('../routes/auth')
const config = require('../config');
const auth=require('./auth')
const passport = require('../passport');
const jwt = require('jsonwebtoken');
const cloudinary = require('cloudinary').v2;
const bodyParser = require('body-parser');
const busboyBodyParser = require('busboy-body-parser');
const Telegram = require('../modules/telegram-bot');

router.use(bodyParser.urlencoded({ extended: true }));
router.use(bodyParser.json());
router.use(busboyBodyParser({ limit: '5mb' }));

cloudinary.config({
    cloud_name: config.cloudName, 
    api_key: config.apiKey, 
    api_secret: config.apiSecret 
})

router.post('/v1/auth/login', (req, res) => {
    passport.authenticate('local', {session: false}, async (err, user, info) => {
        console.log(err);
        if (err || !user) {
            return res.status(400).json({
                message: info ? info.message : 'Login failed',
                user   : user
            });
        }
        // console.log("user");
        // console.log(user);
        req.login(user, {session: false}, async(err) => {
            if (err) {
                res.send(err);
            }
            const token = jwt.sign(await User.getUserToReturn(user.id), 'itsmysecret!');
            console.log("scha zareturnu")
            console.log(token)
            return res.json({user, token});
        });
    })
    (req, res);
});
router.post('/v1/users',(req, res) => {
        new Promise ((resolve, reject) => {
            if (req.files.avaUrl==undefined){
                let x={
                    url:"https://res.cloudinary.com/dksfym6ok/image/upload/v1574199302/no-user_og8v1g.png"
                }
                resolve(x)
            }
            else{
                let file=req.files.avaUrl;
                let buf=file.data;
                cloudinary.uploader.upload_stream({resource_type:"image",public_id:req.body.login}, (error, result) =>{
                    if (error)
                        reject (error)
                    else
                        resolve(result)
                }).end(buf)
            }
        })
            .then((insertedFile)=>{
                let dat=new Date().toISOString();
                let passHesh=authRouter.sha512(req.body.password).passwordHash;
                let us=new User("1",req.body.login,passHesh,false,req.body.fullname,dat,insertedFile.url,"0")
                return us
            })
            .then((us)=>{
                return(User.insert(us))
            })
            .then((us)=>{
                res.json(us,null,2)
            })
            .catch(err => {
                console.log(err)
                if(err.code==11000)
                    res.status(400).json({ error: "there is already user with such name" })
                else   
                res.status(400).json({ error:err})
            });
})

router.use(passport.authenticate('jwt', { session: false }))

router.get('/v1/me',(req, res) => {

        if (req.user != undefined) {
            res.json(req.user, null, 2)
        } else
            res.status(404).json({error:"no such user"})

});

router.get('/v1/users/:id' ,(req, res) => {
    User.getUserToReturn(req.params.id)
        .then(_us => {
            if (_us != undefined) {
                res.json(_us, null, 2)
            } else
                res.json([])
        })
        .catch(err => res.status(404).json({error:err}));
});

router.get('/v1/users/',auth.checkAdmin, (req, res) => {

    User.getAll()
        .then(_us => {
            if (_us != undefined) {
                console.log(req.query);
                for (let par in req.query) {
                    if (par=="page" || par=="searching" )
                        continue;
                    let arr = [];
                    console.log(par)
                    for (let i = 0; i < _us.length; i++) {
                        console.log(_us[i][par])
                        if (_us[i][par] == req.query[par])
                            arr.push(_us[i])
                    }
                    _us = arr
                }
                let tag=req.query.searching;
                
                let photos1=[];
                if (tag!=undefined && tag!=''){
                    _us.forEach(photo => {
                        if(photo.login.indexOf(tag)!==-1)
                            photos1.push(photo)
                    });
                    _us=photos1;
                }
                let totalNumbers=_us.length;
                let page=1;
                let itemsOnOnePage=3
                if(req.query.page!=undefined){
                    if(!(req.query.page>0 && req.query.page*itemsOnOnePage-itemsOnOnePage<_us.length)){
                        res.json({error:"no such page"},null,2)
                        return
                    }

                    page=req.query.page
                }
                //console.log(page)
                let pageN=Math.ceil(totalNumbers/itemsOnOnePage)
                if (pageN==0)
                    pageN=1
                let dispalyData=[]
                for (let i =itemsOnOnePage*page-itemsOnOnePage;i<itemsOnOnePage*page && i<_us.length ;i++){
                    dispalyData.push(_us[i])
                }
                console.log("hmm")
                _us=[]
                dispalyData.forEach(user => {
                    _us.push(User.normalize(user))
                });
 
                res.json({
                    displayData:_us,
                    pageNumber:pageN
                }, null, 2)
            } else
                res.status(404).json({error:"not found"})
        })
        .catch(err => {
            console.log(err)
            res.status(500).json({error:err.toString()})
        });
});

router.delete('/v1/users/:id',auth.checkAdmin, (req, res) => {
    User.deleteById(req.params.id)
        .then((result) => {
            console.log(result)
            res.json(result);
        })
        .catch(err => res.status(404).json({ error: err }));
})

router.put('/v1/users/:id', (req, res) => {
    console.log(req.files)
    let promise=new Promise ((resolve, reject) => {
        console.log(req.files.avaUrl)
        if (req.files.avaUrl==undefined){
            resolve({
                url:undefined
            })
        }
        else{
            let file=req.files.avaUrl;
            let buf=file.data;
            cloudinary.uploader.upload_stream({resource_type:"image"}, (error, result) =>{
                if (error)
                    reject (error)
                else
                    resolve(result)
            }).end(buf)
        }
    })
    Promise.all([
        promise,
        User.getById(req.params.id),
    ])
        .then(([insertedFile,us])=>{
            console.log(us)
            if(insertedFile.url)
                us.avaUrl=insertedFile.url
            console.log(req.body)
             if(req.query.login)
                us.login=req.query.login
            if(req.query.password)
                us.password=authRouter.sha512(req.query.password).passwordHash
            if(req.query.fullname)
                us.fullname=req.query.fullname
            if(req.query.role)
                us.role=req.query.role
            if(req.body.login)
                us.login=req.body.login
            if(req.body.password)
                us.password=authRouter.sha512(req.body.password).passwordHash
            if(req.body.fullname)
                us.fullname=req.body.fullname
            if(req.body.role)
                us.role=req.body.role
            return(us)
        })
        .then((us)=>{
            return User.update(us)
        })
        .then((us)=>{
            res.json(User.normalize(us));
        })
        .catch(err => {
            console.log(err)
            if(err.code==11000)
                res.status(400).json({ error: "there is already user with such name" })
            else   
            res.status(400).json({ error:err})
        
        });
})




router.get('/v1/photos/:id' ,(req, res) => {
    Photo.getById(req.params.id)
        .then(_us => {
            if (_us != undefined) {
                res.json(_us, null, 2)
            } else
                res.status(404).json({})
        })
        .catch(err =>{ 
            if (err.name=="CastError")
                res.status(404).json({ error: "no such id" })
            else
                res.status(500).json({ error: err })
        });
});
router.get('/v1/photonames',(req, res) => {
    Photo.getAll()
        .then(photos=>{
            let names=[];
            photos.forEach(photo => {
                let photoq={};
                photoq.name=photo.name
                photoq.id=photo._id
                names.push(photoq)
            }); 
            console.log("names")
            console.log(names)
            res.json(names,null, 2)
        })
    .catch(err=>{res.status(400).json({err:err})})
})
router.get('/v1/photos/',(req, res) => {

    Photo.getAll()
        .then(_us => {
            if (_us != undefined) {
                console.log(req.query);
                for (let par in req.query) {
                    if (par=="page" || par=="searching" )
                        continue;
                    let arr = [];
                    console.log(par)
                    for (let i = 0; i < _us.length; i++) {
                        console.log(_us[i][par])
                        if (_us[i][par] == req.query[par])
                            arr.push(_us[i])
                    }
                    _us = arr
                }
                let tag=req.query.searching;
                
                let photos1=[];
                if (tag!=undefined && tag!=''){
                    _us.forEach(photo => {
                        if(photo.name.indexOf(tag)!==-1)
                            photos1.push(photo)
                    });
                    _us=photos1;
                    console.log(_us)
                }
                let totalNumbers=_us.length;
                let page=1;
                let itemsOnOnePage=4
                if(req.query.page!=undefined){
                    if(!(req.query.page>0 && req.query.page*itemsOnOnePage-itemsOnOnePage<_us.length)){
                        res.json({error:"no such page"},null,2)
                        return
                    }

                    page=req.query.page
                }
                console.log(page)
                let pageN=Math.ceil(totalNumbers/itemsOnOnePage)
                if (pageN==0)
                    pageN=1
                let dispalyData=[]
                for (let i =itemsOnOnePage*page-itemsOnOnePage;i<itemsOnOnePage*page && i<_us.length ;i++){
                    dispalyData.push(_us[i])
                }
                res.json({
                    displayData:dispalyData,
                    pageNumber:pageN
                }, null, 2)
            } else
                res.sendStatus(404)
        })
        .catch(err => {
            console.log(err)
            res.status(500).send(err.toString())
        })
            
});

router.delete('/v1/photos/:id',async (req, res) => {
    let albums=await Album.getAll()
    albums.forEach(async album => {
        console.log(album.photos)
        album.photos=album.photos.filter(photo => photo._id != req.params.id)
        console.log(album.photos)
        await Album.update(album)
    });
    
    Photo.deleteById(req.params.id)
        .then((result) => {
            console.log(result)
            res.json(result);
        })
        .catch(err => {
            console.log(err)
            res.status(404).json({ error: err })
        });
})
router.post('/v1/photos',(req, res) => {
    new Promise ((resolve, reject) => {
        console.log(req.files.url)
        if (req.files.url==undefined){
            resolve({
                url:undefined
            })
        }
        else{
            let file=req.files.url;
            let buf=file.data;
            cloudinary.uploader.upload_stream({resource_type:"image"}, (error, result) =>{
                if (error)
                    reject (error)
                else
                    resolve(result)
            }).end(buf)
        }
    })
    .then(file=>{
        let date=new Date()
        date= date.toISOString()
        console.log(req.user.id)
        let x=new Photo(
            "1",
            date,
            req.body.author,
            req.user.id,
            req.body.name,
            req.body.description,
            req.body.price,
            file.url
            )
        return(Photo.insert(x))
    })
        .then((result) => {
            console.log(result)
            res.json(result);
        })
        .catch(err => {
            console.log(err)
            if(err.code==11000)
                res.status(400).json({ error: "there is already photo with such name" })
            else   
            res.status(400).json({ error:err})
        });
})
router.put('/v1/photos/:id', (req, res) => {
    let promise=new Promise ((resolve, reject) => {
        if (req.files==undefined || req.files.url==undefined){
            resolve({
                url:undefined
            })
        }
        else{
            let file=req.files.url;
            let buf=file.data;
            cloudinary.uploader.upload_stream({resource_type:"image"}, (error, result) =>{
                if (error)
                    reject (error)
                else
                    resolve(result)
            }).end(buf)
        }
    })
    Promise.all([
        promise,
        Photo.getById(req.params.id),
    ])
        .then(async ([insertedFile, photo])=>{
            if(insertedFile.url!=undefined)
                photo.url=insertedFile.url
            console.log("req.body")
            console.log(req.body)
            console.log(req.files)
            if (req.body.name)
                photo.name=req.body.name
            if(req.body.author)
                photo.author=req.body.author
            if(req.body.price)
                photo.price=req.body.price
            if(req.query.owner){
                photo.owner=req.query.owner
                let newUser= await User.getById(req.query.owner)
                if (newUser.chat_id!="" && newUser.chat_id &&newUser.chat_id.length==9){
                    Telegram.sendMessage({
                        chat_id : newUser.chat_id,
                        text : "you've just bougth a new photo "+" - https://photonee.herokuapp.com/photos/"+photo._id
                    })
                }
            }
            return(photo)
        })
        .then((photo)=>{
            return Photo.update(photo)
        })
        .then((photo)=>{
            res.json(photo);
        })
        .catch(err => {
            console.log(err)
            if(err.code==11000)
                res.status(404).json({ error: "there is already photo with such name" })
            else   
            res.status(404).json({ error:err})
        });
})

// albums

router.get('/v1/albums/:id' ,(req, res) => {
    Album.getById(req.params.id)
        .then(_us => {
            if (_us != undefined) {
                res.json(_us, null, 2)
            } else
                res.status(404).json({})
        })
        .catch(err => res.status(404).json({ error: err }));
});
router.get('/v1/albums/',(req, res) => {

    Album.getAll()
        .then(_us => {
            if (_us != undefined) {
                console.log(req.query);
                for (let par in req.query) {
                    if (par=="page" || par=="searching" )
                        continue;
                    let arr = [];
                    console.log(par)
                    for (let i = 0; i < _us.length; i++) {
                        if (_us[i][par] == req.query[par])
                            arr.push(_us[i])
                    }
                    _us = arr
                }
                let tag=req.query.searching;
                
                let itemsOnOnePage=3
                let photos1=[];
                if (tag!=undefined && tag!=''){
                    _us.forEach(photo => {
                        if(photo.name.indexOf(tag)!==-1)
                            photos1.push(photo)
                    });
                    _us=photos1;
                    console.log(_us)
                }
                let totalNumbers=_us.length;
                let page=1;
                if(req.query.page!=undefined){
                    if(!(req.query.page>0 && req.query.page*itemsOnOnePage-itemsOnOnePage<_us.length)){
                        res.json({error:"no such page"},null,2)
                        return
                    }

                    page=req.query.page
                }
                console.log(page)
                let pageN=Math.ceil(totalNumbers/itemsOnOnePage)
                if (pageN==0)
                    pageN=1
                dispalyData=[]
                for (let i =itemsOnOnePage*page-itemsOnOnePage;i<itemsOnOnePage*page && i<_us.length ;i++){
                    dispalyData.push(_us[i])
                }
                console.log("pageN")
                console.log(pageN)
                res.json({
                    displayData:dispalyData,
                    pageNumber:pageN
                }, null, 2)
            } else
                res.sendStatus(404)
        })
        .catch(err => {
            console.log(err)
            res.status(500).send(err.toString())
        });

});

router.delete('/v1/albums/:id', (req, res) => {
    Album.deleteById(req.params.id)
        .then((result) => {
            console.log(result)
            res.json(result);
        })
        .catch(err => res.status(404).json({ error: err }));
})
router.post('/v1/albums',async (req, res) => {
    let response=await new Promise ((resolve, reject) => {
        console.log(req.files.previewUrl)
        if (req.files.previewUrl==undefined){
            resolve({
                previewUrl:"https://i.pinimg.com/originals/1c/ee/01/1cee011feec8e3c61f41af0e604235ab.jpg"
            })
        }
        else{
            let file=req.files.url;
            let buf=file.data;
            cloudinary.uploader.upload_stream({resource_type:"image"}, (error, result) =>{
                if (error)
                    reject (error)
                else
                    resolve(result)
            }).end(buf)
        }
    })
    let ids=[];
    console.log("body")
    console.log(req.body)
    for(par in req.body){
        console.log(par)
        await Photo.getById(par)
            .then((photo)=>{
                ids.push(photo._id)
            })
            .catch(err=>{})
    }
    console.log(ids)
    let x=new Album(
        "1",
        req.body.name,
        req.body.season,
        req.body.description,
        response.previewUrl,
        ids
        )
    Album.insert(x)
        .then((result) => {
            console.log(result)
            res.json(result);
        })
        .catch(err => res.status(404).json({ error: err }));
})
router.put('/v1/albums', (req, res) => {
    Album.getById(req.query.id)
        .then((album)=>{
            album.description=req.query.description,
            album.previewUrl=req.query.previewUrl,
            album.name=req.query.name
            album.season=req.query.season
            return(album)
        })
        .then((album)=>{
            return Album.update(album)
        })
        .then((album)=>{
            res.json(album);
        })
        //.catch(err => res.status(404).json({ error: err }));
})
router.post("/v1/telegram", (req,res)=>{
    console.log(req.query)
    User.getById(req.user.id)
        .then((user)=>{
            user.chat_id=req.query.chat_id
            return User.update(user)
        })
        .then((user)=>{
            Telegram.sendMessage({
                chat_id : req.query.chat_id,
                text : "You have been successfully loged in\n your current login = "+user.login
            })
            res.json("success")
        }
        )
        .catch(err=>{
            console.log(err)
            res.status(500).json(err.toString())
        })
    
})

module.exports = router