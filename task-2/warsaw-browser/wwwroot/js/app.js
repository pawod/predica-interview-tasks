function loadRemoteData() {
    $('#btn-load > span').show();
    $('#btn-load').prop('disabled', true)
    $('#btn-load').html($('#btn-load').html().replace("Load Remote Data", "Loading ..."));

    $.get({
        url: "/Home/LoadRemoteData/",
        success: function () {
            $('#btn-load > span').hide();
            $('#btn-load').html($('#btn-load').html().replace("Loading ...", "Load Remote Data"));
            $('#btn-load').prop('disabled', false)
            $('.alert').fadeOut();
            return loadTable();
        },
        error: function () {
            $('.alert').fadeIn();
            $('#btn-load > span').hide();
            $('#btn-load').html($('#btn-load').html().replace("Loading ...", "Load Remote Data"));
            $('#btn-load').prop('disabled', false)
        }
    });
}

function loadTable() {
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
        searching: true,
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
                name: "co"
            },
            {
                title: "Category",
                data: "Category",
                name: "co"
            },
            {
                title: "SubCategory",
                data: "SubCategory",
                name: "co"
            },
            {
                title: "NotificationType",
                data: "NotificationType",
                name: "co"
            },
            {
                title: "Source",
                data: "Source",
                name: "co"
            },
            {
                title: "Event",
                data: "Event",
                name: "co"
            },
            {
                title: "CreateDate",
                data: "CreateDate",
                render: function (data, type, row) {
                    return window.moment(row.CreateDate).format("DD/MM/YYYY");
                },
                name: "eq"
            },
        ]
    });
}