/**
 * Created by ines on 19-05-2017.
 */
$('#orderRequest').click(function( event ) {
    event.preventDefault();
    var client_name_var = $("#client_name").val();
    var client_address_var = $("#client_address").val();
    var client_mail_var = $("#client_mail").val();
    var book_Title_var = $("#book_Title").attr("value");
    var book_Price_var = $("#book_Price").attr("value");
    var book_Stock_var = $("#book_Stock").attr("value");
    var book_Quantity_var = $("#book_Quantity").val();

   $.ajax({
        type: 'POST',
        url: '/email',
        data: JSON.stringify({"client_name": client_name_var, "client_address": client_address_var, "client_mail": client_mail_var, "book_Price": book_Price_var, "book_Quantity":book_Quantity_var, "book_Stock": book_Stock_var, "book_Title": book_Title_var}),
        dataType: 'json',
        contentType: 'application/json',
        success: function () {
            console.log(book_Price_var +  ' quat' + book_Quantity_var + '  stock' + book_Title_var);

            $('#bookOrderModel').modal('hide');
        },
        error: function (xhr, status, error) {
            console.log('Error: ' + error);
        }
    });

});