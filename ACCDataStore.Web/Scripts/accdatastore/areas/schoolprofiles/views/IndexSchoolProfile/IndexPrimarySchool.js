﻿
$(document).ready(function () {

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
                'copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'portrait' }, 'print',
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
                'copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'portrait' }, 'print',
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
            'copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'landscape' }, 'print',
        ]}
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
                'copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'landscape' }, 'print',
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
                'copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'landscape' }, 'print',
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
    json.subtitle = subtitle;

    var chart = $('#EnglishLevelGraphContainer').highcharts(json, function(chart)
    {
        //remove English as a \"first-language\" data from graphs
        //console.log(chart)
        //var len = this.series.length
        var newCategories = [];
        //remove point in dataseries
        for (i = 0; i < this.series.length; i++) {
            //var datalen = this.series[i].data.length
            for (j = 0; j < this.series[i].data.length; j++) {
                if (this.series[i].data[j].name == "English as a \"first-language\"") {
                    removeindex = j;
                    this.series[i].data[j].remove();
                    break
                }
            }
        }

       // create new catagories for label in X-line
        for (j = 0; j < this.series[0].data.length; j++) {
            newCategories.push(this.series[0].data[j].name);
        }
        this.xAxis[0].setCategories(newCategories, true); // set new x label
        this.xAxis[0].isDirty = true;
        this.redraw();
    }
    );

}

