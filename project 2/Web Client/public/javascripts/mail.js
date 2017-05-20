/**
 * Created by ines on 19-05-2017.
 */
$('#orderRequest').click(function( event ) {
    event.preventDefault();
    var client_name_var = $("#client_name").val();
    var client_address_var = $("#client_address").val();
    var client_mail_var = $("#client_mail").val();

   $.ajax({
        type: 'POST',
        url: '/email',
        data: JSON.stringify({"client_name": client_name_var, "client_address": client_address_var, "client_mail": client_mail_var}),
        dataType: 'json',
        contentType: 'application/json',
        success: function () {
            $('#bookOrderModel').modal('hide');
        },
        error: function (xhr, status, error) {
            console.log('Error: ' + error);
        }
    });

});