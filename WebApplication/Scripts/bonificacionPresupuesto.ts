function calcularPrecioPresupuesto() {

    var bonificacion = 0;
    if ($('#bonificacion').val() != '')
    {
        bonificacion= parseFloat($('#bonificacion').val());
    }
    var total = parseFloat($('#totalPresupuesto').val()) - bonificacion;

    $('#totalBonif').text("Bonificación: $" + bonificacion.toLocaleString('es-AR'));
    $('#totalPre').text("Total con bonificación: $" + total.toLocaleString('es-AR'));
}
calcularPrecioPresupuesto();