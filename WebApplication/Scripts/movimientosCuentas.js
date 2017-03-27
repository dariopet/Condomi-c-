function cargarMovimientosCuentas() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/MovimientosCuentas/EntreFechas/',
        data: {
            'inicioT': $('#fechaInicio').val(), 'finT': $('#fechaFin').val(), 'idcuenta': $('#cuenta').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    });
}
cargarMovimientosCuentas();
//# sourceMappingURL=movimientosCuentas.js.map