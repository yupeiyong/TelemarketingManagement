
$(function () {
    ////默认值
    var addDefaultValue = "-- 无 --";

    //转换为select2
    $("form.form-edit .u-select2").each(function () {
        //当前对象
        var $this = $(this);
        var url = $this.data("url");

        //远程筛选
        $this.select2({
            ajax: {
                url: url,
                dataType: "json",
                delay: 250,
                data: function(params) {
                    return {
                        Keywords: params.term,
                        page: params.page
                    };
                },
                processResults: function(data, params) {
                    params.page = params.page || 1;
                    if (!data.IsSuccess) {
                        return false;
                    }
                    var arr = data.Data || data.data || {};
                    arr.unshift({ id: 0, text: addDefaultValue });
                    return {
                        results: arr
                    };
                },
                cache: true
            },
            escapeMarkup: function(markup) { return markup; },
            minimumInputLength: 0,
            language: "zh-CN"
        });
    });

    $('.buttons #btnSave').on('click', function () {
        var $form = $("form.form-horizontal");

        //检测验证结果
        if (!$form.valid()) {
            //设置焦点
            $(".form-group input.error")[0].select();
            $(".form-group input.error")[0].focus();
            return false;
        }
        //当前对象
        var $this = $(this);
        var parentModel = $this.closest('.bootstrap-dialog');
        //关闭按钮X
        var $closeBtn = parentModel.find("button.close");

        var url = $this.data("url");
        //保存
        var data = $form.serializeArray();
        var btnOriginalText = $this.text();

        $.ajax({
            url: url,
            type: "json",
            data: data,
            beforeSend: function () {
                $this.attr("disabled", true);
                $closeBtn.attr("disabled", true);
                $this.text("保存中...");
            },
            success: function (data) {
                layer.msg(data.Message, { time: 5000 });
                //记录保存结果
                if (data.Success) {
                    $this.closest('.bootstrap-dialog').modal('hide');
                } 
            },
            error: function (xhr, error, errThrow) {
                layer.msg(errThrow, { time: 5000 });
            },
            complete: function (msg, textStatus) {
                $this.attr("disabled", false);
                $closeBtn.attr("disabled", false);
                $this.text(btnOriginalText);
            }
        });
        //不执行提交动作
        return false;
    });


    //输入框焦点事件
    $("input").focus(function () {
        $(this).select();
    });


    $(".form-edit input").keydown(function (e) {
        if (e.keyCode == 13) {
            switchFoucusEdit(e);
        }
    });

    $(".form-edit textarea").keydown(function (e) {

        if (e.keyCode == 13) {
            switchFoucusEdit(e);
        }
    });

    $('.first').focus().select();

});


//回车键焦点切换
function switchFoucusEdit(e) {
    var arr = [];
    $(".form-edit .form-group").each(function () {
        var input = $(this).find("input");
        var select = $(this).find("select");
        var textarea = $(this).find("textarea");

        var item;
        if (input.length > 0) {
            //for循环用于easyui多个input的情况
            if (input.length > 1) {
                for (var i = 0; i < input.length; i++) {
                    if ($(input[i]).attr("type") !== "hidden" && !$(input[i]).hasClass("hidden-id") && $(input[i]).attr("id") !== "")
                        item = input[i];
                }
            } else {
                if ($(input[0]).attr("type") !== "hidden")
                    item = input[0];
                else
                    return false;
            }
        } else if (select.length > 0) {
            item = select[0];
        } else if (textarea.length > 0) {
            item = textarea[0];
        }

        if (item)
            arr.push(item);
    });

    //easyui
    var currentObj = e.target;
    var idx = arr.indexOf(currentObj);
    if (idx === arr.length - 1) {// 判断是否是最后一个输入框
        var firstButton = $(".form-edit button").first();
        $(firstButton).focus();
        return false;
    } else {
        var next = idx + 1;

        if (arr[next].tagName == "SELECT") {//tagName的值是大写的
            $(arr[next]).select2("open"); //打开select2面板式
        } else {
            arr[next].focus(); // 设置焦点
            arr[next].select(); // 选中文字
        }
    }

    return false;
}

//按钮鼠标事件
$(".form-edit button").keydown(function (e) {
    var buttons = $(".form-edit button");
    var idx = buttons.index(e.target);

    if (e.keyCode === 13) {
        $(this).click();
    }
    if (e.keyCode === 37 && idx === 1) { //左
        $(buttons)[idx - 1].focus();
    }
    if (e.keyCode === 39 && idx === 0) { //右
        $(buttons)[idx + 1].focus();
    }
});

function array_difference(a, b) { // 差集 a - b
    //clone = a
    var clone = a.slice(0);
    for (var i = 0; i < b.length; i++) {
        var temp = b[i];
        for (var j = 0; j < clone.length; j++) {
            if (temp === clone[j]) {
                //remove clone[j]
                clone.splice(j, 1);
            }
        }
    }
    return array_remove_repeat(clone);
}

function array_remove_repeat(a) { // 去重
    var r = [];
    for (var i = 0; i < a.length; i++) {
        var flag = true;
        var temp = a[i];
        for (var j = 0; j < r.length; j++) {
            if (temp === r[j]) {
                flag = false;
                break;
            }
        }
        if (flag) {
            r.push(temp);
        }
    }
    return r;
}

function array_intersection(a, b) { // 交集
    var result = [];
    for (var i = 0; i < b.length; i++) {
        var temp = b[i];
        for (var j = 0; j < a.length; j++) {
            if (temp === a[j]) {
                result.push(temp);
                break;
            }
        }
    }
    return array_remove_repeat(result);
}

function array_union(a, b) { // 并集
    return array_remove_repeat(a.concat(b));
}
