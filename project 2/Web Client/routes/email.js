/**
 * Created by ines on 19-05-2017.
 */
var express = require('express');
var router = express.Router();
var nodemailer = require('nodemailer');

function sendEmail(receiver, message, subject, next){

    var transporter = nodemailer.createTransport({
        service: 'Gmail',
        auth: {
            user: 'bookenterprise123@gmail.com', // Your email id
            pass: 'TdinProjeto2' // Your password
        },
        secure: false,
        tls: {rejectUnauthorized: false}
    });

    // setup e-mail data with unicode symbols
    var mailOptions = {
        from: 'bookstore@gmail.com', // sender address
        to: receiver, // list of receivers
        subject: subject, // Subject line
        text: message, // plaintext body
    };

    transporter.sendMail(mailOptions, function(error, info){
        if(typeof next === 'function') {
            console.log('Message sent: ' + info.response);
            next('success');
        }
    });

}

router.post('/', function(req, res, next) {

    var msg = 'Ola' + req.body.client_name ;
    var sub = req.body.client_name + ', Order information request!';

    sendEmail(req.body.client_mail, msg, sub, function(){
        res.json({});
    });
});

module.exports = router;
