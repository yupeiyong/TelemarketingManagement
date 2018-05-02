/// <reference path="basicData-import.js" />
$(function () {
    var uploadUrl = '/SystemManage/Customer/Upload';
    var removeUrl = '/SystemManage/Customer/RemoveFile';

    var getFileName = function (imgPathClass) {
        var $fileName = $(imgPathClass);
        if ($fileName) {
            var name = $fileName.val();
            return name;
        }
        return null;
    }
    var $fileUpload = $('#excelFile');

    $fileUpload.bootstrapFileInput({
        uploadUrl: uploadUrl,
        maxFileCount: 1,
        showUpload: false, //是否显示上传按钮
        showRemove: false, //是否显示移除按钮
        allowedFileExtensions: ['xls', 'xlsx'],//接收的文件后缀
        fileuploadedCallback: function (event, data, id, index) {
            var result = data.response;
            //layer.msg(result.Message, { time: 5000 });
            if (result.Success) {

                var fname = data.response.FileName;
                var $fileName = $('input[name="FileName"]');
                //保存文件名
                $fileName.val(fname);

                var $OriginalFileName = $('input[name="OriginalFileName"]');
                var originalFileName = data.response.OriginalFileName;
                //保存文件名
                $OriginalFileName.val(originalFileName);
            }
        },
        initialPreview: getFileName('input[name="FileName"]'),
        initialPreviewConfig: [
            { caption: "nature-1.jpg", size: 329892, width: "120px", url: removeUrl, key: getFileName('input[name="FileName"]'), extra: { id: getFileName('input[name="FileName"]') } }
        ]
    });

    //$fileUpload.on('filebatchuploadcomplete', function () {
    //    $fileUpload.fileinput('refresh', {
    //        initialPreview: getFileName('input[name="FileName"]'),
    //        initialPreviewConfig: [
    //            { caption: "nature-1.jpg", size: 329892, width: "120px", url: removeUrl, key: getFileName('input[name="FileName"]'), extra: { id: getFileName('input[name="FileName"]') } }
    //        ]
    //    });
    //});

    var $btnClose = $('.buttons #btnClose');
    $btnClose.on('click', function (e) {
        //当前对象
        var $this = $(this);
        var parentModel = $this.closest('.bootstrap-dialog');
        parentModel.modal('hide');
    });
    var saveFile = function () {
        //检测验证结果
        var $form = $('form');

        //上传文件组件
        var $uploadFile = $('#excelFile');

        var parentModel = $uploadFile.closest('.bootstrap-dialog');
        //'关闭'按钮
        var $closeBtn = parentModel.find('button.close');
        var url = $uploadFile.data('url');
        //保存
        var data = $form.serializeArray();
        var btnOriginalText = $btnClose.text();
        var success = true;
        $.ajax({
            url: url,
            type: 'json',
            data: data,
            beforeSend: function () {
                $closeBtn.attr("disabled", true);
                $btnClose.text('保存中...');
                $btnClose.attr("disabled", true);
                $uploadFile.attr("disabled", true);
                $('.fileinput-remove-button').hide();
            },
            success: function (data) {
                //显示保存信息
                $('.upload-message').text(data.Message);
                //记录保存结果
                if (data.Success) {
                    $('.btn-file').hide();
                } else {
                    success = false;
                    //保存发生错误时，刷新上传组件
                    $uploadFile.fileinput('refresh');
                }
            },
            error: function (xhr, error, errThrow) {
                //显示错误信息在页面
                $('.upload-message').text(errThrow);
                //保存发生错误，刷新上传组件
                $uploadFile.fileinput('refresh');
                success = false;
            },
            complete: function (msg, textStatus) {
                $btnClose.text(btnOriginalText);
                $btnClose.attr("disabled", false);

                if (success) {
                    $btnClose.show();
                } else {
                    $closeBtn.attr("disabled", false);
                    $uploadFile.attr("disabled", false);
                }
            }
        });
        //不执行提交动作
        return false;
    }


    //上传完成后直接保存
    $fileUpload.on("filebatchselected", function (event, files) {
        $(this).fileinput("upload");
    }).on('filebatchuploadcomplete', function () {
        saveFile();
    });

    $("form.import #btnResetIndex").hide();
});
