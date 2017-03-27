var saldo;

function getSaldoEgreso() {
    //ajax function for fetch data  
    $.ajax({
        type: "GET",
        url: '/Egresos/getSaldoEgreso/',
        data: { 'idEgreso': $('#idEgreso').val() },
        success: function (data) {
            saldo = data.replace(",",".");   
            
            saldo = parseFloat(saldo);
            saldo += parseFloat($('#pagoOriginal').val()); 
            saldo += parseFloat($('#retenidoOriginal').val());            
            $('#monto').attr({ 'max': parseFloat(saldo) });
            $('#saldoAPagar').text("Saldo a pagar $" + parseFloat(saldo).toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
        }
    })  
}

function habilitarPagos() {
    var val1 = $('#cuentaOrigen').val(); 
    if (val1 == 2) {//cheque
        $('#idCheque').prop("disabled", false);
        getDatosChequePagos();
        $('#divcheque1').show();
        $('#divcheque2').show();
        $('#divadelanto1').hide();
    }
    else if (val1 == 3) { //adelantos
        $('#idCheque').prop("disabled", true);
        $('#divcheque1').hide();
        $('#divcheque2').hide();
        $('#divadelanto1').show();
        getDatosAdelanto();      
       
    }
    else //cualquier otra cuenta
    {
        $('#idCheque').prop("disabled", true);
        $('#divcheque1').hide();
        $('#divcheque2').hide();
        $('#divadelanto1').hide();
        //pongo como maximo el saldo q tengo q cobrar
        $('#monto').attr({ 'max': parseFloat(saldo) });
    }
}

function getDatosChequePagos() {
    //lo pongo en 0 x si no hay ninguno disponible
     $('#monto').attr({ 'max': 0 });
    $.ajax({
        type: "GET",
        url: '/Pagos/getDatosCheque/',
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

function getDatosAdelanto() {
     //lo pongo en 0 x si no hay ninguno disponible
    $('#monto').attr({ 'max': 0 });
    $.ajax({
        type: "GET",
        url: '/Pagos/getDatosAdelanto/',
        data: { 'idAdelanto': $('#idAdelanto').val() },
        success: function (data) {
            //si el cheque es el mismo q el del cobro original le sumo el pago original           
            if ($('#idAdelanto').val() == $('#adelantoOriginal').val()) {
                data.montoDisponible += parseFloat($('#pagoOriginal').val());
            }           
            $('#montoDisponibleAdelanto').text("$" + data.montoDisponible);           

            if (data.montoDisponible < parseFloat(saldo)) {
                //pongo como maximo el saldo del cheque
                $('#monto').attr({ 'max': data.montoDisponible });
            }
            else {
                //pongo como maximo el saldo q tengo q cobrar
                $('#monto').attr({ 'max': parseFloat(saldo) });
            }

        }
    })
}
getSaldoEgreso();
habilitarPagos();