$(function () {
    InitSpinner();
});

$(document).ready(function () {

    $("input[name='SIMD']").click(function () {
        $('input[name="CheckSIMDAll"]').prop("checked", false);
    });

    //$("input[name='gender']").click(function () {
    //    $('input[name="CheckGenderAll"]').prop("checked", false);
    //});

    $("input[name='years']").click(function () {
        $('input[name="CheckYearAll"]').prop("checked", false);
    });

    $("input[name='CheckSIMDAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="SIMD"]').prop("checked", true);
        } else {
            $('input[name="SIMD"]').prop("checked", false);
        }
    });

    $("input[name='CheckDataitem']").click(function () {
        $('input[name="CheckDataitemAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitemAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="CheckDataitem"]').prop("checked", true);
        } else {
            $('input[name="CheckDataitem"]').prop("checked", false);
        }
    });

    $("input[name='CheckYearAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="years"]').prop("checked", true);
            //$('input[name="years"]').attr( "disabled", "disabled" );
        } else {
            $('input[name="years"]').prop("checked", false);
        }
    });

    //$("input[name='CheckGenderAll']").change(function () {
    //    if (this.checked) {
    //        //alert('ChecknationalityAll check');
    //        $('input[name="gender"]').prop("checked", true);
    //    } else {
    //        $('input[name="gender"]').prop("checked", false);
    //    }
    //});
});

function validateCheckBoxs() {
    // get all checked checkbox
    var arrCheckboxCheckedYear = [];
    $('input[name="years"]:checked').each(function () {
        arrCheckboxCheckedYear.push($(this).val());
    });

    //var arrCheckboxCheckedGender = [];
    //$('input[name="gender"]:checked').each(function () {
    //    arrCheckboxCheckedGender.push($(this).val());
    //});
    var arrCheckboxCheckedSIMD = [];
    $('input[name="SIMD"]:checked').each(function () {
        arrCheckboxCheckedSIMD.push($(this).val());
    });
    //// create 'NationalParams' object as a parameter to controller
    //mNationalParams = {
    //    ListConditionYear: arrCheckboxCheckedYear,
    //    ListConditionGender: arrCheckboxCheckedGender,
    //    ListConditionNationality: arrCheckboxCheckedNationality
    //};

    //if (arrCheckboxCheckedYear.length == 0) {
    //    alert('Please select Year');
    //    return false;
    //} else if (arrCheckboxCheckedGender.length == 0) {
    //    alert('Please select Gender');
    //    return false;
    //    //getNationalData(mNationalParams);	
    //} else 
    if (arrCheckboxCheckedSIMD.length == 0) {
        alert('Please select Nationality');
        return false;
    } else if(arrCheckboxCheckedYear.length ==0)
    {
        alert('Please select Years');
        return false;
    }else{
         return true;
    }

}