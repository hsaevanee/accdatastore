function getScrumChart()
{
    $.get('/ScrumHub/ScrumHub/scrumChart',
        function (res) {

            $("#chart1").highcharts({
                chart: {
                    type:"bar"
                },
                title:
                    {
                        text : "Ages"
                    },
                xAxis: {
                    categories: res.people.map(function (x) { return x.firstname; })
                },
                yAxis: {
                    title:
                        {
                            text: "Age"
                        }
                },
                series: [{ name: 'age', data: res.people.map(function (x) { return x.age; }) }]
            });
        });
}
