

function cargarMovimientosTerceros() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/MovimientosTerceros/EntreFechas/',
        data: {
            'inicioT': $('#fechaInicio').val(), 'finT': $('#fechaFin').val(), 'idTercero':$('#tercero').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    })
}

cargarMovimientosTerceros();
