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

module.exports.books = books;