$(function () {
    "use strict";

    RefreshList();

    $("form.search .easyui-combotree").each(function () {
        //当前对象
        var $this = $(this);
        var url = $this.data("url");
        $this.combotree({
            editable: true,
            width: "100%",
            panelHeight: 150,
            url: url,
            textField: "id",
            valueField: "text",
            collapseAll: true,
            method: "get",
            delay: 0,
            keyHandler: {
                query: function (q) {
                    $(this).combotree("options").url = url + "&Keywords=" + q;
                    $(this).combotree("reload");
                },
                enter: function (e) {
                    var p = $(this).combotree("panel");
                    if (p.is(":visible")) {
                        var component = $(this).combotree("tree");
                        var data = $(component).tree("getChildren");
                        var hoverNode = $(component).find(".tree-node-hover"); //判断是否有指向数据
                        var node = component.tree("getSelected"); //判断是有选中的数据
                        if (data.length > 0 && !node) {
                            var k = $this.combo("textbox").val(); //输入搜索用
                            if (hoverNode.length > 0) {
                                //优先执行hover
                                node = component.tree("getNode", hoverNode);
                                var idx = data.indexOf(node);
                                component.tree("select", data[idx].target);
                            } else {
                                for (var i = 0; i < data.length; i++) {
                                    if (data[i].text.indexOf(k) >= 0) {
                                        component.tree("select", data[i].target);
                                        break;
                                    }
                                }
                            }
                        }
                    } else {
                        //面板关闭状态下回车是切换下一个
                        switchFoucus(e);
                    }
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
                    var hoverNode = $(el).find(".tree-node-hover");
                    if (!node && hoverNode.length <= 0) {
                        $(hoverNode).removeClass("tree-node-hover");
                        $(nodes[0].target).addClass("tree-node-hover");
                    } else {
                        if (!node)
                            node = el.tree("getNode", hoverNode);//通过target获取节点对象

                        var idx = nodes.indexOf(node);
                        var i = 1;
                        if (idx > 0) { // && !node.children
                            $(hoverNode).removeClass("tree-node-hover");
                            var preNode = nodes[idx - 1];
                            var father = el.tree("getParent", preNode.target);
                            if (father && father.state === "closed") {
                                i = father.children.length + 1;
                            }

                            $(nodes[idx - i].target).addClass("tree-node-hover");
                        } else {
                            $(hoverNode).removeClass("tree-node-hover");
                            $(nodes[nodes.length - 1].target).addClass("tree-node-hover");
                            el.tree("scrollTo", nodes[nodes.length - 1].target);

                            return false;
                        }
                        el.tree("scrollTo", nodes[idx - i].target);
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
                    var hoverNode = $(el).find(".tree-node-hover");
                    if (!node && hoverNode.length <= 0) {
                        $(hoverNode).removeClass("tree-node-hover");
                        $(nodes[0].target).addClass("tree-node-hover");
                    } else {
                        if (!node)
                            node = el.tree("getNode", hoverNode);//通过target获取节点对象

                        var idx = nodes.indexOf(node);
                        var i = 1;
                        if (idx < nodes.length) {// && !node.children(不区分有没有子级)
                            $(hoverNode).removeClass("tree-node-hover");
                            if (node.state === "closed" && node.children) {
                                i = node.children.length + 1;
                            }

                            //判断是否是最后一个
                            var indexOf = idx + i;
                            if (indexOf === nodes.length) {
                                $(nodes[0].target).addClass("tree-node-hover");
                                el.tree("scrollTo", nodes[0].target);

                                return false;
                            }

                            $(nodes[idx + i].target).addClass("tree-node-hover");
                        }
                        el.tree("scrollTo", nodes[idx + i].target);
                    }
                },
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
                right: function (e) {
                    $(this).combotree("hidePanel");
                    //右键切换下一条选项
                    switchFoucus(e);
                }
            },
            onBeforeSelect: function (node) {
                if (!$(this).tree('isLeaf', node.target)) {
                    layer.msg("请选择最末一级节点！", { time: 5000 });
                    var input = $this.combo("textbox");
                    $(input).false();
                    return false;
                }
            },
            //增加子节点
            onBeforeExpand: function (row) {
                if (!row.children) {
                    $this.combotree("tree").tree("options").url = url + '&ParentId=' + row.id;
                }
            },
            onLoadSuccess: function (node, data) {
                var t = $this.combotree("tree");
                var k = $this.combo("textbox").val();
                t.tree("collapseAll");//折叠所有的节点

                if (k != null && k !== "") {
                    for (var i = 0; i < data.length; i++) {
                        t.tree("expand", data[i].target); //展开节点
                    }
                }
            },
            onSelect: function (node) {
                $this.combo("textbox").select();
                var container = $this.parents(".col-md-3").find("ul");
                var li = $(container).find("li");
                var html = "<li data-id='" + node.id + "'>" + node.text + "<i class='fa fa-remove' title='点击删除此条件'></i></li>";

                for (var i = 0; i < li.length; i++) {
                    var id = $(li[i]).data("id");
                    if (id === node.id) {
                        alert("该条件已经选择过了哦，请勿重复选择");
                        return false;
                    }
                }
                $(container).append(html);
                getIds(container);
            }
        });
    });

    //删除条件
    $(".conditional ul").on("click", "li i", function () {
        var obj = $(this).parents("ul");
        $(this).parent().remove();

        getIds(obj);
    });

    //日期回车按键
    $("form.search input[type=date]").keydown(function (e) {
        if (e.keyCode === 13) {
            switchFoucus(e);
        }
    });

    //输入框焦点事件
    $("form.search input").focus(function () {
        $(this).select();
    });

    //输入框失去焦点(查询页面如果有text,且id为空。 点击新增后的所有空框插入text的BUG问题)
    $("form.search input.textbox-text").blur(function () {
        var k = $(this).parents(".form-group").find("input.textbox-value").val();
        if (!k) {
            //$(this).val("");
            $(this).parents(".form-group").find("select").combotree("setValue", {
                id: "",
                text: ""
            });
            //$(this).parents(".form-group").find("select").combotree("hidePanel");
        }
    }).bind('input propertychange', function () {//检测input框输入值，解决combotree输入掉文字问题
        $(this).parents(".form-group").find("select").combotree("setValue", {
            id: "",
            text: $(this).val()
        });
    });

    $(document).keydown(function (event) {
        if (event.which == 13) {
            RefreshList();
            return false;
        }
    });

    setTimeout(function () {
        $("form.search input.textbox-text").first().focus().select();
    }, 100);

});

//生成多选ids
function getIds(obj) {
    var li = $(obj).find("li");
    var ids = [];

    for (var i = 0; i < li.length; i++) {
        var id = $(li[i]).data("id");
        ids.push(id);
    }

    $(obj).parents(".col-md-3").find("input.hidden-ids").val(ids.join(";"));

    return false;
}

//回车键焦点切换
function switchFoucus(e) {
    var idx;

    var current = e.target;
    var inputs = $(".conditional input:not([type=hidden])");
    idx = inputs.index(current);
    if ($(current).attr("name") === "Keywords") {
        idx = inputs.index(current);
    }

    if (idx == inputs.length - 1) {// 判断是否是最后一个输入框
        return false;
    } else {
        inputs[(idx + 1)].focus(); // 设置焦点
        inputs[(idx + 1)].select(); // 选中文字
    }
}

function ReportGenerate(btn, url) {
    var formData = $("form.search").serializeArray();

    var $btn = $(btn);
    $btn.attr("disabled", true);
    var originalText = $btn.text();
    $btn.text('正在生成报表...');
    $.post(url, formData, function (response) {
        layer.msg(response.Message, { time: 7000 });
        if (response.Success) {
            RefreshList();
        }
        $btn.text(originalText);
        $btn.attr("disabled", false);
    }, "json");
}

function exportExcel(btn, url) {
    var $btn = $(btn);
    $btn.attr("disabled", true);
    var originalText = $btn.text();
    $btn.text('正在导出报表...');
    $.post(url, null, function (response) {
        layer.msg(response.Message, { time: 7000 });
        if (response.Success) {
            location.href = "/ReportFilePool/Download/?id=" + response.DownloadFileUrl + "&originalFilename=" + response.OriginalFileName;
        }
        $btn.text(originalText);
        $btn.attr("disabled", false);
    }, "json");
}
