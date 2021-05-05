$(".checkbox-menu").on("change", "input[type='checkbox']", function () {
    $(this).closest("li").toggleClass("active", this.checked);
});

$(document).on('click', '.allow-focus .dropdown-menu', function (e) {
    e.stopPropagation();
});

$('#register-dropdown').on('hide.bs.dropdown', function (e) {
    var target = $(e.target);
    if (target.hasClass("keepopen") || target.parents(".keepopen").length) {
        return false; // returning false should stop the dropdown from hiding.
    } else {
        return true;
    }
});