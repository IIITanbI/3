$(function () {

    $('.btnexp').click(function (e) {
        $(this).closest('.parent').children('.child').toggle();
    });

    $('.btnlog').click(function (e) {
        $(this).closest('.accordion').find('.log').toggle();
    });

    $('.passed').click(function (e) {
        $tt = $(this).closest(".parent").children('.child').children();
        for (i = 0; i < $tt.length; i++) {
            $dd = $($tt[i]).find('.panel-heading')[0];
            $zz = $($dd).children("p[class*='status']").attr('class');
            if ($zz.indexOf("Passed") == -1) {
                $($tt[i]).attr("hidden", "");
            }
        }
    });

    $('.failed').click(function (e) {
        $tt = $(this).closest(".parent").children('.child').children();
        for (i = 0; i < $tt.length; i++) {
            $dd = $($tt[i]).find('.panel-heading')[0];
            $zz = $($dd).children("p[class*='status']").attr('class');
            if ($zz.indexOf("Failed") == -1) {
                $($tt[i]).attr("hidden", "");
            }
        }
    });

    $('.skipped').click(function (e) {
        $tt = $(this).closest(".parent").children('.child').children();
        for (i = 0; i < $tt.length; i++) {
            $dd = $($tt[i]).find('.panel-heading')[0];
            $zz = $($dd).children("p[class*='status']").attr('class');
            if ($zz.indexOf("Skipped") == -1) {
                $($tt[i]).attr("hidden", "");
            }
        }
    });

    $('.total').click(function (e) {
        $tt = $(this).closest(".parent").children('.child').children();
        $tt.removeAttr("hidden");
    });

    $('.total').click(function (e) {
        $tt = $(this).closest(".parent").children('.child').children();
        $tt.removeAttr("hidden");
    });
});