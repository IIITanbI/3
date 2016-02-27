$(function () {
    var myFilter = Object.create(FILTER);
    myFilter.className = "log-filter-";
    myFilter.multiSelect = false;

    myFilter.getChilds = function (button) {
        return $(button).closest(".logPanel").children('.log').children("div");
    };

    myFilter.getFilterButtons = function (button) {
        return $(button).closest(".log-filter-btns").find("button[class*='log-filter']");
    };

    myFilter.getTotalButton = function (button) {
        return $(button).closest(".log-filter-btns").find(".log-filter-trace.log-filter-debug.log-filter-warn.log-filter-info.log-filter-error");
    };

    myFilter.getChildStatus = function (child) {
        var $status = $($(child).children("span")[0]).text().toLowerCase();
        return $status;
    };

    myFilter.getDeactivatedClass = function (button) {
        var filter = myFilter.getFilterFromButton(button);
        var str = filter.join(" ");

        var res = "btn-";
        switch (str) {
            case "trace debug warn info error":
                res += "primary";
                break;
            case "debug warn info error":
                res += "success";
                break;
            case "warn info error":
                res += "warning";
                break;
            case "info error":
                res += "info";
                break;
            case "error":
                res += "danger";
                break;
            default:
                res += "info";
                break;
        }
        return res;
    };



    $(".log-filter-button-expander").children("span").attr("class", "glyphicon glyphicon-chevron-right");

    $(".log-filter-button-expander").click(function (e) {
        var $cur = $(e.currentTarget).children("span");
        var $elem = $(this).closest(".logHeader").find(".log-filter-btns");
        $elem.toggle(300, function onCompleteToggle() {
            if ($elem.is(":visible")) {
                $cur.attr("class", "glyphicon glyphicon-chevron-left");
                console.log("visible");
            } else {
                $cur.attr("class", "glyphicon glyphicon-chevron-right");
                console.log("none");
            }
        });
    });

    $("button[class*='log-filter']").click(function (e) {
        myFilter.filterButtonClick(this);
    });
});
