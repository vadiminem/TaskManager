
/*window.onload = () => {
    $.getJSON('https://localhost:44323/Home/GetTasks',
        (data) => {
            for (var i = 0; i < data.length; i++) {
                if (data[i].parentId < 0) {
                    $('.task-names').prepend(AddToList(data[i].id, data[i].name));
                }
                else {
                    $('ul[data-id="' + data[i].parentId + '"]').prepend(AddToList(data[i].id, data[i].name));
                }
            }
        });

    $('body').on('click', '.task', function () {
        var url = 'https://localhost:44323/Home/GetTask?id=' + $(this).data('id');
        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                $('.right-content').html(data);
            }
        });
    });

    $('body').on('click', '.change-status-action', function () {
        var url = 'https://localhost:44323/Home/ChangeStatus?id=' + $(this).data('id') + '&status=' + $(this).data('status');
        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                $('.right-content').html(data);
            }
        });
    });
}

function AddToList(dataId, dataName): string {

    return '<li data-id="' + dataId + '"><span data-id="' + dataId + '" class="task">' + dataName +
        '</span><a href ="https://localhost:44323/Home/Create?id=' + dataId + '" class="task-control"><b>+</b></a><ul data-id="' + dataId + '"></ul></li>';
}*/

$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
});

window.onload = () => {
    LoadTasks();

    $('#tasks-list').on('click', 'svg', function () {
        $(this).toggleClass('fa-caret-right');
        $(this).toggleClass('fa-caret-down');
        $(this).parent('span').parent('a').parent('li').children('ul').toggleClass('collapse');
    });

    $('#tasks-list').on('click', 'a', function () {
        var id = $(this).parent('li').data('id');
        var url = 'https://localhost:44323/Home/GetTask?id=' + id;
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
            }
        });
    });

    $('#sidebar').on('click', '#new-task-action', function () {
        var url = 'https://localhost:44323/Home/Create'
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
            }
        });
    });

    $('body').on('click', '#submit-form', function () {
        var url = 'https://localhost:44323/Home/Create';
        var url1 = $('form').attr('action');
        $.ajax({
            url: url,
            type: 'POST',
            data: $('form').serialize(),
            success: function () {
                $('#task').html('');
                LoadTasks();
            }
        });
    });

    // Кнопка добавления подзадачи.
    $('body').on('click', '#add-subtask-action', function () {
        var id = $(this).data('id');
        var url = 'https://localhost:44323/Home/Create?id=' + id;
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
            }
        });
    });

    // Кнопка изменения статуса на "Выполняется".
    $('body').on('click', '#start-action', function () {
        var id = $(this).data('id');
        var status = $(this).data('status');
        var url = 'https://localhost:44323/Home/ChangeStatus?id=' + id + '&Status=' + status;
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
            }
        });
    });

    // Кнопка изменения статуса на "Приостановлена".
    $('body').on('click', '#pause-action', function () {
        var id = $(this).data('id');
        var status = $(this).data('status');
        var url = 'https://localhost:44323/Home/ChangeStatus?id=' + id + '&Status=' + status;
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
            }
        });
    });

    // Кнопка изменения статуса на "Завершена".
    $('body').on('click', '#complete-action', function () {
        var id = $(this).data('id');
        var status = $(this).data('status');
        var url = 'https://localhost:44323/Home/ChangeStatus?id=' + id + '&Status=' + status;
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
            }
        });
    });

    // Кнопка удаления задачи.
    $('body').on('click', '#remove-action', function () {
        var id = $(this).data('id');
        var url = 'https://localhost:44323/Home/Remove?id=' + id;
        $.ajax({
            url: url,
            method: 'GET',
            success: function () {
                $('#task').html('');
                LoadTasks();
            }
        });
    });

    // Кнопка изменения задачи.
    $('body').on('click', '#edit-action', function () {
        var id = $(this).data('id');
        var url = 'https://localhost:44323/Home/Edit?id=' + id;
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                $('#task-info').html(data);
                $('form').attr('action', '/Home/Edit');
            }
        });
    });
}

function AddToList(dataId, dataName, dataLevel): string {

    return '<li data-id="' + dataId + '" data-level="' + dataLevel + '"><a href="#">' +
        '<span id="indent"' + dataLevel + '"></span>' +
        '<span id="task-caret"></span>' +
        '<span data-id="' + dataId + '">' + dataName +
        '</span></a><ul class="list-unstyled" data-id="' + dataId + '"></ul></li>';
}

function LoadTasks() {
    $('#tasks-list').html('');
    $.getJSON('https://localhost:44323/Home/GetTasks',
        (data) => {
            for (var i = 0; i < data.length; i++) {
                if (data[i].parentId < 0) {
                    $('#tasks-list').prepend(AddToList(data[i].id, data[i].name, data[i].level));
                }
                else {
                    $('ul[data-id="' + data[i].parentId + '"]').prepend(AddToList(data[i].id, data[i].name, data[i].level));
                }
            }

            $('#tasks-list li').each(function () {
                var attrValue = 'padding-left: ' + ($(this).data('level') * 20) + 'px;';
                $(this).children('a')
                    .children('#indent')
                    .attr('style', attrValue);
            });

            var mTasks = $('li ul li');
            mTasks.each(function () {
                $(this).parent('ul').toggleClass('collapse');
                $(this).parent('ul').parent('li').children('a').children('#task-caret')
                    .append('<i class="fas fa-caret-right"></i>');
            });

        });
}

enum Status {
    Assigned,
    InProgress,
    Paused,
    Completed
}