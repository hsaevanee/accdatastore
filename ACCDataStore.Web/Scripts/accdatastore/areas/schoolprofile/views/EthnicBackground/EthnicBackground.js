var mSchCriteriaParams, mdataParams;
var dataNationality;

$(function () {
    InitSpinner();
});

$(document).ready(function () {

    $("input[name='ethnicity']").click(function () {
        $('input[name="CheckEthnicityAll"]').prop("checked", false);
    });

    $("input[name='CheckEthnicityAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="ethnicity"]').prop("checked", true);
        } else {
            $('input[name="ethnicity"]').prop("checked", false);
        }
    });

    $("input[name='CheckDataitem']").click(function () {
        $('input[name="CheckDataitemAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitemAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="CheckDataitem"]').prop("checked", true);
        } else {
            $('input[name="CheckDataitem"]').prop("checked", false);
        }
    });

});

function validateCheckBoxs() {
    // get all checked checkbox
    var arrCheckboxCheckedEthnic = [];
    $('input[name="ethnicity"]:checked').each(function () {
        arrCheckboxCheckedEthnic.push($(this).val());
    });

    var sSchoolNameText = $('#selectSchoolname option:selected').text();

    mSchCriteriaParams = {
        selectedschoolname: sSchoolNameText,
        ListConditionEthnicbg: arrCheckboxCheckedEthnic,        
    };

    if (arrCheckboxCheckedEthnic.length == 0) {
        alert('Please select EthnicBackground');
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
            url: sContextPath + 'SchoolProfile/EthnicBackground/GetChartDataEthnic',
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
            url: sContextPath + 'SchoolProfile/EthnicBackground/GetChartDataEthnic',
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

function GenerateExcelFile() {
    JSONToExcelConvertor(dataNationality, "Nationality Report", true);
}

function JSONToExcelConvertor(JSONData, ReportTitle, ShowLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = '';
    //Set Report title in first row or line

    CSV += ReportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);

        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "MyNationalityReport_";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function GenerateCSVFile() {
    JSONToCSVConvertor(dataNationality, "Nationality Report", true);
}