$(function () {
    $(document.body).on('click', '.a-close-popup-information', function () {
        $("#popup-information").hide(250);
    });
    initMap();
});

function initMap() {
    var mapCenter = new google.maps.LatLng(57.151810, -2.094451);
    var mapOptions = {
        zoom: 9,
        center: mapCenter
    }

    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    var layerKml = new google.maps.KmlLayer({
        suppressInfoWindows: true,
        //url:'https://dl.dropboxusercontent.com/u/55734762/Postcodes.kml'

        url: 'https://dl.dropboxusercontent.com/u/55734762/ABpostcodedistricts.kml'

        //url: 'https://dl.dropboxusercontent.com/u/870146/KML/UK%20postcode%20districts.kml' + "?rand=" + (new Date()).valueOf()
    });
    layerKml.setMap(map);

    google.maps.event.addListener(layerKml, 'click',
	function (kmlEvent) {
	    SearchByName(kmlEvent.featureData.name);
	});
}
function ShowPopupInformation(sInformation) {
    var popupInformation = document.getElementById('popup-information');
    popupInformation.innerHTML = sInformation;
    $("#popup-information").show(250);
}

function SearchByName(sName) {
    var param = JSON.stringify({ 'sName': sName });

    $.ajax({
        type: "POST",
        url: sContextPath + "SchoolProfile/Nationality/SearchByName",
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowPopupInfo(data,sName);
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

function SetErrorMessage(xhr) {
    if (xhr.responseText.length > 0) {
        var sErrorMessage = JSON.parse(xhr.responseText).Message;
        alert(sErrorMessage);
    }
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


