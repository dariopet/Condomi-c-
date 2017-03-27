

function cargarProximosPagos() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/Egresos/ProximosPagosPartial/',
        data: { 'tope': $('#fechaTope').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    })
}

cargarProximosPagos();
