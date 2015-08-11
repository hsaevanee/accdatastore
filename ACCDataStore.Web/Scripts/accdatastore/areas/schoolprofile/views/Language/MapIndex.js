var map; // map object

// initialize all kml layers
var kml = {
    // type : 0 = direct kml file, 1 = kml from fusion table
    //a: {
    //    name: "Aberdeen PostCode",
    //    type: 1,
    //    query: {
    //        select: "col4",
    //        from: "1FDKdWdKSXGkpQmvvMxVuxkBUmXlIuAqbuGpKpugM",
    //        where: ""
    //    }
    //},
    a: {
        name: "Aberdeen DataZone Districts",
        type: 0,
        url: 'https://dl.dropboxusercontent.com/u/55734762/Datazone_with_Desc.kml' + "?rand=" + (new Date()).valueOf(),
        dataType : 0
    },
    b: {
        name: "Primary Schools Locations",
        type: 0,
        url: 'https://dl.dropboxusercontent.com/u/55734762/PrimarySchoollocations_with_Desc.kml' + "?rand=" + (new Date()).valueOf(),
        dataType: 1
    }
};

// on document ready
$(function () {
    // binding 'close' click event to popup windows
    $(document.body).on('click', '.a-close-popup-information', function () {
        $("#popup-information").hide(250);
    });
    InitSpinner();
    InitMap();
});

// initialize spinner on ajax loading
function InitSpinner() {
    $(document).ajaxSend(function () {
        $('#divSpinner').show();
    }).ajaxComplete(function () {
        $('#divSpinner').hide();
    }).ajaxError(function (e, xhr) {
        // do something on ajax error
    });
}

// initialize map object
function InitMap() {
    var mapCenter = new google.maps.LatLng(57.151810, -2.094451);
    var mapOptions = {
        zoom: 9,
        center: mapCenter
    }

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    CreateLayerControl();
}

// show search result when click on map
function ShowPopupInformation(sInformation) {
    //var popupInformation = document.getElementById('popup-information');
    //popupInformation.innerHTML = sInformation;
    //$("#popup-information").show(250);
    $("#divinformationContainer").html(sInformation);
}



// call server side method via ajax
function SearchData(sCondition,sKeyname) {
    //var param = JSON.stringify({ 'sCondition': sCondition }); // just an example, need to adjust
    var JSONObject = {
        "keyvalue": sCondition,
        "keyname": sKeyname
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "SchoolProfile/Language/SearchByName",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowPopupInfo(data);
            myFunctionColumn(data);
                
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

function myFunctionColumn(pdata) {
        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/Language/GetChartDataLanguageforMap',
            data: JSON.stringify(pdata.dataSeries),
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
                                text: 'Level of English'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Level'
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

function ShowPopupInfo(data) {
    //var sInformation = "<a href='#' class='a-close-popup-information'>Close</a><h3>" + sName + "</h3>";
    var sInformation = "<h3 align='center'> Level of English - " + data.dataTitle + "</h3>";
    sInformation += "<table class='style2'>";
    sInformation += "<thead><tr><th>Level of English</th><th>Female</th><th>Male</th><th>Total</th></tr></thead>";
    sInformation += "<tbody>";
    if (data.dataSeries.length != 0) {
        for (var i = 0; i < data.dataSeries.length; i++) {
            sInformation += "<tr><td>" + data.dataSeries[i].IdentityName + "</td><td  align='center'>" + data.dataSeries[i].PercentageFemaleAllSchool.toFixed(2) + "</td><td  align='center'>" + data.dataSeries[i].PercentageMaleAllSchool.toFixed(2) + "</td><td  align='center'>" + data.dataSeries[i].PercentageAllSchool.toFixed(2) + "</td><tr>";

        }

    } else {


        sInformation += "<tr><td colspan='4' align='center'> No data available</td><tr>";
    }


    sInformation += "</tbody></table>";
    ShowPopupInformation(sInformation);

}

function SetErrorMessage(xhr) {
    if (xhr.responseText.length > 0) {
        var sErrorMessage = JSON.parse(xhr.responseText).Message;
        alert(sErrorMessage);
    }
}

// show/hide layer on click event
function ToggleKMLLayer(checked, id) {
    if (checked) {
        var layer;
        switch (kml[id].type) {
            case 0: // direct kml file
                layer = new google.maps.KmlLayer(kml[id].url, {
                    preserveViewport: true,
                    suppressInfoWindows: true
                });
                break;
            case 1: // kml from fusion table
                layer = new google.maps.FusionTablesLayer({
                    query: kml[id].query,
                    suppressInfoWindows: true
                });
                break;
        }

        kml[id].obj = layer;
        kml[id].obj.setMap(map);

        google.maps.event.addListener(layer, 'click',
        function (kmlEvent) {
            if (kml[id].dataType == 0) {
                SearchData(kmlEvent.featureData.description, "ZoneCode");
            } else if (kml[id].dataType == 1) {
                SearchData(kmlEvent.featureData.description, "SchCode");
            }
        });
    } else {
        kml[id].obj.setMap(null);
        delete kml[id].obj;
    }
};

// create layer control box on top right of screen
function CreateLayerControl() {
    var i = -1;
    var html = "<form action='' name='formLayer'><ul>";
    for (var prop in kml) {
        i++;
        html += "<li id=\"selector" + i + "\"><input name='box' type='checkbox' id='" + prop + "'" +
        " onclick='ToggleKMLLayer(this.checked, this.id)' \/>&nbsp;" +
        kml[prop].name + "<\/li>";
    }
    html += "<li class='control'><a href='#' onclick='RemoveAllLayers();return false;'>" +
    "Remove all layers<\/a><\/li>" +
    "<\/ul><\/form>";

    document.getElementById("mapcontrolbox").innerHTML = html;
}

// remove all layers
function RemoveAllLayers() {
    for (var prop in kml) {
        if (kml[prop].obj) {
            kml[prop].obj.setMap(null);
            delete kml[prop].obj;
        }
    }

    var boxes = document.getElementsByName("box");
    for (var i = 0, m; m = boxes[i]; i++) {
        m.checked = false;
    }
}