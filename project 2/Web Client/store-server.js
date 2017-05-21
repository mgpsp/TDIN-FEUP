var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('store.db');

var mq = require('./rabbitmq');
var msgQueue = new mq("toWarehouse");

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
        db.get("SELECT stock FROM book WHERE id = ?", [sell.id],  function (err, rows) {
            var newStock = rows.stock - sell.quantity;
            db.run("UPDATE book SET stock = ? WHERE id = ?", [newStock, sell.id], function () {
                socket.emit("sold");
            });
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

function acceptOrder(order) {
    db.serialize(function() {
        db.run("UPDATE warehouse_order SET status = 'Accepted' WHERE id = ?", [order.id]);
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