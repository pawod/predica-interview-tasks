function loadRemoteData() {
    $('#btn-load > span').show();
    $('#btn-load').prop('disabled', true)
    $('#btn-load').html($('#btn-load').html().replace("Load Remote Data", "Loading ..."));

    var dateFrom = new Date($("#dateFrom").datetimepicker('date')).getTime();
    var dateTo = new Date($("#dateTo").datetimepicker('date')).getTime();
    var url = `/Home/LoadRemoteData?dateFrom=${dateFrom}&dateTo=${dateTo}`

    $.get({
        url: url,
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
        lengthMenu: [[5, 10, 25, 50, 100], [5, 10, 25, 50, 100]],
        processing: true,
        serverSide: true,
        orderCellsTop: true,
        paging: true,
        pagingType: "full_numbers",
        searching: false,
        deferRender: true,
        ajax: {
            type: "POST",
            url: "/Home/LoadTable/",
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
                name: "eq"
            },
            {
                title: "Source",
                data: "Source",
                name: "eq"
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
                    return window.moment(row.CreateDate).format("DD/MM/YYYY HH:mm");
                },
                name: "gt"
            },
        ]
    });
}

$('#dateFrom').datetimepicker({
    icons: {
        time: 'far fa-clock',
        date: 'far fa-calendar',
        up: 'fas fa-arrow-up',
        down: 'fas fa-arrow-down',
        previous: 'fas fa-chevron-left',
        next: 'fas fa-chevron-right',
        today: 'far fa-calendar-check-o',
        clear: 'far fa-trash',
        close: 'far fa-times'
    },
    format: 'DD.MM.YYYY HH:mm',
    defaultDate: moment().subtract(1, 'days')
    }
);

var p2 = $('#dateTo').datetimepicker({
    icons: {
        time: 'far fa-clock',
        date: 'far fa-calendar',
        up: 'fas fa-arrow-up',
        down: 'fas fa-arrow-down',
        previous: 'fas fa-chevron-left',
        next: 'fas fa-chevron-right',
        today: 'far fa-calendar-check-o',
        clear: 'far fa-trash',
        close: 'far fa-times'
    },
    format: 'DD.MM.YYYY HH:mm',
    //buttons: {
    //    showToday: true,
    //    showClear: true,
    //    showClose: true
    //},
    defaultDate: new Date($.now())
    }
);