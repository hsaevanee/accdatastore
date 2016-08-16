var hGraphs = {
    cache: {},
    getData: function () {
        $.get('/DatahubProfile/IndexDatahub/MainPieChartData',
            function (data) {
                console.log(data);
                hGraphs.cache.mainChart = data;
                hGraphs.mainChart();
        });
    },
    mainChart: function () {
        var seriesTotal = [], seriesSpecific = [];
        for (var key in hGraphs.cache.mainChart.totals) {
            seriesTotal.push({ name: key, y: hGraphs.cache.mainChart.totals[key] });
        }
        if (hGraphs.cache.mainChart.selected != null) {
            for (var key in hGraphs.cache.mainChart.selected) {
                seriesSpecific.push({ name: key, y: hGraphs.cache.mainChart.selected[key] });
            }
        }
        if (seriesTotal.length > 0) {
            hGraphs.draw('#datahub-index-mainpiechart', seriesTotal);
        }
        if (seriesSpecific.length > 0) {
            hGraphs.draw('#datahub-index-specificpiechart', seriesSpecific);
        }
    },
    draw: function (id, data) {
        $(id).highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                ploitShadow: false,
                type: 'pie'
            },
            title: {
                text: 'Student Overview'
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
    construct: function(type) {
        hGraphs.getData();
        hGraphs[type]();
    }
};

window.onload = hGraphs.getData();