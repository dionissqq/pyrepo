const express = require('express');
const path=require('path')
const busboyBodyParser = require('busboy-body-parser');
const app = express();
const mongoose = require('mongoose');
const config = require('./config');
const url = config.DatabaseUrl;
const serverPort = config.ServerPort;
const connectionOptions = { useNewUrlParser: true, useUnifiedTopology: true }
require('./passport');
const logger = require('morgan');


app.use(logger('dev'));

mongoose.connect(url, connectionOptions)
    .then(() => console.log('Database connected'))
    .then(() => app.listen(serverPort))
    .then(() => console.log('Running at Port '+serverPort))
    .catch(err => console.log(err.toString()))

app.use(busboyBodyParser());

app.use(express.static(path.join(__dirname, 'build')));



const userapiRouter = require('./routes/api')
app.use('/api', userapiRouter)

app.get('/*', (req, res) => {
  res.sendFile(path.join(__dirname, 'build', 'index.html'));
});


