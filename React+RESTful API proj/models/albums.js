const mongoose=require('mongoose')
const albumSchema=new mongoose.Schema({
    name:{type:String},
    season:{type:String},
    description:{type: String},
    previewUrl: {type:String},
    photos:{type:Array}
})
const albumModel=mongoose.model('Album',albumSchema)

class Album{
    constructor(id,name,season,description,previewUrl,photos){
        this.id=id;
        this.season=season;
        this.description=description;
        this.name=name;
        this.previewUrl=previewUrl;
        this.photos=photos;
    }
    static insert(x){
        return albumModel(x).save();
    }
    
    static getAll() {
        return albumModel.find({})
    }
    static getById(id){
        return albumModel.findOne({_id:id})
    };
    static update(x){
        return albumModel.findOneAndUpdate({_id:x.id},{name:x.name,author:x.author, description:x.description,photos:x.photos,price:x.price,url:x.url})
    }
    static deleteById(id){
        return albumModel.findOneAndDelete({_id:id})
    }

}
module.exports = Album;