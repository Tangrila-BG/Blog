$(document).ready(function () {
    function notifications(item) {
        if ($(item)) {
            $(item).show(function () {
                $(this).hide();
            })
        }
    }
});