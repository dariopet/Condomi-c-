function cargarProximosCobros() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/Ingresos/ProximosCobrosPartial/',
        data: { 'tope': $('#fechaTope').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    });
}
cargarProximosCobros();
//# sourceMappingURL=proximosCobros.js.map