var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('warehouse.db');

function getOrders(socket) {
    console.log("getOrders()");
    db.serialize(function() {
        db.all("SELECT * FROM orders", function (err, rows) {
            if (err)
                socket.emit("order-error", err);
            else
                socket.emit("orders", rows);
        });
    });
}
io.on('connection', function(socket){
    console.log("Warehouse connected to server");
    socket.on('getOrders', function () {
        console.log("Retrieving orders");
        getOrders(socket);
    })
});
server.listen(3002);