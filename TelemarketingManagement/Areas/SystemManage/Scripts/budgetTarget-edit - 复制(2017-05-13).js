$(function () {
    //var addDefaultValue = "-- 无 --";

    ////转换为select2
    //$(".form-edit .u-select2").each(function () {
    //    //当前对象
    //    var $this = $(this);
    //    var url = $this.data("url");

    //    //远程筛选
    //    $this.select2({
    //        ajax: {
    //            url: url,
    //            dataType: "json",
    //            delay: 200,
    //            data: function (params) {
    //                return {
    //                    Keywords: params.term,
    //                    page: params.page
    //                };
    //            },
    //            processResults: function (data, params) {
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
    //        escapeMarkup: function (markup) { return markup; },
    //        minimumInputLength: 0,
    //        language: "zh-CN"
    //    });
    //    //选中数据时触发
    //    $this.on('select2:select', function (evt) {
    //        switchFoucusEdit(evt);
    //    });
    //});


    //$(".form-edit .easyui-combotree").each(function () {
    //    //当前对象
    //    var $this = $(this);
    //    var url = $this.data("url");
    //    $this.combotree({
    //        editable: true,
    //        panelHeight: 150,
    //        //valueField: 'text',
    //        //textField: 'id',
    //        url: url,
    //        collapseAll: true,
    //        delay: 500,
    //        keyHandler: {
    //            query: function (q) {
    //                $(this).combotree("options").url = url + '&Keywords=' + q;
    //                $(this).combotree("reload");
    //                //设置文本输入的值,输入框内容是text,值是id
    //                $(this).combotree("setValue", {
    //                    id: "",
    //                    text: q
    //                });
    //            },
    //            enter: function (e) {
    //                var el = $(this);
    //                var p = el.combotree("panel");
    //                if (p.is(":visible")) {
    //                    var $tree = $(this).combotree("tree");
    //                    var node = $tree.tree("getSelected");
    //                    if (node) {                            
    //                        if (!$(this).tree('isLeaf', node.target)) {
    //                            layer.msg("请选择最末一级节点！", { time: 5000 });
    //                            $tree.tree("select", node.target);
    //                            return false;
    //                        }
    //                        $this.combotree("setValue", {
    //                            id: node.id,
    //                            text: node.text
    //                        });
    //                        $this.siblings('.hidden-id').val(node.id);
    //                    }
    //                }
    //                switchFoucusEdit(e);
    //                $(this).combotree("hidePanel");
    //                //没有提交查询
    //            },
    //            up: function () {
    //                var el = $(this);
    //                var p = el.combotree("panel");
    //                if (!p.is(":visible")) {
    //                    el.combotree("showPanel");
    //                }

    //                var $tree = el.combotree("tree");
    //                var nodes = $tree.tree("getChildren");
    //                if (!nodes.length) {
    //                    return;
    //                }
    //                var node = $tree.tree("getSelected");
    //                var index=0;
    //                if (node) {
    //                    index= nodes.indexOf(node);
    //                    index > 0 ? index-- : 0;
    //                }

    //                $tree.tree("select", nodes[index].target);
    //                $tree.tree("scrollTo", nodes[index].target);
    //            },
    //            down: function () {
    //                var el = $(this);
    //                var p = el.combotree("panel");
    //                if (!p.is(":visible")) {
    //                    el.combotree("showPanel");
    //                }
    //                var $tree = el.combotree("tree");
    //                var nodes = $tree.tree("getChildren");
    //                if (!nodes.length) {
    //                    return false;
    //                }
    //                var node = $tree.tree("getSelected");
    //                var index = 0;
    //                if (node) {
    //                    index = nodes.indexOf(node);
    //                    index <nodes.length ? index++ :index;
    //                }
    //                var nextNode = nodes[index];
    //                $tree.tree("select", nextNode.target);
    //                $tree.tree("scrollTo", nextNode.target);
    //            },
    //            left: function () {

    //            },
    //            right: function () {
    //                var el = $(this);
    //                var p = el.combotree("panel");
    //                if (!p.is(":visible")) {
    //                    el.combotree("showPanel");
    //                }

    //                el = el.combotree("tree");
    //                var nodes = el.tree("getChildren");
    //                if (!nodes.length) {
    //                    return false;
    //                }
    //                var node = el.tree("getSelected");
    //                if (node) {
    //                    if (!node.children) {
    //                        el.tree('expand', node.target);
    //                    }
    //                }
    //            },
    //        },
    //        onLoadSuccess: function (node, data) {
    //            var t = $this.combotree("tree");

    //            //获取返回条数
    //            t.tree("collapseAll");
    //            if (data.length === 1) {
    //                //获取节点;
    //                var currentData = t.tree("getChildren");
    //                t.tree("expand", currentData[0].target); //展开节点
    //            }
    //        },
    //        onBeforeExpand: function (row) {
    //            if (!row.children) {
    //                $this.combotree("tree").tree("options").url = url + '&ParentId=' + row.id;
    //            }
    //        },
    //        onExpand: function (row) {
    //            var $combotree = $(this);
    //            debugger;
    //            if (row.children && row.children.length > 0) {
    //                var node = row.children[0];
    //                $combotree.tree("select", node.target);
    //            }
    //        },
    //        onClick: function (node) {
    //            if (!$(this).tree('isLeaf', node.target)) {
    //                layer.msg("请选择最末一级节点！", { time: 5000 });
    //                return false;
    //            }
    //            $this.combotree("setValue", {
    //                id: node.id,
    //                text: node.text
    //            });
    //            $this.siblings('.hidden-id').val(node.id);
    //        }

    //    });
    //});
    $(".combo input.textbox-text").keyup(function () {
        var $this = $(this);
        var value = $this.val();
        if (!value || value.trim().length === 0) {
            //输入框为空，即设置值为0（解决删除内容后，仍然有Id值的问题）
            $this.parent().siblings(".hidden-id").val(0);
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
                    item = input[0];
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
        var idx;
        var currentObj = e.target;
        idx = arr.indexOf(currentObj);
        if (idx == arr.length - 1) {// 判断是否是最后一个输入框
            if (confirm("最后一个输入框已经输入,是否提交?")) // 用户确认
                //$("form[name='contractForm']").submit(); // 提交表单
                alert("submit");
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
    $("input").focus(function () {
        $(this).select();
    });

    //初始化组合树组件
    $(".form-edit .easyui-combotree").easyuiCombotree({
        keyHandlerEnterCallback: switchFoucusEdit
    });
});