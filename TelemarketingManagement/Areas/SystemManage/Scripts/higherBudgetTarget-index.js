$(function () {
    "use strict";

    window.operateFormatter = function (value, row, index) {
        return [
            "<a href=\"#\" class=\"u-edit\" data-u-url=\"/Admin/Manager/HigherBudgetTarget/Edit/" + value + "\">",
                "编辑",
                "<i class=\"fa fa-pencil\" aria-hidden=\"true\"></i>",
            "</a>",
            "<a href=\"#\" class=\"u-delete\" data-u-url=\"/Admin/Manager/HigherBudgetTarget/Remove/" + value + "\">",
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

            var title = "新增-上级专款指标";
            var isModify = $this.hasClass("u-edit");
            if (isModify) {
                var $tr = $this.closest("tr");
                var rowId = $tr[0].id;
                if (!rowId || rowId.length <= 0) {
                    return false;
                }
                title = "编辑 " + rowId;
            }

            //获取链接路径
            var url = $(this).data("u-url");
            //弹窗显示详情
            BootstrapDialog.show({
                title: title,
                size: BootstrapDialog.SIZE_WIDE,
                closable: true,
                closeByBackdrop: false,
                closeByKeyboard: true,
                draggable: true,
                message: function (dialog) {
                    var dialogRef = dialog;
                    var $message = $("<div></div>");
                    $message.load(url, function () {
                        var $form = dialogRef.$modalBody.find("form");

                        //添加按钮添加事件处理函数
                        dialogRef.$modalBody.find("#btnSave, #btnSaveContinue").off("click").on("click", function () {
                            //检测验证结果
                            if (!$form.valid()) {
                                $(".form-group input.error")[0].select();
                                $(".form-group input.error")[0].focus();
                                return false;
                            }
                            //当前对象
                            var $this = $(this);
                            //是否关闭编辑窗口
                            var isCloseWindow = isModify ? true : ($this.attr("id") == "btnSave" ? true : false);

                            var $closeBtn = dialogRef.$modalHeader.find("button.close");

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
                                    if (data.Success) {
                                        if (isCloseWindow) {
                                            $this.closest(".bootstrap-dialog").modal("hide");
                                        } else {
                                            $form[0].reset();
                                            $form.find(".easyui-combotree").each(function (e) {
                                                var $e = $(this);
                                                $e.combotree("setValue", "");
                                            });
                                            $form.find("input").each(function (e) {
                                                var $e = $(this);
                                                $e.val("");
                                            });
                                            $form.find(".u-select2").each(function (e) {
                                                var $e = $(this);
                                                $e.val("").trigger("change");
                                            });
                                        }
                                        $table.bootstrapTable("refresh");
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
                    return $message;
                },
                onshow: function (dialogRef) {
                    dialogRef.$modal.removeAttr("tabindex");
                },
                onshown: function (dialogRef) {
                    //打开页面时设置焦点
                    $("input.first").focus().select();
                },
                onhidden: function (dialogRef) {
                    ////编辑窗口所有组合树组件
                    //dialogRef.$modalBody.find(".form-edit .easyui-combotree").each(function () {
                    //    var el = $(this);
                    //    var $tree = el.combotree("tree");
                    //    //删除组合框和包含的树
                    //    $tree.parent().parent().remove();
                    //});
                    $(".form-edit .easyui-combotree").combotree("setValue", {
                        id: "",
                        text: ""
                    });
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
        searchForm: "form.search"
    }).on("load-success.bs.table", function () {
        //处理获取到的数据
        addCustColClick();
    });

    //组合树
    $("form.search .easyui-combotree").each(function () {
        //当前对象
        var $this = $(this);
        var url = $this.data("url");
        $this.combotree({
            editable: true,
            panelHeight: 150,
            url: url,
            textField: "id",
            valueField: "text",
            width: "100%",
            collapseAll: true,
            method: "get",
            keyHandler: {
                query: function (q) {
                    $(this).combotree("options").url = url + "&Keywords=" + q;
                    $(this).combotree("reload");
                    //设置文本输入的值,输入框内容是text,值是id
                    //$(this).combotree("setValue", {
                    //    id: "",
                    //    text: q
                    //});
                },
                enter: function (e) {
                    var p = $(this).combotree("panel");
                    if (p.is(":visible")) {
                        var component = $(this).combotree("tree");
                        var data = $(component).tree("getChildren");
                        var node = component.tree("getSelected");//判断是有选中的数据
                        if (data.length > 0 && !node) {
                            var k = $this.combo("textbox").val();
                            for (var i = 0; i < data.length; i++) {
                                if (data[i].text.indexOf(k) >= 0) {
                                    $this.combotree("setValue", {
                                        id: data[i].id,
                                        text: data[i].text
                                    });

                                    break;
                                }
                            }
                        }
                    }

                    switchFoucus(e);
                    $(this).combotree("hidePanel");
                    //没有提交查询
                },
                up: function () {
                    var el = $(this);
                    var p = el.combotree("panel");
                    if (!p.is(":visible")) {
                        el.combotree("showPanel");
                    }

                    el = el.combotree("tree");
                    var nodes = el.tree("getChildren");
                    if (!nodes.length) {
                        return;
                    }
                    var node = el.tree("getSelected");
                    if (!node) {
                        el.tree("select", nodes[0].target);
                    } else {
                        var idx = nodes.indexOf(node);
                        var i = 1;
                        var indexOf;
                        if (idx > 0) { // && !node.children
                            var preNode = nodes[idx - 1];
                            var father = el.tree("getParent", preNode.target);
                            if (father && father.state === "closed") {
                                i = father.children.length + 1;
                            }
                            indexOf = idx - i;
                        } else {
                            indexOf = nodes.length - 1;
                        }

                        el.tree("select", nodes[indexOf].target);
                        $this.combotree("setValue", {
                            id: nodes[indexOf].id,
                            text: nodes[indexOf].text
                        });

                        el.tree("scrollTo", nodes[indexOf].target);
                    }
                },
                down: function () {
                    var el = $(this);
                    var p = el.combotree("panel");
                    if (!p.is(":visible")) {
                        el.combotree("showPanel");
                    }

                    el = el.combotree("tree");
                    var nodes = el.tree("getChildren");
                    if (!nodes.length) {
                        return;
                    }
                    var node = el.tree("getSelected");
                    if (!node) {
                        el.tree("select", nodes[0].target);
                    } else {
                        var idx = nodes.indexOf(node);
                        var i = 1;
                        var indexOf = idx + i;
                        if (idx < nodes.length) {// && !node.children
                            if (node.state === "closed" && node.children) {
                                i = node.children.length + 1;
                            }

                            indexOf = idx + i;

                            //判断是否是最后一个
                            if (indexOf === nodes.length) {
                                indexOf = 0;
                            }

                            el.tree("select", nodes[indexOf].target);
                            $this.combotree("setValue", {
                                id: nodes[indexOf].id,
                                text: nodes[indexOf].text
                            });
                        }
                        el.tree("scrollTo", nodes[indexOf].target);
                    }
                }
                //    left: function() {//收起节点
                //        var el = $(this);

                //        var p = el.combotree("panel");
                //        if (!p.is(":visible")) { return; }

                //        var t = el.combotree("tree");
                //        var node = t.tree("getSelected");
                //        if (node) {
                //            var isLeaf = t.tree("isLeaf", node.target);
                //            if (!isLeaf && node.state == "open") {
                //                // 如果不是叶子节点, 并且当前是展开状态, 则收缩当前分支
                //                t.tree("collapse", node.target);
                //            } else {
                //                // 如果有父节点, 则收缩父节点, 并且选中父节点
                //                var parent = t.tree("getParent", node.target);
                //                if (parent) {
                //                    t.tree("collapse", parent.target);
                //                    t.tree("select", parent.target);

                //                    // 需要时, 更新textbox的值
                //                    var opts = el.combo("options");
                //                    if (opts.selectOnNavigation) {
                //                        el.combotree("setValue", parent.id);
                //                    }
                //                }
                //            }
                //        }
                //    },
                //    right: function() {//展开节点
                //        var el = $(this);

                //        var p = el.combotree("panel");
                //        if (!p.is(":visible")) { return; }

                //        // 展开当前节点的分支
                //        var t = el.combotree("tree");
                //        var node = t.tree("getSelected");
                //        if (node) {
                //            t.tree("expand", node.target);
                //        }
                //    }
            },
            //增加子节点
            onBeforeExpand: function (row) {
                if (!row.children) {
                    $this.combotree("tree").tree("options").url = url + "&ParentId=" + row.id;
                }
            },
            onLoadSuccess: function (node, data) {
                var t = $this.combotree("tree");
                var k = $this.combo("textbox").val();
                //获取返回条数
                t.tree("collapseAll");

                if (k != null && k !== "") {
                    for (var i = 0; i < data.length; i++) {
                        t.tree("expand", data[i].target); //展开节点
                    }
                }
                //if (data.length === 1) {
                //    var currentData = t.tree("getChildren");
                //    t.tree("expand", currentData[0].target); //展开节点
                //}
            }
        });
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

    $("form.search #btnExport").off("click").on("click", function () {
        var $this = $(this);
        var $form = $this.closest("form");
        var url = $this.data("u-url");
        var originalUrl = $form.attr("action");
        $form.attr("action", url);
        $form.submit();
        $form.attr("action", originalUrl);
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
});

