function Getdata() {
    var schcode = $('#selectedschoolcode :selected').val();
    var schname = $('#selectedschoolcode :selected').text();

    var JSONObject = {
        "schcode": schcode,
        "schname": schname
    }

        $.ajax({
            type: 'POST',
            url: sContextPath + 'InsightProfile/BenchmarkMeasure/GetLeaverDestinationData',
            data: JSON.stringify(JSONObject),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                drawChartBar(buttonID, data);
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
}