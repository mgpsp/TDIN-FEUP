var amqp = require('amqplib/callback_api');

var mq = function (queue) {
    var channel;
    amqp.connect("amqp://localhost", function(err, conn) {
        conn.createChannel(function(err, ch) {
            channel = ch;
            channel.assertQueue(queue, {durable: false});
        });
    });

    this.sendMessage = function (msg) {
        channel.sendToQueue(queue, new Buffer(JSON.stringify(msg)));
    }
}

module.exports = mq;