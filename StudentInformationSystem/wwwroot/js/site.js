$('.reply-ticket-btn').click(function () {
    console.log("Work!");
    var ticketId = $(this).data('ticket-id');
    var userNameWhoReplies = $(this).data('user-name');
    $('#replyTicketId').val(ticketId);
    $('#userNameWhoReplies').val(userNameWhoReplies);
    $('#replyTicketModal').modal('show');
});

$('#replyTicketForm').submit(function (e) {
    $('#replyTicketModal').modal('hide');
});

var departments = [];
var programs = [];

$(document).ready(function () {
    // 1: Admin
    // 2: Student
    // 3: Instructor
    $('#bringUserForm').on('submit', function (e) {
        e.preventDefault();

        var formData = {
            firstName: $('#inputFirstName').val(),
            lastName: $('#inputLastName').val()
        };

        $.ajax({
            type: 'POST',
            url: '/Panel/BringUser',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#bringUserPanel').hide();

                    var departmentName = departments.find(d => d.id === response.data.departmentID)?.name;
                    var programName = programs.find(p => p.id === response.data.programID)?.name;
                    $('#fillUserId').val(response.data.id);

                    switch (response.data.roleId) {
                        case 1:
                            $('#edit-title').text("Admin " + response.data.firstName);

                            $('#fillHireDateDiv').remove();
                            $('#fillDepartmentNameDiv').remove();
                            $('#fillProgramNameDiv').remove();

                            $('#fillFirstName').val(response.data.firstName);
                            $('#fillLastName').val(response.data.lastName);
                            $('#fillGender').val(response.data.gender);
                            $('#fillContact').val(response.data.contact);
                            $('#fillDateOfBirth').val(response.data.dateOfBirth);
                            $('#fillPhoneNumber').val(response.data.phoneNumber);
                            $('#fillEmail').val(response.data.email);
                            break;
                        case 2:
                            $('#edit-title').text("Student " + response.data.firstName);

                            $('#fillHireDateDiv').remove();
                            $('#fillDepartmentNameDiv').remove();

                            $('#fillFirstName').val(response.data.firstName);
                            $('#fillLastName').val(response.data.lastName);
                            $('#fillGender').val(response.data.gender);
                            $('#fillContact').val(response.data.contact);
                            $('#fillDateOfBirth').val(response.data.dateOfBirth);
                            $('#fillPhoneNumber').val(response.data.phoneNumber);
                            $('#fillEmail').val(response.data.email);
                            $('#fillProgramName').val(programName);
                            break;
                        case 3:
                            $('#edit-title').text("Instructor " + response.data.firstName);

                            $('#fillProgramNameDiv').remove();

                            $('#fillFirstName').val(response.data.firstName);
                            $('#fillLastName').val(response.data.lastName);
                            $('#fillGender').val(response.data.gender);
                            $('#fillContact').val(response.data.contact);
                            $('#fillDateOfBirth').val(response.data.dateOfBirth);
                            $('#fillHireDate').val(response.data.hireDate);
                            $('#fillPhoneNumber').val(response.data.phoneNumber);
                            $('#fillEmail').val(response.data.email);
                            $('#fillDepartmentName').val(departmentName);
                            break;
                        default:
                    }
                    $('#userDetails').show();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error fetching user.');
            }
        });
    });
});
// AJAX to fetch departments
$(document).ready(function () {
    $.ajax({
        type: 'POST',
        url: '/Panel/GetDepartments',
        success: function (response) {
            if (response.success) {
                departments = response.data;
            }
            else {
                alert(response.message);
            }
        },
        error: function () {
            alert('Error fetching departments.');
        }
    });
});

// AJAX to fetch programs
$(document).ready(function () {
    $.ajax({
        type: 'POST',
        url: '/Panel/GetPrograms',
        success: function (response) {
            if (response.success) {
                programs = response.data;
            }
            else {
                alert(response.message);
            }
        },
        error: function () {
            alert('Error fetching departments.');
        }
    });
});

$('.delete-user-btn').click(function () {
    var userId = $(this).data('user-id');
    $('#deleteUserId').val(userId);
    $('#deleteUserModal').modal('show');
});

$('#deleteUserForm').submit(function (e) {
    $('#deleteUserModal').modal('hide');
});

$(document).ready(function () {
    $('#bringCourseForm').on('submit', function (e) {
        e.preventDefault();

        var formData = {
            courseName: $('#inputCourseName').val(),
        };

        $.ajax({
            type: 'POST',
            url: '/Panel/BringCourse',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#bringCoursePanel').hide();

                    var departmentName = departments.find(d => d.id === response.data.departmentID)?.name;
                    $('#fillCourseId').val(response.data.id);
                    $('#edit-title').text(response.data.name);

                    $('#fillCourseName').val(response.data.name);
                    $('#fillCourseDescription').val(response.data.description);
                    $('#fillCredits').val(response.data.credit);
                    $('#fillDepartmentName').val(departmentName);

                    $('#courseDetails').show();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error fetching course.');
            }
        });
    });
});
$('.delete-course-btn').click(function () {
    var courseId = $(this).data('course-id');
    $('#deleteCourseId').val(courseId);
    $('#deleteCourseModal').modal('show');
});

$('#deleteUserForm').submit(function (e) {
    $('#deleteUserModal').modal('hide');
});

$(document).ready(function () {
    $('#bringProgramForm').on('submit', function (e) {
        e.preventDefault();

        var formData = {
            programName: $('#inputProgramName').val(),
        };

        $.ajax({
            type: 'POST',
            url: '/Panel/BringProgram',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#bringProgramPanel').hide();

                    var departmentName = departments.find(d => d.id === response.data.departmentID)?.name;
                    $('#fillProgramId').val(response.data.id);
                    $('#edit-title').text(response.data.name);

                    $('#fillProgramName').val(response.data.name);
                    $('#fillProgramDescription').val(response.data.description);
                    $('#fillDurationInYears').val(response.data.durationInYears);
                    $('#fillDepartmentName').val(departmentName);

                    $('#programDetails').show();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error fetching program.');
            }
        });
    });
});

$('.delete-program-btn').click(function () {
    var programId = $(this).data('program-id');
    $('#deleteProgramId').val(programId);
    $('#deleteProgramModal').modal('show');
});

$('#deleteProgramForm').submit(function (e) {
    $('#deleteProgramModal').modal('hide');
});

$(document).ready(function () {
    $('#bringDepartmentForm').on('submit', function (e) {
        e.preventDefault();

        var formData = {
            departmentName: $('#inputDepartmentName').val(),
        };

        $.ajax({
            type: 'POST',
            url: '/Panel/BringDepartment',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#bringDepartmentPanel').hide();

                    $('#fillDepartmentId').val(response.data.id);
                    $('#edit-title').text(response.data.name);

                    $('#fillDepartmentName').val(response.data.name);
                    $('#fillDepartmentDescription').val(response.data.description);

                    $('#departmentDetails').show();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error fetching department.');
            }
        });
    });
});

$('.delete-department-btn').click(function () {
    var departmentId = $(this).data('department-id');
    $('#deleteDepartmentId').val(departmentId);
    $('#deleteDepartmentModal').modal('show');
});

$('#deleteDepartmentForm').submit(function (e) {
    $('#deleteDepartmentModal').modal('hide');
})