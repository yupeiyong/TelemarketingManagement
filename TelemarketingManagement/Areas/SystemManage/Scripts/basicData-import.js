$(function () {
    alert("456");
    var uploadUrl = '/Admin/Manager/BasicData/Upload';
    var removeUrl = '/FilePool_Remove';

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
        allowedFileExtensions: ['xlsx'],//接收的文件后缀
        showUpload: false, //是否显示上传按钮
        showRemove: true, //是否显示移除按钮
        fileuploadedCallback: function (event, data, id, index) {
            var result = data.response;
            layer.msg(result.Message, { time: 5000 });
            if (result.Success) {
                var fname = data.response.Files[index];
                var $fileName = $('input[name="FileName"]');
                //保存文件名
                $fileName.val(fname);

                var $OriginalFileName = $('input[name="OriginalFileName"]');
                var originalFileName = data.response.OriginalFiles[index];
                //保存文件名
                $OriginalFileName.val(originalFileName);
            }
        },
        initialPreview: getFileName('input[name="FileName"]'),
        initialPreviewConfig: [
            { caption: "nature-1.jpg", size: 329892, width: "120px", url: removeUrl, key: getFileName('input[name="FileName"]'), extra: { id: getFileName('input[name="FileName"]') } }
        ]
    });

    //上传完成后刷新
    //$fileUpload.on('filebatchuploadcomplete', function () {
    //    $fileUpload.fileinput('refresh', {
    //        initialPreview: getFileName('input[name="FileName"]'),
    //        initialPreviewConfig: [  
    //            { caption: "nature-1.jpg", size: 329892, width: "120px", url: removeUrl, key: getFileName('input[name="FileName"]'), extra: { id: getFileName('input[name="FileName"]') } }
    //        ]
    //    });
    //});

    //上传完成后直接保存
    $fileUpload.on("filebatchselected", function (event, files) {
        $(".upload-message").text("文件上传中，请稍等。。。");
        $(this).fileinput("upload");
    }).on('filebatchuploadcomplete', function () {
        $(".upload-message").text("文件上传完成，请等待服务器处理。。。");
        $("#btnSave").click();
    });
});