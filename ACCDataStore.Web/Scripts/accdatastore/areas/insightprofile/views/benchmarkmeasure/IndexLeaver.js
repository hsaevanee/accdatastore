function Getdestinationbreakdown() {
    var schcode = $('#selectedschoolcode :selected').val();
    var schname = $('#selectedschoolcode :selected').text();
    var year = $('#selectedyear :selected').val();

    var JSONObject = {
        "schcode": schcode,
        "selectedschname": schname,
        "year": year
    }

    if (schcode == '') {
        alert("Please select school");
    } else {

        $.ajax({
            type: 'POST',
            url: sContextPath + 'InsightProfile/BenchmarkMeasure/GetChartLeaverDestinationBreakdown',
            data: JSON.stringify(JSONObject),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                    drawChartColumn(data);           
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
    }

}

function myChartFunction(buttonID, charttype) {
    var schcode = $('#selectedschoolcode :selected').val();
    var schname = $('#selectedschoolcode :selected').text();

    var JSONObject = {
        "schcode": schcode,
        "selectedschname": schname
    }

    if (schcode == '') {
        alert("Please select school");
    } else {

        $.ajax({
            type: 'POST',
            url: sContextPath + 'InsightProfile/BenchmarkMeasure/GetChartLeaverDestination',
            data: JSON.stringify(JSONObject),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (buttonID == 1){
                    drawChartBar(data);
                }                    
                else if (buttonID == 2) {
                    drawChartColumn(data);
                }
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
    }

}

function drawChartBar(data) {
    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'bar'
                        },
                        title: {
                            text: data.ChartTitle + ' Leaver Initial Destination'
                        },
                        subtitle: {
                            text: 'Increasing post-school participation <br> Percentage od School Leavers in a Positive Destination'
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: ''
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Percentage'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td nowrap style="color:{series.color};padding:0">{series.name}: </td>'
                                    + '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: data.ChartSeries,
                        credits: {
                            enabled: false
                        }
                    });
}

function drawChartColumn(data) {
    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: data.ChartTitle +' Leaver Initial Destination'
                        },
                        subtitle: {
                            text: 'Increasing post-school participation <br> Percentage od School Leavers in a Positive Destination'
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: ''
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Percentage'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td nowrap style="color:{series.color};padding:0">{series.name}: </td>'
                                    + '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: data.ChartSeries,
                        credits: {
                            enabled: false
                        }
                    });
}