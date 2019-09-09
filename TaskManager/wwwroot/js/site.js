window.onload = function () {
    $.getJSON('https://localhost:44323/Home/GetTasks', function (data) {
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
};
function AddToList(dataId, dataName) {
    return '<li data-id="' + dataId + '"><span data-id="' + dataId + '" class="task">' + dataName +
        '</span><ul data-id="' + dataId + '"></ul></li>';
}
//# sourceMappingURL=site.js.map