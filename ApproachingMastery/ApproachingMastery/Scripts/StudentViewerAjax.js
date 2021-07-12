var getAirTestURL = '/Student/GetAirTest';
var addAirTestURL = '/Student/AddAirTest';

var getAccomodationListURL = '/Student/GetAccomodationsList';
var getReadingAccomodationTestsURL = '/Student/GetReadingAccomodationTests';
var getMathAccomodationTestsURL = '/Student/GetMathAccomodationTests';
var getWritingAccomodationTestsURL = '/Student/GetWritingAccomodationTests';
var AddAccomodationTestURL = '/Student/AddAccomodationTest';
var EditAccomodationTestURL = '/Student/EditAccomodationTest';
var AddAccomodationURL = '/Student/Accommodations';
var EditAccomodationURL = '/Student/Accommodations';

var deleteStudentURL = '/User/DeactivateStudent';
var getSRIURL = '/Student/GetSRI';
var editSRIURL = "/Student/EditSRIYear";
var addSRIURL = "/Student/AddSRIYear";
var getProCoreURL = '/Student/GetProCore';
var editProCoreURL = '/Student/EditProCoreYear';
var addProCoreURL = '/Student/AddProCoreYear';
var getTenMarkURL = '/Student/GetTenMark';
var editTenMarkURL = '/Student/EditTenMarkYear';
var addTenMarkURL = '/Student/AddTenMarkYear';
var getABCURL = '/Student/GetABCChart';
var editAirTestURL = "/Student/EditAirTest";
var addABCEntryURL = "/Student/AddABCEntry";
var editABCEntryURL = "/Student/EditABCEntry";
var deleteABCEntryURL = "/Student/DeleteABCEntry";

var getAcademicGoalURL = "/Student/GetAcademicGoal";
var addAcademicGoalURL = "/Student/AddAcademicGoal";
var editAcademicGoalURL = "/Student/EditAcademicGoal";
var deleteAcademicGoalURL = "/Student/DeleteAcademicGoal";

var getBehaviorGoalURL = "/Student/GetBehaviorGoal";
var addBehaviorGoalURL = "/Student/AddBehaviorGoal";
var editBehaviorGoalURL = "/Student/EditBehaviorGoal";
var deleteBehaviorGoalURL = "/Student/DeleteBehaviorGoal";

var addGoalAssignmentURL = "/Student/AddGoalAssignment";
var editGoalAssignmentURL = "/Student/EditGoalAssignment";
var deleteGoalAssignmentURL = "/Student/DeleteGoalAssignment";

var getGoalAssignmentsURL = "/Student/GetGoalAssignments";

var deleteAirTestURL = "/Student/DeleteAirTest";

var addAccommodationDetailURL = "/Student/AddAccommodationDetail";
var editAccommodationDetailURL = "/Student/EditAccommodationDetail";

var deleteAccommodationURL = "/Student/RemoveAccommodation";
var deleteAccommodationDetailURL = "/Student/RemoveAccommodationDetail";







var dialogOpen = false;

$(document).ready(function () {
    loadAirTests();
    loadSRI();
    loadTenMark();
    loadProCore();
});


function loadEditModal(buttonClicked, urlName, contentItem, modalItem, dataName) {
    var id = '';
    if (buttonClicked !== null && buttonClicked.attr('data-id') !== undefined)
        id = buttonClicked.attr('data-id');
    if (buttonClicked !== null && buttonClicked.attr('goal-id') !== undefined)
        id = buttonClicked.attr('goal-id');
    $.ajax({
        type: "GET",
        url: urlName,
        contentType: "application/json; charset=utf-8",
        data: dataName !== "" ? dataName + "=" + id : "",
        datatype: "json",
        success: function (data) {
            if (data.html !== undefined)
                contentItem.html(data.html);
            else
                contentItem.html(data);
            //$('#EditStudentModal').modal(options);
            //$('#EditStudentModal').style.display = 'block';
            modalItem.modal('show');
        }
    });
}

function createDataString(dataName, data) {
    var dataString = "";
    if (dataName !== undefined && dataName !== null && data !== null && data !== undefined) {
        if (Array.isArray(dataName)) {
            for (var i = 0; i < dataName.length; i++) {
                if (i > 0) dataString += ";";
                dataString += dataName[i] + "=" + data[i];
            }
        }
        else dataString = dataName + "=" + data;
    }
    return dataString;
}

function loadTable(urlPath, viewer, loading, table, dataName, data) {
    loading.show();
    table.hide();

    var dataString = createDataString(dataName, data);

    $.ajax({
        type: "GET",
        url: urlPath,
        dataType: 'json',
        data: dataString,
        success: function (data) {
            if (!data.Success) table.html("<div>" + data.Message + "</div>");
            else table.html(data.html);
            loading.hide();
            table.show();
            viewer.css({ 'height': '' });
        },
        error: function (xhr, status, error) {
            table.show();
            // viewer.css({ 'height': '' });
            //viewer.html("<div>" + error + "</div><div>" + xhr.responseText + "</div>");
            loading.hide();
        }
    });
}

function loadAddTestModal(modalDiv, modalData, urlString) {
    $.ajax({
        type: "GET",
        url: urlString,
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (data) {
            modalData.html(data);
            //$('#EditStudentModal').modal(options);
            //$('#EditStudentModal').style.display = 'block';
            modalDiv.modal('show');
        }
    });
}

function AddAccomodationTest() {
    loadEditModal(null, AddAccomodationTestURL, $("#accomodationTestContent"), $("#addAccomodationTest"), null);
}

function loadAirTests() {
    loadTable(getAirTestURL, $("#airTestViewer"), $("#airTestsLoading"), $("#getAirTests"));
}

function loadSRI() {
    loadTable(getSRIURL, $("#sriViewer"), $("#sriLoading"), $("#getSRI"));

}

function loadTenMark() {
    loadTable(getTenMarkURL, $("#tenMarkViewer"), $("#tenMarkLoading"), $("#getTenMark"));

}


function loadProCore() {
    loadTable(getProCoreURL, $("#proCoreViewer"), $("#proCoreLoading"), $("#getProCore"));
}


function loadABCChart() {
    loadTable(getABCURL, $("#abcChartViewer"), $("#abcChartLoading"), $("#getABCChart"));
}

function loadAccomodations() {

    loadAccomodationsList();
    loadReadingAccomodationTests();
    loadWritingAccomodationTests();
    loadMathAccomodationTests();

}

function loadAccomodationsList() {
    loadTable(getAccomodationListURL, $("#accomodationsViewer"), $("#AccomodationsListLoading"), $("#getAccomodationsList"));
}

function loadReadingAccomodationTests() {
    loadTable(getReadingAccomodationTestsURL, $("#accomodationTestViewer"), $("#ReadingAccomodationTestsLoading"), $("#getReadingAccomodationTests"));
}


function loadMathAccomodationTests() {
    loadTable(getMathAccomodationTestsURL, $("#accomodationTestViewer"), $("#MathAccomodationTestsLoading"), $("#getMathAccomodationTests"));
}

function loadWritingAccomodationTests() {
    loadTable(getWritingAccomodationTestsURL, $("#accomodationTestViewer"), $("#WritingAccomodationTestsLoading"), $("#getWritingAccomodationTests"));
}

function AddABCEntry() {
    loadEditModal(null, addABCEntryURL, $("#abcChartContent"), $("#abcChartModal"), null);
}

function loadAcademicGoals() {
    var div = $("#getAcademicGoals");
    var loading = $("#AcademicGoalsLoading");
    var viewer = $("#AcademicGoalsViewer");

    loadTable(getAcademicGoalURL, viewer, loading, div);
}

function loadAcademicsTab() {
    loadAcademicGoals();
}

function loadBehaviorTab() {
    loadABCChart();
    loadBehaviorGoals();
}

function loadBehaviorGoals() {
    var div = $("#getBehaviorGoals");
    var loading = $("#behaviorGoalsLoading");
    var viewer = $("#behaviorGoalsViewer");
    loadTable(getBehaviorGoalURL, viewer, loading, div);
}

function AddAccommodation() {
    loadEditModal(null, AddAccomodationURL, $("#AddEditGoal"), $("#AddEditGoalModal"), null);
}

function AddAirTest() {
    loadEditModal(null, addAirTestURL, $("#AddEditGoal"), $("#AddEditGoalModal"), null);
}

function AddAcademicGoal() {
    loadEditModal(null, addAcademicGoalURL, $("#AddEditGoal"), $("#AddEditGoalModal"), null);
}
function AddBehaviorGoal() {
    loadEditModal(null, addBehaviorGoalURL, $("#AddEditGoal"), $("#AddEditGoalModal"), null);
}

function loadGoalAssignments(goalID) {

    goalID = goalID.replace("{", "").replace("}", "");

    var spinnerID = "#spinner-" + goalID;
    var divID = "#" + goalID + "-assignments";
    loadTable(getGoalAssignmentsURL, null, $(spinnerID), $(divID), "goalID", "{" + goalID + "}");
}

function assignmentAdded(data) {
    if (data.Success) {
        $("#AddEditGoalModal").modal('hide');
    }
    alert(data.Message);
    loadGoalAssignments(data.goalID);
    dialogOpen = false;
}

function assignmentUpdated(data) {
    if (data.Success) {
        $("#AddEditGoalModal").modal('hide');
    }
    alert(data.Message);
    loadGoalAssignments(data.goalID);
    dialogOpen = false;
}