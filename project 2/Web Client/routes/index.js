var express = require('express');
var router = express.Router();
var db = require('./database');

/* GET home page. */
router.get('/', function(req, res, next) {
    db.books(function (rows) {
      console.log(rows);
      for(var i= 0; i < rows.length; i++){
          rows[i].hasStock = (rows[i].stock > 0);
      }
        res.render('index', { title: 'Express', books: rows});
    });

});

module.exports = router;
