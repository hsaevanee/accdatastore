$(document).ready(function () {
    console.log('success!');
    initMap();
})

function initMap()
{
    var url = "https:\/\/maps.googleapis.com/maps/api/geocode/json?&address=Scotland";
    var loc;
    var bounds;
    
    $.getJSON(url, function (result) {
        console.log(result);
        loc = result.results[0].geometry.location;
        var sw = result.results[0].geometry.bounds.southwest;
        var ne = result.results[0].geometry.bounds.northeast;
        bounds = new google.maps.LatLngBounds(sw, ne);
    })

    var mapCenter = new google.maps.LatLng(56.49067119999999, -4.2026458);
    var mapOptions = {
        zoom: 7,
        center: mapCenter
    }

    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    //var cities = ["S12000033", "S12000046"]
    //cities.forEach( function(item) {
    //    $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetGeoJSON?id=" + item, function (result) {
    //        console.log(result);
    //        map.data.addGeoJson(result);
    //    })
    //})

    var cities = ["Aberdeen City", "Glasgow City", "Aberdeenshire"];
    cities.forEach( function(item) {
        $.getJSON("http://maps.googleapis.com/maps/api/geocode/json?&address=" + item + ",Scotland,UK")
            .done(function (result) {
                console.log(result);
                var marker = new google.maps.Marker({
                    position: result.results[0].geometry.location,
                    map: map,
                    title: item
                });
                marker.addListener('click', function () {
                    console.log(marker.title);
                    //
                });

            });
        });

    

    //map.fitBounds(bounds);
}