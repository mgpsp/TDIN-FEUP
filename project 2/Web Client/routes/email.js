/**
 * Created by ines on 19-05-2017.
 */
var express = require('express');
var router = express.Router();
var nodemailer = require('nodemailer');
var db = require('./database');

let mq = require('../rabbitmq');
let msgQueue = new mq("toWarehouse");

function sendEmail(receiver, message, subject, next) {

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

    transporter.sendMail(mailOptions, function (error, info) {
        if (typeof next === 'function') {
            console.log('Message sent: ' + info.response);
            next('success');
        }
    });

}

router.post('/', function (req, res, next) {
    var msg = '';
    let sub = 'Order information request!';
    if (req.body.bookHasStock === "false") {
        db.websiteOrder({name: req.body.book_Title, quantity: req.body.book_Quantity, clientName: req.body.client_name, clientEmail: req.body.client_mail}, function (err) {
            msg = 'Hello, ' + req.body.client_name + '!' + '\n' + 'Thank you for ordering ' + req.body.book_Quantity + 'x' + req.body.book_Title.replace(/&nbsp/g, ' ') + ' (' + req.body.book_Price + '€).' + '\n' + 'Your order is in the value of ' + req.body.book_Price * req.body.book_Quantity + '€.\n'+ 'We are currently waiting for expedition.\nYour order ID is: ' + this.lastID;
            sendEmail(req.body.client_mail, msg, sub, function () {
                res.json({});
            });
        });
        db.queueOrder(req.body.book_Title, parseInt(req.body.book_Quantity)+10);
    }
    else {
        let date = new Date();
        date.setDate(date.getDate() + 1);
        msg = 'Hello, ' + req.body.client_name + '!\n' + 'Thank you for ordering ' + req.body.book_Quantity + 'x' + req.body.book_Title.replace(/&nbsp/g, ' ') + ' (' + req.body.book_Price + '€)! \n' + 'Your order is in the value of ' + req.body.book_Price * req.body.book_Quantity + '€.\n' + 'It Will be dispatched at' + date + '.';
        sendEmail(req.body.client_mail, msg, sub, function () {
            res.json({});
        });
        let newStock = Math.abs(req.body.book_Stock - req.body.book_Quantity);

        let err = db.order(newStock, req.body.bookId);
    }
});

module.exports = router;
