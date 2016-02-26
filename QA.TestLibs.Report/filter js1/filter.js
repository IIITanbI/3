var FILTER = {
    className: "",
    currentButton: {},
	defaultButton: {},
	multiSelect: true,
    activatedClassName: "activated"
};

FILTER.prepare = function(button) {
    var $filterButtons = this.getFilterButtons(button);
    var $totalButton = this.getTotalButton(button);

    this.deactivateButtons($filterButtons, $totalButton);
    this.activateButton($totalButton);
};

FILTER.filterButtonClick = function (button) {

    this.currentButton =  $(button);
    this.deactivateButton(button) || this.activateButton(button);

    var $filterButtons = this.getFilterButtons(button);
    var $totalButton = this.getTotalButton(button);

    if (!this.multiSelect)  {
        if (!this.isActive(button)){
            this.activateButton($totalButton);
            this.deactivateButtons($filterButtons, $totalButton);
        }
        else {
            this.deactivateButtons($filterButtons, button);
        }
    }
    else {
        if (!$(button).is($totalButton)){
            this.deactivateButton($totalButton);
        }
        else {
            this.deactivateButtons($filterButtons, $totalButton);
        }
    }

    this.doFilter(button);
};

FILTER.activateButton = function (button) {
    var $button = $(button);
    if (this.isActive(button)) return false;

    $button.removeClass(this.getDeactivatedClass(button));
    $button.addClass(this.getActivatedClass(button));
    $button.addClass(this.activatedClassName);
    return true;
};

FILTER.deactivateButton = function (button) {
    var $button = $(button);
    if (!this.isActive(button)) return false;

    $button.removeClass(this.activatedClassName);
    $button.removeClass(this.getActivatedClass(button));
    $button.addClass(this.getDeactivatedClass(button));
    return true;
};

FILTER.deactivateButtons = function(buttons, excludeButton){
    var thisObj = this;
    if (excludeButton === undefined || excludeButton === null){
        $.each(buttons, function(i, value){
            thisObj.deactivateButton(value);
        });
        return;
    }
    $.each(buttons, function(i, value){
        if (!$(value).is($(excludeButton)))
            thisObj.deactivateButton(value);
    });
};

FILTER.getDeactivatedClass = function (button) {
    return "btn-info";
};
FILTER.getDeactivatedStyleAttribute = function (button) {
    return {};
};

FILTER.getActivatedClass = function (button) {
    return "btn-warning";
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

FILTER.getChilds = function (button) { return $() };
FILTER.getFilterButtons = function (button) {return $() };
FILTER.getTotalButton = function (button) {return $() };
FILTER.getChildStatus = function (child) { return $()};


FILTER.isActive = function(button){
    return($(button).hasClass(this.activatedClassName));
};

FILTER.doFilter = function (button) {
    var $filters = [];
    var $filterButtons = this.getFilterButtons(button);
	var $totalButton = this.getTotalButton(button);
    var thisObj = this;

    $filterButtons.each(function (index, item) {
        if (thisObj.isActive(item)) {
            $.merge($filters, thisObj.getFilterFromButton(item));
        }
    });
    if ($filters.length === 0) {
        $totalButton.click();
        return;
    }

    var $childs = this.getChilds(button);
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










