var dataNationality;
var mNationalParams;
$(function () {
    InitSpinner();
});

$(document).ready(function () {

    $("input[name='nationality']").click(function () {
        $('input[name="CheckNationalityAll"]').prop("checked", false);
    });

    $("input[name='gender']").click(function () {
        $('input[name="CheckGenderAll"]').prop("checked", false);
    });

    $("input[name='years']").click(function () {
        $('input[name="CheckYearAll"]').prop("checked", false);
    });

    $("input[name='CheckNationalityAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="nationality"]').prop("checked", true);
        } else {
            $('input[name="nationality"]').prop("checked", false);
        }
    });

    $("input[name='CheckYearAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="years"]').prop("checked", true);
            //$('input[name="years"]').attr( "disabled", "disabled" );
        } else {
            $('input[name="years"]').prop("checked", false);
        }
    });

    $("input[name='CheckGenderAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="gender"]').prop("checked", true);
        } else {
            $('input[name="gender"]').prop("checked", false);
        }
    });


});

function validateCheckBoxs() {
    // get all checked checkbox
    var arrCheckboxCheckedYear = [];
    $('input[name="years"]:checked').each(function () {
        arrCheckboxCheckedYear.push($(this).val());
    });

    var arrCheckboxCheckedGender = [];
    $('input[name="gender"]:checked').each(function () {
        arrCheckboxCheckedGender.push($(this).val());
    });
    var arrCheckboxCheckedNationality = [];
    $('input[name="nationality"]:checked').each(function () {
        arrCheckboxCheckedNationality.push($(this).val());
    });
    // create 'NationalParams' object as a parameter to controller
    mNationalParams = {
        ListConditionYear: arrCheckboxCheckedYear,
        ListConditionGender: arrCheckboxCheckedGender,
        ListConditionNationality: arrCheckboxCheckedNationality
    };

    if (arrCheckboxCheckedYear.length == 0) {
        alert('Please select Year');
        return false;
    } else if (arrCheckboxCheckedGender.length == 0) {
        alert('Please select Gender');
        return false;
        //getNationalData(mNationalParams);	
    } else if (arrCheckboxCheckedNationality.length == 0) {
        alert('Please select Nationality');
        return false;
    } else {
        return true;
    }

}

function myFunctionBar() {
    var arrCheckboxCheckedCheckDataitem = [];
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {
        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/Nationality/GetChartDataNationality',
            data: JSON.stringify(arrCheckboxCheckedCheckDataitem),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                drawChartBar(data);
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
                            text: 'Ethnic Background - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Deciles'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Decile (Census 2013)'
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

function myFunctionColumn() {
    //alert("myFunctionColumn");
    var arrCheckboxCheckedCheckDataitem = [];
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {

        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/Nationality/GetChartDataNationality',
            data: JSON.stringify(arrCheckboxCheckedCheckDataitem),
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
                            text: 'Nationality - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Deciles'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Decile (Census 2013)'
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
