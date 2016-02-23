$(function () {

    function doFilter(button) {

        var $childs = $(button).closest(".logPanel").children('.log').children();
        var $needClass = "status";

		var $filters = []
		var $filterButtons = $(button).closest(".table").find("button[class*='log-filter']");

		$filterButtons.each(function(index, item){
			if ($(item).hasClass("activated")){
				$filters.push(getFilterFromButton(item));
			}
			//console.log(index);
			//console.log(item);
		});

		var $totalButton = $(button).closest(".table").find(".log-filter-total");

		if (getFilterFromButton(button) === "total" || $filters.length === 0){
		    $childs.removeAttr("hidden");

			var $filterButtons = $(button).closest(".table").find("button[class*='log-filter']");
			$filterButtons.each(function(index){
				console.log(this);
				deactivateButton(this);
			});
			activateButton($totalButton);
			return;
		}
		deactivateButton($totalButton);

        for (var i = 0; i < $childs.length; i++) {
            var $status = $($childs[i]).children("span").text().toLowerCase();

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

	function getFilterFromButton(button){
		var filter = null;
		var classList = $(button).attr('class').split(' ');

		for(var i = 0; i < classList.length; i++){
			if (classList[i].match("log-filter-*")){
				filter = classList[i].substr("log-filter-".length);
				break;
			}
		}
		console.log("log filter = " + filter);
		return filter;
	}

	function activateButton(button){
		var $button = $(button);
		$button.removeClass("btn-info");
        $button.addClass("btn-warning");
        $button.addClass("activated");
	}

	function deactivateButton(button){
		var $button = $(button);
		$button.removeClass("activated");
		$button.removeClass("btn-warning");
		$button.addClass("btn-info");
	}

	$("button[class*='log-filter']").click(function(e) {
		filterButtonClick(this);
	});
});
