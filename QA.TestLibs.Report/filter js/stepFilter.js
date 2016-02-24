$(function () {

    function doFilter(button) {
        var $childs = $(button).closest(".parent").children('.child').children();
        var $needClass = "status";

        var $filters = [];
        var $filterButtons = $(button).closest(".table").find("button[class*='step-filter']");

        $filterButtons.each(function (index, item) {
            if ($(item).hasClass("activated")) {
                $filters.push(getFilterFromButton(item));
            }
            //console.log(index);
            //console.log(item);
        });

        var $totalButton = $(button).closest(".table").find(".step-filter-total");

        if ($(button).is($totalButton) || $filters.length === 0) {
            $childs.removeAttr("hidden");

            $filterButtons.each(function (index) {
                console.log(this);
                deactivateButton(this);
            });
            activateButton($totalButton);
            return;
        }
        deactivateButton($totalButton);

        for (var i = 0; i < $childs.length; i++) {
            var $panelHeading = $($childs[i]).find('.panel-heading')[0];
            var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
            var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

            if ($.inArray($status, $filters) === -1) {
                $($childs[i]).attr("hidden", "");
            } else {
                $($childs[i]).removeAttr("hidden");
            }
        }
    }

    function filterButtonClick(button) {
        var $button = $(button);

        if ($button.hasClass("activated")) {
            deactivateButton(button);
        }
        else {
            activateButton(button);
        }

        doFilter(button);
    }

    function getFilterFromButton(button) {
        var filter = null;
        //var classList = $(button).attr('class').split(' ');

        var matches = $(button).attr('class').match(new RegExp("step-filter-\\w*", 'g'));
        if (matches.length > 0)
            filter = matches[0].substr("step-filter-".length);

        //for (var i = 0; i < classList.length; i++) {
        //    if (classList[i].match("step-filter-*")) {
        //        filter = classList[i].substr("step-filter-".length);
        //        break;
        //    }
        //}
        console.log("log filter = " + filter);
        return filter;
    }

    function getColor(filter) {
        switch (filter) {
            case 'notexecuted':
                return "primary";
            case 'debug':
                return "passed";
            case 'warn':
                return "failed";
            case 'info':
                return "skipped";
            case 'error':
                return "unknown";
            default:
                return "info";
        }
    }

    function activateButton(button) {
        var $button = $(button);
        $button.removeClass("btn-" + getColor(getFilterFromButton(button)));
        $button.addClass("btn-warning");
        $button.addClass("activated");
    }

    function deactivateButton(button) {
        var $button = $(button);
        $button.removeClass("activated");
        $button.removeClass("btn-warning");
        $button.addClass("btn-" + getColor(getFilterFromButton(button)));
    }


    $("button[class*='step-filter']").click(function (e) {
        filterButtonClick(this);
    });
});
