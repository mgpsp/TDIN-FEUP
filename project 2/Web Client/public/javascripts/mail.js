/**
 * Created by ines on 19-05-2017.
 */
$('#orderRequest').click(function (event) {
    event.preventDefault();
    event.stopPropagation();
    event.stopImmediatePropagation();

    var client_name_var = $("#client_name").val();
    var client_address_var = $("#client_address").val();
    var client_mail_var = $("#client_mail").val();
    var book_Title_var = $("#book_Title").attr("value");
    var book_Price = $("#book_Price").attr("value");
    var book_Stock_var = $("#book_Stock").attr("value");
    var book_Quantity_var = parseInt($("#book_Quantity").val());
    var book_Id_var = $("#bookOrderModel").attr("id-book");
    var bookAsStock_var = $("#bookHasStock").attr("value");

    console.log('mail sent... ' + client_mail_var + ' book_title ' + book_Title_var);

    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (re.test(client_mail_var)) {

        $.ajax({
            type: 'POST',
            url: '/email',
            data: JSON.stringify({
                "client_name": client_name_var,
                "client_address": client_address_var,
                "client_mail": client_mail_var,
                "book_Price": book_Price,
                "book_Quantity": book_Quantity_var,
                "book_Stock": book_Stock_var,
                "book_Title": book_Title_var,
                "bookHasStock": bookAsStock_var,
                "bookId": book_Id_var
            }),
            dataType: 'json',
            contentType: 'application/json',
            success: function () {
                console.log('book stock: ' + book_Stock_var + 'has_Stock ' + bookAsStock_var );

                $('#bookOrderModel').modal('hide');
            },
            error: function (xhr, status, error) {
                console.log( error);
                swal("Error", error, "error")
            }
        });
    }
    else {
        swal("Error", "Mail not valid", "error")
    }

});

$("#upQuantity").click(function (event) {
    event.preventDefault();

    var book_Stock = $("#book_Stock").attr("value");
    var book_Quantity = parseInt($("#book_Quantity").val())+1;

    var newStock = book_Stock - book_Quantity;

    $("#bookHasStock").attr("value", newStock >= 0);

    console.log('diff: ' + newStock + ' book Stock Atual: ' + book_Stock + ' Quantidade pedida ' + book_Quantity + ' Has Stock: ' + $("#bookHasStock").attr("value"));

    if ($("#bookHasStock").attr("value") === "true") {
        $("#bookHasStock").html("<p class=\"\" id=\"book_Stock\" value=" + book_Stock + "><i class=\"fa fa-check fa-fw\" aria-hidden=\"true\"></i> Stock</p>")
    } else
        $("#bookHasStock").html("<p class=\"\" id=\"book_Stock\" value=" + book_Stock + "><i class=\"fa fa-times fa-fw\" aria-hidden=\"true\"></i> No Stock</p>");

});

$("#downQuantity").click(function (event) {
    event.preventDefault();

    var book_Stock = $("#book_Stock").attr("value");
    var book_Quantity = parseInt($("#book_Quantity").val())-1;

    var newStock = book_Stock - book_Quantity ;

    $("#bookHasStock").attr("value", newStock >= 0);

    console.log('diff: ' + newStock + ' book Stock Atual: ' + book_Stock + ' Qauntidade pedida ' + book_Quantity + ' Has Stock: ' + $("#bookHasStock").attr("value"));

    if ($("#bookHasStock").attr("value") === "true") {
        $("#bookHasStock").html("<p class=\"\" id=\"book_Stock\" value=" + book_Stock + "><i class=\"fa fa-check fa-fw\" aria-hidden=\"true\"></i> Stock</p>")
    } else
        $("#bookHasStock").html("<p class=\"\" id=\"book_Stock\" value=" + book_Stock + "><i class=\"fa fa-times fa-fw\" aria-hidden=\"true\"></i> No Stock</p>");

});

$('#bookOrderModel').on('hidden.bs.modal', function () {
    $("#book_Quantity").val("1");
})