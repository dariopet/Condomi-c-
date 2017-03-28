
//fetch precios
function calcularIva() {

    var subtotal = parseFloat($('#totalegreso').val().replace(",", "."));
     
    var iva = $('#iva').val();   
    var resultadoiva;
    var ingresosbrutos;
    var impuestosinternos;
    var percepcioniva;

    var conceptosNoGravados = parseFloat($('#conceptos').val().replace(",", "."));
    var totalfinal;


    if ($('#tipofactura').val() == 0) {//factura A IVA descriminado

        $('#txtsubtotal').show();
        $('#txtingresosbrutos').show();
        $('#txtimpuestosinternos').show();
        $('#txtpercepcioniva').show();
        $('#txtresultadoiva').show();
        $("#txtconceptos").show();

        resultadoiva = subtotal * ((parseFloat(iva) / 100));
        ingresosbrutos = (subtotal + conceptosNoGravados) * ((parseFloat($('#ingresosbrutos').val()) / 100));
        impuestosinternos = subtotal * ((parseFloat($('#impuestosinternos').val()) / 100) );
        percepcioniva = subtotal * ((parseFloat($('#percepcioniva').val()) / 100));

        $("#txtconceptos").text("Conceptos no gravados: $" + conceptosNoGravados.toLocaleString("es-ES",  { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('#txtsubtotal').text("Subtotal: $"+subtotal.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        $('#txtingresosbrutos').text("Ingresos brutos: $" +ingresosbrutos.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        $('#txtimpuestosinternos').text("Impuestos internos: $"+impuestosinternos.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        $('#txtpercepcioniva').text("Percepción Iva: $"+percepcioniva.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        $('#txtresultadoiva').text("IVA: $"+resultadoiva.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
  
        $('#txttotalfinal').text("Total: $" + (subtotal + ingresosbrutos + impuestosinternos + percepcioniva + resultadoiva + conceptosNoGravados).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
    }
    else
    {
        $('#txtsubtotal').hide();
        $('#txtingresosbrutos').hide();
        $("txtconceptos").hide();
        $('#txtimpuestosinternos').hide();
        $('#txtpercepcioniva').hide();
        $('#txtresultadoiva').hide();
        $('#txttotalfinal').text("Total: $" + (subtotal).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
    }
   
  
}

calcularIva();

