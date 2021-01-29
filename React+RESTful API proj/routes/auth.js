const User = require('../models/user');
const express = require('express');
const router = express.Router();
const config = require('../config');
const cloudinary = require('cloudinary').v2;
const passport = require('../passport');
const crypto = require('crypto');
const jwt= require('jsonwebtoken');

router.sha512= function (password) {
    const hash = crypto.createHmac('sha512', config.serverSalt);
    hash.update(password);
    const value = hash.digest('hex');
    return {
        salt: config.serverSalt,
        passwordHash: value
    };
};

cloudinary.config({
    cloud_name: config.cloudName, 
    api_key: config.apiKey, 
    api_secret: config.apiSecret 
})

router.get('/auth/login',function(req, res, next) {
    res.render('login')
});

router.get('/auth/register',function(req, res, next) {
    let data={
        error:"Please enter your data:"
    }
    res.render('register',data)
});
router.post('/auth/register',function(req, res, next) {
    let error=undefined;
    if(req.body.password!=req.body.password1){
        error="passwords don't match try again:"
        let data={
            error:error
        }
        res.render('register',data)
        return;
    }
    
    new Promise ((resolve, reject) => {
        console.log(req.files.avaUrl)
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
            let passHesh=router.sha512(req.body.password).passwordHash;
            let us=new User("1",req.body.login,passHesh,false,req.body.fullname,dat,insertedFile.url,"0")
            console.log(us)
            return(us)
        })
        .then((us)=>{
            return(User.insert(us))
        })
        .then(()=>{
            res.redirect("login")
        })
        .catch((err)=>{
            console.log(err)
            if (err.code=11000){
                err="this login is already taken"
            }
            let data={
                error:err
            }
            res.render('register',data)
        })
        
});
router.get('/auth/logout',(req, res) => {
        req.logout();
        res.redirect('login');
});
router.post('/auth/login', function (req, res, next) {

    passport.authenticate('local', {session: true}, (err, user, info) => {
        console.log(err);
        if (err || !user) {
            return res.status(400).json({
                message: info ? info.message : 'Login failed',
                user   : user
            });
        }
        console.log("user");
        console.log(user);
        req.login(user, {session: true}, (err) => {
            if (err) {
                res.send(err);
            }

            const token = jwt.sign(JSON.parse(JSON.stringify(user)), 'itsmysecret!');

            return res.json({user, token});
        });
    })
    (req, res);

});

router.checkAdmin=function (req, res, next) {
    
    if (!req.user) return res.sendStatus(401);
    User.getById(req.user.id)
        .then(user=>{
            if (user.role !== true) return res.sendStatus(403);
            next();
        })
}
router.checkAuth=function (req, res, next) {
    if (!req.user) return res.sendStatus(401);
    next();
}

module.exports = router;