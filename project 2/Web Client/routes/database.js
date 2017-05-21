/**
 * Created by inesa on 20/05/2017.
 */
let sqlite3 = require('sqlite3').verbose();
let db = new sqlite3.Database('store.db');


let mq = require('../rabbitmq');
let msgQueue = new mq("toWarehouse");

/* GET home page. */
let books = function getBooks(callback) {
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

let queueOrder = function queueOrderBooks(title, quantity){
    let order = JSON.parse("{name:" + title + ", quantity:" + quantity + "}");
    console.log("Ordering " + order.name + " (" + order.quantity + ") from warehouse")
    msgQueue.sendMessage(order);
    return true;
};


module.exports.books = books;
module.exports.order = order;
module.exports.queueOrder = queueOrder;