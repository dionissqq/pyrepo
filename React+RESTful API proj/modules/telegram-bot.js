const config = require('../config');
const User= require ('../models/user')
const Photo= require ('../models/photos')
const TelegramBotApi = require('telegram-bot-api');

const telegramBotApi = new TelegramBotApi({
    token: config.bot_token,
    polling:true,
    updates: {
        enabled: true // do message pull
    }
});

telegramBotApi.on('inline.callback.query', function(message) {
    const data = message.data;
    if (data !== "auth") {
        sendCommandIsNotSupported(message.chat.id);
        return;
    }
});

telegramBotApi.on('message', async (message) => {
    try {
        const user = await User.getByTelegramId(message.chat.id);
        if (!user) {
            if (message.text && message.text === "/start")
                await sendAuthMessage(message.from.id, `Welcome to ${botName}! You must log in to continue.`);
            else
                await sendAuthMessage(message.from.id, `You must log in first.`);
            return;
        }
        else{
            if(message.text && message.text === "/logout")
                await logout(message.from.id)
            if(message.text && message.text === "/myphotos"){
                let text= await getallUsersPhotos(message.from.id)
                telegramBotApi.sendMessage({
                    chat_id : message.chat.id,
                    text :text
                });
            }
        }   
    } catch (err) {
        await sendError(message.chat.id);
        console.error(err);
    }
});
async function logout(chat_id){
    let user=await User.getByTelegramId(chat_id);
    user.chat_id=""
    await User.update(user)
    telegramBotApi.sendMessage({
        chat_id : chat_id,
        text : "you've successfully loged out"
    });

}
async function getallUsersPhotos(chat_id){
    let user=await User.getByTelegramId(chat_id);
    let allPhotos=await Photo.getAll()
    let photos=[];
    allPhotos.map(photo=>{
        if(photo.owner==user._id)
            photos.push(photo)
    })
    let text="your current photo list:\n"
    if (photos.length==0)
        text="you don't have any photos yet"
    photos.map(photo=>{
        text+=" "+ photo.name+ " - https://photonee.herokuapp.com/photos/"+photo._id+"\n"
    })
    return text
}

function sendAuthMessage (chat_id, message) {
    console.log(`\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n${config.hostUrl + "bot/auth?id=" + chat_id}\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n`);
    return telegramBotApi.sendMessage({
        chat_id : chat_id,
        text : message,
        reply_markup: JSON.stringify({
            inline_keyboard : [
                [
                    {
                        text : "Auth",
                        callback_data: "auth",
                        url: config.hostUrl + "bot/auth?id=" + chat_id
                    }
                ]
            ]
        })
    });
}

function sendCommandIsNotSupported (chat_id) {
    return telegramBotApi.sendMessage({
        chat_id : chat_id,
        text : "Sorry, can’t understand your command"
    });
}

function sendError (chat_id) {
    return telegramBotApi.sendMessage({
        chat_id : chat_id,
        text : "Something seems to have gone wrong... Try again later."
    });
}

module.exports = telegramBotApi;