function cargarMovimientosPlanCuentas() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/MovimientosPlanCuentas/EntreFechas/',
        data: {
            'inicioT': $('#fechaInicio').val(), 'finT': $('#fechaFin').val(), 'idPlan': $('#plan').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    });
}
cargarMovimientosPlanCuentas();
//# sourceMappingURL=movimientosPlanCuentas.js.map