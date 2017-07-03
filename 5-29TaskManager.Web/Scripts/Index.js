$(function () {
    var userId;
    $.get('/account/getuserid', function (id) {
        userId = id;
    })

    var hub = $.connection.tasksHub;
    $.connection.hub.start();

    hub.client.newTask = function (task) {
        $('.table').append(`<tr id="${task.Id}"><td>${task.Title}</td><td><button data-id="${task.Id}" class="btn btn-primary btn-status">I'm Doing This Task</button></td></tr>`)
        $('#task').val('');
    }

    hub.client.markAsCompleted = function (task) {
        if (userId !== task.UserId) {
            $('#' + task.Id).find('td:eq(1)').replaceWith(`<td><button data-id="${task.Id}" class="btn btn-warning btn-status" disabled>${task.FirstName} ${task.LastName} is Doing This Task</button></td>`);
        }
        else {
            $('#' + task.Id).find('td:eq(1)').replaceWith(`<td><button data-id="${task.Id}" class="btn btn-success btn-status">Done!!!</button></td>`);
        }
    }

    hub.client.removeTask = function (id) {
        $('#' + id).remove();
    }

    $('#add').on('click', function () {
        hub.server.addTask($('#task').val());
    })

    $('.table').on('click', '.btn-status', function () {
        hub.server.taskChanged($(this).data('id'));
    })
})