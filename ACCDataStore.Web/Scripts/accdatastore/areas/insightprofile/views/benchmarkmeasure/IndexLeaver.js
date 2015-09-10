
function myFunctionColumn() {
    //alert("myFunctionColumn");
    var schcode = $('#selectedschoolcode :selected').val();
    var schname = $('#selectedschoolcode :selected').text();
    var syear = $('#selectedyear :selected').val();

    var arrCheckboxCheckedgender = [];

    var JSONObject = {
        "schcode": schcode,
        "selectedschname": schname,
        "year": syear
        //"year": syear
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

function drawChartColumn(data) {
    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: 'Leaver Initial Destination Breakdown by destication'
                        },
                        subtitle: {
                            text: 'Increasing participation'
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
                                text: '% Pupils'
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