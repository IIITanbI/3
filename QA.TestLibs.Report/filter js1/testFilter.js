$(function () {
    var myFilter = Object.create(FILTER);
    myFilter.className = "filter-";
    myFilter.getChilds = function (button) {
        return $(button).closest(".parent").children('.child').children();
    };
    myFilter.getFilterButtons = function (button) {
        return $(button).closest(".table").find("button[class*='filter']");
    };
    myFilter.getTotalButton = function (button) {
        return $(button).closest(".table").find(".filter-total");
    };
    myFilter.getChildStatus = function (child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };

    $("button[class*=' filter']").click(function (e) {
        myFilter.filterButtonClick(this);
    });

    $(".btnexp").click(function (e) {
        $(this).closest(".parent").children('.child').toggle(500);
    });
    $('.btnlog').click(function (e) {
        $(this).closest(".accordion").find('.logPanel').slideToggle();
    });

});

