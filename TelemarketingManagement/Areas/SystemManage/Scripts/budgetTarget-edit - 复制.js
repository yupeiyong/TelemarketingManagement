$(function () {
    var addDefaultValue = "-- 无 --";

    //转换为select2
    $(".form-edit .u-select2").each(function () {
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
        //选中数据时触发
        $this.on('select2:close', function (evt) {
            switchFoucusEdit(evt);
        });
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
            keyHandler: {
                query: function (q) {
                    $(this).combotree("options").url = url +  '&Keywords=' + q;
                    $(this).combotree("reload");
                    //设置文本输入的值,输入框内容是text,值是id
                    $(this).combotree("setValue", {
                        id: "",
                        text: q
                    });
                },
                enter: function (e) {
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
                        if (idx > 0 && !node.children) {
                            el.tree("select", nodes[idx - 1].target);
                            $this.combotree("setValue", {
                                id: nodes[idx - 1].id,
                                text: nodes[idx - 1].text
                            });
                        }
                        el.tree("scrollTo", nodes[idx - 1].target);
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
                        if (idx < nodes.length && !node.children) {
                            el.tree("select", nodes[idx + 1].target);
                            $this.combotree("setValue", {
                                id: nodes[idx + 1].id,
                                text: nodes[idx + 1].text
                            });
                        }
                        el.tree("scrollTo", nodes[idx + 1].target);
                    }
                }
            },
            onLoadSuccess: function (node, data) {
                var t = $this.combotree("tree");

                //获取返回条数
                t.tree("collapseAll");
                if (data.length === 1) {
                    //获取节点;
                    var currentData = t.tree("getChildren");
                    t.tree("expand", currentData[0].target); //展开节点
                    if (!currentData[0].children) {
                        t.tree("select", currentData[0].target);  //选中数据
                        $this.combotree("setValue", {
                            id: currentData[0].id,
                            text: currentData[0].text
                        });
                    }
                }
            },
            onBeforeSelect: function (node) {
                if (!$(this).tree('isLeaf', node.target)) {
                    layer.msg("请选择最末一级节点！", { time: 5000 });
                    return false;
                }
            },
            onBeforeExpand: function (row) {
                var r = row;
                if (!row.children) {
                    $.ajax({
                        url: url + '&ParentId=' + row.id,
                        type: "post",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            $this.combotree('tree').tree('append', {
                                parent: r.target,
                                data: data
                            });
                        },
                        error: function (xhr, textStatus, errorThrown) {
                        }
                    });
                }
            },
            onSelect: function (node) {
                $this.siblings('.hidden-id').val(node.id);
            }
        });
    });
    $('#TotalAmount').off('change').on('change', function () {
        var $this = $(this);
        var totalAmount =parseFloat($this.val());
        var totalAppropriated =parseFloat($('#TotalAppropriated').val());
        if (totalAmount < totalAppropriated) {
            layer.msg('指标金额不能小于已拨付金额', { time: 5000 });
            $this.focus();
            return false;
        }
        var $balance=$('#Balance');
        var balance = totalAmount - totalAppropriated;
        $balance.val(balance);
        return true;
    });


    $('.first').focus();

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

    //日期回车按键
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
});