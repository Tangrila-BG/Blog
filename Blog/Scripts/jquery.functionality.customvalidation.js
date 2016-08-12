(function ($) {
    jQuery.validator.addMethod("notequalto", function (value, element, params) {
        if (this.optional(element)) return true;

        var props = params.split(',');

        for (var i in props) {
            if ($('#' + props[i]).val() == value) return false;
        }

        return true;
    });

    jQuery.validator.unobtrusive.adapters.addSingleVal("notequalto", "otherproperties");
}(jQuery));