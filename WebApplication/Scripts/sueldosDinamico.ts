var adelantos;
var horas;

function getDatosSueldo(accion) {
    //ajax function for fetch data  
    $.ajax({
        type: "GET",
        url: '/Sueldos/getDatosSueldo/',
        data: { 'idTercero': $('#empleados').val(), 'periodo': $('#periodo').val() },
        success: function (data) {             
            $('#horasNoTrabajadas').text(data.horasNoTrabajadas);
            $('#cobradoAnterior').val(data.cobradoAnterior);
            adelantos = parseFloat(data.totalAdelantos);
            adelantos -= (parseFloat($('#pagoOriginal').val()));
            $('#adelantos').text("$" + adelantos.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
            if (accion == "create")
            { $('#base').val(data.ultimoPago); }
            getPrecioHora();
        }
    })
}

function getPrecioHora() {
    //ajax function for fetch data   
    $.ajax({
        type: "GET",
        url: '/Sueldos/getPrecioHora/',
        data: { 'sueldo': $('#base').val(), 'periodo': $('#periodo').val() },
        success: function (data) {
            $('#valorhora').val(data.precioHora);           
            calcularSueldo();
        }
    })
}

function calcularSueldo() {
    var aPagar;
    var base = (parseFloat($('#base').val()));     
    var valorhora = (parseFloat($('#valorhora').val()));
    aPagar = base - (valorhora * parseInt($('#horasNoTrabajadas').text())) -adelantos;   
    $('#aPagar').text("Saldo a Pagar: $" + aPagar.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
}

