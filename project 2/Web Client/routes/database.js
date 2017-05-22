/**
 * Created by inesa on 20/05/2017.
 */
let sqlite3 = require('sqlite3').verbose();
let db = new sqlite3.Database('store.db');

let mq = require('../rabbitmq');
let msgQueue = new mq("toWarehouse");

let books = function getBooks(callback) {
    console.log('oorder books: ');
   db.serialize(function() {
       db.all("SELECT * FROM book", function (err, rows) {
            if (err)
                return err;
            else {
               callback(rows);
            }
        });
    });
};

let order = function orderBooksStore(newStock, id) {
    db.serialize(function() {
        console.log('new stock: ' + newStock + ' id ' + id);
        db.run("UPDATE book SET stock=? WHERE id=?",[newStock, id],function (err) {
            if (err){
                console.log(err);
                return err;
            }
            else {
                console.log('updating stock to ' + newStock);
                return true;
            }
        });
    });
};

let websiteOrder = function newWebsiteOrder(order, cb) {
    db.serialize(function() {
        db.run("INSERT INTO website_order(name, quantity, status, clientName, clientEmail) VALUES(?,?,?,?,?)",[order.name.replace(/&nbsp/g, ' '), order.quantity, "Waiting expedition", order.clientName, order.clientEmail],cb);
    });
}

let view = function viewOrder(id, cb) {
    db.serialize(function() {
        db.get("SELECT * FROM website_order WHERE id = ?", [id], function (err, order) {
            db.get("SELECT * FROM book WHERE name = ?", [order.name], function (err, book) {
                cb(order, book);
            })
        });
    });
}

let queueOrder = function queueOrderBooks(title, quantity){
    let order = {name:title, quantity: quantity};
    console.log("Ordering " + order.name + " (" + order.quantity + ") from warehouse");
    msgQueue.sendMessage(order);
    return true;
};

module.exports.books = books;
module.exports.order = order;
module.exports.queueOrder = queueOrder;
module.exports.websiteOrder = websiteOrder;
module.exports.view = view;