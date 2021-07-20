jQuery(document).ready(function ($) {

    //navbar pop-up
    $("#floatbar").mouseenter(function (e) {
        e.preventDefault();
        $(this).find(".popup").fadeIn("slow");
    });
    $("#floatbar").mouseleave(function (e) {
        e.preventDefault();
        $(this).find(".popup").fadeOut("slow");
    });


    // Mask for phone number
    $(function () {
        $('[mask]').each(function (e) {
            $(this).mask($(this).attr('mask'));
        });
    });

    // date picker for all date fields
    $(function () {
        $(".datepicker").datepicker();
        $('.datepicker').datepicker().datepicker('setDate', 'today');
    });


    //draggable modal
    $(".modal").draggable({
        handle: ".modal-header"
    });
});