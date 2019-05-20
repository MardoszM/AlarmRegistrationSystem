var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();

function searchActivator(formid) {
    $('.search-form select, .search-form input').change(function () {
        var $container = $(this).closest('form').data('container');
        sendForm($container, formid);
    });
    $('.search-form input').keyup(function () {
        var obj = this;
        delay(function () {
            var $container = $(obj).closest('form').data('container');
            sendForm($container, formid);
        }, 250);
    });
}

function sendForm(container, formid,func = null) {
    var $form = $('#' + formid);
    var $url = $form.attr('action');
    var $data = $form.serialize();
    $.ajax({
        type: 'POST',
        data: $data,
        url: $url,
        success: function (result) {
            if (func !== null) { func(); }   
            $('#' + container).html(result);
        }
    });
}

function changeCurrentPage(obj) {
    var page = $(obj).attr("value");
    $('#CurrentPage').attr("value", page).trigger("change");
}

function deleteActivator(obj, action) {
    var mainForm = $(obj).data('mainform');
    var mainFormInput = $(obj).data('mainforminput');
    var deleteMachineId = $(obj).data('deletemachineid');
    var container = $('#' + mainForm).data('container');
    var oldAction = $('#' + mainForm).attr('action');

    $('#' + mainForm).attr('action', action);
    $('#' + mainFormInput).attr('value', deleteMachineId);
    var func = (function () {
        $('#' + mainForm).attr('action', oldAction);
    });
    sendForm(container, mainForm, func);
}

function passAttr(trigger) {
    var deleteMachineId = $(trigger).data('deletemachineid');
    var mainForm = $(trigger).data('mainform');
    var mainFormInput = $(trigger).data('mainforminput');
    $('.modal-true').attr("data-mainform", mainForm);
    $('.modal-true').attr('data-mainforminput', mainFormInput);
    $('.modal-true').attr("data-deletemachineid", deleteMachineId);
}


function AjaxAndFunc($url, func, $data) {
    $.ajax({
        type: 'POST',
        url: $url,
        data: $data,
        success: function (result) {
            func(result);
        }
    });
}