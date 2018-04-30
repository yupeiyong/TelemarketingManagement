$(function () {
    "use strict";

    //年初指标和本级指标
    var beginningBudgetTarget = 'beginningBudgetTarget',
        budgetTarget = 'budgetTarget';
    window.operateFormatter = function (value, row, index) {
        var links = [
            "<a href=\"#\" class=\"u-edit\" data-u-url=\"/Admin/Manager/BudgetTarget/Edit/" + value + "\">",
                "编辑",
                "<i class=\"fa fa-pencil\" aria-hidden=\"true\"></i>",
            "</a>",
            "<a href=\"/Admin/Manager/BudgetTarget/Print/" + value + "\" style=\"display:inline-block\">",
                "打印",
                "<i class=\"fa fa-print\" aria-hidden=\"true\"></i>",
            "</a>",
            "<a href=\"#\" class=\"u-delete\" data-u-url=\"/Admin/Manager/BudgetTarget/Remove/" + value + "\">",
                "删除",
                "<i class=\"fa fa-trash\" aria-hidden=\"true\"></i>",
            "</a>"
        ];
        if (row.IsLocked) {
            links.push("<a href=\"#\" class=\"u-unlock\" data-u-url=\"/Admin/Manager/BudgetTarget/Unlock/" + value + "\">",
                "解锁",
                "<i class=\"fa fa-unlock\" aria-hidden=\"true\"></i>",
            "</a>");
        }
        return links.join("");
    };

    window.operateFormatterBeginning = function (value, row, index) {
        var links = [
            "<a href=\"#\" class=\"u-edit\" data-u-url=\"/Admin/Manager/BeginningBudgetTarget/Edit/" + value + "\">",
                "编辑",
                "<i class=\"fa fa-pencil\" aria-hidden=\"true\"></i>",
            "</a>",
            "<a href=\"#\" class=\"u-delete\" data-u-url=\"/Admin/Manager/BeginningBudgetTarget/Remove/" + value + "\">",
                "删除",
                "<i class=\"fa fa-trash\" aria-hidden=\"true\"></i>",
            "</a>"
        ];
        if (row.IsLocked) {
            links.push("<a href=\"#\" class=\"u-unlock\" data-u-url=\"/Admin/Manager/BeginningBudgetTarget/Unlock/" + value + "\">",
                "解锁",
                "<i class=\"fa fa-unlock\" aria-hidden=\"true\"></i>",
            "</a>");
        }
        return links.join("");
    };

    //指标说明允许显示的最多字符数
    var maxSummaryLenth = 20;
    window.summaryFormatter = function (value, row, index) {
        if (value && value.length > maxSummaryLenth) {
            var summary = value;
            summary = summary.substr(0, maxSummaryLenth - 1) + '......';
            return '<span title="' + value + '">' + summary + '</span>';
        }
        return value;
    }
    var $table = $("#tab");

    var $tableBeginning = $('#tabBeginning');

    //tab标签页名称，默认为本级指标
    var tabName = "budgetTarget";
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        // 获取已激活的标签页的名称
        var href = $(e.target).attr('href');
        tabName = href.substr(1);
        $('form.search .search-buttons').hide();
        $('form.search .' + tabName).show();
    });
    $('form.search .beginningBudgetTarget').hide();
    //为自定义列设置事件
    function addCustColClick() {

        //编辑
        $("#btnAdd, .table .u-edit").off("click").on("click", function () {
            var $this = $(this);

            var addTitles = { beginningBudgetTarget: "年初指标新增", budgetTarget: "本级指标新增" }
            var updateTitles = { beginningBudgetTarget: "年初指标编辑", budgetTarget: "本级指标编辑" }

            var title = addTitles[budgetTarget];
            var $currentTable = $table;
            if (tabName == beginningBudgetTarget) {
                title = addTitles[beginningBudgetTarget];
                $currentTable = $tableBeginning;
            }
            var isModify = $this.hasClass("u-edit");
            if (isModify) {
                var $tr = $this.closest("tr");
                var rowId = $tr[0].id;
                if (!rowId || rowId.length <= 0) {
                    return false;
                }
                title = updateTitles[budgetTarget];
                if (tabName == beginningBudgetTarget) {
                    title = updateTitles[beginningBudgetTarget];
                }

                title = title +' - '+ rowId + "<span class='redRequired hidden'>*</span>";
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
                    $currentTable.bootstrapTable('refresh');
                    //设置easyui树值为空。请不要删除，谢谢
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
            var $currentTable = $table;
            if (tabName == beginningBudgetTarget) {
                $currentTable = $tableBeginning;
            }

            var $tr = $this.closest("tr");
            var rowId = $tr[0].id;
            if (!rowId || rowId.length <= 0) {
                return false;
            }

            if ($this.parent().find("a.u-unlock").length > 0) {
                BootstrapDialog.confirm({
                    title: "提示",
                    message: "谨慎操作：已经打印的数据删除需要先解锁，您确定解锁后删除吗？",
                    type: BootstrapDialog.TYPE_WARNING,
                    closable: true,
                    draggable: true,
                    btnCancelLabel: "取消",
                    btnOKLabel: "知道了",
                    btnOKClass: "btn-warning",
                    callback: function (result) {
                        if (result) {
                            var unlockUrl = $this.parent().find("a.u-unlock").data("u-url");
                            $.post(unlockUrl, null, function (data) {
                                layer.msg(data.Message, { time: 2000 });
                                if (data.Success) {
                                    $this.parent().find("a.u-unlock").remove(); //删除解锁按钮
                                    $currentTable.bootstrapTable("refresh");

                                    //$this.parent().find("a.u-delete").click();//执行删除
                                    setTimeout(function () {
                                        var deleteUrl = $this.parent().find("a.u-delete").data("u-url");
                                        $.post(deleteUrl, null, function (data1) {
                                            layer.msg(data1.Message, { time: 5000 });
                                            if (data1.Success) {
                                                $(this).closest(".bootstrap-dialog").modal("hide");
                                                $currentTable.bootstrapTable("refresh");
                                            }
                                        });
                                    }, 2000);
                                } else {
                                    return false;
                                }
                            });
                        } else {
                            $(this).closest(".bootstrap-dialog").modal("hide");
                            return false;
                        }
                    }
                });
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
                                $currentTable.bootstrapTable("refresh");
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
        //解除锁定
        $(".table .u-unlock").off("click").on("click", function (e) {
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
            //确认解除锁定
            BootstrapDialog.confirm({
                title: "解除锁定",
                message: "该数据已被打印过，确认解除锁定第" + rowId + "条记录吗?",
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
        singleSelect: false,
        formatNoMatches: function () {
            return "没有找到匹配的记录，导入<a href='/Admin/Administrator/BudgetTargetImport' target='_blank'><span style='color:blue;font-weight:bold;'>[本级指标]</span></a>！";
        }
    }).on("load-success.bs.table", function () {
        //处理获取到的数据
        addCustColClick();
    });

    $tableBeginning.extendBootstrapTable({
        searchButton: "#btnBeginningSearch",
        searchForm: "form.search",
        formatNoMatches: function () {
            return "没有找到匹配的记录，导入<a href='/Admin/Administrator/BeginningBudgetTargetImport' target='_blank'><span style='color:blue;font-weight:bold;'>[年初预算指标]</span></a>！";
        }
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
            delay: 0,
            //multiple: true,//多选
            //checkbox: true,
            //children:'children',
            keyHandler: {
                query: function (q) {
                    $(this).combotree("options").url = url + "&Keywords=" + q;
                    $(this).combotree("reload");
                    //设置文本输入的值,输入框内容是text,值是id(移除在外面传入值)
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
                        var hoverNode = $(component).find(".tree-node-hover"); //判断是否有指向数据
                        var node = component.tree("getSelected"); //判断是有选中的数据
                        if (data.length > 0 && !node) {
                            var k = $this.combo("textbox").val(); //输入搜索用
                            if (hoverNode.length > 0) {
                                //优先执行hover
                                node = component.tree("getNode", hoverNode);
                                var idx = data.indexOf(node);
                                component.tree("select", data[idx].target);
                                //$this.combotree("setValue", {
                                //    id: data[idx].id,
                                //    text: data[idx].text
                                //});
                            } else {
                                for (var i = 0; i < data.length; i++) {
                                    if (data[i].text.indexOf(k) >= 0) {
                                        component.tree("select", data[i].target);
                                        //$this.combotree("setValue", {
                                        //    id: data[i].id,
                                        //    text: data[i].text
                                        //});
                                        //$this.siblings('.hidden-id').val(data[i].id);
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
                        //el.tree("select", nodes[0].target);
                        //$this.combotree("setValue", {
                        //    id: nodes[0].id,
                        //    text: nodes[0].text
                        //});
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
                            //el.tree("select", nodes[idx - 1].target);
                            //$this.combotree("setValue", {
                            //    id: nodes[idx - 1].id,
                            //    text: nodes[idx - 1].text
                            //});
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
                        //el.tree("select", nodes[0].target);
                        //$this.combotree("setValue", {
                        //    id: nodes[0].id,
                        //    text: nodes[0].text
                        //});
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
                            //el.tree("select", nodes[idx + 1].target);
                            //$this.combotree("setValue", {
                            //    id: nodes[idx + 1].id,
                            //    text: nodes[idx + 1].text
                            //});
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
                //if (data.length > 0) {
                //    //获取节点;
                //    var currentData = t.tree("getChildren");
                //    t.tree("expand", currentData[0].target); //展开节点
                //}
            },
            onSelect: function (node) {
                //var values = $this.combotree("getValue").join(";");
                //$this.siblings('.hidden-id').val(values);
                //$this.combo("textbox").focus();
                $this.combo("textbox").select();
                //$this.combotree("setValue", {
                //    id: data[idx].id,
                //    text: data[idx].text
                //});
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

    //生成多选ids
    function getIds(obj) {
        var li = $(obj).find("li");
        var ids = [];

        for (var i = 0; i < li.length; i++) {
            var id = $(li[i]).data("id");
            ids.push(id);
        }
        var t = $(obj).siblings("form-group").find("input.hidden-ids");
        $(obj).parents(".col-md-3").find("input.hidden-ids").val(ids.join(";"));

        return false;
    }

    //删除条件
    $(".conditional ul.SelectedCondition").on("click", "li i", function () {
        var obj = $(this).parents("ul");
        $(this).parent().remove();

        getIds(obj);
    });


    $("form.search #btnClear").off("click").on("click", function () {
        var $this = $(this);
        var $form = $this.closest("form");
        $this.parents(".conditional").find("ul.SelectedCondition").html("");
        $this.parents(".conditional").find("input.hidden-ids").val("");
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

    $("form.search #btnBatchPrint").off("click").on("click", function () {
        var $this = $(this);
        var $form = $this.closest("form");
        var url = $this.data("u-url");
        var originalUrl = $form.attr("action");
        $form.attr("action", url);

        var selectedRows = $table.bootstrapTable('getAllSelections');
        if (selectedRows == null || selectedRows.length == 0)
            return;

        var ids = [];
        for (var i = 0; i < selectedRows.length; i++) {
            var id = selectedRows[i].Id;
            ids.push(id);
        }

        debugger;

        var idsJsonStr = JSON.stringify(ids);
        $("form.search input[name='targetIds']").val(idsJsonStr);
        $form.submit();
        $form.attr("action", originalUrl);
    });

    $("form.search #btnBeginningBatchPrint").off("click").on("click", function () {
        var $this = $(this);
        var $form = $this.closest("form");
        var url = $this.data("u-url");
        var originalUrl = $form.attr("action");
        $form.attr("action", url);

        $table
        $form.submit();
        $form.attr("action", originalUrl);
    });


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
        }
    }).bind('input propertychange', function () {//检测input框输入值，解决combotree输入掉文字问题
        $(this).parents(".form-group").find("select").combotree("setValue", {
            id: "",
            text: $(this).val()
        });
    });

    $("form.search input.textbox-text").first().focus();
});
