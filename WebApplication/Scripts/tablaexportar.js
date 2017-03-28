var datepickerDefaults = {
    autoclose: true  
};

//si no estan los valores ocultos, lo mando a la columna 0 q esta oculta, sino cargo los filtros donde van
if (!$('#fecha1').length) { fecha1 = 0; }
else { var fecha1 = $('#fecha1').val(); }

if (!$('#fecha2').length) { fecha2 = 0;}
else { var fecha2 = $('#fecha2').val(); }


if (!$('#suma1').length) { suma1 = 0; }
else { var suma1 = $('#suma1').val(); }

if (!$('#suma2').length) { suma2 = 0; }
else { var suma2 = $('#suma2').val(); }

if (!$('#suma3').length) { suma3 = 0; }
else { var suma3 = $('#suma3').val(); }

if (!$('#suma4').length) { suma4 = 0; }
else { var suma4 = $('#suma4').val(); }

if (!$('#texto1').length) { texto1 = 0; }
else { var texto1 = $('#texto1').val(); }

if (!$('#texto2').length) { texto2 = 0; }
else { var texto2 = $('#texto2').val(); }


$(function () {

    var dTable = $('#tabla').dataTable({
        dom: 'Bfrtip',
        buttons: [
           
                     {
                         extend: 'excelHtml5',
                         exportOptions: {
                             columns: [':not(:first):not(:last)']
                         },
                         text: '<i class="fa fa-file-excel-o"></i>',
                         title: 'Condomi',
                         titleAttr: 'Excel'
                     },

                     {

                         extend: 'pdfHtml5',
                         exportOptions: {
                             columns: [':not(:first):not(:last)']
                         },

                         text: '<i class="fa fa-file-pdf-o"></i>',
                         title: 'Condomi',
                         titleAttr: 'PDF'
                     },
                     {
                         extend: 'print',
                         exportOptions: {
                             columns: [':not(:first):not(:last)']
                         },
                         text: '<i class="fa fa-print"></i>',
                         titleAttr: 'Imprimir',
                         title: 'Condomi'
                     },
                     {
                         extend: 'print',
                         text: '<i class="fa fa-print"></i> Seleccionado',
                         exportOptions: {
                             modifier: {
                                 selected: true
                             }
                         }
                     },
                    'pageLength',
                     {
                         extend: 'colvis',
                         columns: ':not(:first-child)'
                     }
        ],
        select: true,
        "autoWidth": false,
        responsive: true,
        language: {
            url: '../../AdminLTE/plugins/datatables/es.json',
            buttons: {
                pageLength: { '-1': 'Mostrar todas las filas', _: 'Mostrar %d filas' }
            }
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData[0] == "0") {
                $(nRow).css('color', 'grey').css('font-style', 'italic').css('text-decoration', 'line-through');

            }

        },

        //oculto la columna 0 (activo)

        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": true
            },
             //pongo prioridad para la columna 1 y la ultima de las acciones
            { responsivePriority: 1, targets: 1 },
            { responsivePriority: 1, targets: -1 }
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            // Remove the formatting to get integer data for summation
            var intVal = function (i) {                
                return typeof i === 'string' ?                    
                    i.replace(/[^\d.-]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };

            if (suma1 != 0) {
                // SUMA1
                pageTotal = api
                    .column(parseInt(suma1), { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        if (typeof a === 'string') {
                            a = a.substring(a.indexOf("$"), a.length-1);
                            a = a.replace("\\w", "");
                            a = a.replace(".", "");
                            a = a.replace(",", ".");
                            a = a.replace("$", "");                            
                            a = a.replace(/[^\d.-]/g, '') * 1;
                        }
                        if (typeof b === 'string') {
                            b = b.substring(b.indexOf("$"), b.length-1);
                            b = b.replace("\\w", "");
                            b = b.replace(".", "");
                            b = b.replace(",", ".");
                            b = b.replace("$", "");
                            b = b.replace(/[^\d.-]/g, '') * 1;
                        }
                        return a + b;
                    }, 0);
                // Update footer

                $(api.column(parseInt(suma1)).footer()).html(
                    '$' + pageTotal.toLocaleString('es-ES', {minimumFractionDigits: 2, maximumFractionDigits:2, maximumFractionDigits:2 }));
            }
            if (suma2 != 0) {
                // SUMA2
                pageTotal = api
                    .column(parseInt(suma2), { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        if (typeof a === 'string') {
                            a = a.substring(a.indexOf("$"), a.length - 1);
                            a = a.replace("\\w", "");
                            a = a.replace(".", "");
                            a = a.replace(",", ".");
                            a = a.replace("$", "");
                            a = a.replace(/[^\d.-]/g, '') * 1;
                        }
                        if (typeof b === 'string') {
                            b = b.substring(b.indexOf("$"), b.length - 1);
                            b = b.replace("\\w", "");
                            b = b.replace(".", "");
                            b = b.replace(",", ".");
                            b = b.replace("$", "");
                            b = b.replace(/[^\d.-]/g, '') * 1;
                        }
                        return a + b;
                    }, 0);
                // Update footer
                $(api.column(parseInt(suma2)).footer()).html(
                     '$' + pageTotal.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
            }
            if (suma3 != 0) {
                // SUMA3
                pageTotal = api
                    .column(parseInt(suma3), { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        if (typeof a === 'string') {
                            a = a.substring(a.indexOf("$"), a.length - 1);
                            a = a.replace("\\w", "");
                            a = a.replace(".", "");
                            a = a.replace(",", ".");
                            a = a.replace("$", "");
                            a = a.replace(/[^\d.-]/g, '') * 1;
                        }
                        if (typeof b === 'string') {
                            b = b.substring(b.indexOf("$"), b.length - 1);
                            b = b.replace("\\w", "");
                            b = b.replace(".", "");
                            b = b.replace(",", ".");
                            b = b.replace("$", "");
                            b = b.replace(/[^\d.-]/g, '') * 1;
                        }
                        return a + b;
                    }, 0);
                // Update footer
                $(api.column(parseInt(suma3)).footer()).html(
                      '$' + pageTotal.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
            }
            if (suma4 != 0) {
                // SUMA4
                pageTotal = api
                    .column(parseInt(suma4), { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        if (typeof a === 'string') {
                            a = a.substring(a.indexOf("$"), a.length - 1);
                            a = a.replace("\\w", "");
                            a = a.replace(".", "");
                            a = a.replace(",", ".");
                            a = a.replace("$", "");
                            a = a.replace(/[^\d.-]/g, '') * 1;
                        }
                        if (typeof b === 'string') {
                            b = b.substring(b.indexOf("$"), b.length - 1);
                            b = b.replace("\\w", "");
                            b = b.replace(".", "");
                            b = b.replace(",", ".");
                            b = b.replace("$", "");
                            b = b.replace(/[^\d.-]/g, '') * 1;
                        }
                        return a + b;
                    }, 0);
                // Update footer
                $(api.column(parseInt(suma4)).footer()).html(
                     '$' + pageTotal.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits:2 }));
            }
        }
    })
        .yadcf([

            {                
                column_number: parseInt(texto1),
                filter_type: "text"              
                   
            },
            {
      
                column_number: parseInt(texto2),
                filter_type: "text"               

            },
   {
      
       column_number: parseInt(fecha1),
       filter_type: "range_date",
       date_format: 'dd/mm/yyyy',

   },
    {
        column_number: parseInt(fecha2),
        filter_type: "range_date",       
        date_format: 'dd/mm/yyyy',       
    
    }])
    ;
    //programo el checkbox
    dTable.fnFilter("1", 0, false, false);
    $("#cbAnulados").click(function () {
        if (!$("#cbAnulados").is(":checked")) {
            dTable.fnFilter("1", 0, false, false);
        } else {
            dTable.fnFilter("", 0, false, false);
        }
    });
});


