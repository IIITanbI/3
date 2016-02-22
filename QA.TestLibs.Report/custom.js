$(function () {
	
	
	
    $filters = ["total"];


    function doFilter(button) {
		
        var $childs = $(button).closest(".parent").children('.child').children();
        var $needClass = "status";
		
		
		if ($filters.length > 0 && $filters[$filters.length - 1] === "total"){
			$childs.removeAttr("hidden");
			
			var $filterButtons = $(button).closest(".table").find("button[class*='filter']");
			$filterButtons.each(function(index){
				console.log(this);
				deactivateButton(this);
			});
			activateButton(button);
			return;
		}
		else {
			$totalButton = $(button).closest(".table").find(".filter-total");
			deactivateButton($totalButton);
		}
		
        for (var i = 0; i < $childs.length; i++) {
            var $panelHeading = $($childs[i]).find('.panel-heading')[0];
            var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
            var $actionName = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();


            if ($.inArray($actionName, $filters) === -1) {
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
		console.log(button);
		console.log($(button));
		console.log($(button).attr('class')	);
		var classList =$(button).attr('class').split(' ');

		
		for(var i = 0; i < classList.length; i++){
			if (classList[i].match("filter-*")){
				filter = classList[i].substr("filter-".length);
				break;
			}
		}
		console.log("filter = " + filter);
		return filter;
	}

	function activateButton(button){
		var $button = $(button);
		$button.removeClass("btn-info");
        $button.addClass("btn-warning");
        $button.addClass("activated");
        $filters.push(getFilterFromButton(button));
	}
	
	function deactivateButton(button){
		var $button = $(button);
		$button.removeClass("activated");
		$button.removeClass("btn-warning");
		$button.addClass("btn-info");
		
		var num = $.inArray(getFilterFromButton(button), $filters);
		if (num === -1)
			num = $filters.length;

		$filters.splice(num, 1);
	}
	
 
	
	$("button[class*='filter']").click(function(e) {
		filterButtonClick(this);
	});


  

    $(".btnexp").click(function (e) {
		$(this).closest(".parent").children('.child').toggle();
    });
    $('.btnlog').click(function (e) {
        $(this).closest(".accordion").find('.log').toggle();
    });

});

