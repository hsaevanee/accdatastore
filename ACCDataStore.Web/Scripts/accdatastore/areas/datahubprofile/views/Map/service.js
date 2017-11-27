//Map Controller
angular.module('root.services', [])

.factory('mapService', function ($http, $rootScope) {
    return {
        getGeoJSON: function (layertype) {
            return $http.get(sContextPath + "DatahubProfile/Map/GetGeoJson", { params: { "layertype": layertype } });
        },
        getData: function (layertype,RefNO, dataset) {
            return $http.get(sContextPath + "DatahubProfile/Map/GetData", { params: { "layertype": layertype, "seedcode": RefNO, "dataset": dataset } });
        },
        getCondition: function () {
            return $http.get(sContextPath + "DatahubProfile/Map/GetCondition");
    }
    };
});
