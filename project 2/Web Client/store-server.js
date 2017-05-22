var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('store.db');

var mq = require('./rabbitmq');
var msgQueue = new mq("toWarehouse");

var nodemailer = require('nodemailer');

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
        if (error)
            console.log(error);
        else
            console.log("Email sent");
    });

}

var amqp = require('amqplib/callback_api');
amqp.connect("amqp://localhost", function(err, conn) {
    conn.createChannel(function(err, ch) {
        ch.assertQueue("fromWarehouse", {durable: false});
        ch.consume("fromWarehouse", function(msg) {
            insertWarehouseOrder(JSON.parse(msg.content.toString()));
        }, {noAck: true});
    });
});

function getBooks(socket) {
    db.serialize(function() {
        db.all("SELECT * FROM book", function (err, rows) {
            if (err)
                socket.emit("book-error", err);
            else
                socket.emit("books", rows);
        });
    });
}

function getWarehouseOrders(socket) {
    db.serialize(function() {
        db.all("SELECT * FROM warehouse_order WHERE status = 'Pending'", function (err, rows) {
            if (err)
                socket.emit("warehouseOrder-error", err);
            else
                socket.emit("warehouseOrders", rows);
        });
    });
}

function sellBook(sell, socket) {
    db.serialize(function() {
        db.get("SELECT stock FROM book WHERE name = ?", [sell.name],  function (err, rows) {
            var newStock = rows.stock - sell.quantity;
            if (newStock >= 0) {
                db.run("UPDATE book SET stock = ? WHERE name = ?", [newStock, sell.name], function () {
                    socket.emit("sold");
                    socket.emit("updateBookStock", {name: sell.name, stock: newStock});
                });
            }
        });
    });
}

function dispatchOrder(sell, orderid) {
    db.serialize(function() {
        db.get("SELECT stock FROM book WHERE name = ?", [sell.name],  function (err, rows) {
            var newStock = rows.stock - sell.quantity;
            if (newStock >= 0) {
                db.get("SELECT * FROM website_order WHERE id = ?", [orderid], function (err, row) {
                    db.run("UPDATE book SET stock = ? WHERE name = ?", [newStock, sell.name], function () {
                        io.emit("updateBookStock", {name: sell.name, stock: newStock});
                        let date = new Date();
                        date.setDate(date.getDate() + 1);
                        var msg = 'Hello, ' + row.clientName + '!\n' + 'Your order of ' + sell.name + ' will be dispatched at ' + date;
                        sendEmail(row.clientEmail, msg, "Your order has been dispatched", function () {
                            return true;
                        })
                    });
                    db.run("UPDATE website_order SET status = 'Dispatched' WHERE id = ?", [orderid]);
                });
            }
        });
    });
}

function insertWarehouseOrder(order) {
    db.serialize(function() {
        db.run("INSERT INTO warehouse_order(name, quantity, status) VALUES(?, ?, ?)", [order.name, order.quantity, "Pending"], function (err) {
            if (!err) {
                db.get("SELECT * FROM warehouse_order WHERE id = ?", [this.lastID], function (err, rows) {
                    if (!err)
                        io.emit("warehouseOrder", rows);
                });
            }
        });
    });
}

function shipOrders(name) {
    db.serialize(function() {
        db.each("SELECT * FROM website_order WHERE name = ? AND status = 'Waiting expedition'", [name], function (err, row) {
            dispatchOrder({name: row.name, quantity: row.quantity}, row.id);
        });
    });
}

function acceptOrder(order) {
    db.serialize(function() {
        db.run("UPDATE warehouse_order SET status = 'Accepted' WHERE id = ?", [order.id]);
        db.get("SELECT * FROM book WHERE name = ?", [order.name], function (err, rows) {
            if (!err) {
                var qt = rows.stock + order.quantity;
                db.run("UPDATE book SET stock = ? WHERE name = ?", [qt, order.name], function () {
                    io.emit("updateBookStock", {name: order.name, stock: qt});
                    shipOrders(order.name);
                });
            }
        })
    });
}

io.on('connection', function(socket){
    console.log("Store connected to server");
    socket.on('getBooks', function () {
        console.log("Retrieving books");
        getBooks(socket);
    });

    socket.on('getWarehouseOrders', function () {
        console.log("Retrieving warehouse orders");
        getWarehouseOrders(socket);
    });

    socket.on('sellBook', function (sell) {
        console.log("Selling " + sell.name + " (" + sell.quantity + ")");
        sellBook(sell, socket);
    });

    socket.on("order", function (order) {
        console.log("Ordering " + order.name + " (" + order.quantity + ") from warehouse");
        msgQueue.sendMessage(order);
    })

    socket.on("acceptOrder", function (order) {
        console.log("Accepting order " + order.name + " (" + order.quantity + ")");
        acceptOrder(order);
    })
});


server.listen(3001);