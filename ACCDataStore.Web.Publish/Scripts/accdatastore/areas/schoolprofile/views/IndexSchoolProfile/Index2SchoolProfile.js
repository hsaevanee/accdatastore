
$(document).ready(function () {


    $("#selectedschoolname").change(function () {
        //alert("selectedschoolname");
        //window.location = sContextPath + 'SchoolProfile/IndexSchoolProfile/Index/'+$(this).val();
        if (validateDropdownlist()==true) {
            document.forms[0].submit();
        }        
    });

    $("#selectedschoolname2").change(function () {
        //alert("selectedschoolname2");
        //window.location = sContextPath + 'SchoolProfile/IndexSchoolProfile/Index/'+$(this).val();
        if (validateDropdownlist() == true) {
            document.forms[0].submit();
        }
    });


});

function validateDropdownlist() {
    var value1 = $('#selectedschoolname :selected').text();
    var value2 = $('#selectedschoolname2 :selected').text();

    if (value1 == "---Please Select School---" && value2 == "---Please Select School---") {
        alert('Please select School');
        return false;
    } else {
        return true;
    }

}