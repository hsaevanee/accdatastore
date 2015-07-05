﻿var mSchCriteriaParams, mdataParams;
var dataNationality;

$(function () {
    InitSpinner();
});

$(document).ready(function () {

    $('#buttonGetData').click(function () {
        if (validateCheckBoxs() == true && validateDropdownlist() == true) {
            document.forms[0].submit();
        }

    });


    $("input[name='subject']").click(function () {
        $('input[name="CheckStageAll"]').prop("checked", false);
    });

    $("input[name='CheckSubjectAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="subject"]').prop("checked", true);
        } else {
            $('input[name="subject"]').prop("checked", false);
        }
    });

    $("input[name='gender']").click(function () {
        $('input[name="CheckGenderAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitem']").click(function () {
        $('input[name="CheckDataitemAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitemAll']").change(function () {
        if (this.checked) {
            $('input[name="CheckDataitem"]').prop("checked", true);
        } else {
            $('input[name="CheckDataitem"]').prop("checked", false);
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

function validateDropdownlist() {
    var value1 = $('#selectedschoolname :selected').text();
    //var value2 = $('#selectedschoolname2 :selected').text();

    if (value1 == "---Please Select School---") {
        alert('Please select School');
        return false;
    } else {
        return true;
    }

}

function validateCheckBoxs() {
    // get all checked checkbox
    var arrCheckboxCheckedStage = [];
    $('input[name="subject"]:checked').each(function () {
        arrCheckboxCheckedStage.push($(this).val());
    });

    if (arrCheckboxCheckedStage.length == 0) {
        alert('Please select Subject');
        return false;
    } else {
        return true;
    }

}


function myFunctionBar(buttonID, dataname) {

    var arrCheckboxCheckedCheckDataitem = [];
    if (buttonID == 1) {
        $('input[name="CheckDataitem1"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    } else if (buttonID == 2) {
        $('input[name="CheckDataitem2"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    }else if (buttonID == 3) {
    $('input[name="CheckDataitem3"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });
    }else if (buttonID == 4) {
        $('input[name="CheckDataitem4"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    }else if (buttonID == 5) {
        $('input[name="CheckDataitem5"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    }else if (buttonID == 6) {
        $('input[name="CheckDataitem6"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    }else if (buttonID == 7) {
        $('input[name="CheckDataitem7"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    } else if (buttonID == 8) {
        $('input[name="CheckDataitem8"]:checked').each(function () {
            arrCheckboxCheckedCheckDataitem.push($(this).val());
        });
    }
    


    //var JSONObject = {
    //    "dataname": dataname,
    //    "arrCheckboxCheckedCheckDataitem":arrCheckboxCheckedCheckDataitem
    //}

    var JSONObject = {
        "dataname": dataname,
        "indexDataitem":arrCheckboxCheckedCheckDataitem
    }

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {
        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/Curriculum/GetChartDataCurriculum',
            data: JSON.stringify(JSONObject),
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
                            text: 'Student Stage - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Stages'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Stage'
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
            url: sContextPath + 'SchoolProfile/StudentStage/GetChartDataStudentStage',
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
                            text: 'Student Stage - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Stages'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Stage'
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


