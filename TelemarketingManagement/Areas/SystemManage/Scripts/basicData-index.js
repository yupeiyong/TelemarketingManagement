$(function () {
    'use strict';
    window.operateFormatter = function (value, row, index) {
        return [
            '<a href="/Admin/Manager/BasicData/Download/' + value + '">',
                '下载',
                '<i class="fa fa-arrow-circle-o-down" aria-hidden="true"></i>',
            '</a>',
            '<a href="#" class="u-delete"',
                ' data-u-url="/Admin/Manager/BasicData/Remove/' + value + '"',
            '>',
                '删除',
                '<i class="fa fa-trash" aria-hidden="true"></i>',
            '</a>'
        ].join('');
    };
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
                    var $message = $('<div></div>');
                    $message.load(url, function () {
                        var $form = dialogRef.$modalBody.find('form');

                        $form.eq(0).validate(
                             {
                                 rules: {
                                     CustomedNumber: {
                                         required: true
                                     }
                                 },
                                 messages: {
                                     CustomedNumber: {
                                         required: '*必填项'
                                     }
                                 }
                             }
                        );
                        //添加按钮添加事件处理函数
                        dialogRef.$modalBody.find('#btnSave').off('click').on('click', function () {
                            //检测验证结果
                            if (!$form.valid()) {
                                return false;
                            }
                            //当前对象
                            var $this = $(this);
                            //关闭按钮
                            var $closeBtn = dialogRef.$modalHeader.find('button.close');
                            //上传文件组件
                            var $uploadFile = $('#excelFile');
                            var url = $this.data('url');
                            //保存
                            var data = $form.serializeArray();
                            var btnOriginalText = $this.text();
                            $.ajax({
                                url: url,
                                type: 'json',
                                data: data,
                                beforeSend: function () {
                                    $this.attr("disabled", true);
                                    $closeBtn.attr("disabled", true);
                                    $this.text('保存中...');
                                    $uploadFile.attr("disabled", true);
                                },
                                success: function (data) {
                                    //显示保存信息
                                    $('.upload-message').text(data.Message);
                                    if (data.Success) {
                                       // $this.closest('.bootstrap-dialog').modal('hide');  不自动关闭窗口
                                        $table.bootstrapTable('refresh');
                                    } else {
                                        $uploadFile.attr("disabled", false);
                                        //保存发生错误，刷新上传组件
                                        $uploadFile.fileinput('refresh');
                                    }
                                },
                                error: function (xhr, error, errThrow) {
                                    //显示错误信息在页面
                                    $('.upload-message').text(errThrow);
                                    $uploadFile.attr("disabled", false);
                                    //保存发生错误，刷新上传组件 刷新会继承disabled
                                    $uploadFile.fileinput('refresh');
                                    
                                },
                                complete: function (msg, textStatus) {
                                    $this.attr("disabled", false);
                                    $closeBtn.attr("disabled", false);
                                    $this.text(btnOriginalText);
                                    $uploadFile.attr("disabled", false);
                                }
                            });
                            //不执行提交动作
                            return false;
                        });
                    });
                    return $message;
                },
                onshow: function (dialogRef) {
                    dialogRef.$modal.removeAttr('tabindex');
                },
                onshown: function (dialogRef) {
                    //打开页面时设置焦点
                    $("input.first").focus().select();
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