const express = require('express');
const router = express.Router();

router.get('/developer/v1',(req, res)=> {
    res.render('doc')
});

module.exports=router;