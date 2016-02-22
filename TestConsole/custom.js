$(function () {

    $filters = [];


    function doFilter(env, filter) {
        filterButtonClick(env, filter);

        var $childs = $(env).closest(".parent").children('.child').children();
        var $needClass = "status";

        for (var i = 0; i < $childs.length; i++) {
            var $panelHeading = $($childs[i]).find('.panel-heading')[0];
            var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
            var $actionName = $className.substring($className.indexOf($needClass) + $needClass.length);


            if ($.inArray($actionName, $filters) === -1) {
                $($childs[i]).attr("hidden", "");
            } else {
                $($childs[i]).removeAttr("hidden");
            }

            //if ($actionName !== filter){
            //    $($childs[i]).attr("hidden","");
            //} else {
            //    $($childs[i]).removeAttr("hidden");
            //}
        }
    }

    function filterButtonClick(button, filter) {
        var $button = $(button);
        if ($button.hasClass("activated")) {
            $button.removeClass("activated");
            $button.removeClass("btn-warning");
            $button.addClass("btn-info");
            var num = $.inArray(filter, $filters);
            if (num === -1)
                num = $filters.length;

            $filters.splice(num, 1);
        }
        else {
            $button.removeClass("btn-info");
            $button.addClass("btn-warning");
            $button.addClass("activated");
            $filters.push(filter);
        }
    }

    function showAll(env) {
        var $tt = $(env).closest(".parent").children('.child').children();
        $tt.removeAttr("hidden");
    }

    $('.passed').click(function (e) {
        doFilter(this, "Passed");
    });

    $('.failed').click(function (e) {
        doFilter(this, "Failed");
    });

    $('.skipped').click(function (e) {
        doFilter(this, "Skipped");
    });

    $('.total').click(function (e) {
        //filterButtonClick(this);
        showAll(this);
    });

    $(".btnexp").click(function (e) {
        $(this).closest(".parent").children('.child').toggle();
    });
    $('.btnlog').click(function (e) {
        $(this).closest(".accordion").find('.log').toggle();
    });

});

