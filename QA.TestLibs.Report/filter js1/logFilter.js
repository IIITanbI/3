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
    
	myFilter.getDeactivatedColor = function (button) {
        var filter = myFilter.getFilterFromButton(button);
		var str = filter.join(" ");
        switch (str) {
            case "trace debug warn info error":
                return "primary";
            case "debug warn info error":
                return "success";
            case "warn info error":
                return "warning";
            case "info error":
                return "info";
            case "error":
                return "danger";
            default:
                return "info";
        }
    }

	$(".logexp").css("background-image", "url(expander.png)");
	
	$(".logexp").click(function(e){
		var $cur = $(e.currentTarget);
		var $elem = $(this).closest(".logPanel").find(".log-filter-btns");
		$elem.toggle(300, function onCompleteToggle(){
			if ($elem.is(":visible")){
				$cur.css("background-image", "url(expander-off.png)");
				console.log("visible");
			} else {
				$cur.css("background-image", "url(expander.png)");
				console.log("none");
			}
		});
	});
	
    $("button[class*='log-filter']").click(function (e) {
        myFilter.filterButtonClick(this);
    });
});
