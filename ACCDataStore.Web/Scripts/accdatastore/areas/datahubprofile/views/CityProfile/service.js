//Map Controller
angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getGeoJSON: function (layertype) {
            return $http.get(sContextPath + "DatahubProfile/Map/GetGeoJson", { params: { "layertype": layertype } });
        },
        getData: function (sYear) {
            return $http.get(sContextPath + "DatahubProfile/CityProfile/GetData", { params: { "sYear": sYear } });
        },
        getCondition: function () {
            return $http.get(sContextPath + "DatahubProfile/CityProfile/GetCondition");
        },
        LoadHeatMapdata: function (dataset) {
            return $http.get(sContextPath + "DatahubProfile/Map/LoadHeatMapdata", { params: { "dataset": dataset } });
        }
    };
});
