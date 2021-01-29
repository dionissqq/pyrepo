const mongoose = require('mongoose')
const userSchema = new mongoose.Schema({
    login: { type: String, required: true, unique: true },
    password: { type: String, required: true },
    fullname: { type: String },
    role: { type: Boolean },
    registeredAt: { type: String },
    avaUrl: { type: String },
    isDisabled: { type: Boolean },
    chat_id: { type: String }
})
const userModel = mongoose.model('User', userSchema)

class User {

    constructor(id, login, password, role, fullname, registeredAt, avaUrl, isDisabled) {
        this.id = id;
        this.password = password;
        this.login = login;
        this.role = role;
        this.fullname = fullname;
        this.registeredAt = registeredAt;
        this.avaUrl = avaUrl;
        this.isDisabled = isDisabled;
    }
    static getByTelegramId(chat_id){
        return userModel.findOne({ chat_id: chat_id })
    }
    static getById(id) {
        return userModel.findOne({ _id: id })
    }
    static getUserToReturn(id){
        return this.getById(id)
            .then(x=>{
                let user={
                    id:x._id,
                    login:x.login,
                    fullname:x.fullname,
                    avaUrl:x.avaUrl,
                    registeredAt:x.registeredAt,
                    role:x.role,
                    chat_id:x.chat_id
                }
                
                return user;
            })
            .catch(err=>Promise.reject(err))
    }
    static findOne(username) {
        return userModel.findOne({ login: username })
    }
    static normalize(x){
        let user={
            id:x._id,
            login:x.login,
            fullname:x.fullname,
            avaUrl:x.avaUrl,
            registeredAt:x.registeredAt,
            role:x.role,
            chat_id:x.chat_id
        }
        return user
    }
    static getAll() {
        return userModel.find({})
    }
    static insert(x) {
        return userModel(x).save();
    }
    static update(x) {
        return userModel.findOneAndUpdate({ _id: x._id }, { login: x.login, password: x.password, role: x.role, fullname: x.fullname,chat_id:x.chat_id, avaUrl: x.avaUrl });
    }
    static deleteById(id){
        return userModel.findOneAndDelete({_id:id})
    }
};

module.exports = User;
