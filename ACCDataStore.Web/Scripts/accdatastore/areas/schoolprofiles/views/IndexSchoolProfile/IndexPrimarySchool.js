
$(document).ready(function () {

    $('#listSelectedSchoolname').multiselect({
        buttonWidth: '80%',
        nonSelectedText: '--- School Name ---',
        maxHeight: 200,
        enableFiltering: true,
        numberDisplayed: 3,
        onChange: function (option, checked) {
            // Get selected options.
            var selectedOptions = $('#listSelectedSchoolname option:selected');

            if (selectedOptions.length >= 3) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $('#listSelectedSchoolname option').filter(function () {
                    return !$(this).is(':selected');
                });

                var dropdown = $('listSelectedSchoolname').siblings('.multiselect-container');
                nonSelectedOptions.each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                var dropdown = $('#listSelectedSchoolname').siblings('.multiselect-container');
                $('#listSelectedSchoolname option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
            }
        }
    });

    $('#Ethnicdatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

    $('#Nationalitydatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

    $('#EnglishLevelatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

});

function drawChartColumn(data) {
    $('#divColumnChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: 'Ethnic Background - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Ethnic Background'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Ethnic Background'
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