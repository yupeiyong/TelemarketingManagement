$(function () {
    $('input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        increaseArea: '20%'
    }).on('ifChecked', function (event) {
        $(this).val(true);
    }).on('ifUnchecked', function (event) {
        $(this).val(false);
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
                //记录保存结果
                if (data.Success) {
                    $this.closest('.bootstrap-dialog').modal('hide');
                } else {
                    layer.msg(data.Message, { time: 5000 });
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

});