// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.toggle-task-status').change(function () {
        var taskId = $(this).data('task-id');
        var isCompleted = $(this).prop('checked');

        $.ajax({
            url: '/Task/UpdateStatus/' + taskId,
            type: 'POST',
            data: { isCompleted: isCompleted },
            success: function (result) {
                // Handle success, if needed
            },
            error: function (err) {
                console.error(err);
            }
        });
    });
});