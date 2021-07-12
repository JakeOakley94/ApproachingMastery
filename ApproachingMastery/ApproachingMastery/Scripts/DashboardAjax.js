var editStudentURL = '/Student/EditStudent';
var getStudentsURL = '/User/GetStudents';
var addStudentURL = '/Student/AddStudent';
var deleteStudentURL = '/User/DeactivateStudent';




$(document).ready(function () {
    loadStudents();
});


function loadStudents() {
    var height = $("#studentViewer").height();
    if (height > 0)
        $("#studentViewer").css({ 'height': height });
    $("#getStudentsLoading").show();
    $("#getStudents").hide();
    $.ajax({
        type: "GET",
        url: getStudentsURL,
        dataType: 'json',
        
        success: function (data) {
            $("#getStudents").html(data.html);
            $("#studentCount").text(data.StudentCount);
            $("#getStudentsLoading").hide();
            $("#getStudents").show();
            $("#studentViewer").css({ 'height': '' });
        },
        error: function (xhr, status, error) {
            //alert(xhr.status);

            $("#getStudents").show();
            $("#studentViewer").css({ 'height': '' });
            $("#getStudents").html("<div>" + error + "</div><div>" + xhr.responseText + "</div>");
            $("#getStudentsLoading").hide();

        }
    });
}

function AddStudent() {
    //var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: addStudentURL,
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            $('#addStudentContent').html(data);
            //$('#EditStudentModal').modal(options);
            //$('#EditStudentModal').style.display = 'block';
            $('#addStudentModal').modal('show');
        },
        error: function () {
            alert("Error loading student data");
        }
    });
}

