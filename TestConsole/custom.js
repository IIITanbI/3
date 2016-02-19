$(function(){
	$(".btnexp").click(function(e){
		$(this).closest(".parent").children('.child').toggle();
	}); 
	$('.btnlog').click(function(e){
		$(this).closest(".accordion").find('.log').toggle();
	}); 
	

	$('.passed').click(function(e){
		filter(this, "Passed");
	}); 

	$('.failed').click(function(e){
		filter(this, "Failed");
	}); 
	$('.skipped').click(function(e){
		filter(this, "Skipped");
	}); 

	$('.total').click(function(e){
		showAll(this);
	}); 


	function filter(env, filter){
		$tt = $(env).closest(".parent").children('.child').children();
		for(i=0;i < $tt.length; i++) {
			$dd = $($tt[i]).find('.panel-heading')[0];
			$zz = $($dd).children("p[class*='status']").attr('class');
			if ($zz.indexOf(filter) == -1){
				$($tt[i]).attr("hidden","");
			} else {
				$($tt[i]).removeAttr("hidden");	
			}
		}
	}

	function showAll(env){
		$tt = $(env).closest(".parent").children('.child').children();
		$tt.removeAttr("hidden");
	}

});


