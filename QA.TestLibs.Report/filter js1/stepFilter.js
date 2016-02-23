$(function () {

    var myFilter = Object.create(FILTER);
    myFilter.className = "step-filter-";
    myFilter.getChilds = function (button) {
        return $(button).closest(".parent").children('.child').children();
    };
    myFilter.getFilterButtons = function (button) {
        return $(button).closest(".table").find("button[class*='step-filter']");
    };
    myFilter.getTotalButton = function (button) {
        return $(button).closest(".table").find(".step-filter-total");
    };
    myFilter.getChildStatus = function (child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };
    myFilter.getColor = function (filter) {
        switch (filter) {
            case 'notexecuted':
                return "primary";
            case 'passed':
                return "success";
            case 'failed':
                return "warning";
            case 'skipped':
                return "info";
            case 'unknown':
                return "danger";
            default:
                return "info";
        }
    }



    $("button[class*='step-filter']").click(function (e) {
        myFilter.filterButtonClick(this);
    });
});
