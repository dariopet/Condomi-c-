
function cargarIvaSaldo() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/SaldoIva/EntreFechas/',
        data: { 'inicio': $('#fechaInicio').val(), 'fin': $('#fechaFin').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    })
}

cargarIvaSaldo();
