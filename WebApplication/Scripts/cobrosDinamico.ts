var saldo;

function getSaldoIngreso() {
    //ajax function for fetch data  
    $.ajax({
        type: "GET",
        url: '/Ingresos/getSaldoIngreso/',
        data: { 'idIngreso': $('#idingreso').val() },
        success: function (data) {
            
            saldo = data.replace(",", ".");   
            saldo = parseFloat(saldo);
            saldo += parseFloat($('#pagoOriginal').val());          
            $('#monto').attr({ 'max': parseFloat(saldo) });
            $('#saldoACobrar').text("Saldo a cobrar $" + parseFloat(saldo).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        }
    })  
}

function habilitarCobros() {
    var val1 = $('#cuentaDestino').val(); 
    if (val1 ==2) {
        $('#idCheque').prop("disabled", false);
        getDatosChequeCobros();
        $('#divcheque1').show();
        $('#divcheque2').show();
    }
    else {
        $('#idCheque').prop("disabled", true);
        $('#divcheque1').hide();
        $('#divcheque2').hide();
        //pongo como maximo el saldo q tengo q cobrar
        $('#monto').attr({ 'max': parseFloat(saldo) });      
    }
}

function getDatosChequeCobros() {
    //lo pongo en 0 x si no hay ninguno disponible
    $('#monto').attr({ 'max': 0 });
    $.ajax({
        type: "GET",
        url: '/Cobros/getDatosCheque/',
        data: { 'idCheque': $('#idCheque').val() },
        success: function (data) {
            //si el cheque es el mismo q el del cobro original le sumo el pago original           
            if ($('#idCheque').val() == $('#chequeOriginal').val())
            {
                data.montoDisponible += parseFloat($('#pagoOriginal').val());                   
            }
            $('#montoDisponible').text("$" + data.montoDisponible);
            $('#banco').text( data.banco);
            $('#recibido').text(data.recibido);

            if (data.montoDisponible < parseFloat(saldo)) {
                 //pongo como maximo el saldo del cheque
                $('#monto').attr({ 'max': data.montoDisponible });
            }
            else
            {
                //pongo como maximo el saldo q tengo q cobrar
                $('#monto').attr({ 'max': parseFloat(saldo) });
            }
           
        }
    })
}
getSaldoIngreso();
habilitarCobros();