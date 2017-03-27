function getPrecioUltimaJornada() {
    //ajax function for fetch data
    $('#jornadas').val("1");
    $.ajax({
        type: "GET",
        url: '/HorasTrabajadasTecnico/getPrecioUltimaJornada/',
        data: { 'idTercero': $('#tecnicos').val() },
        success: function (data) {
            $('#precioJornada').val(data.precioUltimoPago);
            $('#dimmer').val(data.precioDimmer);
            $('#seguidor').val(data.precioSeguidor);
            calcularPago();
        }
    });
}
function calcularPago() {
    var dimmer;
    var seguidor;
    var precioJornada;
    var jornadas;
    var suma;
    dimmer = (parseFloat($('#dimmer').val()));
    seguidor = (parseFloat($('#seguidor').val()));
    jornadas = (parseFloat($('#jornadas').val()));
    precioJornada = (parseFloat($('#precioJornada').val()));
    suma = (jornadas * precioJornada) + dimmer + seguidor;
    $('#total').text("Total: $" + suma.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
}
//# sourceMappingURL=pagosTecnicos.js.map