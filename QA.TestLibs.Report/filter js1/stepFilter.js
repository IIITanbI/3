$(function () {
    var myFilter = Object.create(FILTER);
    myFilter.className = "step-filter-";
	
    myFilter.getChilds = function (button) {
        return $(button).closest(".parent").children('.step').children();
    };
    myFilter.getFilterButtons = function (button) {
		return $(button).closest(".step-filter-btns").children("button[class*='step-filter']");
    };
    myFilter.getTotalButton = function (button) {
		return $(button).closest(".step-filter-btns").find(".step-filter-passed.step-filter-failed.step-filter-skipped.step-filter-unknown");
    };
    myFilter.getChildStatus = function (child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };
    myFilter.getDeactivatedClass = function (button) {
        var filter = myFilter.getFilterFromButton(button);
		var str = filter.join(" ");

        var res = "btn-";
        switch (str) {
            case 'notexecuted':
                res += "primary";
                break;
            case 'passed':
                res += "success";
                break;
            case 'failed':
                res += "warning";
                break;
            case 'skipped':
                res += "info";
                break;
            case 'unknown':
                res += "danger";
                break;
            default:
                res += "info";
                break;
        }
        return res;
    };

    $("button[class*='step-filter']").click(function (e) {
        myFilter.filterButtonClick(this);
    });
});
