
$(document).ready(function () {


    $("#selectedschoolcode").change(function () {    
        document.forms[0].submit();
        myFunctionColumn()

    });

});

function myFunctionColumn() {
    $.ajax({
        type: 'POST',
        url: sContextPath + 'DatahubProfile/IndexDatahub/GetChartDataforMap',
        data: JSON.stringify(),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            drawChartColumn(data, pdata.dataTitle);
        },
        error: function (xhr, err) {
            if (xhr.readyState != 0 && xhr.status != 0) {
                alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                alert('responseText: ' + xhr.responseText);
            }
        }
    });


}

function drawChartColumn(data, sCondition) {

    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: data.ChartTitle
                        },
                        subtitle: {
                            text: sCondition
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Nationality'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Nationality'
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