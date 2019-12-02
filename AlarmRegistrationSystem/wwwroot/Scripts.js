function getSearchedParam(key) {
    var p = {};
    location.search.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (s, k, v) { p[k] = v })
    return key ? p[key] : p;
}

var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();

function searchActivator(formid) {
    $(document).on('change', '.search-form select, .search-form input', function () {
        console.log("a");
        var $container = $(this).closest('form').data('container');
        sendForm($container, formid);
    });
    $(document).on('keyup', '.search-form input',function () {
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
    var deleteObjectId = $(obj).attr('data-deleteobjectid');
    var container = $('#' + mainForm).data('container');
    var oldAction = $('#' + mainForm).attr('action');
    $('#' + mainForm).attr('action', action);
    $('#' + mainFormInput).attr('value', deleteObjectId);
    var func = (function () {
        $('#' + mainForm).attr('action', oldAction);
    });
    sendForm(container, mainForm, func);
}

function passAttr(trigger, args) {
    if (args !== undefined) {
        args(trigger);
    }
    var deleteObjectId = $(trigger).data('deleteobjectid');
    var mainForm = $(trigger).data('mainform');
    var mainFormInput = $(trigger).data('mainforminput');
    $('.modal-true').attr("data-mainform", mainForm);
    $('.modal-true').attr('data-mainforminput', mainFormInput);
    $('.modal-true').attr("data-deleteobjectid", deleteObjectId);
}

function AjaxAndFunc($url, func, $data) {
    $.ajax({
        type: 'POST',
        url: $url,
        data: $data,
        success: function (result) {
            if (func !== null)
            {
                func(result);
            }
        }
    });
}

function ChangeLanguage($culture) {
    var $url = "/system/SetLanguage";
    var func = function (result) {
        if (result !== null) {
            window.location.reload();
        }
    };
        var $data = { culture: $culture };
        AjaxAndFunc($url, func, $data);
}

function ChipsInit() {
    var notificationId = new URLSearchParams(window.location.search).get('notificationId');
    $.ajax({
        type: 'GET',
        url: '/notification/GetNotificationSubassemblies?notificationId=' + notificationId,
        success: function (result) {
            var notificationTags = [];
            var allTags = {};
            var placeholder = result[3][0];
            var secondaryplaceholder = result[3][1];
            var hasRight = result[0];
            for (i = 0; i < result[1].length; i++) {
                notificationTags.push({ tag: result[1][i] });
            }
            for (i = 0; i < result[2].length; i++) {
                allTags[result[2][i]] = null;
            }

            $('.chips-autocomplete').chips({
                data: notificationTags,
                autocompleteOptions: {
                    data: allTags,
                    limit: 5
                },
                placeholder: placeholder,
                secondaryPlaceholder: secondaryplaceholder,
                onChipAdd: function (e, data) {
                    let name = data.childNodes[0].textContent;
                    if (name.length >= 5) {
                        let $url = "/Notification/AddSubassembly";
                        let $data = "name=" + name + "&notificationId=" + notificationId;
                        if (hasRight) {
                            AjaxAndFunc($url, null, $data);
                        }
                    }
                    else {
                        $(".chip:contains(" + name + ")").remove();
                    }
                },
                onChipDelete: function (e, data) {
                    let name = data.childNodes[0].textContent;
                    if (name.length >= 5) {
                        let $url = "/Notification/DeleteNotificationSubassembly";
                        let $data = "notificationId=" + notificationId + "&name=" + name;
                        if (hasRight) {
                            AjaxAndFunc($url, null, $data);
                        }
                    }
                }
            });

            if (!hasRight) {
                $('.chips .input').remove();
                $('.chip i').remove();
                $('.chip').css("pointer-events", "none");
                $('#parts').css("cursor", "auto");
            }
        }
    });
}

function ManageAttr(id, name, value, func)
{
    var obj = '#' + id;
    if (func) {
        $(obj).removeAttr(name);
    }
    else {
        $(obj).attr(name, value);
    }
}

function ChangeButtonValues(id1, id2) {
    let val1 = $('#' + id1).val();
    let val2 = $('#' + id2).val();
    $('#' + id1).val(val2);
    $('#' + id2).val(val1);
}

function InitSignalR() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    connection.on("ReceiveMessage", function (user, message) {
        console.log(message[0]);
        if (message[0] === "Message") {
            M.toast({ html: message[1], classes: "teal darken-3", displayLength: 2500 });
        }
        else if (message[0] === "Error") {
            M.toast({ html: message[1], classes: "amber darken-1 special", displayLength: 2500 });
        }
    });

    var user = '@User.Identity.Name';

    connection.start();
}

function SendToastError(text) {
    M.toast({ html: text, classes: "amber darken-1 special", displayLength: 5000 });
}

function Translate(text) {
    var translation = $.ajax({
        type: 'GET',
        url: '/system/Translate?text=' + text,
        async: false
    }).responseText;
    return translation;
}
