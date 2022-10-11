$(document).ready(function () {
    $('#Products').dataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/ApiProducts",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],





        "columns": [
            
            { "data": "photo", "name": "Photo", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "description", "name": "Description", "autowidth": true },
            { "data": "price", "name": "Price", "autowidth": true },
            { "data": "color", "name": "Color", "autowidth": true },
            { "data": "quantity", "name": "Quantity", "autowidth": true },
            
            
            
            
        ]
    });
});