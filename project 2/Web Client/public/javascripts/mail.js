/**
 * Created by ines on 19-05-2017.
 */
$('#orderRequest').click(function( event ) {
    event.preventDefault();
    var client_name = $("#client_name").val();
    var client_address = $("#client_address").val();
    var client_mail = $("#client_mail").val();

   $.ajax({
        type: 'POST',
        url: '/email',
        data: '{client_name: client_name, client_address: client_address, client_mail: client_mail}',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert('yay');
            console.log('Success: ')
            $('#bookOrderModel').modal('hide');
        },
        error: function (xhr, status, error) {
            alert('no');
            console.log(client_mail +  client_name + client_address);
            console.log('Error: ' + error.message);
        },
    });

});