
$(document).ready(function () {


    $('#AttendanceDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Attendance ' + year
                }, 'print',
            ]
        }
    });


    $('#ExclusionDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Exclusions-Annual ' + year
                }, 'print',
            ]
        }
    });

    $('#FreeMealDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Free School Meal Entitlement ' + year
                }, 'print',
            ]
        }
    });

    $('#PIPSDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'PIPS Baseline P1 for ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayDTTableV').DataTable({
        dom: 'Bfrtip',
        "scrollY": "200px",
        "scrollCollapse": true,
        "paging": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'portrait' }, 'print',
            ]
        }
    });

    $('#LookedAfterDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Looked After Children ' + year
                }, 'print',
            ]
        }
    });

    $('#StageDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'School Roll Year Groups ' + year
                }, 'print',
            ]
        }
    });

    $('#Ethnicdatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [ {
            "targets"  : 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Ethnicity for ' + year
                }, 'print',
            ]
        }
    });

    $('#Nationalitydatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Nationality for ' + year
                }, 'print',
            ]
        }
    });

    $('#EnglishLevelDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'English Level for ' + year
                }, 'print',
            ]
        }
    })

    //DrawTempGraph();
    DrawEnglishLevelGraph();
    DrawNationlityGraph();


});

function DrawTempGraph() {
    var options = {
        chart: {
            renderTo: 'TempGraphContainer',
            type: 'spline'
        },
        series: [{}]
    };

    $.getJSON('jsonDataTable', function (data) {
        options.series[0].data = data;
        var chart = new Highcharts.Chart(options);
    });

}

function requestData() {
    $.ajax({
        url: 'api/v1/dashboard/month_mention_graphic',
        type: "GET",
        dataType: "json",
        data: { username: "demo" },
        success: function (data) {
            chart.addSeries({
                name: "mentions",
                data: data.month_mentions_graphic
            });
        },
        cache: false
    });
}


function DrawNationlityGraph() {
    var data = {
        table: 'Nationalitydatatable',
        switchRowsAndColumns: true

    };
    var chart = {
        type: 'column'
    };
    var title = {
        text: 'Nationality'
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: 'Percentage (%)'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ' + this.point.name.toLowerCase();
        }
    };
    var credits = {
        enabled: false
    };

    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;
    
    $('#NationalityGraphContainer').highcharts(json);

}


function DrawEnglishLevelGraph() {
    var data = {
        table: 'EnglishLevelDatatable',
        switchRowsAndColumns: true
    };
    var chart = {
        type: 'column'
    };
    var title = {
        text: 'Level of English'
    };
    var subtitle = {
        text: 'Graph excluded "English as a first-language" data',
        x: -20
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: 'Percentage (%)'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ' ;
        }
    };

    var credits = {
        enabled: false
    };

    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;
    json.subtitle = subtitle;

    var chart = $('#EnglishLevelGraphContainer').highcharts(json);

}

