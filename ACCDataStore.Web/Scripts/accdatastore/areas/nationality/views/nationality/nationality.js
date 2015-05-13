var dataNationality;
var mNationalParams;
$(function () {
    $('#buttonGetData').click(function () {
        if (validateCheckBoxs() == true) {
            getNationalData(mNationalParams);
        }

    });
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

function getNationalData(mNationalParams) {
    $.ajax({
        type: 'POST',
        url: sContextPath + 'Nationality/Nationality/GetNationalityData',
        data: JSON.stringify(mNationalParams),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            // controller return 'NationalData' object in json format, add more properties as you want..
            // using jquery to create html table
            dataNationality = data;
            setNationalDataToTable(data, mNationalParams);
            //drawChart(data);
        },
        error: function (xhr, err) {
            if (xhr.readyState != 0 && xhr.status != 0) {
                alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                alert('responseText: ' + xhr.responseText);
            }
        },
        async: false
    });
}

function setNationalDataToTable(listNationalData, mNationalParams) {
    // remove all existing header & row
    //$("#tableNationalData").html("");

    // create header

    var sTableHtml = "<thead><tr><th rowspan='2'>Nationality</th>";
    for (var j = 0, maxj = mNationalParams.ListConditionYear.length ; j < maxj; j++) {
        sTableHtml += ("<th colspan='" + mNationalParams.ListConditionGender.length + "'>" + mNationalParams.ListConditionYear[j] + "</th>");
    }

    sTableHtml += "</tr><tr>";
    for (var i = 0, maxi = mNationalParams.ListConditionYear.length ; i < maxi; i++) {
        for (var j = 0, maxj = (mNationalParams.ListConditionGender.length) ; j < maxj; j++) {
            if (mNationalParams.ListConditionGender[j] == "F") {
                sTableHtml += "<th>Female</th>";
            } else if (mNationalParams.ListConditionGender[j] == "M") {
                sTableHtml += "<th>Male</th>";
            } else {
                sTableHtml += "<th>Total</th>";
            }

        }
    }
    sTableHtml += "<th>Select data to Create Chart</th>";
    sTableHtml += "</tr></thead>";

    // create row data
    sTableHtml += "<tbody>";
    for (var i = 0, maxi = mNationalParams.ListConditionNationality.length; i < maxi; i++) {
        var temp = mNationalParams.ListConditionNationality[i];
        //alert('Value' + listNationalData[i].name);
        if (temp == "01") {
            sTableHtml += ("<tr><td> Scottish </td>");
        } else if (temp == "02") {
            sTableHtml += ("<tr><td> English </td>");
        } else if (temp == "03") {
            sTableHtml += ("<tr><td> Northern Irish </td>");
        } else if (temp == "04") {
            sTableHtml += ("<tr><td> Welsh </td>");
        } else if (temp == "05") {
            sTableHtml += ("<tr><td> British </td>");
        } else if (temp == "10") {
            sTableHtml += ("<tr><td> Other </td>");
        } else if (temp == "98") {
            sTableHtml += ("<tr><td> Not Disclosed </td>");
        } else if (temp == "99") {
            sTableHtml += ("<tr><td> Not Known </td>");
        }

        for (var j = 0, maxj = listNationalData.length; j < maxj; j++) {
            //alert('Value' + listNationalData[j].data.length);
            sTableHtml += ("<td>" + listNationalData[j].data[i] + "</td>");
        }
        sTableHtml += ("<td><input type='checkbox' name='CheckDataitem' id='item' value='" + i + "'/></td>");
        sTableHtml += "</tr>";

    }
    sTableHtml += ("<tr><td><button onclick='myFunctionBar()'><label>Bar Chart</label></button></td></tr>");
    sTableHtml += ("<tr><td><button onclick='myFunctionColumn()'><label>Column Chart</label></button></td></tr>");
    sTableHtml += ("<tr><td><button onclick='GenerateExcelFile()'><label>Generate CSV File</label></button></td></tr>");
    sTableHtml += "</tbody></table>";
    sTableHtml += "</tbody>";
    $("#tableNationalDataContainer").html(sTableHtml);
}

function myFunctionBar() {
    alert("myFunctionBar");
    var arrCheckboxCheckedCheckDataitem = {};
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem[$(this).val()] = $(this).val();
    });

    var arrDataNationalityChart = [];
    $(dataNationality).each(function (index, item) {
        var copiedChartData = {};
        jQuery.extend(copiedChartData, item);
        arrDataNationalityChart.push(copiedChartData);
    });

    $(dataNationality).each(function (index, item) {
        arrDataNationalityChart[index].data = [];
        $(item.data).each(function (index2, item2) {
            if (arrCheckboxCheckedCheckDataitem[index2] != null) {
                arrDataNationalityChart[index].data.push(item2);
            }
        });
    });
    var categoriesData = [];

    for (var key in arrCheckboxCheckedCheckDataitem) {
        if (mNationalParams.ListConditionNationality[key] == "01") {
            categoriesData.push("Scottish");
        } else if (mNationalParams.ListConditionNationality[key] == "02") {
            categoriesData.push("English");
        } else if (mNationalParams.ListConditionNationality[key] == "03") {
            categoriesData.push("Northern Irish");
        } else if (mNationalParams.ListConditionNationality[key] == "04") {
            categoriesData.push("Welsh");
        } else if (mNationalParams.ListConditionNationality[key] == "05") {
            categoriesData.push("British");
        } else if (mNationalParams.ListConditionNationality[key] == "10") {
            categoriesData.push("Other");
        } else if (mNationalParams.ListConditionNationality[key] == "98") {
            categoriesData.push("Not Disclosed");
        } else if (mNationalParams.ListConditionNationality[key] == "99") {
            categoriesData.push("Not known");
        }
    }

    drawChartBar(arrDataNationalityChart, categoriesData);

}

function drawChartBar(data, categoriesData) {
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
                            categories: categoriesData,
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
                        series: data,
                        credits: {
                            enabled: false
                        }
                    });
}

function myFunctionColumn() {
    alert("myFunctionBar");

    var arrCheckboxCheckedCheckDataitem = {};
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem[$(this).val()] = $(this).val();
    });

    var arrDataNationalityChart = [];
    $(dataNationality).each(function (index, item) {
        var copiedChartData = {};
        jQuery.extend(copiedChartData, item);
        arrDataNationalityChart.push(copiedChartData);
    });

    $(dataNationality).each(function (index, item) {
        arrDataNationalityChart[index].data = [];
        $(item.data).each(function (index2, item2) {
            if (arrCheckboxCheckedCheckDataitem[index2] != null) {
                arrDataNationalityChart[index].data.push(item2);
            }
        });
    });
    var categoriesData = [];

    for (var key in arrCheckboxCheckedCheckDataitem) {
        if (mNationalParams.ListConditionNationality[key] == "01") {
            categoriesData.push("Scottish");
        } else if (mNationalParams.ListConditionNationality[key] == "02") {
            categoriesData.push("English");
        } else if (mNationalParams.ListConditionNationality[key] == "03") {
            categoriesData.push("Northern Irish");
        } else if (mNationalParams.ListConditionNationality[key] == "04") {
            categoriesData.push("Welsh");
        } else if (mNationalParams.ListConditionNationality[key] == "05") {
            categoriesData.push("British");
        } else if (mNationalParams.ListConditionNationality[key] == "10") {
            categoriesData.push("Other");
        } else if (mNationalParams.ListConditionNationality[key] == "98") {
            categoriesData.push("Not Disclosed");
        } else if (mNationalParams.ListConditionNationality[key] == "99") {
            categoriesData.push("Not known");
        }
    }

    drawChartColumn(arrDataNationalityChart, categoriesData);

}

function drawChartColumn(data, categoriesData) {
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
                            categories: categoriesData,
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
                        series: data,
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
