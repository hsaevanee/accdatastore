//$(function () {
//    $(document.body).on('click', '.a-close-popup-information', function () {
//        $("#popup-information").hide(250);
//    });
//    initMap();
//});

//function initMap() {
//    var mapCenter = new google.maps.LatLng(57.151810, -2.094451);
//    var mapOptions = {
//        zoom: 9,
//        center: mapCenter
//    }

//    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

//    var layerKml = new google.maps.KmlLayer({
//        suppressInfoWindows: true,
//        //url:'https://dl.dropboxusercontent.com/u/55734762/Postcodes.kml'

//        //url: 'https://dl.dropboxusercontent.com/u/55734762/ABpostcodedistricts.kml'

//        url: 'https://dl.dropboxusercontent.com/u/55734762/Datazone_with_Desc.kml'

//        //url: 'https://dl.dropboxusercontent.com/u/870146/KML/UK%20postcode%20districts.kml' + "?rand=" + (new Date()).valueOf()
//    });
//    layerKml.setMap(map);

//    google.maps.event.addListener(layerKml, 'click',
//	function (kmlEvent) {
//	    SearchByName(kmlEvent.featureData.description);
//	});
//}
//function ShowPopupInformation(sInformation) {
//    var popupInformation = document.getElementById('popup-information');
//    popupInformation.innerHTML = sInformation;
//    $("#popup-information").show(250);
//}

//function SearchByName(sName) {
//    var param = JSON.stringify({ 'sName': sName });

//    $.ajax({
//        type: "POST",
//        url: sContextPath + "SchoolProfile/Nationality/SearchByName",
//        data: param,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            ShowPopupInfo(data,sName);
//        },
//        error: function (xhr, err) {
//            SetErrorMessage(xhr);
//        }
//    });
//}

//function SetErrorMessage(xhr) {
//    if (xhr.responseText.length > 0) {
//        var sErrorMessage = JSON.parse(xhr.responseText).Message;
//        alert(sErrorMessage);
//    }
//}


//function ShowPopupInfo(data, sName) {
//    var sInformation = "<a href='#' class='a-close-popup-information'>Close</a><h3>" + sName + "</h3>";
//    sInformation += "<table class='style2'>";
//    sInformation += "<thead><tr><th>Nationality</th><th>Female</th><th>Male</th></tr></thead>";
//    sInformation += "<tbody>";

//    for (var i = 0; i < data.length; i++) {        
//        sInformation += "<tr><td>" + data[i].IdentityName + "</td><td  align='center'>" + data[i].PercentageFemaleAllSchool.toFixed(2) + "</td><td  align='center'>" + data[i].PercentageMaleAllSchool.toFixed(2) + "</td><tr>";
        
//    }

//    sInformation += "</tbody></table>";
//    ShowPopupInformation(sInformation);

//}


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
        name: "Aberdeen Postcode Districts",
        type: 0,
        url: 'https://dl.dropboxusercontent.com/u/55734762/ABpostcodedistricts.kml' + "?rand=" + (new Date()).valueOf(),
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
    var popupInformation = document.getElementById('popup-information');
    popupInformation.innerHTML = sInformation;
    $("#popup-information").show(250);
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
        url: sContextPath + "SchoolProfile/Nationality/SearchByName",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowPopupInfo(data, sCondition);
            ShowPopupInformation(sInformation);
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}


function ShowPopupInfo(data, sName) {
    var sInformation = "<a href='#' class='a-close-popup-information'>Close</a><h3>" + sName + "</h3>";
    sInformation += "<table class='style2'>";
    sInformation += "<thead><tr><th>Nationality</th><th>Female</th><th>Male</th></tr></thead>";
    sInformation += "<tbody>";

    for (var i = 0; i < data.length; i++) {        
        sInformation += "<tr><td>" + data[i].IdentityName + "</td><td  align='center'>" + data[i].PercentageFemaleAllSchool.toFixed(2) + "</td><td  align='center'>" + data[i].PercentageMaleAllSchool.toFixed(2) + "</td><tr>";

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
                SearchData(kmlEvent.featureData.name,"Postcode");
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