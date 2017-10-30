function ResetTable(divId, parentpadding) {
    var newheight = parseInt($(window).height()) - parentpadding;
    $("#" + divId + "").css("height", newheight);
}