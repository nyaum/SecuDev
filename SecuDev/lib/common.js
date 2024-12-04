$().ready(function () {

    //엔터키 이벤트
    $("input[ent]").keypress(function (e) {
        if (e.keyCode == 13) {
            eval($(this).attr("ent") + "();");
            return false;
        }
    });

})

