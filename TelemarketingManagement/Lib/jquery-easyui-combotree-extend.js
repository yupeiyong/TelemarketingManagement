(function ($) {
    "use strict";

    //EasyuiCombotree插件
    function EasyuiCombotree($combotree, param) {
        //保存参数
        var options = $.extend(true, {}, this.DEFAULT, param);
        if (!options.url) {
            var url = $combotree.data("url");
            options.url = url;
        }
        //组合树默认事件
        var combotreeDefaultEvents = {
            keyHandler: {
                query: function (q) {
                    $(this).combotree("options").url = options.url + "&" + options.queryKeywordsName + "=" + q;
                    $(this).combotree("reload");
                    //设置文本输入的值,输入框内容是text,值是id
                    $(this).combotree("setValue", {
                        id: "",
                        text: q
                    });
                },
                enter: function (e) {
                    var el = $(this);
                    var p = el.combotree("panel");
                    if (p.is(":visible")) {
                        var $tree = $(this).combotree("tree");
                        var node = $tree.tree("getSelected");
                        if (node) {
                            if (options.isOnlySelectLeaf && !$(this).tree("isLeaf", node.target)) {
                                layer.msg("请选择最末一级节点！", { time: 5000 });
                                $tree.tree("select", node.target);
                                return false;
                            }
                            $combotree.combotree("setValue", {
                                id: node.id,
                                text: node.text
                            });
                            $combotree.siblings(options.siblingsInputClass).val(node.id);
                        }
                    }
                    //回车之后的回调方法
                    if (options.keyHandlerEnterCallback) {
                        options.keyHandlerEnterCallback(e);
                    }
                    $(this).combotree("hidePanel");
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
                    var index = 0;
                    if (node) {
                        index = nodes.indexOf(node);
                        index > 0 ? index-- : 0;
                    }

                    $tree.tree("select", nodes[index].target);
                    $tree.tree("scrollTo", nodes[index].target);
                },
                down: function () {
                    var el = $(this);
                    var p = el.combotree("panel");
                    if (!p.is(":visible")) {
                        el.combotree("showPanel");
                    }
                    var $tree = el.combotree("tree");
                    var nodes = $tree.tree("getChildren");
                    if (!nodes.length) {
                        return false;
                    }
                    var node = $tree.tree("getSelected");
                    var index = 0;
                    if (node) {
                        index = nodes.indexOf(node);
                        index < nodes.length ? index++ : index;
                    }
                    var nextNode = nodes[index];
                    $tree.tree("select", nextNode.target);
                    $tree.tree("scrollTo", nextNode.target);
                },
                left: function () {

                },
                right: function () {
                    var el = $(this);
                    var p = el.combotree("panel");
                    if (!p.is(":visible")) {
                        el.combotree("showPanel");
                    }

                    var $tree = el.combotree("tree");
                    var nodes = $tree.tree("getChildren");
                    if (!nodes.length) {
                        return false;
                    }
                    var node = $tree.tree("getSelected");
                    if (node) {
                        if (!node.children) {
                            $tree.tree("expand", node.target);
                        }
                    }
                },
            },
            onLoadSuccess: function (node, data) {
                var $tree = $("this");
                //获取返回条数
                $tree.tree("collapseAll");
                if (data.length === 1) {
                    //获取节点;
                    var currentData = $tree.tree("getChildren");
                    $tree.tree("expand", currentData[0].target); //展开节点
                }
            },
            onBeforeExpand: function (node) {
                if (!node.children) {
                    $combotree.combotree("tree").tree("options").url = options.url + "&" + options.queryParentIdName + "=" + node.id;
                }
            },
            onExpand: function (node) {
                var $tree = $(this);
                if (node.children && node.children.length > 0) {
                    var subNode = node.children[0];
                    $tree.tree("select", subNode.target);
                }
            },
            onClick: function (node) {
                if (options.isOnlySelectLeaf && !$(this).tree("isLeaf", node.target)) {
                    layer.msg("请选择最末一级节点！", { time: 5000 });
                    return false;
                } else {
                    $combotree.combotree("setValue", {
                        id: node.id,
                        text: node.text
                    });
                    $combotree.siblings(options.siblingsInputClass).val(node.id);
                }
            }
        };

        //保存参数
        this.options = $.extend(true, {}, combotreeDefaultEvents, options);
        this.$combotree = $combotree;

        //执行初始化
        this.Init();
    }

    //默认值
    EasyuiCombotree.prototype.DEFAULT = {
        editable: true,
        panelHeight: 150,
        url: "",
        collapseAll: true,
        delay: 500,
        isOnlySelectLeaf: true,//只能选择叶结点
        keyHandlerEnterCallback: null,//回车事件的回调方法
        queryKeywordsName: "Keywords",//查询关键字的名称
        siblingsInputClass: ".hidden-id",//选择之后保存值的兄弟输入框类名
        queryParentIdName: "ParentId"//查询子树时的父结点Id名称
    };

    //初始化combotree
    EasyuiCombotree.prototype.Init = function () {
        //当前对象
        var $this = this.$combotree;
        var options = this.options;
        //初始化combotree
        $this.combotree(options);
    };

    //设置到jquery的扩展接口供外部访问
    $.fn.easyuiCombotree = function (options) {
        //遍历传入的对象数组
        this.each(function () {
            //当前对象
            var $this = $(this);
            //检测是否已经初始化过
            //没有就创建一个新的easyuiCombotree对象
            var oFile = $this.data("easyui_easyuiCombotree") || new EasyuiCombotree($this, options);
            //缓存到html元素中
            $this.data("easyui_easyuiCombotree", oFile);
            //直接返回新的对象
            return oFile;
        });
    }
})(jQuery);