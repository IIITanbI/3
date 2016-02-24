$(function () {

    var myFilter = Object.create(FILTER);
    myFilter.className = "log-filter-";
    myFilter.getChilds = function (button) {
        return $(button).closest(".logPanel").children('.log').children();
    };
    myFilter.getFilterButtons = function (button) {
        return $(button).closest(".table").find("button[class*='log-filter']");
    };
    myFilter.getTotalButton = function (button) {
        return $(button).closest(".table").find(".log-filter-total");
    };
    myFilter.getChildStatus = function (child) {
        var $status = $(child).children("span").text().toLowerCase();
        return $status;
    };
    myFilter.getColor = function (filter) {
        switch (filter) {
            case 'trace':
                return "primary";
            case 'debug':
                return "success";
            case 'warn':
                return "warning";
            case 'info':
                return "info";
            case 'error':
                return "danger";
            default:
                return "info";
        }
    }


    $("button[class*='log-filter']").click(function (e) {
        myFilter.filterButtonClick(this);
    });
});
