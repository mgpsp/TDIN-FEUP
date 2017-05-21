var amqp = require('amqplib/callback_api');

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('warehouse.db');

var mq = function (queue, socket) {
    var channel;
    amqp.connect("amqp://localhost", function(err, conn) {
        conn.createChannel(function(err, ch) {
            channel = ch;
            channel.assertQueue(queue, {durable: false});
        });
    });

    this.sendMessage = function (msg) {
        console.log("Sending message to warehouse");
        channel.sendToQueue(queue, new Buffer(JSON.stringify(msg)));
    }
    
    this.receiveMessage = function () {
        console.log("Listening to messages");
        channel.consume(queue, function(msg) {
            console.log(" [x] Received %s", msg.content.toString());
            insertOrder(msg.content.toJSON(), socket);
        }, {noAck: true});
    }
}

module.exports = mq;