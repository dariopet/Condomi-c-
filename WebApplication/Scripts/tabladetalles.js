$(function () {
    var mTable = $('#tabladetalles').DataTable({
        dom: 'Bfrtip', buttons: [

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
                         extend: 'colvis',
                         columns: ':not(:first-child)'
                     }
        ],
        responsive: true,
        "autoWidth": true,
        language: {
            url: '../../AdminLTE/plugins/datatables/es.json'            
        },
        "columnDefs": [
    { responsivePriority: 1, targets: 1 },
            { responsivePriority: 1, targets: -1 }
        ]
    })
   
});