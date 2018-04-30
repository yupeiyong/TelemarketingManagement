 
$(function () {
    'use strict';

    window.operateFormatter = function (value, row, index) {
        return [
            '<a href="#" class="u-edit" data-u-url="/Admin/Manager/FundingSources/Edit/' + value + '">',
                '编辑',
                '<i class="fa fa-pencil" aria-hidden="true"></i>',
            '</a>',
            '<a href="#" class="u-delete" data-u-url="/Admin/Manager/FundingSources/Remove/' + value + '">',
                '删除',
                '<i class="fa fa-trash" aria-hidden="true"></i>',
            '</a>'
        ].join('');
    };
    window.isDefaultFormatter = function (value, row, index) {
        return value ? '<span style="color:red;">是</span>' : '<span>否</span>';
    }
    var $table = $('#tab');
    //为自定义列设置事件
    function addCustColClick() {
        //编辑
        $('#btnAdd,.table .u-edit').off('click').on('click', function () {
            var $this = $(this);

            var title = '新增';
            var isModify = $this.hasClass('u-edit');
            if (isModify) {
                var $tr = $this.closest('tr');
                var rowId = $tr[0].id;
                if (!rowId || rowId.length <= 0) {
                    return false;
                }
                title = '编辑 ' + rowId;
            }

            //获取链接路径
            var url = $(this).data('u-url');

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
        $('.table .u-delete').off('click').on('click', function (e) {
            //不执行默认行为
            e.preventDefault();
            //当前对象
            var $this = $(this);

            var $tr = $this.closest('tr');
            var rowId = $tr[0].id;
            if (!rowId || rowId.length <= 0) {
                return false;
            }

            //获取链接路径
            var url = $this.data('u-url');
            //确认删除
            BootstrapDialog.confirm({
                title: '删除',
                message: '确认删除第' + rowId + '条记录吗?',
                type: BootstrapDialog.TYPE_WARNING,
                closable: true,
                draggable: true,
                btnCancelLabel: '取消',
                btnOKLabel: '确认',
                btnOKClass: 'btn-warning',
                callback: function (result) {
                    if (result) {
                        $.post(url, null, function (data) {
                            layer.msg(data.Message, { time: 5000 });
                            if (data.Success) {
                                var $this = $(this);
                                $this.closest('.bootstrap-dialog').modal('hide');
                                $table.bootstrapTable('refresh');
                            }
                        });
                    } else {
                        var $this = $(this);
                        $this.closest('.bootstrap-dialog').modal('hide');
                    }
                }
            });
            return false;
        });
    }

    $table.extendBootstrapTable({
        searchButton: '#btnSearch',
        searchForm: 'form.search'
    }).on('load-success.bs.table', function () {
        //处理获取到的数据
        addCustColClick();
    });

    $("input[name=Keywords]").focus().select();
});
