
//fetch precios
function calcularIvaIngresos() {

    var subtotal = parseFloat($('#totalingreso').val().replace(",", "."));
    var iva = $('#iva').val();   
    var resultadoiva;  
    var totalfinal;


    if ($('#tipofactura').val() == 0) {//factura A IVA descriminado

        $('#txtsubtotal').show();       
        $('#txtresultadoiva').show();

        resultadoiva = subtotal * ((parseFloat(iva) / 100));
      
        $('#txtsubtotal').text("Subtotal: $"+subtotal.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        $('#txtresultadoiva').text("IVA: $"+resultadoiva.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
  
        $('#txttotalfinal').text("Total: $" + (subtotal + resultadoiva).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
    }
    else
    {
        $('#txtsubtotal').hide();       
        $('#txtresultadoiva').hide();
        $('#txttotalfinal').text("Total: $" + (subtotal).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
    }
   
  
}

calcularIvaIngresos();

