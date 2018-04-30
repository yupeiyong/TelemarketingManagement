; (function ($) {
    //使用严格模式
    'use strict';

    //全局对象
    var JJS = window.JJS = window.JJS || {};

    //为全局对象添加命名空间
    JJS.select2 = {
        //默认参数
        DEFAULT: {
            //是否包含x,这个清空按钮
            allowClear: true,
            //使用简体中文
            language: 'zh-CN',
            //多少条记录的时候才显示输入框,-1为永远不显示
            minimumResultsForSearch: -1
        },

        //默认参数
        DEFAULT_AJAX: {
            //调用ajax
            ajax: {
                //返回数据格式
                dataType: 'json',
                //执行查询的延时时间
                delay: 1000,
                //是否启用缓存
                cache: false
            },
            //转码html代码
            escapeMarkup: function (markup) {
                //转码html代码
                return markup;
            },
            //输入多少个字符才开始查询
            minimumInputLength: 1,
            //多少条记录的时候才显示输入框,-1为永远不显示
            minimumResultsForSearch: 1,
            //查询取得的每一行数据生成的html标签内容模版
            templateResult: function (result) {
                //直接返回文本
                if (result.loading) {
                    //返回提示内容
                    return result.text;
                }
                //html内容
                var markup = result.name;
                //返回内容
                return markup;
            },
            //选中后下拉框显示的内容
            templateSelection: function (selection) {
                //直接返回内容
                return selection.name || selection.text;
            }
        },

        //执行转换
        convert: function ($select2, options) {
            //检测用户传入的参数
            options = options || {};
            //待取得的参数清单
            var p = $.extend(true, {}, JJS.select2.DEFAULT);
            //是否使用ajax设置
            if (options.useAjax !== false && (options.useAjax || options.ajax)) {
                //合并默认参数
                p = $.extend(true, p, JJS.select2.DEFAULT_AJAX);
            }
            //设置默认弹框的父级
            options.dropdownParent = $($select2.eq(0).closest('.bootstrap-dialog')[0] || document.body);
            //合并参数
            p = $.extend(true, p, options);
            //执行转换为select2
            return $select2.select2(p);
        }
    };

}(jQuery));