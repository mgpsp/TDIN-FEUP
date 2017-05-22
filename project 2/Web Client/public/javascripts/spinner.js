(function ($) {
    $('.spinner .btn:first-of-type').on('click', function(event) {
        var el=$(this).parents('.spinner').children('input');
        var maxAllowed = el.attr("max");
        var minAllowed = el.attr("min");
        var step = el.attr("step");

        var previousVal = el.val();
        if (!previousVal) {
            previousVal = 0;
        }

        var auxValue = 0;
        if (step) {
            step = parseFloat(step);
            if (step === Number(step) && step % 1 !== 0) {
                // is double
                auxValue = parseFloat(previousVal) + step;
            } else {
                // Is int
                auxValue = parseInt(previousVal, 10) + step;
            }
        } else {
            step=1;
            auxValue = parseInt(previousVal, 10) + step;
        }
        if (maxAllowed && auxValue>parseFloat(maxAllowed)) {
            auxValue = maxAllowed;
        }
        if (minAllowed && auxValue<parseFloat(minAllowed)) {
            auxValue = minAllowed;
        }
        el.val(auxValue);
    });
    $('.spinner .btn:last-of-type').on('click', function(e) {
        var el=$(this).parents('.spinner').children('input');
        var minAllowed = el.attr("min");
        var maxAllowed = el.attr("max");
        var step = el.attr("step");

        var auxValue = 0;
        var decimalDigits = 0;

        var previousVal = el.val();
        if (!previousVal) {
            previousVal = 0;
        }
        if (step) {
            step = parseFloat(step);
            debugger;
            if (step === Number(step) && step % 1 !== 0) {
                // is double
                auxValue = parseFloat(previousVal) - step;
                var strStep = ""+step;
                decimalDigits = (strStep).split(".")[1].length;
                auxValue = auxValue.toFixed(decimalDigits);
            } else {
                // Is int
                auxValue = parseInt(previousVal, 10) - step;
            }
        } else {
            step=1;
            auxValue = parseInt(previousVal, 10) - step;
        }
        if (maxAllowed && auxValue>parseFloat(maxAllowed)) {
            auxValue = maxAllowed;
        }
        if (minAllowed && auxValue<parseFloat(minAllowed)) {
            auxValue = minAllowed;
        }
        el.val(auxValue);
    });
})(jQuery);