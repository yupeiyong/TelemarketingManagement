
$(function () {
    ////默认值
    //var addDefaultValue = "-- 无 --";

    ////转换为select2
    //$(".form-edit .u-select2").each(function() {
    //    //当前对象
    //    var $this = $(this);
    //    var url = $this.data("url");

    //    //远程筛选
    //    $this.select2({
    //        ajax: {
    //            url: url,
    //            dataType: "json",
    //            delay: 250,
    //            data: function(params) {
    //                return {
    //                    Keywords: params.term,
    //                    page: params.page
    //                };
    //            },
    //            processResults: function(data, params) {
    //                params.page = params.page || 1;
    //                if (!data.IsSuccess) {
    //                    return false;
    //                }
    //                var arr = data.Data || data.data || {};
    //                arr.unshift({ id: 0, text: addDefaultValue });
    //                return {
    //                    results: arr
    //                };
    //            },
    //            cache: true
    //        },
    //        escapeMarkup: function(markup) { return markup; },
    //        minimumInputLength: 0,
    //        language: "zh-CN"
    //    });
    //});

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


    $(".form-edit .easyui-combotree").each(function () {
        //当前对象
        var $this = $(this);
        var url = $this.data("url");
        $this.combotree({
            editable: true,
            panelHeight: 150,
            //valueField: 'text',
            //textField: 'id',
            url: url,
            collapseAll: true,
            delay: 0,
            keyHandler: {
                query: function (q) {
                    $(this).combotree("options").url = url + '&Keywords=' + q;
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
                                    $this.siblings('.hidden-id').val(data[i].id);

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
                            $this.siblings('.hidden-id').val(nodes[idx - 1].id);
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
                            $this.siblings('.hidden-id').val(nodes[idx + 1].id);
                        }
                        el.tree("scrollTo", nodes[idx + 1].target);
                    }
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
            },
            onBeforeSelect: function (node) {
                if (!$(this).tree('isLeaf', node.target)) {
                    layer.msg("请选择最末一级节点！", { time: 5000 });
                    var input = $this.combo("textbox");
                    $(input).false();//故意抛错、阻止程序继续运行
                    return false;
                }
            },
            onBeforeExpand: function (row) {
                if (!row.children) {
                    $this.combotree("tree").tree("options").url = url + '&ParentId=' + row.id;
                }
            },
            onExpand: function (row) {
                //请不要自动选择，考虑客户可能输入错误的情况
                //var $combotree = $(this);
                //if (row.children && row.children.length > 0) {
                //    var node = row.children[0];
                //    $combotree.tree("select", node.target);
                //    $this.combotree("setValue", {
                //        id: node.id,
                //        text: node.text
                //    });
                //}
            },
            onSelect: function (node) {
                $this.siblings('.hidden-id').val(node.id);
            }
        });
    });

    $('.combo input.textbox-text').keyup(function () {
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

    //检测input框输入值，解决combotree输入掉文字问题
    $(".form-edit input.textbox-text").blur(function () {
        var k = $(this).parents(".form-group").find("input.textbox-value").val();
        if (!k) {
            //$(this).val("");
            $(this).parents(".form-group").find(".easyui-combotree").combotree("setValue", {
                id: "",
                text: ""
            });
        }
    }).bind('input propertychange', function () {
        $(this).parents(".form-group").find(".easyui-combotree").combotree("setValue", {
            id: "",
            text: $(this).val()
        });
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
