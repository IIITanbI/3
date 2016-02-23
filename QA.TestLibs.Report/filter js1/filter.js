var FILTER = {
    className: "",
    currentButton: {}
};

FILTER.filterButtonClick = function (button) {
    var $button = $(button);
    this.currentButton = $button;
    if ($button.hasClass("activated")) {
        this.deactivateButton(button);
    }
    else {
        this.activateButton(button);
    }

    this.doFilter(button);
};

FILTER.activateButton = function (button) {
    var $button = $(button);
    $button.removeClass("btn-" + this.getColor(this.getFilterFromButton(button)));
    $button.addClass("btn-warning");
    $button.addClass("activated");
};

FILTER.deactivateButton = function (button) {
    var $button = $(button);
    $button.removeClass("activated");
    $button.removeClass("btn-warning");
    $button.addClass("btn-" + this.getColor(this.getFilterFromButton(button)));
};

FILTER.getColor = function (filter) {
    return "info";
};

FILTER.getFilterFromButton = function (button) {
    var filter = "";
    //var classList = $(button).attr('class').split(' ');

    var matches = $(button).attr('class').match(new RegExp(this.className + '\\w*', 'g'));
    if (matches.length > 0)
        filter = matches[0].substr(this.className.length);

    //for (var i = 0; i < classList.length; i++) {
    //    if (classList[i].match(this.className + "*")) {
    //        filter = classList[i].substr(this.className.length);
    //        //filter.push(classList[i].substr("filter-".length));
    //        break;
    //    }
    //}
    console.log("filter = " + filter);
    return filter;
};

FILTER.getChilds = function (button) { };
FILTER.getFilterButtons = function (button) { };
FILTER.getTotalButton = function (button) { };
FILTER.getChildStatus = function (child) { };

FILTER.doFilter = function (button) {
    var $childs = this.getChilds(button);
    var $needClass = "status";

    var $filters = [];
    var $filterButtons = this.getFilterButtons(button);

    var thisObj = this;
    $filterButtons.each(function (index, item) {
        if ($(item).hasClass("activated")) {
            $filters.push(thisObj.getFilterFromButton(item));
            //$.merge( $filters,  getFilterFromButton(item) );
        }
        //console.log(index);
        //console.log(item);
    });

    var $totalButton = this.getTotalButton(button);

    if ($(button).is($totalButton) || $filters.length === 0) {
        $childs.removeAttr("hidden");

        $filterButtons.each(function (index, item) {
            console.log(this);
            thisObj.deactivateButton(this);
        });
        this.activateButton($totalButton);
        return;
    }
    this.deactivateButton($totalButton);

    for (var i = 0; i < $childs.length; i++) {
        var $status = this.getChildStatus($childs[i]);


        if ($.inArray($status, $filters) === -1) {
            $($childs[i]).attr("hidden", "");
        } else {
            $($childs[i]).removeAttr("hidden");
        }
    }
};










