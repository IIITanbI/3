﻿$(function () {
	
    function doFilter(button) {
		
        var $childs = $(button).closest(".parent").children('.child').children();
        var $needClass = "status";
		
		var $filters = []
		var $filterButtons = $(button).closest(".table").find("button[class*='filter']");
		
		$filterButtons.each(function(index, item){
			if ($(item).hasClass("activated")){
				$filters.push(getFilterFromButton(item));
			}
			//console.log(index);
			//console.log(item);
		});
		 
		
		if (getFilterFromButton(button) === "total" || $filters.length === 0){
			$childs.removeAttr("hidden");
			
			var $filterButtons = $(button).closest(".table").find("button[class*='filter']");
			$filterButtons.each(function(index){
				console.log(this);
				deactivateButton(this);
			});
			var totalButton = $(button).closest(".table").find(".filter-total");
			activateButton(totalButton);
			return;
		}
		
		$totalButton = $(button).closest(".table").find(".filter-total");
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
	
	function getFilterFromButton(button){
		var filter = null;
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
	}
	
	function deactivateButton(button){
		var $button = $(button);
		$button.removeClass("activated");
		$button.removeClass("btn-warning");
		$button.addClass("btn-info");
	}
	
 
	
	$("button[class*=' filter']").click(function(e) {
		filterButtonClick(this);
	});


  

    $(".btnexp").click(function (e) {
		$(this).closest(".parent").children('.child').toggle();
    });
    $('.btnlog').click(function (e) {
        $(this).closest(".accordion").find('.logPanel').toggle();
    });

});
