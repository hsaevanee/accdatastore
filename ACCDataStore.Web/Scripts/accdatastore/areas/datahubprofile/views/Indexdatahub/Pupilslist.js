$(document).ready(function () {
    $('#datatable').DataTable({
        paging: false,
        "aaSorting": [[1, "asc"]],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copyHtml5',
            },
            {
                extend: 'excelHtml5',              
            },
            {
                extend: 'csvHtml5',               
            },
            {
                extend: 'pdfHtml5',
                message: 'PDF created by PDFMake with Buttons for DataTables.'
            },
            {
                extend: 'print',
            }
        ]
    });
});

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