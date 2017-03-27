

function cargarMovimientosEventos() {
    //ajax function for fetch data    
    $.ajax({
        type: "GET",
        url: '/MovimientosEventos/PorEvento/',
        data: { 'idEvento': $('#cmbEvento').val() },
        success: function (data) {
            $('#contenido').html(data);
        }
    })
}

cargarMovimientosEventos();
