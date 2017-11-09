$('#filter-modal button.drpicker').each(function (i, elem) {
    var $elem = $(elem),
        dateFormat = 'DD/MM/YYYY',
        $beg = $elem.siblings("input[id$='hdnBegin']"),
        $end = $elem.siblings("input[id$='hdnEnd']"),
        s = moment($beg.val(), dateFormat),
        e = moment($end.val(), dateFormat),
        startDate = s.isValid() ? s : moment().subtract(1, 'month').startOf('month'),
        endDate = e.isValid() ? e : moment().endOf('month');
    $beg.val(startDate.format(dateFormat));
    $end.val(endDate.format(dateFormat));
    $(elem).daterangepicker({
        "locale": {
            "format": dateFormat
        },
        "ranges": {
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'This Year': [moment().startOf('year'), moment()],
            'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        },
        "startDate": startDate,
        "endDate": endDate,
        "maxDate": moment(),
        "opens": "right",
        "drops": "down"
    }, function (start, end) {
        $(this.element[0]).siblings("input[id$='hdnBegin']").val(start.format('DD/MM/YYYY'));
        $(this.element[0]).siblings("input[id$='hdnEnd']").val(end.format('DD/MM/YYYY'));
    });
});
$(function () {
    //Fix for IE page transitions
    $("body").removeClass("hold-transition");
});