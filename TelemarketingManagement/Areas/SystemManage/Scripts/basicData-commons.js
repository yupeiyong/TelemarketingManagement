$(function () {
    setTimeout(function () {
        $(".first").focus().select();
    },500);

    $("input").click(function () {
        $(this).focus().select();
    });
});

//回车键焦点切换
function switchFoucusEdit(e) {
    var arr = [];
    $(".form-horizontal .form-group").each(function () {
        var input = $(this).find("input");

        var item;
        if (input.length > 0) {
            //for循环用于form-group有多个input的情况下
            if (input.length > 1) {
                for (var i = 0; i < input.length; i++) {
                    if ($(input[i]).attr("type") !== "hidden")
                        item = input[i];
                }
            } else {
                if ($(input[0]).attr("type") !== "hidden")
                    item = input[0];
                else
                    return false;
            }
        }

        if (item)
            arr.push(item);
    });

    //easyui
    var currentObj = e.target;
    var idx = arr.indexOf(currentObj);
    if (idx == arr.length - 1) {// 判断是否是最后一个输入框
        //if (confirm("最后一个输入框已经输入,是否提交?")) // 用户确认
        //$("#btnSave").click(); // 提交表单
        $("#btnSave").focus();
    } else {
        var next = idx + 1;
        arr[next].focus(); // 设置焦点
        arr[next].select(); // 选中文字
    }

    return false;
}

//input回车按键
$(".form-horizontal input").keydown(function (e) {
    if (e.keyCode == 13) {
        switchFoucusEdit(e);
    }
});

//空格选中/取消cheackBox
$(".form-horizontal input[type=checkbox]").keydown(function (e) {
    if (e.keyCode == 32) {
        if ($(this).is(":checked")) {
            $(this).checked = false;
        } else {
            $(this).checked = true;
        }
    }
});