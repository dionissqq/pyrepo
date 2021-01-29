const passport = require('passport')
const LocalStrategy = require('passport-local').Strategy
const User = require('./models/user')
const authRouter = require('./routes/auth')
const config=require('./config')

passport.use(new LocalStrategy((username, password, done) => {
    console.log('localstrategy');
    User.getAll()
        .then(users => {
            let us;
            for (let user of users) {
                if (user.login == username && user.password == authRouter.sha512(password, config.serverSalt).passwordHash) {
                    us = user;
                }
            }
            if (us != undefined)
                return done(null, us)
            else {
                return done("no such user", null)
            }
        })
        .catch((err) => done(err, null))
}));

const passportJWT = require("passport-jwt");
const JWTStrategy   = passportJWT.Strategy;
const ExtractJWT = passportJWT.ExtractJwt;

passport.use(new JWTStrategy({
        jwtFromRequest: ExtractJWT.fromAuthHeaderAsBearerToken(),
        secretOrKey   : 'itsmysecret!'
    },
    function (jwtPayload, cb) {
        console.log("jwtstrategy")
        //find the user in db if needed. This functionality may be omitted if you store everything you'll need in JWT payload.
        return User.getUserToReturn(jwtPayload.id)
            .then(user => {
                return cb(null, user);
            })
            .catch(err => {
                return cb(err,null);
            });
    }
));

module.exports = passport;