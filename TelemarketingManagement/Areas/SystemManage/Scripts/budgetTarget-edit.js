$(function () {
    //默认选择
    $("input[name='IsBeginningOfYear']").iCheck({
        checkboxClass: "icheckbox_square-blue",
        increaseArea: "20%"
    }).on("ifChecked", function (event) {
        $(this).val(true);
        checkDataChange();
    }).on("ifUnchecked", function (event) {
        $(this).val(false);
        checkDataChange();
    });

    $('.buttons #btnSave,.buttons #btnSaveContinue').on('click', function () {
        var $form = $("form.form-horizontal");
        //判断表单是否改变
        if (!$("form.form-edit").data("changed")) {
            $("form.form-edit").find("p.tips").html("当前没有修改任何数据");
            return false;
        }

        //检测验证结果
        if (!$form.valid()) {
            $(".form-group input.error")[0].select();
            $(".form-group input.error")[0].focus();
            return false;
        }
        //当前对象
        var $this = $(this);
        var parentModel = $this.closest('.bootstrap-dialog');
        //关闭按钮X
        var $closeBtn = parentModel.find("button.close");

        //是否关闭编辑窗口
        var isCloseWindow = $this.attr('id') === 'btnSaveContinue' ? false : true;
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
                getBackMessage(data.Message);
                if (data.Success) {
                    if (isCloseWindow) {
                        $this.closest(".bootstrap-dialog").modal("hide");
                    } else {
                        //保存并新增
                        $form.find("p.tips").html("新增成功，如需继续新增，请注意修改对应数据。");
                        $('input[name="IsBeginningOfYear"]').val(false);
                        $("input.first").focus();

                        //禁用提交（表单数据值改变前）
                        $this.parent().find("button").attr("disabled", true);
                        $("form.form-edit").data("changed", false);
                    }
                }
            },
            error: function (xhr, error, errThrow) {
                layer.msg(errThrow, { time: 5000 });
                getBackMessage(errThrow);
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

    //解锁
    $(".buttons #btnUnlock").off("click").on("click", function () {
        var $this = $(this);
        BootstrapDialog.confirm({
            title: "提示",
            message: "谨慎操作：该数据已打印过，您确定解锁后编辑吗？",
            type: BootstrapDialog.TYPE_WARNING,
            closable: true,
            draggable: true,
            btnCancelLabel: "取消",
            btnOKLabel: "确定",
            btnOKClass: "btn-warning",
            callback: function (result) {
                if (result) {
                    var unlockUrl = $this.data("url");
                    $.post(unlockUrl, null, function (data) {
                        layer.msg(data.Message, { time: 5000 });
                        getBackMessage(data.Message);
                        if (data.Success) {
                            $this.remove();//删除解锁按钮
                            $("form.form-edit #btnSave").removeClass("hidden");//显示保存按钮
                        }
                    });
                }
            }
        });
    });


    $(".form-edit .easyui-combotree").each(function () {
        //当前对象
        var $this = $(this);
        var url = $this.data("url");
        $this.combotree({
            editable: true,
            panelHeight: 150,
            url: url,
            collapseAll: true,
            delay: 0,
            keyHandler: {
                query: function (q) {
                    var parentContainer = $(this).parent();
                    if ($(parentContainer).hasClass("mapOption"))
                        $(this).combotree("options").url = url + "?Keywords=" + q;
                    else
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
                        var node = component.tree("getSelected");//判断是否有选中的数据
                        if (data.length > 0 && !node) {
                            var k = $this.combo("textbox").val();
                            for (var i = 0; i < data.length; i++) {
                                //data[i].children 判断是否为末级
                                if (data[i].text.indexOf(k) >= 0 && !data[i].children) {
                                    $this.combotree("setValue", {
                                        id: data[i].id,
                                        text: data[i].text
                                    });
                                    $this.siblings(".hidden-id").val(data[i].id);

                                    break;
                                }
                            }
                        }
                    }

                    switchFoucusEdit(e);
                    $(this).combotree("hidePanel");
                    //没有提交查询
                },
                up: function () {
                    var el = $(this);
                    var p = el.combotree("panel");
                    if (!p.is(":visible")) {
                        el.combotree("showPanel");
                    }

                    var $tree = el.combotree("tree");
                    var nodes = $tree.tree("getChildren");
                    if (!nodes.length) {
                        return;
                    }
                    var node = $tree.tree("getSelected");
                    if (!node) {
                        $tree.tree("select", nodes[0].target);
                        $this.combotree("setValue", {
                            id: nodes[0].id,
                            text: nodes[0].text
                        });
                    } else {
                        var idx = nodes.indexOf(node);
                        if (idx > 0 && !node.children) {
                            $tree.tree("select", nodes[idx - 1].target);
                            $this.combotree("setValue", {
                                id: nodes[idx - 1].id,
                                text: nodes[idx - 1].text
                            });
                            $this.siblings(".hidden-id").val(nodes[idx - 1].id);
                        }
                        $tree.tree("scrollTo", nodes[idx - 1].target);
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
                        return false;
                    }
                    var node = el.tree("getSelected");
                    if (!node) {
                        node = nodes[0];
                        //if (!node.children) {
                        //    el.tree('expand', node.target);
                        //} else {
                        //    el.tree("select", node.target);
                        //    $this.combotree("setValue", {
                        //        id: node.id,
                        //        text: node.text
                        //    });
                        //}
                        el.tree("select", node.target);
                        $this.combotree("setValue", {
                            id: node.id,
                            text: node.text
                        });

                    } else {
                        var idx = nodes.indexOf(node);
                        if (idx < nodes.length && !node.children) {
                            el.tree("select", nodes[idx + 1].target);
                            $this.combotree("setValue", {
                                id: nodes[idx + 1].id,
                                text: nodes[idx + 1].text
                            });
                            $this.siblings(".hidden-id").val(nodes[idx + 1].id);
                        }
                        el.tree("scrollTo", nodes[idx + 1].target);
                    }
                }
            },
            onLoadSuccess: function (node, data) {
                //console.info(data);
                var t = $this.combotree("tree");
                var k = $this.combo("textbox").val();

                t.tree("collapseAll");//折叠所有的节点

                if (k != null && k !== "") {
                    for (var i = 0; i < data.length; i++) {
                        t.tree("expand", data[i].target); //展开节点
                        if (data[i].children && data[i].children[0].text.indexOf(k) >= 0) {
                            t.tree("expand", data[i].children[0].target);
                        }
                    }
                }
                //if (data.length === 1) {
                //    var currentData = t.tree("getChildren");
                //    t.tree("expand", currentData[0].target); //展开节点
                //}
            },
            onBeforeSelect: function (node) {
                if (!$(this).tree("isLeaf", node.target)) {
                    layer.msg("请选择最末一级节点！", { time: 5000 });
                    var input = $this.combo("textbox");
                    $(input).false(); //故意抛错、阻止程序继续运行
                    return false;
                }
            },
            onBeforeExpand: function (row) {
                if (!row.children) {
                    $this.combotree("tree").tree("options").url = url + "&ParentId=" + row.id;
                }
            },
            onExpand: function (row) {
                //请不要自动选择，考虑客户可能输入错误的情况
                //var $combotree = $(this);
                //展开树并选中第一条 原：(row.children && row.children.length > 0)
                //if (row.children && row.children.length === 1) {
                //    var node = row.children[0];
                //    $combotree.tree("select", node.target);
                //    $this.combotree("setValue", {
                //        id: node.id,
                //        text: node.text
                //    });
                //}
            },
            onSelect: function (node) {
                $this.siblings(".hidden-id").val(node.id);
            },
            onChange: function (newValue, oldValue) {
                checkDataChange($(this));
            }
        });
    });
    $(".combo input.textbox-text").keyup(function () {
        var $this = $(this);
        var value = $this.val();
        if (value && value.trim().length === 0) {
            //输入框为空，即设置值为0（解决删除内容后，仍然有Id值的问题）
            $this.parent().siblings('.hidden-id').val(0);
        }
    });
    $("#TotalAmount").off("change").on("change", function () {
        var $this = $(this);
        var totalAmount = parseFloat($this.val());
        var totalAppropriated = parseFloat($("#TotalAppropriated").val());
        if (totalAmount < totalAppropriated) {
            layer.msg("指标金额不能小于已拨付金额", { time: 5000 });
            $this.focus();
            return false;
        }
        var $balance = $("#Balance");
        var balance = totalAmount - totalAppropriated;
        $balance.val(balance);
        return true;
    });


    $(".first").focus().select();

    //input回车按键
    $(".form-edit input").keydown(function (e) {
        if (e.keyCode == 13) {
            switchFoucusEdit(e);
        }
    });

    //textarea回车按键
    $(".form-edit textarea").keydown(function (e) {
        if (e.keyCode == 13) {
            switchFoucusEdit(e);
        }
    });

    //输入框焦点事件
    $(".form-edit input").focus(function () {
        $(this).select();
    });

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

    //检测input框输入值，解决combotree输入掉文字问题
    $(".form-edit input").blur(function () {
        var k = $(this).parents(".form-group").find("input.textbox-value").val();
        if (!k) {
            //$(this).val("");
            $(this).parents(".form-group").find(".easyui-combotree").combotree("setValue", {
                id: "",
                text: ""
            });
        }
    }).bind("input propertychange", function () {
        if ($(this).hasClass("textbox-text")) {
            $(this).parents(".form-group").find(".easyui-combotree").combotree("setValue", {
                id: "",
                text: $(this).val()
            });
        }
        //编辑判断表单是否改变
        checkDataChange();
    });

    $(".form-edit textarea").bind("input propertychange", function () {
        checkDataChange();
    });
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

function checkDataChange(obj) {
    //编辑判断表单是否改变
    //$("form.form-edit").find("p.tips").html("当前数据已更改");
    $(".bootstrap-dialog-title span.redRequired").removeClass("hidden");
    $("form.form-edit").find("button").attr("disabled", false);
    $("form.form-edit").data("changed", true);

    //经济科目选项数据改变
    var contanier = $(obj).parent();
    if ($(contanier).hasClass("economicsSubject")) {
        changeCountryEconomicsSubjec();
    } else if ($(contanier).hasClass("mapOption")) {
        changeCountryEconomicsSubjec();
    }
}

//修改返回信息
function getBackMessage(message) {
    message = message.replace("\r\n", "</br>");
    
    $("form.form-edit").find("p.tips").html(message);
}

//更改政府经济科目
var changeCountryEconomicsSubjec = function () {
    //修改单位性质时拉取结果并显示
    var url = $("input[name=CountryEconomicsSubjectId]").data("url");
    var category = $("input[name=BudgetUnitCategory]").val();
    var easyuiSubjectNumber = $(".economicsSubject").find(".easyui-combotree").combotree("getValue");
    var easyuiCategory = $(".mapOption").find(".easyui-combotree").combotree("getValue");

    //$(".countryEconomicsSubjectString").combotree("setValue", {
    //    id: "",
    //    text: ""
    //});
    //$("input[name=CountryEconomicsSubjectId]").val("");

    //不允许经济科目序号为空
    if (easyuiSubjectNumber <= 0)
        return false;

    //针对关闭模态框时清空数据阻止重新拉取政府经济科目
    if (easyuiSubjectNumber <= 0 && easyuiCategory <= 0)
        return false;

    $.post(url, { subjectId: easyuiSubjectNumber, category: category }, function (data) {
        if (data.id <= 0) {
            layer.msg(data.text, { time: 5000 });
            getBackMessage(data.text);
            return false;
        }

        $(".countryEconomicsSubjectString").combotree("setValue", {
            id: data.id,
            text: data.text
        });
        $("input[name=CountryEconomicsSubjectId]").val(data.id);

        return false;
    }, "json");
    return false;
}