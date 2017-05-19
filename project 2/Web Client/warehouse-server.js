var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('warehouse.db');

function getOrders() {
    db.serialize(function() {
        db.all("SELECT * FROM orders", function (err, rows) {
            if (err)
                console.log(err);
            else
                console.log(rows);
        });
    });
}
io.on('connection', function(){
    console.log("Warehouse connected to server");
    getOrders();
});
server.listen(3002);