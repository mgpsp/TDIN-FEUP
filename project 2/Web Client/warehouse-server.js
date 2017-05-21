var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('warehouse.db');

var mq = require('./rabbitmq');
var msgQueue = new mq("fromWarehouse");

var amqp = require('amqplib/callback_api');
amqp.connect("amqp://localhost", function(err, conn) {
    conn.createChannel(function(err, ch) {
        ch.assertQueue("toWarehouse", {durable: false});
        ch.consume("toWarehouse", function(msg) {
            insertOrder(JSON.parse(msg.content.toString()));
        }, {noAck: true});
    });
});

function getOrders(socket) {
    db.serialize(function() {
        db.all("SELECT * FROM orders", function (err, rows) {
            if (err)
                socket.emit("order-error", err);
            else
                socket.emit("orders", rows);
        });
    });
}

function insertOrder(order) {
    db.serialize(function() {
        db.run("INSERT INTO orders(name, quantity, status) VALUES(?, ?, ?)", [order.name, order.quantity, "Waiting expedition"], function (err) {
            if (!err) {
                db.get("SELECT * FROM orders WHERE id = ?", [this.lastID], function (err, rows) {
                    if (!err)
                        io.emit("newOrder", rows);
                });
            }
        });
    });
}

function shipOrder(order) {
    db.serialize(function() {
        db.run("UPDATE orders SET status = 'Dispatched' WHERE id = ?", [order.id]);
    });
}

io.on('connection', function(socket){
    console.log("Warehouse connected to server");

    socket.on('getOrders', function () {
        console.log("Retrieving orders");
        getOrders(socket);
    });

    socket.on('orderShipped', function (order) {
        shipOrder(order);
        msgQueue.sendMessage(order);
    });
});
server.listen(3002);