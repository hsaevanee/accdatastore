function goToCreateURL(object) {
    var sSchoolNameText = $('#selectSchoolname option:selected').text();
    return object.href += sSchoolNameText;
}
