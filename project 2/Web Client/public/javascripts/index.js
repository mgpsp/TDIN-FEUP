/**
 * Created by ines on 18-05-2017.
 */
function createModal(book) {
    $('#bookOrderModel').attr("id-book", book.id);
    $('#book_Title').attr("value", book.title);
    $('#bookHasStock').attr("value", book.hasStock);
    $('#book_Stock').attr("value", book.stock);
    $('#book_Quantity').attr("value", book.quantity);

    $("#book_Title").replaceWith("<h3 class=\"modal-title\" id=\"book_Title\" value=" + book.title + "\>Complete your order for " + book.title + " : </h3>");
    $("#book_Price").replaceWith("<label id=\"book_Price\" value=" + book.price + ">Price: " + book.price + " €</label>");

    if(book.hasStock == "true"){
        $("#bookHasStock").html("<p class=\"\" id=\"book_Stock\" value=" + book.stock + "><i class=\"fa fa-check fa-fw\" aria-hidden=\"true\"></i> Stock</p>")
    }else
        $("#bookHasStock").html("<p class=\"\" id=\"book_Stock\" value=" + book.stock + "><i class=\"fa fa-times fa-fw\" aria-hidden=\"true\"></i> No Stock</p>");



}

$('.order-book').click(function (event) {
        event.preventDefault();
        event.stopPropagation();
        event.stopImmediatePropagation();

        let bookID = $(event.currentTarget).attr("bookID");
        let bookTitle = $(event.currentTarget).attr("bookTitle");
        let bookYear = $(event.currentTarget).attr("bookYear");
        let bookPrice = $(event.currentTarget).attr("bookPrice");
        let bookStock = $(event.currentTarget).attr("bookStock");
        let bookQuantity = $(event.currentTarget).attr("bookQuantity");
        let bookHasStock = $(event.currentTarget).attr("bookHasStock");

        let book = {
            id: bookID,
            title: bookTitle,
            year: bookYear,
            price: bookPrice,
            stock: bookStock,
            quantity: bookQuantity,
            hasStock: bookHasStock
        };
        createModal(book);
        $('#bookOrderModel').modal('show');
    }
);