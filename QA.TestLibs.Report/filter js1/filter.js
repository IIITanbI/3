var FILTER = {
    className: "",
    currentButton: {},
	defaultButton: {},
	multiSelect: true
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
    var filter = [];
    var matches = $(button).attr('class').match(new RegExp(this.className + '\\w*', 'g'));
	 
	for (var i = 0; i < matches.length; i++){
		filter.push(matches[i].substr(this.className.length));
	}

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
	var $totalButton = this.getTotalButton(button);
		
    var thisObj = this;
	if (!thisObj.multiSelect)  {
		if (!$(button).hasClass("activated")){
			this.activateButton($totalButton);
			$filters = thisObj.getFilterFromButton($totalButton);
			for (var i = 0; i < $filterButtons.length; i++) {
				if (!$($filterButtons[i]).is($totalButton))
					this.deactivateButton($filterButtons[i]);
			}
		}
		else {
			for (var i = 0; i < $filterButtons.length; i++){
				if (!$($filterButtons[i]).is($(button)))
					this.deactivateButton($filterButtons[i]);
			}
			$filters = thisObj.getFilterFromButton(button);
		}
	}
	else {
		$filterButtons.each(function (index, item) {
			if ($(item).hasClass("activated")) {
				if ($(item).is($totalButton)) return;
				$.merge($filters, thisObj.getFilterFromButton(item));
			}
		});
		
		if ($filters.length === 0 || $(button).is($totalButton)) {
			this.activateButton($totalButton);
			$filters = thisObj.getFilterFromButton($totalButton);
			for (var i = 0; i < $filterButtons.length; i++) {
				if (!$($filterButtons[i]).is($totalButton))
					this.deactivateButton($filterButtons[i]);
			}
		}
		else this.deactivateButton($totalButton);
	}
    for (var i = 0; i < $childs.length; i++) {
        var $child = $($childs[i]);
		var $status = this.getChildStatus($child);
		
		var css = $child.css("display");
        if ($.inArray($status, $filters) === -1) {
			if (css == "block"){
				$child.slideToggle();
			}
        } else {
			if (css == "none"){
				$child.slideToggle();
			}
        }
    }
};










