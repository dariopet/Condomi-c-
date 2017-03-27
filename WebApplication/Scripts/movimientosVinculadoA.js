function cargarMovimientosVinculadoA() {
    //ajax function for fetch data    
    var valor = 0;
    if ($('#vinculadoA').val() == '') {
        valor = -1;
    }
    else {
        valor = $('#vinculadoA').val();
    }
    $.ajax({
        type: "GET",
        url: '/MovimientosVinculadoA/EntreFechas/',
        data: { 'inicioT': $('#fechaInicio').val(), 'finT': $('#fechaFin').val(), 'vinculadoA': valor },
        success: function (data) {
            $('#contenido').html(data);
        }
    });
}
$('#vinculadoA').val(0);
cargarMovimientosVinculadoA();
//# sourceMappingURL=movimientosVinculadoA.js.map