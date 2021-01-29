const mongoose=require('mongoose')
const photoSchema=new mongoose.Schema({
    name:{type:String, required:true, unique:true},
    owner:{type:String},
    author:{type:String},
    description:{type: String},
    date:{type:String},
    url: {type:String},
    price: {type:String}
})
const photoModel=mongoose.model('Photo',photoSchema)

class Photo{
    constructor(id,date,author,owner,name,description,price,url){
        this.id=id;
        this.author=author;
        this.owner=owner;
        this.date=date;
        this.description=description;
        this.name=name;
        this.price=price;
        this.url=url;
    }
    static insert(x){
        return photoModel(x).save();
    }
    static getAll() {
        return photoModel.find({})
    }
    static getById(id){
        return photoModel.findOne({_id:id})
    };
    static update(x){
        return photoModel.findOneAndUpdate({_id:x.id},{name:x.name,author:x.author,owner:x.owner, description:x.description,price:x.price,url:x.url})
    }
    static deleteById(id){
        return photoModel.findOneAndDelete({_id:id})
    }

}
module.exports = Photo;