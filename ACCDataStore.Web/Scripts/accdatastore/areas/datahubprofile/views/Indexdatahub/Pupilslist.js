function exportPDF(schoolname, levercategory) {
    //var pupilsList = @(Html.Raw(Json.Encode(data)));

    var JSONObject = {
        "schoolname": schoolname,
        "levercategory": levercategory
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/getJsonPupilList",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            BuildPDF(data)
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

function BuildPDF(data) {
    var dd = {
        content: [
             { text: 'This is a header', style: 'header' },
             'No styling here, this is a standard paragraph',
             { text: 'Another text', style: 'anotherStyle' },
             { text: 'Multiple styles applied', style: ['header', 'anotherStyle'] }
        ],

        styles: {
            header: {
                fontSize: 22,
                bold: true
            },
            anotherStyle: {
                italic: true,
                alignment: 'right'
            }
        }
    }

    pdfMake.createPdf(dd).open();
}