/**
 * Created by inesa on 20/05/2017.
 */
var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('store.db');

/* GET home page. */
var books = function getBooks(callback) {
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


var order = function orderBooksStore(newStock, id) {
    db.serialize(function() {
        console.log('new stock: ' + newStock + ' id ' + id);
        db.run("UPDATE book SET stock=? WHERE id=?",[newStock, id],function (err) {
            if (err){
                console.log(err);
                return err;
            }
            else {
                console.log('updated');
                return true;
            }
        });
    });
};


module.exports.books = books;
module.exports.order = order;