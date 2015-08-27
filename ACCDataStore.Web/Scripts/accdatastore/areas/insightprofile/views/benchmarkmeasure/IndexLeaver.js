function Getdata() {
    var schcode = $('#selectedschoolcode :selected').val();
    var schname = $('#selectedschoolcode :selected').text();
    //var syear = $('#selectedyear :selected').val();
    
    var arrCheckboxCheckedgender = [];

    $('input[name="gender"]:checked').each(function () {
        arrCheckboxCheckedgender.push($(this).val());
    });

    var JSONObject = {
        "schcode": schcode,
        "selectedschname": schname,
        "checkedgender": arrCheckboxCheckedgender
        //"year": syear
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