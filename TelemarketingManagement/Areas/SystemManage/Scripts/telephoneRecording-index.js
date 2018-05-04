$(function () {
    "use strict";

    var addDefaultValue = "-- 无 --";

    //转换为select2
    $("form.search .u-select2").each(function () {
        //当前对象
        var $this = $(this);
        var url = $this.data("url");

        //远程筛选
        $this.select2({
            ajax: {
                url: url,
                dataType: "json",
                delay: 250,
                data: function (params) {
                    return {
                        Keywords: params.term,
                        page: params.page
                    };
                },
                processResults: function (data, params) {
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
            escapeMarkup: function (markup) { return markup; },
            minimumInputLength: 0,
            language: "zh-CN"
        });
    });


    window.operateFormatter = function (value, row, index) {
        return [
            "<a href=\"#\" class=\"u-edit\" data-u-url=\"/SystemManage/TelephoneRecording/Edit/" + value + "\">",
                "编辑",
                "<i class=\"fa fa-pencil\" aria-hidden=\"true\"></i>",
            "</a>",
            "<a href=\"#\" class=\"u-delete\" data-u-url=\"/SystemManage/TelephoneRecording/Remove/" + value + "\">",
                "删除",
                "<i class=\"fa fa-trash\" aria-hidden=\"true\"></i>",
            "</a>"
        ].join("");
    };

    var $table = $("#tab");
    //为自定义列设置事件
    function addCustColClick() {

        //编辑
        $("#btnAdd, .table .u-edit").off("click").on("click", function () {
            var $this = $(this);

            var title = "新增-采访录音记录";
            var isModify = $this.hasClass("u-edit");
            if (isModify) {
                var $tr = $this.closest("tr");
                var rowId = $tr[0].id;
                if (!rowId || rowId.length <= 0) {
                    return false;
                }
                title = "修改采访录音记录  " + rowId;
            }

            //获取链接路径
            var url = $(this).data("u-url");

            //打开编辑页
            BootstrapDialog.show({
                title: title,
                size: BootstrapDialog.SIZE_WIDE,
                closable: true,
                closeByBackdrop: false,
                closeByKeyboard: true,
                draggable: true,
                message: $('<div></div>').load(url),
                onshow: function (dialogRef) {
                    dialogRef.$modal.removeAttr('tabindex');
                },
                onshown: function (dialogRef) {
                    //打开页面时设置焦点
                    $("input.first").focus().select();
                },
                onhidden: function (dialogRef) {
                    $table.bootstrapTable('refresh');
                }
            });

            return false;
        });
        //删除
        $(".table .u-delete").off("click").on("click", function (e) {
            //不执行默认行为
            e.preventDefault();
            //当前对象
            var $this = $(this);

            var $tr = $this.closest("tr");
            var rowId = $tr[0].id;
            if (!rowId || rowId.length <= 0) {
                return false;
            }

            //获取链接路径
            var url = $this.data("u-url");
            //确认删除
            BootstrapDialog.confirm({
                title: "删除",
                message: "确认删除第" + rowId + "条记录吗?",
                type: BootstrapDialog.TYPE_WARNING,
                closable: true,
                draggable: true,
                btnCancelLabel: "取消",
                btnOKLabel: "确认",
                btnOKClass: "btn-warning",
                callback: function (result) {
                    if (result) {
                        $.post(url, null, function (data) {
                            layer.msg(data.Message, { time: 5000 });
                            if (data.Success) {
                                var $this = $(this);
                                $this.closest(".bootstrap-dialog").modal("hide");
                                $table.bootstrapTable("refresh");
                            }
                        });
                    } else {
                        var $this = $(this);
                        $this.closest(".bootstrap-dialog").modal("hide");
                    }
                }
            });
            return false;
        });
    }

    $table.extendBootstrapTable({
        searchButton: "#btnSearch",
        searchForm: "form.search",
        checkboxHeader: true,  //表头是否显示复选框
        clickToSelect: true,  //点击行即可选中单选/复选框  
        singleSelect: false
    }).on("load-success.bs.table", function () {
        //处理获取到的数据
        addCustColClick();
    });



    $("form.search #btnClear").off("click").on("click", function () {
        var $this = $(this);
        var $form = $this.closest("form");
        $form[0].reset();
        $form.find(".easyui-combotree").each(function (e) {
            var $e = $(this);
            $e.combotree("setValue", "");
        });
        $table.bootstrapTable("refresh");
    });

    //回车键焦点切换
    function switchFoucus(e) {
        var idx;

        var current = e.target;
        var inputs = $(".conditional input[type!=hidden]");
        idx = inputs.index(current);
        if ($(current).attr("name") === "Keywords") {
            idx = inputs.index(current);
        }
        if (idx == inputs.length - 1) {// 判断是否是最后一个输入框
            if (confirm("最后一个输入框已经输入,是否提交?")) // 用户确认
                //$("form[name='contractForm']").submit(); // 提交表单
                alert("submit");
        } else {
            inputs[(idx + 1)].focus(); // 设置焦点
            inputs[(idx + 1)].select(); // 选中文字
        }
    }

    //日期回车按键
    $("form.search input[type=date]").keydown(function (e) {
        if (e.keyCode == 13) {
            switchFoucus(e);
        }
    });

    //输入框焦点事件
    $("form.search input").focus(function () {
        $(this).select();
    });

    //失去焦点
    $("form.search input.textbox-text").blur(function () {
        var k = $(this).parents(".form-group").find("input.textbox-value").val();
        if (!k) {
            //$(this).val("");
            $(this).parents(".form-group").find("select").combotree("setValue", {
                id: "",
                text: ""
            });
        }
    }).bind('input propertychange', function () {//检测input框输入值，解决combotree输入掉文字问题
        $(this).parents(".form-group").find("select").combotree("setValue", {
            id: "",
            text: $(this).val()
        });
    });

    $("form.search input.textbox-text").first().focus();

    $("form.search #btnBatchRemove").off("click").on("click", function () {
        var $this = $(this);

        var selectedRows = $table.bootstrapTable('getAllSelections');
        if (selectedRows == null || selectedRows.length == 0)
            return false;

        var ids = [];
        for (var i = 0; i < selectedRows.length; i++) {
            var id = selectedRows[i].Id;
            ids.push(id);
        }

        var idsJsonStr = JSON.stringify(ids);

        //获取链接路径
        var url = $this.data("u-url");
        //确认删除
        BootstrapDialog.confirm({
            title: "删除",
            message: "确认删除选中的" +ids.length  + "条的记录吗?",
            type: BootstrapDialog.TYPE_WARNING,
            closable: true,
            draggable: true,
            btnCancelLabel: "取消",
            btnOKLabel: "确认",
            btnOKClass: "btn-warning",
            callback: function (result) {
                if (result) {
                    $.post(url, {ids:idsJsonStr}, function (data) {
                        layer.msg(data.Message, { time: 5000 });
                        if (data.Success) {
                            var $this = $(this);
                            $this.closest(".bootstrap-dialog").modal("hide");
                            $table.bootstrapTable("refresh");
                        }
                    });
                } else {
                    var $this = $(this);
                    $this.closest(".bootstrap-dialog").modal("hide");
                }
            }
        });
        return false;
    });

});

