﻿
$(function () {

    var $linkChangeCode = $('.link-change-securityCode');
    $linkChangeCode.off('click').on('click', function () {
        var $this = $(this);
        var $image = $this.find(".SecurityCodeImage");
        var url = $this.data("url") + "/?IsForceRegenerating=true&" + Math.random();
        $image.attr("src", url);
    });

    $('#btnLogin').off('click').on('click', function () {
        var $this = $(this);
        var url = $this.data('url');
        var accountName = $("input[name='AccountName']").val();
        var password = $("input[name='Password']").val();
        //var securityCode = $("input[name='SecurityCode']").val();

        var formData = {
            AccountName: accountName,
            Password: password,
            //SecurityCode: securityCode
        };
        $.ajax({
            url: url,
            data: formData,
            cache: false,
            async: false,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                //$("p.messageTips").html(data.Message);
                if (data.Success) {
                    location.href = data.RedirectUrl;
                } else {
                    $("p.messageTips").html(data.Message);
                    $linkChangeCode.click();
                }
            },
            error: function (xhr, error, errThrow) {
                layer.msg(errThrow, { time: 5000 });
                $("p.messageTips").html(errThrow);
            }
        });
        return false;
    });

    $linkChangeCode.click();
    $('input[name="AccountName"]').focus();
});