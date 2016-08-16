
var hGraphs = {
    cache: {},
    getData: function (callback) {
        $.get('/DatahubProfile/IndexDatahub/MainPieChartData',
            function (data) {
                console.log(data);
                hGraphs.cache[callback] = data;
                hGraphs[callback]();
        });
    },
    mainChart: function () {
        var seriesTotal = [], seriesSpecific = [];
        for (var key in hGraphs.cache.mainChart.totals) {
            if (key != 'title') {
                seriesTotal.push({ name: key, y: hGraphs.cache.mainChart.totals[key] });
            }
        }
        if (hGraphs.cache.mainChart.selected != null) {
            for (var key in hGraphs.cache.mainChart.selected) {
                if (key != 'title') {
                    seriesSpecific.push({ name: key, y: hGraphs.cache.mainChart.selected[key] });
                }
            }
        }
        if (seriesTotal.length > 0) {
            hGraphs.draw('#datahub-index-mainpiechart', seriesTotal, hGraphs.cache.mainChart.totals.title);
        }
        if (seriesSpecific.length > 0) {
            hGraphs.draw('#datahub-index-specificpiechart', seriesSpecific, hGraphs.cache.mainChart.selected.title);
        }
    },
    draw: function (id, data, title) {
        $(id).highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                ploitShadow: false,
                type: 'pie'
            },
            title: {
                text: title + ' Students'
            },
            /*tooltip: {
                pointFormat: ''
            },*/
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f}%',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                name: 'Student groups',
                colorByPoint: true,
                data: data
            }]
        });
    },
    construct: function (type) {
        if (hGraphs.cache[type] != null) {
            hGraphs[type]();
        } else {
            hGraphs.getData(type);
        }
    }
};

window.onload = hGraphs.construct('mainChart');