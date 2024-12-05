$().ready(function () {

    // 엔터키 이벤트 (ent 요소 유무 확인)
    $("input[ent]").keypress(function (e) {
        if (e.keyCode == 13) {
            eval($(this).attr("ent") + "();");
            return false;
        }
    });

    // 드랍다운 이벤트 (pgcnt 요소 유무 확인)
    $("select[pgcnt]").on("change", function () {

        var p = $(this).find(":selected").val()

        $("#frmPage > #frmPage").val(1)
        $("#frmPage > #frm" + $(this).attr("id")).val(p);

        $("#frmPage").submit();

    })

})

