function CargarPrecios() {
    $.ajax({
        type: "GET",
        url: "/ModelosEquipos/getPreciosModelosEquipos",
        data: { 'idModelo': $('#tipoServicio').val() },
        success: function (data) {
            data = JSON.parse(data);
            $.each(data, function (i, val) {
                $('#costoHora').val(val.costoHora);
                $('#costoDia').val(val.costoDia);
                $('#precioHora').val(val.precioHora);
                $('#precioDia').val(val.precioDia);
                //$('#cantidad').val("1");
                //$('#horas').val("0");
                //$('#dias').val("1");
                calcularTotal();
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
}
function calcularTotal() {
    var total;
    total = (parseFloat($('#precioHora').val())) * Number($('#horas').val());
    total += (parseFloat($('#precioDia').val())) * $('#dias').val();
    total *= $('#cantidad').val();
    $('#total').text("Total: $ " + total.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
}
CargarPrecios();
//var Modelos = []
////fetch categories from database
//function CargarModelos(element) {
//    if (Modelos.length == 0) {
//        //ajax function for fetch data
//        $.ajax({
//            type: "GET",
//            url: '/ModelosEquipos/getModelosEquipos',
//            success: function (data) {
//                Modelos = data;
//                //render catagory                
//                renderModelos(element);
//            }
//        })
//    }
//    else {
//        //render modelos to the element
//        renderModelos(element);
//    }
//    CargarPrecios();
//}
//function CargarModelosTexto(element, val, text) {
//    if (Modelos.length == 0) {
//        //ajax function for fetch data
//        $.ajax({
//            type: "GET",
//            url: '/ModelosEquipos/getModelosEquipos',
//            success: function (data) {
//                Modelos = data;
//                //render catagory                
//                renderModelos(element);
//            }
//        })
//    }
//    else {
//        //render modelos to the element
//        renderModelos(element);
//    }    
//    var $ele = $(element);
//    $ele.val(val).text(text);
//}
//function renderModelos(element) {
//    var $ele = $(element);
//    $ele.empty();
//    Modelos = JSON.parse(Modelos.toString());
//    $.each(Modelos, function (i, val) {
//        $ele.append($('<option/>').val(val.idModelo).text(val.descripcion));
//    })
//}
//fetch precios
//# sourceMappingURL=detallePresupuestoDinamico.js.map