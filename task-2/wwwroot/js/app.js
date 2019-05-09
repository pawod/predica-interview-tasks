"use strict";


function loadRemoteData()
{
    $('#btn-load > span').show(); 
    $('#btn-load').addClass('disabled');
    $('#btn-load').html($('#btn-load').html().replace("Load Remote Data","Loading...")); 

    $.get({
        url: "/Home/LoadRemoteData/",
        success: function () {
            $('#btn-load > span').hide();
            $('#btn-load').html($('#btn-load').html().replace("Loading...","Load Remote Data"));
            return loadTable();
        },
        error: function(){
            $('#notifTable').DataTable()
        }
    });
}

function loadTable()
{
    var table = $('#notifTable').DataTable({
        language: {
            processing: "Loading Data...",
            zeroRecords: "No matching records found"
        },
        processing: true,
        serverSide: true,
        // orderCellsTop: true,
        autoWidth: true,
        paging: true,
        pagingType: "simple",
        searching: false,
        // deferRender: true,
        // dom: '<tr>',
        ajax: {
            type: "POST",
            url: '/Home/LoadTable/',
            contentType: "application/json; charset=utf-8",
            async: true,
            data: function (data) {
                return JSON.stringify(data);
            }
        },
        columns: [
            {
                title: "District",
                data: "District",
                name: "eq"
            },
            {
                title: "Category",
                data: "Category",
                name: "eq"
            },
            {
                title: "SubCategory",
                data: "SubCategory",
                name: "eq"
            },
            {
                title: "NotificationType",
                data: "NotificationType",
                name: "lte"
            },
            {
                title: "Source",
                data: "Source",
                name: "lte"
            },
            {
                title: "Event",
                data: "Event",
                name: "eq"
            },
            {
                title: "CreateDate",
                data: "CreateDate",
                render: function (data, type, row) {
                    return window.moment(row.CreateDate).format("DD/MM/YYYY");
                },
                name: "gt"
            },
        ]
    });
}