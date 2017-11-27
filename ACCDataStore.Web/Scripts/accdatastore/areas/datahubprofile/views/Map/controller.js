angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables', 'ngMap'])

.controller('rootCtrl', function ($scope, $rootScope) {

})

.controller('MapCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, mapService) {
    var map;
    var mapBounds;

    var infowindow = new google.maps.InfoWindow();
    
    $scope.mMap = {
    };
    
    mapService.getCondition().then(function (response) {
        $scope.mMap.bShowContent = false;
        $scope.mMap = response.data;
    }, function (response) {
    });

    google.maps.event.addDomListener(window, 'load', initMap);

    function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: new google.maps.LatLng(57.151810, -2.094451),
            mapTypeId: google.maps.MapTypeId.TERRAIN
            //mapTypeControl: true,
            //mapTypeControlOptions: {
            //    style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            //    position: google.maps.ControlPosition.RIGHT_BOTTOM
            //},
            //zoomControl: true,
            //zoomControlOptions: {
            //    position: google.maps.ControlPosition.RIGHT_BOTTOM
            //},
            //scaleControl: true,
            //streetViewControl: true,
            //streetViewControlOptions: {
            //    position: google.maps.ControlPosition.RIGHT_BOTTOM
            //}
        });
        //initial layer set to Neighbourhoods
        createLayer($scope.selectedLayer);
    }

    $scope.updateDataset = function () {
        //console.log($scope.dataset_item.code, $scope.dataset_item.name)
        createLayer($scope.mMap.selectedDataset)
        $scope.mMap.selectedLayer = $scope.mMap.selectedLayer;
        $scope.mMap.selectedDataset = $scope.mMap.selectedDataset;
        $scope.mMap.bShowContent = false;
    }

    $scope.updateLayer = function () {

        $scope.mMap.selectedLayer = $scope.mMap.selectedLayer;
        $scope.mMap.selectedDataset = $scope.mMap.selectedDataset;
        $scope.mMap.bShowContent = false;
        //console.log($scope.layer_item.code, $scope.layer_item.name)
        if ($scope.mMap.selectedLayer.Code == 'S01') {
            createLayer($scope.selectedLayer);
        } else {
            createMarker($scope.selectedLayer);
        }

    }

    $scope.GetPositiveChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = false;
    };
    $scope.GetNonPositiveChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = false;
    };
    $scope.GetUnknownChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = true;
    };
    $scope.GetTimelineChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = true;
    };

    var createLayer = function (layertype) {
        //get json data from database via controller 
        //render on google map

        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: new google.maps.LatLng(57.151810, -2.094451)
        });

        map.data.addGeoJson(Neighbourhoodsjsondata);

        map.data.setStyle(function (feature) {
            var color = '#0E94FF';
            if (feature.getProperty('isColorful')) {
                color = '#006bc1';
            }
            return /** @type {google.maps.Data.StyleOptions} */({
                fillColor: color,
                strokeColor: color,
                strokeWeight: 2
            });
        });

        // When the user clicks, set 'isColorful', changing the color of the letters.
        map.data.addListener('click', function (event) {
            event.feature.setProperty('isColorful', true);

            mapService.getData($scope.mMap.selectedLayer.Code,event.feature.getProperty('REFNO'), $scope.mMap.selectedDataset.Code).then(function (response) {
                $rootScope.bShowLoading = false;
                $scope.mMap = response.data;
                $scope.mMap.bShowContent = true;
            }, function (response) {
                $rootScope.bShowLoading = false;
            });
        });

        // When the user hovers, tempt them to click by outlining the letters.
        // Call revertStyle() to remove all overrides. This will use the style rules
        // defined in the function passed to setStyle()
        map.data.addListener('mouseover', function (event) {
            map.data.revertStyle();
            map.data.overrideStyle(event.feature, { strokeWeight: 7 });
            infowindow.setContent(event.feature.getProperty('name'));
            infowindow.setPosition(event.latLng);
            //infowindow.setOptions({ pixelOffset: new google.maps.Size(0, -34) });
            infowindow.open(map);
        });

        map.data.addListener('mouseout', function (event) {
            map.data.revertStyle();
        });


 
    }

    var createMarker = function (layertype) {
        //get json data from database via controller 
        //render on google map

        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: new google.maps.LatLng(57.151810, -2.094451)
        });

        for (var i = 0; i < InsightSchoollocationsjsondata.features.length; i++) {
            var coords = InsightSchoollocationsjsondata.features[i].geometry.coordinates;
            var title = InsightSchoollocationsjsondata.features[i].properties.name;
            var latLng = new google.maps.LatLng(coords[1], coords[0]);
            var marker = new google.maps.Marker({
                position: latLng,
                map: map,
                title: title
            });

            //google.maps.event.addListener(marker, 'click', function () {
            //    infowindow.setContent('<h2>' + marker.title + '</h2>');
            //    infowindow.open($scope.map, marker);
            //});

            (function (i, marker) {
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.setContent('<h2>' + marker.title + '</h2>');
                    infowindow.open($scope.map, marker);
                    //get data here
                });

                google.maps.event.addListener(marker, 'mouseover', function () {
                    infowindow.setContent(marker.title);
                    infowindow.open($scope.map, marker);
                });
            })(i, marker);
        }

    }
   
});
 