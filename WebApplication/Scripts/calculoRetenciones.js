var pagado;
var pagadoFactura;
var retenidoConcepto;
var totalFactura;
function getPagado(idTipoEgreso, original) {
    //ajax function for fetch data 
    $.ajax({
        type: "GET",
        url: '/Pagos/getPagadoTipo/',
        data: { 'idTipoEgreso': idTipoEgreso, 'fechaPago': $('#fechaPago').val(), 'idTercero': $('#idTercero').val() },
        success: function (data) {
            pagado = data;
            pagado = parseFloat(pagado) - parseFloat(original);
            $('#pagado').text("$ " + pagado.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        }
    });
}
function getTotalBruto() {
    //ajax function for fetch data 
    $.ajax({
        type: "GET",
        url: '/Egresos/getTotalBrutoEgreso/',
        data: { 'idEgreso': $('#idegreso').val() },
        success: function (data) {
            totalFactura = data;
            totalFactura = parseFloat(totalFactura);
        }
    });
}
function getRetenidoTipo(idTipoEgreso, retenidoOriginal) {
    //ajax function for fetch data
    $.ajax({
        type: "GET",
        url: '/Pagos/getRetenidoTipo/',
        data: { 'idTipoEgreso': idTipoEgreso, 'fechaPago': $('#fechaPago').val() },
        success: function (data) {
            retenidoConcepto = data;
            retenidoConcepto = parseFloat(retenidoConcepto) - parseFloat(retenidoOriginal);
            // $('#pagado').text("$ " + pagado.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        }
    });
    getPagadoFactura();
}
function getPagadoFactura() {
    //ajax function for fetch data
    var pagoOriginal = parseFloat($('#pagoOriginal').val());
    $.ajax({
        type: "GET",
        url: '/Pagos/getPagado/',
        data: { 'idEgreso': $('#idegreso').val() },
        success: function (data) {
            pagadoFactura = data;
            pagadoFactura = parseFloat(pagadoFactura) - pagoOriginal;
            //  $('#pagado').text("$ " + pagado.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        }
    });
}
function calcularRetenido(porcentaje, limite) {
    if ($('#pagar').prop("checked")) {
        var retenido;
        var importeTotal;
        var excedente;
        var limiteDisponible;
        var yaRetenido;
        var yaRetenidoConcepto;
        var monto;
        var yaPagadoEsteConcepto;
        var yaPagadoFactura;
        var retenidoOriginal;
        retenidoOriginal = parseFloat($('#retenidoOriginal').val());
        yaPagadoFactura = parseFloat(pagadoFactura);
        yaPagadoEsteConcepto = parseFloat(pagado);
        yaRetenidoConcepto = parseFloat(retenidoConcepto);
        retenido = 0;
        //recupero el monto q esta pagando y le saco el iva
        // monto = parseFloat($('#monto').val());   
        // monto = monto / (1 + (parseFloat($('#iva').val()) / 100));
        //recupero lo que ya fue retenido de este pago
        yaRetenido = parseFloat($('#yaRetenido').val());
        //recupero el limite de este concepto
        limite = parseFloat(limite);
        importeTotal = totalFactura; //(yaPagadoFactura + monto)
        limiteDisponible = limite - (yaPagadoEsteConcepto - yaPagadoFactura);
        if (limiteDisponible < 0) {
            limiteDisponible = 0;
        }
        importeTotal = importeTotal - limiteDisponible;
        retenido = importeTotal * (parseFloat(porcentaje) / 100);
        retenido -= retenidoOriginal;
        retenido -= yaRetenido;
        if (retenido < 0) {
            retenido = 0;
        }
        $('#retenido').val(retenido.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    }
}
getTotalBruto();
//# sourceMappingURL=calculoRetenciones.js.map