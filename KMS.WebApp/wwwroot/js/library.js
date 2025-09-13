toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toastr-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};
var objSwal = {
    title: "Thông báo",
    html: "",
    width: null,
    footer: null,
    icon: "",
    textConfirm: "Xác nhận",
    textCancel: "Đóng",
    timer: null,
    showCloseButton: false,
    showCancelButton: true,
    focusCancel: false,
    showConfirmButton: true,
    allowOutsideClick: true
};
function ResetSwalDefault() {
    objSwal.title = "Thông báo";
    objSwal.html = "";
    objSwal.width = null;
    objSwal.footer = null;
    objSwal.type = "";
    objSwal.textConfirm = "Xác nhận";
    objSwal.textCancel = "Đóng";
    objSwal.timer = null;
    objSwal.showCloseButton = false;
    objSwal.showCancelButton = true;
    objSwal.focusCancel = false;
    objSwal.showConfirmButton = true;
    objSwal.allowOutsideClick = true;
}
function AjaxAsync(url, objAjax, successCallback) {
    var type = "GET";
    if (url.indexOf("InsertAsync") > 0)
        type = "POST";
    if (url.indexOf("UpdateAsync") > 0)
        type = "PUT";
    if (url.indexOf("RemoveAsync") > 0)
        type = "DELETE";
    AjaxRequestClientAPI(type, url, objAjax, successCallback, true, "text");
}
function AjaxPostAsync(url, objAjax, successCallback) {
    AjaxRequestClientAPI("POST", url, objAjax, successCallback, true, "text");
}
function AjaxGetAsync(url, objAjax, successCallback) {
    AjaxRequestClientAPI("GET", url, objAjax, successCallback, true, "text");
}
function AjaxPutAsync(url, objAjax, successCallback) {
    AjaxRequestClientAPI("PUT", url, objAjax, successCallback, true, "text");
}
function AjaxDeleteAsync(url, objAjax, successCallback) {
    AjaxRequestClientAPI("DELETE", url, objAjax, successCallback, true, "text");
}
function AjaxPost(url, objAjax, successCallback) {
    AjaxRequestClientAPI("POST", url, objAjax, successCallback, false, "text");
}
function AjaxGet(url, objAjax, successCallback) {
    AjaxRequestClientAPI("GET", url, objAjax, successCallback, false, "text");
}
function AjaxPut(url, objAjax, successCallback) {
    AjaxRequestClientAPI("PUT", url, objAjax, successCallback, false, "text");
}
function AjaxDelete(url, objAjax, successCallback) {
    AjaxRequestClientAPI("DELETE", url, objAjax, successCallback, false, "text");
}
function AjaxRequestClientAPI(type, url, objAjax, successCallback, async, dataType) {
    var parameters = undefined, divLoading = undefined;
    if (objAjax !== undefined && objAjax !== null) {
        parameters = objAjax.parameters;
        divLoading = objAjax.divLoading;
    }
    if (divLoading !== undefined && divLoading !== "") {
        StartLoading(divLoading);
    }
    $.ajax({
        type: type === undefined ? "POST" : type,
        url: url === undefined ? "" : url,
        contentType: "application/json; charset=utf-8",
        data: parameters === undefined ? "" : JSON.stringify(parameters),
        dataType: dataType === undefined ? "text" : dataType,
        async: async === undefined ? true : async,
        crossDomain: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Content-Type", "application/json");
        },
        success: function (data) {
            try {
                data = JSON.parse(data);
            } catch {
                data = data
            }
            if (divLoading !== undefined && divLoading !== "") StopLoading(divLoading);
            if (successCallback !== undefined && typeof successCallback === "function") successCallback(data);
        },
        error: function (xhr, textStatus) {
            if (divLoading !== undefined && divLoading !== "") StopLoading(divLoading);

            var message = xhr.responseText !== undefined ? xhr.responseText : textStatus;
            SwalMessage(message, "Có lỗi xảy ra", undefined, "error");
            console.log(message);
            return;
        }
    });
}
function AjaxLoadPartial(url, divLoading, successCallback) {
    AjaxRequestPartial(url, divLoading, successCallback);
}
function AjaxPostLoadPartial(url, objAjax, successCallback) {
    AjaxPostPartial(url, successCallback, objAjax);
}
function AjaxRequestPartial(url, divLoading, successCallback) {
    if (divLoading !== undefined && divLoading !== "")
        StartLoading(divLoading);
    $.ajax({
        type: 'GET',
        url: url,
        async: true,
        success: function (data) {
            if (divLoading !== undefined && divLoading !== "") StopLoading(divLoading);
            successCallback(data);
        },
        error: function (xhr, textStatus) {
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            if (xhr.status === 400 || xhr.status === 401) {
                console.log("StatusText: " + xhr.statusText + ", Message: " + xhr.responseJSON.Message);
                SwalNotifyInfo(xhr.responseJSON.Message);
                return;
            }
            else if (xhr.status === 401) {
                SwalNotifyError("Bạn không có quyền thực hiện chức năng này. Vui lòng đăng nhập lại hệ thống");
                return;
            } else {
                var message = xhr.responseText !== undefined ? xhr.responseText : textStatus;
                SwalMessage(message, "Có lỗi xảy ra", undefined, "error");
                console.log(message);
                return;
            }
        }
    });
}
function AjaxPostPartial(url, successCallback, objAjax) {
    var parameters = undefined, divLoading = undefined;
    if (objAjax !== undefined && objAjax !== null) {
        parameters = objAjax.parameters;
        divLoading = objAjax.divLoading;
    }
    if (divLoading !== undefined && divLoading !== "")
        StartLoading(divLoading);
    $.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json; charset=utf-8",
        data: parameters === undefined ? "" : parameters,
        async: true,
        crossDomain: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        },

        success: function (data) {
            if (data.data !== undefined)
                data = data.data;
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            if (successCallback !== undefined && typeof successCallback === "function") successCallback(data);
        },
        error: function (xhr, textStatus) {
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            if (xhr.status === 400 || xhr.status === 401) {
                console.log("StatusText: " + xhr.statusText + ", Message: " + xhr.responseText);
                if (xhr.responseText.includes("https://tools.ietf.org/html")) {
                    SwalNotifyError(JSON.parse(xhr.responseText).errors.Name.join(";"));
                } else {
                    SwalNotifyError(xhr.responseText);
                }
                return;
            } else {
                var message = xhr.responseText !== undefined ? xhr.responseText : textStatus;
                console.log(message);
                SwalNotifyError("Có lỗi xảy ra. Xin vui lòng thử lại sau hoặc thông báo với quản trị. Chi tiết: F12");
                return;
            }
        }
    });
}
function AjaxFile(url, objAjax, successCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        contentType: false,
        processData: false,
        data: objAjax.parameters,
        success: function (response) {
            successCallback(response);
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 400 || xhr.status === 401) {
                console.log("StatusText: " + xhr.statusText + ", Message: " + xhr.responseText);
                SwalMessage(xhr.responseText, "Thông báo", undefined, "error");
                return;
            } else {
                var message = xhr.responseText !== undefined ? xhr.responseText : textStatus;
                console.log(message);
                SwalNotifyError("Có lỗi xảy ra. Xin vui lòng thử lại sau hoặc thông báo với quản trị. Chi tiết: F12");
                return;
            }
        }
    });
}
//Loading//
function StartLoading(selector) {
    var height = $(selector).height();
    if (height < 50) {
        $(selector).loading({
            message: '<img src="https://cdn.phudev.com/images/loading2.gif" width="30"></div>'
        });
    }
    if (height < 100 && height >= 50) {
        $(selector).loading({
            message: '<img src="https://cdn.phudev.com/images/loading2.gif" width="50"></div>'
        });
    }
    if (height < 200 && height >= 100) {
        $(selector).loading({
            message: '<img src="https://cdn.phudev.com/images/loading2.gif" width="70"> <div style="padding-left: 10px;">Đang tải...</div>'
        });
    }
    if (height < 500 && height >= 200) {
        $(selector).loading({
            message: '<img src="https://cdn.phudev.com/images/loading2.gif" width="80"> <div style="padding-left: 10px;">Đang tải...</div>'
        });
    }
    if (height >= 500) {
        $(selector).loading({
            message: '<img src="https://cdn.phudev.com/images/loading2.gif" width="100"> <div style="padding-left: 10px;">Đang tải...</div>'
        });
    }
}
function StopLoading(selector) {
    $(selector).loading('stop');
}
//Thông báo góc màn hình
function SwalNotifyError(title) {
    toastr.error(title, "Lỗi");
}
function SwalNotifyWarning(title) {
    toastr.warning(title, "Cảnh báo");
}
function SwalNotifySuccess(title) {
    toastr.success(title, "Thành công");
}
function SwalNotifyInfo(title) {
    toastr.info(title, "Thông tin");
}
function SwalMain(objSwal, funcConfirm) {
    Swal.fire({
        title: objSwal.title,
        html: objSwal.html,
        width: objSwal.width,
        footer: objSwal.footer,
        icon: objSwal.icon,
        confirmButtonText: objSwal.textConfirm,
        cancelButtonText: objSwal.textCancel,
        timer: objSwal.timer,
        showCloseButton: objSwal.showCloseButton,
        showCancelButton: objSwal.showCancelButton,
        focusCancel: objSwal.focusCancel,
        showConfirmButton: objSwal.showConfirmButton,
        allowOutsideClick: objSwal.allowOutsideClick,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-danger"
        }
    }).then((result) => {
        if (result.value) {
            funcConfirm();
        }
    });
}
//Thông báo
function SwalMessage(html, title, width, icon) {
    ResetSwalDefault();
    objSwal.html = html;
    objSwal.title = title === undefined ? "Thông báo" : title;
    objSwal.width = width === undefined ? null : width;
    objSwal.icon = icon === undefined ? "warning" : icon;
    objSwal.showConfirmButton = false;
    objSwal.focusCancel = true;
    SwalMain(objSwal, null);
}
//Alert theo div
function SwalMessageDiv(divId, funcConfirm, title, width, icon) {
    ResetSwalDefault();
    objSwal.html = $("#" + divId).html();
    objSwal.title = title === undefined ? "Thông báo" : title;
    objSwal.width = width === undefined ? null : width;
    objSwal.icon = icon === undefined ? "question" : icon;
    objSwal.allowOutsideClick = false;
    SwalMain(objSwal, funcConfirm);
}
//Alert có nút Comfirm AlertConfirm
function SwalConfirm(html, funcConfirm, title, width, icon, textConfirm) {
    ResetSwalDefault();
    objSwal.html = html;
    objSwal.title = (title == undefined) ? "Thông báo" : title;
    objSwal.width = (width == undefined) ? null : width;
    objSwal.icon = (icon == undefined) ? "question" : icon;
    objSwal.textConfirm = (textConfirm == undefined) ? "Xác nhận" : textConfirm;
    objSwal.allowOutsideClick = false;
    SwalMain(objSwal, funcConfirm);
}

//Hàm thực hiện Show thông báo cảnh báo
function AlertMessSwal(sMess) {
    Swal.fire({
        title: "Thông báo",
        html: sMess,
        icon: 'warning',
        animation: false,
        showCancelButton: true,
        showConfirmButton: false,
    });
}
/*SetCookie*/
function SetCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + exdays * 24 * 60 * 60 * 1000);
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
/*GetCookie*/
function GetCookie(cname) {
    if (document.cookie === "") return "";
    var name = cname + "=";
    try {
        var ca = document.cookie.split(";");
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === " ") {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    } catch (e) {
        return "";
    }
}
/*CreateURL*/
function CreateURL(prefix, objValue) {
    if (prefix === undefined) return "";
    if (objValue === undefined) return prefix;
    var url = prefix + "?";
    for (var prop in objValue) {
        if (objValue.hasOwnProperty(prop)) {
            url += prop + "=" + objValue[prop] + "&";
        }
    }
    return (url + "*").replace("&*", "");
}

/*Thực hiện kiểm tra UserName*/
function IsSpaceString(value) {
    var spaceChars = " ";
    for (var i = 0; i < value.length; i++) {
        if (spaceChars.indexOf(value[i]) >= 0)
            return true;
    }
    return false;
}
function Is_String(value) {
    var spaceChars = "_";
    for (var i = 0; i < value.length; i++) {
        if (spaceChars.indexOf(value[i]) >= 0)
            return true;
    }
    return false;
}
function IsInvalidUserName(value) {
    if (Is_String(value))
        value = value.replace(/_/g, "");
    var pattern = /^[a-zA-Z0-9]+$/;
    if (pattern.test(value)) {
        return true;
    }
    else return false;
}
/*
 *FormatMoney
 */
String.prototype.reverse = function () { return this.split("").reverse().join(""); }
function FormatMoney(input) {
    var x = input.value;
    x = x.replace(/,/g, ""); // Strip out all commas
    x = x.reverse();
    x = x.replace(/.../g, function (e) {
        return e + ",";
    }); // Insert new commas
    x = x.reverse();
    x = x.replace(/^,/, ""); // Remove leading comma
    input.value = x;
}

var FormatMoneyNew = function (input) {
    var num = input.value;
    var sign = num.charAt(0);
    var str = num.toString().replace("$", ""), parts = false, output = [], i = 1, formatted = null;
    if (str.indexOf(".") > 0) {
        parts = str.split(".");
        str = parts[0];
    }
    str = str.split("").reverse();
    for (var j = 0, len = str.length; j < len; j++) {
        if (str[j] != ",") {
            output.push(str[j]);
            if (i % 3 == 0 && j < (len - 1)) {
                output.push(",");
            }
            i++;
        }
    }
    formatted = output.reverse().join("");

    var value = (formatted + ((parts) ? "." + parts[1].substr(0, 3) : ""));
    if (sign == '-') {
        value = ('-' + value).replaceAll("--", "-");
    }
    input.value = value.replaceAll("--", "-").replaceAll("-,", "-");
};

function LoadComboboxApiForName(selector, url, isAll, valueSelect, objAjax, textAll, valueAll) {
    var prefixValue = "Value";
    var prefixAvatar = "Avatar";
    var prefixDescription = "Description";
    var prefixName = "Name";
    if (valueAll === undefined) valueAll = 0;
    var htmlText = "";
    AjaxRequestClientAPI("GET", url, objAjax, function (data) {
        try {
            data = JSON.parse(data);
        } catch {
            data = data
        }
        console.log(data);
        if (isAll) {
            htmlText = "<option selected=\"selected\" data-user-avatar=''  data-user-description='' value= \"" + valueAll + "\">" + ((textAll === undefined || textAll === "") ? "Tất cả" : textAll) + "</option>";
        }
        for (var i = 0; i < data.length; i++) {
            var selected = (valueSelect !== undefined && valueSelect === data[i][prefixValue]) ? "selected=\"selected\"" : "";
            htmlText += "<option " + selected + " data-user-avatar='" + (data[i][prefixAvatar] ?? "") + "' data-user-description='" + (data[i][prefixDescription] ?? "") + "' value='" + data[i][prefixName] + "'>" + data[i][prefixName] + "</option>";
        }
        console.log(htmlText);
        $(selector).html(htmlText);
        var optionFormat = function (item) {
            if (!item.id) {
                return item.text;
            }
            var span = document.createElement('span');
            var avatar = item.element.getAttribute('data-user-avatar');

            var template = '';
            if (!(avatar === '' || avatar === null || avatar === undefined || avatar === "null"))
                template += '<img src="' + avatar + '" class="rounded-circle h-20px  w-20px me-2" alt="image"/>';
            template += item.text;
            span.innerHTML = template;
            return $(span);
        };

        var optionResult = (item) => {
            if (!item.id) {
                return item.text;
            }
            var span = document.createElement('span');
            var avatar = item.element.getAttribute('data-user-avatar');
            var description = item.element.getAttribute('data-user-description');
            var template = '';
            template += '<div class="d-flex align-items-center">';
            if (!(avatar === '' || avatar === null || avatar === undefined || avatar === "null"))
                template += '<img src="' + avatar + '" class="rounded-circle h-40px  w-40px me-3" alt="image"/>';
            template += '<div class="d-flex flex-column">';
            template += '<span style="font-weight: bold;color:#ff907a">' + item.text + '</span>';
            if (!(description === '' || description === null || description === undefined || description === "null"))
                template += '<span>' + description + '</span>';
            template += '</div>';
            template += '</div>';

            span.innerHTML = template;

            return $(span);
        }
        $(selector).select2({
            templateSelection: optionFormat,
            templateResult: optionResult
        });
    }, true, "text");
}

/*
 ***********************LoadCombobox***********************
 */
//Bộ chọn cho trường hợp Api

function LoadComboboxApi(selector, url, isAll, valueSelect, objAjax, textAll, valueAll) {
    var prefixValue = "Value";
    var prefixAvatar = "Avatar";
    var prefixDescription = "Description";
    var prefixName = "Name";
    if (valueAll === undefined) valueAll = 0;
    var htmlText = "";
    AjaxRequestClientAPI("GET", url, objAjax, function (data) {
        try {
            data = JSON.parse(data);
        } catch {
            data = data
        }
        console.log(data);
        if (isAll) {
            htmlText = "<option selected=\"selected\" data-user-avatar=''  data-user-description='' value= \"" + valueAll + "\">" + ((textAll === undefined || textAll === "") ? "Tất cả" : textAll) + "</option>";
        }
        for (var i = 0; i < data.length; i++) {
            var selected = (valueSelect !== undefined && valueSelect === data[i][prefixValue]) ? "selected=\"selected\"" : "";
            htmlText += "<option " + selected + " data-user-avatar='" + (data[i][prefixAvatar] ?? "") + "' data-user-description='" + (data[i][prefixDescription] ?? "") + "' value='" + data[i][prefixValue] + "'>" + data[i][prefixName] + "</option>";
        }
        console.log(htmlText);
        $(selector).html(htmlText);
        var optionFormat = function (item) {
            if (!item.id) {
                return item.text;
            }
            var span = document.createElement('span');
            var avatar = item.element.getAttribute('data-user-avatar');

            var template = '';
            if (!(avatar === '' || avatar === null || avatar === undefined || avatar === "null"))
                template += '<img src="' + avatar + '" class="rounded-circle h-20px  w-20px me-2" alt="image"/>';
            template += item.text;
            span.innerHTML = template;
            return $(span);
        };

        var optionResult = (item) => {
            if (!item.id) {
                return item.text;
            }
            var span = document.createElement('span');
            var avatar = item.element.getAttribute('data-user-avatar');
            var description = item.element.getAttribute('data-user-description');
            var template = '';
            template += '<div class="d-flex align-items-center">';
            if (!(avatar === '' || avatar === null || avatar === undefined || avatar === "null"))
                template += '<img src="' + avatar + '" class="rounded-circle h-40px  w-40px me-3" alt="image"/>';
            template += '<div class="d-flex flex-column">';
            template += '<span style="font-weight: bold;color:#ff907a">' + item.text + '</span>';
            if (!(description === '' || description === null || description === undefined || description === "null"))
                template += '<span>' + description + '</span>';
            template += '</div>';
            template += '</div>';

            span.innerHTML = template;

            return $(span);
        }
        $(selector).select2({
            templateSelection: optionFormat,
            templateResult: optionResult
        });
    }, true, "text");
}
//Bộ chọn cho trường hợp Api
function LoadComboboxUserApi(selector, url, isAll, valueSelect, objAjax, prefixId, prefixName, prefixDes, prefixAvatar, textAll, idModal, valueAll) {
    if (isAll === undefined) isAll = false;
    if (idModal === undefined) idModal = "";
    if (valueAll === undefined) valueAll = 0;

    var htmlText = "";

    var parameters = undefined, divLoading = undefined;
    if (objAjax !== undefined && objAjax !== null) {
        parameters = objAjax.parameters;
        divLoading = objAjax.divLoading;
    }
    if (divLoading !== undefined && divLoading !== "")
        StartLoading(divLoading);
    $.ajax({
        processData: false,
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: parameters === undefined ? "" : parameters,
        dataType: "json",
        async: false,
        crossDomain: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        },
        success: function (data) {
            try {
                data = JSON.parse(data);
            } catch {
                data = data
            }
            if (data.data !== undefined)
                data = data.data;
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            if (isAll) {
                if (textAll === undefined)
                    htmlText = "<option selected=\"selected\" data-user-avatar=''  data-user-des='' value= \"" + valueAll + "\">Tất cả</option>";
                else
                    htmlText = "<option selected=\"selected\" data-user-avatar=''  data-user-des='' value= \"" + valueAll + "\">" + textAll + "</option>";
            }
            for (var i = 0; i < data.length; i++) {
                if (valueSelect !== undefined && valueSelect === data[i][prefixId])
                    htmlText += "<option selected=\"selected\" data-user-avatar='" + data[i][prefixAvatar] + "' data-user-des='" + data[i][prefixDes] + "' value='" + data[i][prefixId] + "'>" + data[i][prefixName] + "</option>";
                else
                    htmlText += "<option data-user-avatar='" + data[i][prefixAvatar] + "'  data-user-des='" + data[i][prefixDes] + "' value='" + data[i][prefixId] + "'>" + data[i][prefixName] + "</option>";
            }
            $(selector).html(htmlText);

            var optionFormat = function (item) {
                if (!item.id) {
                    return item.text;
                }
                var span = document.createElement('span');
                var imgUrl = item.element.getAttribute('data-user-avatar');

                var template = '';
                if (!(imgUrl === '' || imgUrl === null || imgUrl === undefined))
                    template += '<img src="' + imgUrl + '" class="rounded-circle h-20px  w-20px me-2" alt="image"/>';
                template += item.text;
                span.innerHTML = template;
                return $(span);
            };

            var optionResult = (item) => {
                if (!item.id) {
                    return item.text;
                }
                var span = document.createElement('span');
                var imgUrl = item.element.getAttribute('data-user-avatar');
                var des = item.element.getAttribute('data-user-des');
                var template = '';
                template += '<div class="d-flex align-items-center">';
                if (!(imgUrl === '' || imgUrl === null || imgUrl === undefined))
                    template += '<img src="' + imgUrl + '" class="rounded-circle h-40px  w-40px me-3" alt="image"/>';
                template += '<div class="d-flex flex-column">';
                template += '<span>' + item.text + '</span>';
                if (!(des === '' || des === null || des === undefined))
                    template += '<span>' + des + '</span>';
                template += '</div>';
                template += '</div>';

                span.innerHTML = template;

                return $(span);
            }
            if (idModal !== "") {
                $(selector).select2({
                    dropdownParent: $('#' + idModal),
                    templateSelection: optionFormat,
                    templateResult: optionResult
                });
            } else {
                $(selector).select2({
                    templateSelection: optionFormat,
                    templateResult: optionResult
                });
            }
        },
        error: function (xhr, textStatus) {
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            if (xhr.status === 400 || xhr.status === 401) {
                console.log("StatusText: " + xhr.statusText + ", Message: " + xhr.responseJSON.Message);
                SwalNotifyError(xhr.responseJSON.Message);
                return;
            } else {
                var message = xhr.responseText !== undefined ? xhr.responseText : textStatus;
                console.log(message);
                SwalNotifyError("Có lỗi xảy ra. Xin vui lòng thử lại sau hoặc thông báo với quản trị. Chi tiết: F12");
                return;
            }
        }
    });
}
//Bộ chọn cho trường hợp gõ => vào DB tìm kiếm
function LoadComboboxSearch(selector, url, minimumInputLength, valueSelectText, valueSelectId, prefixId, prefixText, dataType, idModal) {
    if (idModal === undefined) idModal = "";
    if (minimumInputLength === undefined) minimumInputLength = 3;
    $(selector).select2({
        minimumInputLength: minimumInputLength,
        dropdownParent: idModal !== "" ? $('#' + idModal) : $('body'),
        //allowClear: true,
        ajax: {
            url: url,
            dataType: dataType,
            async: true,
            crossDomain: false,
            type: 'GET',
            quietMillis: 250,
            contentType: "application/json; charset=utf-8",
            data: function (params) {
                return {
                    q: params.term,
                    page: params.page
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        try {
                            data = JSON.parse(data);
                        } catch {
                            data = data
                        }
                        return {
                            text: item[prefixText],
                            id: item[prefixId]
                        }
                    })
                };
            },
            cache: true
        }
    });
    if (valueSelectText !== "")
        $(selector).append(new Option(valueSelectText, valueSelectId, true, true)).trigger('change');
}
function LoadComboboxUserSearch(selector, url, minimumInputLength, valueSelectText, valueSelectId, prefixId, prefixText, prefixDes, prefixAvatar, dataType, idModal) {
    if (idModal === undefined) idModal = "";
    if (minimumInputLength === undefined) minimumInputLength = 3;

    var optionFormat = function (item) {
        if (!item.id) {
            return item.text;
        }
        var span = document.createElement('span');
        var imgUrl = item.avatar;

        var template = '';
        if (!(imgUrl === '' || imgUrl === null || imgUrl === undefined))
            template += '<img src="' + imgUrl + '" class="rounded-circle h-20px w-20px me-2" alt="image"/>';
        template += item.text;
        span.innerHTML = template;
        return $(span);
    };

    var optionResult = (item) => {
        if (!item.id) {
            return item.text;
        }
        var span = document.createElement('span');
        var imgUrl = item.avatar;
        var des = item.des;
        var template = '';
        template += '<div class="d-flex align-items-center">';
        if (!(imgUrl === '' || imgUrl === null || imgUrl === undefined))
            template += '<img src="' + imgUrl + '" class="rounded-circle h-40px w-40px me-3" alt="image"/>';
        template += '<div class="d-flex flex-column">';
        template += '<span>' + item.text + '</span>';
        if (!(des === '' || des === null || des === undefined))
            template += '<span>' + des + '</span>';
        template += '</div>';
        template += '</div>';

        span.innerHTML = template;

        return $(span);
    }
    $(selector).select2({
        minimumInputLength: minimumInputLength,
        dropdownParent: idModal !== "" ? $('#' + idModal) : $('body'),
        ajax: {
            url: url,
            dataType: dataType,
            async: true,
            crossDomain: false,
            type: 'GET',
            quietMillis: 250,
            contentType: "application/json; charset=utf-8",
            data: function (params) {
                return {
                    q: params.term,
                    page: params.page
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item[prefixText],
                            id: item[prefixId],
                            avatar: item[prefixAvatar],
                            des: item[prefixDes]
                        }
                    })
                };
            },
            cache: true
        },
        templateSelection: optionFormat,
        templateResult: optionResult
    });

    if (valueSelectText !== "")
        $(selector).append(new Option(valueSelectText, valueSelectId, true, true)).trigger('change');
}
function LoadComboboxAsync(divId, url, isAll, isChosen, valueSelect, valueMultiple, objAjax, prefixId, prefixName, maxHeight, valueAll) {
    if (isAll === undefined) isAll = false;
    if (isChosen === undefined) isChosen = false;
    if (valueAll === undefined) valueAll = 0;
    if (maxHeight === undefined) maxHeight = 150;
    var htmlText = "";
    AjaxGetAsync(url, objAjax,
        function (result) {
            if (isAll)
                htmlText = '<option selected="selected" value= "' + valueAll + '">Tất cả</option>';
            for (var i = 0; i < result.length; i++) {
                if (valueSelect !== undefined && valueSelect === data[i][prefixId])
                    htmlText += "<option selected=\"selected\" value='" + data[i][prefixId] + "'>" + data[i][prefixName] + "</option>";
                else
                    htmlText += "<option value='" + data[i][prefixId] + "'>" + data[i][prefixName] + "</option>";
            }
            $("#" + divId).html(htmlText);
            if (valueMultiple !== undefined)
                $("#" + divId).val(JSON.parse("[" + valueMultiple + "]")).trigger("chosen:updated");
        });
}
//Bộ chọn select 2 có tag
function LoadComboboxTagApi(selector, url, value, objAjax, prefixId, prefixName, idModal) {
    if (idModal === undefined) idModal = "";
    if (value === undefined) value = "";
    var htmlText = "";

    var parameters = undefined, divLoading = undefined;
    if (objAjax !== undefined && objAjax !== null) {
        parameters = objAjax.parameters;
        divLoading = objAjax.divLoading;
    }
    if (divLoading !== undefined && divLoading !== "")
        StartLoading(divLoading);
    $.ajax({
        processData: false,
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: parameters === undefined ? "" : parameters,
        dataType: "json",
        async: false,
        crossDomain: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        },
        success: function (data) {
            if (data !== "" && data !== undefined)
                data = JSON.parse(data);
            if (data.data !== undefined)
                data = data.data;
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i][prefixId] + "'>" + data[i][prefixName] + "</option>";
            }
            $(selector).html(htmlText);
            if (idModal !== "") {
                $(selector).select2({
                    createSearchChoice: function (term, data) {
                        if ($(data).filter(function () {
                            return this.text.localeCompare(term) === 0;
                        }).length === 0) {
                            return { id: '00000000-0000-0000-0000-000000000000', text: term };
                        }
                        return { id: '00000000-0000-0000-0000-000000000000', text: term };
                    },
                    dropdownParent: $('#' + idModal),
                    multiple: true,
                    tags: true
                });
            } else {
                $(selector).select2({
                    createSearchChoice: function (term, data) {
                        if ($(data).filter(function () {
                            return this.text.localeCompare(term) === 0;
                        }).length === 0) {
                            return { id: '00000000-0000-0000-0000-000000000000', text: term };
                        }
                        return { id: '00000000-0000-0000-0000-000000000000', text: term };
                    },
                    multiple: true,
                    tags: true
                });
            }
            $(selector).val(value).trigger("change");
        },
        error: function (xhr, textStatus) {
            if (divLoading !== undefined && divLoading !== "")
                StopLoading(divLoading);
            if (xhr.status === 400 || xhr.status === 401) {
                console.log("StatusText: " + xhr.statusText + ", Message: " + xhr.responseJSON.Message);
                SwalNotifyError(xhr.responseJSON.Message);
                return;
            } else {
                var message = xhr.responseText !== undefined ? xhr.responseText : textStatus;
                console.log(message);
                SwalNotifyError("Có lỗi xảy ra. Xin vui lòng thử lại sau hoặc thông báo với quản trị. Chi tiết: F12");
                return;
            }
        }
    });
}
//*************************************************************************
//****************************summernote**********************************
//**************************************************************************
function Summernote(strId, height) {
    if (height === undefined) height = 100;
    $(strId).summernote({
        lang: 'vi-VN',
        focus: true,
        height: height,
        tabsize: 2,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['insert', ['link', 'picture', 'video']],
            ['height', ['height']],
            ['view', ['fullscreen', 'codeview']]
        ],
        callbacks: {
            onImageUpload: function (image) {
                UploadImage(image[0], strId);
            }
        }
    });
}
function UploadImage(image, id) {
    var data = new FormData();
    data.append("image", image);
    $.ajax({
        data: data,
        type: "POST",
        url: "/Home/UploadFileSummerNote",
        cache: false,
        processData: false,
        contentType: false,
        success: function (response) {
            var data = response.data;
            if (response.code === 200)
                $(id).summernote("insertImage", data);
            else
                ShowMessage(data, "Thông báo lỗi", 550);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + xhr.responseText);
            ShowMessage("Có lỗi xảy ra. Xin vui lòng thử lại sau hoặc thông báo với quản trị.", "Thông báo lỗi", 550);
        }
    });
}
//Hàm làm tròn số: 3.2222222 -> 3.22
function MathRound(x) {
    return Math.round(parseFloat(x) * 1000) / 1000;
}
function MathPercent(iNumber, iTotal) {
    if (iTotal === 0) return 0;
    return parseFloat((iNumber * 100 / iTotal).toFixed(1));
}
function ToFixed(x, fix) {
    return Number.parseFloat(x).toFixed(fix);
}

//VD: "ABC" -> 'ABC'
function ReplaceJS(str) {
    return str.replace(/\"/g, '\'');
}
function ConvertStringMoneyToNumber(number) {
    return number.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + " VNĐ";
}
function DateTimeNow() {
    return $.datepicker.formatDate('dd/mm/yy', new Date());
}
//"2018-07-20T12:30:00" -> 01-08-2017 12:30
function ConvertDateSql(datetime) {
    var arrDate = datetime.substring(0, 10).split('-');
    var arrTime = datetime.substring(11, 16);
    return arrDate[2] + '/' + arrDate[1] + '/' + arrDate[0] + " " + arrTime;
}
//"2018-07-20T12:30:00" -> 01/08/2017
function ConvertDateSqlToDateString(datetime) {
    var arrDate = datetime.substring(0, 10).split('-');
    return arrDate[2] + '/' + arrDate[1] + '/' + arrDate[0];
}
//"2018-07-20T12:30:00" -> 12:30
function ConvertDateSqlToTimeString(datetime) {
    var arrTime = datetime.substring(11, 16);
    return arrTime;
}
//"20/07/2018 12:30" -> 2018-07-20 12:30
function CheckDateTime(datetime) {
    try {
        var dateTimeArr = datetime.trim().split(" ");
        var arrDate = dateTimeArr[0].split('/');
        var arrTime = dateTimeArr[1].split(':');
        if (arrDate[0] > 31 || arrDate[1] > 12 || arrDate[2] > 2099 || arrTime[0] > 24 || arrTime[1] > 60)
            return false;
        else
            return true;
    } catch (e) {
        return false;
    }
}
//"20/07/2018 12:30" -> 2018-07-20 12:30
function ConvertDateTime(datetime) {
    var arrDate = datetime.substring(0, 10).split('/');
    var arrTime = datetime.substring(11, 16);
    return new Date((arrDate[2] + '-' + arrDate[1] + '-' + arrDate[0] + " " + arrTime)?.trim());
}
//18/07/2018 => 2018/07/18
function FormatDate(strDate) {
    var arrBirthDate = strDate.split('/');
    return arrBirthDate[2] + '/' + arrBirthDate[1] + '/' + arrBirthDate[0];
}
//18/07/2018 => 2018/07/18
function FormatDateUrl(strDate) {
    var arrBirthDate = strDate.split('/');
    return arrBirthDate[2] + '-' + arrBirthDate[1] + '-' + arrBirthDate[0];
}
//07/18/2018 => 2018/07/18
function FormatDateMMddyyyy(strDate) {
    var arrBirthDate = strDate.split('/');
    return arrBirthDate[2] + '/' + arrBirthDate[0] + '/' + arrBirthDate[1];
}
function ConvertStringToDate(strDate) {//dd/MM/YYYY
    var arrBirthDate = strDate.split('/');
    return new Date(arrBirthDate[2] + '/' + arrBirthDate[1] + '/' + arrBirthDate[0]);
}
function ConvertStringToDateTime(strDateTime) {//dd/MM/YYYY hh:mm
    var arr = strDateTime.trim().split(' ');
    var arrBirthDate = arr[0].split('/');
    var arrTime = arr[1].split(':');
    return new Date(arrBirthDate[2] + '/' + arrBirthDate[1] + '/' + arrBirthDate[0] + ' ' + arrTime[0] + ':' + arrTime[1]);
}
function ConvertDate(strDate) {//dd/MM/YYYY
    var arrBirthDate = strDate.split('/');
    return arrBirthDate[2] + '/' + arrBirthDate[1] + '/' + arrBirthDate[0];
}
function ConvertDateFormat(date) {//dd/MM/YYYY
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year
    var hour = date.getHours();     // yields hours
    var minute = date.getMinutes(); // yields minutes
    var second = date.getSeconds(); // yields seconds

    // After this construct a string with the above results as below
    return month + "/" + day + "/" + year + " " + hour + ':' + minute + ':' + second;
}

function ConvertDateTimeFormat(date) {//dd/MM/YYYY HH:mm:ss
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year
    var hour = ("0" + date.getHours()).slice(-2);     // yields hours
    var minute = ("0" + date.getMinutes()).slice(-2); // yields minutes
    var second = ("0" + date.getSeconds()).slice(-2); // yields seconds

    // After this construct a string with the above results as below
    return day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;
}

function ConvertFulltimeToStr(fulltime) {
    var arrFull = fulltime.split('T');
    var arrDate = arrFull[0].substring(0, 10).split('-');
    var strDate = arrDate[2] + '/' + arrDate[1] + '/' + arrDate[0];
    var strTime = arrFull[1].substring(0, 8);
    return strTime + " " + strDate;
}
function CheckNumber(value) {
    return $.isNumeric(value);
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//VD 1000 -> 1,000
function FormatMoneyVi(iNumber) {
    return iNumber.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

//VD 1,000 -> 1000
function ConvertMoney(strMoney) {
    if (strMoney === "" || strMoney === undefined || strMoney === null)
        return 0;
    else
        return parseFloat(strMoney.replace(/,/g, ""));
}
//VD 8941.25 -> 8,924.25
function ForamatUsd(value) {
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });
    return formatter.format(value).replace("$", "");
}
function FormatUsd(value) {
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });
    return formatter.format(value).replace("$", "");
}
function CheckMoney(strMoney) {
    if (strMoney === "" || strMoney === undefined || strMoney === null)
        return true;
    else
        return CheckNumber(strMoney.replace(/,/g, ""));
}
function EncodeBase64(str) {
    var base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64; } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9\+\/\=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u !== 64) { t = t + String.fromCharCode(r); } if (a !== 64) { t = t + String.fromCharCode(i); } } t = base64._utf8_decode(t); return t; }, _utf8_encode: function (e) { e = e.replace(/\r\n/g, "\n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128); } } return t; }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++; } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3; } } return t; } };
    // Encode the String
    return base64.encode(str);
}
function DecodeBase64(str) {
    var base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64; } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9\+\/\=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u !== 64) { t = t + String.fromCharCode(r); } if (a !== 64) { t = t + String.fromCharCode(i); } } t = base64._utf8_decode(t); return t; }, _utf8_encode: function (e) { e = e.replace(/\r\n/g, "\n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128); } } return t; }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++; } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3; } } return t; } };
    // Decode the String
    return base64.decode(str);
}
//Hàm kiểm tra số container
function CheckContainerNumber(containerNumber) {
    if (containerNumber.length !== 11) {
        SwalNotifyError("Container '" + containerNumber + "' chưa đủ 11 ký tự. Kiểm tra lại!");
        return false;
    }
    var prefix = containerNumber.slice(0, 4);
    var serial = containerNumber.slice(4, 10);
    var end = containerNumber.slice(10, 11);
    if (prefix.charAt(3).toUpperCase() !== "U") {
        SwalNotifyError("Ký tự cuối cùng của Tiếp đầu ngữ thường là chữ 'U'. Kiểm tra lại!");
        return false;
    }
    if (prefix.match(/[^a-z]/i)) {
        SwalNotifyError("Tiếp đầu ngữ chỉ có thể là chữ cái!");
        return false;
    }
    if (isNaN(Number(serial))) {
        SwalNotifyError("Số Sê-ri '" + serial + "' không hợp lệ. Kiểm tra lại.");
        return false;
    }
    containerNumber = containerNumber.toUpperCase();
    var contArray = new Array;
    for (var i = 0; i < 10; i++) {
        contArray[i] = containerNumber.charAt(i);
    }
    for (var j = 0; j < 4; j++) {
        var tiepdaungu = contArray[j];
        var alphabet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        var so = [
            10, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 34, 35, 36, 37, 38
        ];
        var pos = alphabet.search(tiepdaungu);
        contArray[j] = so[pos];
    }
    var tongso = 0;
    var sokiemtra;
    for (var k = 0; k < 10; k++) {
        contArray[k] = contArray[k] * Math.pow(2, k);
        tongso = tongso + contArray[k];
    }
    sokiemtra = tongso % 11;
    if (sokiemtra === 10) {
        sokiemtra = 0;
    }
    if (parseInt(end) !== sokiemtra) {
        SwalNotifyError("Số Container '" + containerNumber + "' không đúng. Vui lòng kiểm tra lại!");
        return false;
    }
    return true;
}
function RoundToTwo(num) {
    return +(Math.round(num + "e+2") + "e-2");
}
function RoundToThree(num) {
    return +(Math.round(num + "e+3") + "e-3");
}
function CopyToClipboard(text) {
    var dummy = document.createElement("textarea");
    document.body.appendChild(dummy);
    dummy.value = text;
    dummy.select();
    document.execCommand("copy");
    document.body.removeChild(dummy);
    SwalNotifySuccess("Đã Copy");
}
function isMobileDevice() {
    return (typeof window.orientation !== "undefined") || (navigator.userAgent.indexOf('IEMobile') !== -1);
}
function GetTimeFloat(dateTime) {
    if (dateTime.getMinutes() >= 30)
        return parseFloat(dateTime.getHours() + ".5");
    return dateTime.getHours();
}

function IsVietnameseString(value) {
    var vietnameseChars = "ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ";
    for (var i = 0; i < value.length; i++) {
        if (vietnameseChars.indexOf(value[i]) >= 0)
            return true;
    }
    return false;
}
function IsSpaceString(value) {
    var spaceChars = " ";
    for (var i = 0; i < value.length; i++) {
        if (spaceChars.indexOf(value[i]) >= 0)
            return true;
    }
    return false;
}
//Kiểm tra tên đăng nhập
function IsInvalidUser(value) {
    var pattern = /^[a-zA-Z0-9]+$/;
    if (pattern.test(value)) {
        return true;
    }
    else return false;
}

function FormatVietnameseNumber(value) { // format value ở dạng số mà code hiểu sang dạng Việt Nam
    if (isNaN(value)) return false;

    var pointParts = value.toString().split(".");

    value = "";
    var first = pointParts[0];
    while (first.length > 0) {
        var i = first.length - 3;
        if (i < 0) i = 0;
        if (value.length > 0) value = "." + value;
        value = first.substring(i) + value;
        first = first.substring(0, i);
    }
    if (pointParts.length > 1) value = value + "," + pointParts[1];

    return value;
}
// Kiểm tra số điện thoại có đúng không
function checkPhoneNumber(phone) {
    var flag = false;
    phone = phone.replace('(+84)', '0');
    phone = phone.replace('+84', '0');
    phone = phone.replace('0084', '0');
    phone = phone.replace(/ /g, '');
    if (phone !== '') {
        var firstNumber = phone.substring(0, 2);
        if ((firstNumber === '09' || firstNumber === '08' || firstNumber === '03' || firstNumber === '07' || firstNumber === '05') && phone.length === 10) {
            if (phone.match(/^\d{10}/)) {
                flag = true;
            }
        } else if (firstNumber === '01' && phone.length === 11) {
            if (phone.match(/^\d{11}/)) {
                flag = true;
            }
        }
    }
    return flag;
}

function GetTextSelect2(id, prefix) {
    if (prefix === undefined) prefix = ", ";
    var text = "";
    var selected = $("#" + id).select2("data");
    for (var i = 0; i <= selected.length - 1; i++) {
        text += selected[i].text + prefix;
    }

    return (text + prefix).replace(prefix + prefix, "");
}
function Distinct(value, index, self) {
    return self.indexOf(value) === index;
}
function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 45 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length >= 3)) {
        return false;
    }
    return true;
}

function validateIntKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 45 && charCode > 31 && (charCode < 48 || charCode > 57 || charCode == 46)) {
        return false;
    }
    return true;
}

function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}
function KeyupNumber(id) {
    $('#' + id).keypress(function (event) {
        if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });
}
// không cho nhập số vào (Hùng thêm)
function KeyupText(id) {
    var ele = document.getElementById(id);
    console.log(ele);
    ele.addEventListener('keypress', function (e) {
        // Get the code of pressed key
        var key = e.which || e.keyCode;
        // 0, 1, ..., 9 have key code of 48, 49, ..., 57, respectively
        // Space has key code of 32
        if (key >= 48 && key <= 57) {
            // Prevent the default action
            e.preventDefault();
        }
    });
}

var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

function inWordsContainer(num) {
    if ((num = num.toString()).length > 9) return 'overflow';
    n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
    if (!n) return; var str = '';
    str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
    str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
    str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
    str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
    str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + '(' + num + ') container(s) only ' : '';
    return str.toUpperCase();
}

/*kiểm tra xem co item trong Attr hay không*/
function checkItemInAttribute(strInput, strTaget) {
    if (strInput == "" || strInput == null || strInput == undefined) {
        return false;
    }
    else {
        var ar = strInput.split(" ");
        for (var i = 0; i < ar.length; i++) {
            if (ar[i] == strTaget) {
                return true;
            }
        }
        return false;
    }
    return false;
}

/*lấy view từ html Tĩnh*/
getHtmlView = function (url) {
    var obj = $.get({
        url: url,
        async: false
    });
    if (obj.status == 200)
        return $(obj.responseText);
    else
        return $(undefined);
}

//convert  theo id
function ConvertDateTimeToString(id) {
    var dateTime = $(id).val().split(' ')[0].split("/").reverse().join("/") + ' ' + $(id).val().split(' ')[1];
    return dateTime;
}

/*Lấy thời gian chênh lệch với thời gian hiện tại*/
function GetTimeDiff(datetime) {
    var arrDate = datetime.substring(0, 10);
    var arrTime = datetime.substring(11, 16);
    var _datetime = typeof datetime !== 'undefined' ? new Date((arrDate + "T" + arrTime + ":00")) : new Date();
    var _datetime = _datetime.getTime();
    var now = new Date().getTime();
    if (isNaN(_datetime)) {
        return "";
    }
    if (_datetime < now) {
        var milisec_diff = now - _datetime;
    } else {
        var milisec_diff = _datetime - now;
    }
    var days = milisec_diff / 1000 / 60 / 60 / 24;
    var hours = milisec_diff / 1000 / 60 / 60;
    var minute = milisec_diff / 1000 / 60;
    if (Math.floor(days) > 0) {
        if (Math.floor(days) > 30) {
            return Math.floor((Math.floor(days) / 30)) + " tháng";
        }
        return Math.floor(days) + " ngày";
    } else if (Math.floor(days) == 0 && Math.floor(hours) > 0) {
        if (Math.floor(hours) > 6) {
            return Math.floor(hours) + " giờ ";
        }
        return Math.floor(hours) + " giờ " + (Math.floor(minute) - (Math.floor(hours) * 60)) + " phút";
    }
    else if (Math.floor(days) == 0 && Math.floor(hours) == 0) {
        if (Math.floor(minute) <= 1) {
            return "new";
        }
        return Math.floor(minute) + " phút";
    } else {
        return "new";
    }
}

////////////////////////////////////////////////////////////////////////
///////////////////////////////flatpickr////////////////////////////////
////////////////////////////////////////////////////////////////////////
function LoadInputmask() {
    $(".inputmask-datetime").inputmask("datetime", {
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        separator: "/",
        alias: "dd/mm/yyyy"
    });
    $(".inputmask-time").inputmask("datetime", {
        mask: "h:s",
        placeholder: "hh:mm",
        alias: "hh:mm"
    });
}

function LoadFlatpickr() {
    $(".input-datetime").flatpickr({
        enableTime: true,
        dateFormat: "d/m/Y H:i",
        altFormat: "d/m/Y H:i",
        locale: "vn",
        allowInput: true,
        altInput: true,
        time_24hr: true
    });
    $(".input-daterange").flatpickr({
        allowInput: true,
        altInput: true,
        altFormat: "d/m/Y",
        dateFormat: "d/m/Y",
        mode: "range",
        locale: "vn"
    });
    $(".input-date").flatpickr({
        enableTime: false,
        allowInput: true,
        altInput: true,
        dateFormat: "d/m/Y",
        altFormat: "d/m/Y",
        locale: "vn"
    });
}
function SetDatetimeFlatpickr(id, value) {
    window.flatpickr(id,
        {
            enableTime: true,
            dateFormat: "d/m/Y H:i",
            altFormat: "d/m/Y H:i",
            locale: "vn",
            allowInput: true,
            altInput: true,
            time_24hr: true
        }).setDate(ConvertDateSql(value), true, "d/m/Y H:i");
}
function SetTimeFlatpickr(id, value) {
    window.flatpickr(id,
        {
            enableTime: true,
            noCalendar: true,
            dateFormat: "H:i",
            altFormat: "H:i",
            locale: "vn",
            time_24hr: true,
            allowInput: true,
            altInput: true
        }).setDate(value, true, "H:i");
}
function SetDateRangeFlatpickr(id, value) {
    window.flatpickr(id,
        {
            allowInput: true,
            altInput: true,
            altFormat: "d/m/Y",
            dateFormat: "d/m/Y",
            mode: "range",
            locale: "vn"
        }).setDate(value, true, "d/m/Y");
}
function SetDateFlatpickr(id, value) {
    window.flatpickr(id,
        {
            enableTime: false,
            allowInput: true,
            altInput: true,
            dateFormat: "d/m/Y",
            altFormat: "d/m/Y",
            locale: "vn"
        }).setDate(ConvertDateSqlToDateString(value), true, "d/m/Y");
}
function GetDate(id) {
    var datetime = $(id).val();
    var arrDate = datetime.substring(0, 10).split('/');
    return new Date((arrDate[2] + '-' + arrDate[1] + '-' + arrDate[0] + "T00:00:00.000Z")?.trim());
}
function GetDateTime(id) { //new Date('2022-07-13T12:13:00.000Z')
    var datetime = $(id).val();
    var arrDate = datetime.substring(0, 10).split('/');
    var arrTime = datetime.substring(11, 16);
    return new Date((arrDate[2] + '-' + arrDate[1] + '-' + arrDate[0] + "T" + arrTime + ":00.000Z"));
}
function GetTime(id) {
    return $(id).val();
}
function GetDateFlatpickr(id) {
    return GetDate(id);
}
function GetDateTimeFlatpickr(id) { //new Date('2022-07-13T12:13:00.000Z')
    return GetDateTime(id);
}
function GetTimeFlatpickr(id) {
    return GetTime(id);
}

function RenameFileName(response) {
    if (response === "") return "";
    var array = response.split('.');
    var arrName = array[0].split(']')[1].split('-');
    if (arrName.length >= 3) {
        return arrName[0] + "-" + arrName[1] + " ..." + arrName[arrName.length - 1] + "." + array[1];
    }
    if (arrName.length === 2) {
        return arrName[0] + "-" + arrName[1] + "." + array[1];
    }
    if (arrName.length < 2) {
        return arrName[0] + "." + array[1];
    }
    return response;
}
// Check email hợp lệ hay không (Hùng thêm)
function ValidateEmail(email) {
    var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/
    if (!filter.test(email)) {
        SwalNotifyWarning("Địa chỉ Email không hợp lệ!");
        $("#Email").focus();
        return false;
    }
}

function GetHourInDay(hours) {
    if (hours <= 0) return 0;
    else if (hours <= 5.5 && hours > 0) return 0.5;
    else return 1;
}

function RoundHour(minutes) {
    if (minutes <= 30) return 0;
    else if (minutes <= 60 && minutes > 30) return 0.5;
    else return 1;
}

//tính ngày làm việc(chọn cả giờ) - ngày làm việc từ t2-t6
function CalculateWorkingDays(from, to) {
    //ConvertStringToDateTime($('Id').val())
    //dữ liệu truyền vào dạng string dd/MM/yyyy hh:mm
    var dtStart = ConvertStringToDateTime(from);
    var dtStartTemp = ConvertStringToDateTime(from);
    var dtEnd = ConvertStringToDateTime(to);
    var dtEndTemp = ConvertStringToDateTime(to);
    var totalDay = 0;
    var totalHours = 0;
    //*check hợp lệ và trường hợp đặc biệt
    if (isNaN(dtStart.getTime()) || isNaN(dtEnd.getTime())) return -1;
    if (dtEnd < dtStart) return -1;
    if (dtStart.getTime() - dtEnd.getTime() == 0) return -1;
    if (dtStart.getDay() == 6 && dtEnd.getDay() == 0 && (dtStart.getDate() == dtEnd.getDate())) return -1;

    //Trường hợp chọn cùng 1 ngày
    if (((dtEnd - dtStart) / 1000 / 60 / 60) < 24) {
        if (dtStartTemp.getHours() >= 17 || dtEndTemp.getHours() <= 8) return -1;
        else if (dtStartTemp.getHours() <= 8 && dtEndTemp.getHours() >= 17) totalDay += 1;
        else {
            var milisecDiff = dtEnd.getTime() - dtStart.getTime();
            totalHours += (milisecDiff / 1000 / 60 / 60);
            return GetHourInDay(totalHours);
        }
    }
    else {
        //Kiểm tra ngày bắt đầu
        if (dtStart.getDay() !== 0 && dtStart.getDay() !== 6 && (dtStart.getTime() <= new Date(dtStartTemp.setHours(8, 0, 0, 0)).getTime())) {
            totalDay += 1;
        }
        else if (dtStart.getDay() !== 0 && dtStart.getDay() !== 6 && dtStart.getTime() > new Date(dtStartTemp.setHours(8, 0, 0, 0)).getTime() && dtStart.getTime() <= new Date(dtStartTemp.setHours(17, 0, 0, 0)).getTime()) {
            var milisecDiff = new Date(dtStartTemp.setHours(17, 0, 0, 0)).getTime() - dtStart.getTime();
            totalHours += (milisecDiff / 1000 / 60 / 60);
        }
        //Kiểm tra ngày kết thúc
        if (dtEnd.getDay() !== 0 && dtEnd.getDay() !== 6 && dtEnd.getTime() >= new Date(dtEndTemp.setHours(17, 0, 0, 0)).getTime()) {
            totalDay += 1;
        }
        else if (dtEnd.getDay() !== 0 && dtEnd.getDay() !== 6 && dtEnd.getTime() >= new Date(dtEndTemp.setHours(8, 0, 0, 0)).getTime() && dtEnd.getTime() < new Date(dtEndTemp.setHours(17, 0, 0, 0)).getTime()) {
            var milisecDiff = dtEnd.getTime() - new Date(dtEndTemp.setHours(8, 0, 0, 0)).getTime();
            totalHours += (milisecDiff / 1000 / 60 / 60);
        }

        if (totalHours > 0 && totalHours <= 9) {
            totalDay += GetHourInDay(totalHours)
        }
        else if (totalHours > 9) {
            var dayTrunc = Math.trunc(totalHours / 9);
            totalDay = (dayTrunc + GetHourInDay(totalHours - dayTrunc * 9))
        }
        //Truong hợp chọn >= 3 ngày: duyệt từ sau ngày bắt đầu đến trước ngày kết thúc
        if (((dtEnd - dtStart) / 1000 / 60 / 60 / 24) > 1) {
            dtStart.setHours(0, 0, 0, 1);
            dtEnd.setHours(0, 0, 0, 1);
            var dtStartLoop = new Date(dtStart.setDate(dtStart.getDate() + 1))
            var dtEndLoop = new Date(dtEnd.setDate(dtEnd.getDate() - 1))
            while (dtStartLoop <= dtEndLoop) {
                if (dtStartLoop.getDay() !== 0 && dtStartLoop.getDay() !== 6) totalDay++;
                dtStartLoop.setDate(dtStartLoop.getDate() + 1);
            }
        }
    }
    return totalDay;
}

function CalculateHours(from, to) {
    var dtStart = ConvertStringToDateTime(from);
    var timeTemp = ConvertStringToDateTime(from);
    var dtFinish = ConvertStringToDateTime(to);
    if (isNaN(dtStart) || isNaN(dtFinish)) return -1;
    var timeTemp = new Date(timeTemp.setHours(17, 30, 0, 0));
    if (dtFinish <= timeTemp) return -1;
    if (timeTemp > dtStart) {
        dtStart = timeTemp;
    }

    if (((dtFinish - dtStart) / 1000 / 60 / 60 / 24) > 1) return -1;
    var datetimeStart = dtStart.getTime();
    var datetimeFinish = dtFinish.getTime();

    var milisecDiff = datetimeFinish - datetimeStart;
    var minutes = milisecDiff / 1000 / 60;
    var hours = minutes / 60;
    if (minutes === 0) return 0;
    else if (minutes <= 60) return RoundHour(minutes);
    else {
        var hourTrunc = Math.trunc(hours);
        minutes = minutes - hourTrunc * 60;
        return hourTrunc + RoundHour(minutes);
    }
}
////////////////////localStorage/////////////////////////////
function SaveToken(token) {
    window.localStorage.removeItem("access-token");
    window.localStorage.setItem("access-token", token);
}
function GetToken(tokenKey) {
    return window.localStorage.getItem(tokenKey);
}
function SaveRefreshToken(token) {
    window.localStorage.removeItem("refresh-token");
    window.localStorage.setItem("refresh-token", token);
}

function LoadComboboxTreeApiForName(selector, url, isAll, valueSelect, objAjax, textAll, valueAll) {
    var prefixValue = "Value";
    var prefixAvatar = "Avatar";
    var prefixDescription = "Description";
    var prefixName = "Name";
    var prefixDisplayName = "DisplayName";
    var prefixLevel = "Level";

    if (valueAll === undefined) valueAll = "00000000-0000-0000-0000-000000000000";
    var htmlText = "";

    AjaxRequestClientAPI("GET", url, objAjax, function (data) {
        try {
            data = JSON.parse(data);
        } catch {
            data = data
        }

        console.log(data);

        if (isAll) {
            htmlText = "<option selected=\"selected\" data-user-avatar='' data-user-description='' data-level='0' value=\"" + valueAll + "\">" +
                ((textAll === undefined || textAll === "") ? "Tất cả" : textAll) + "</option>";
        }

        for (var i = 0; i < data.length; i++) {
            var selected = (valueSelect !== undefined && valueSelect === data[i][prefixValue]) ? "selected=\"selected\"" : "";
            var displayName = data[i][prefixDisplayName] || data[i][prefixName];
            var level = data[i][prefixLevel] || 0;

            htmlText += "<option " + selected +
                " data-user-avatar='" + (data[i][prefixAvatar] ?? "") + "'" +
                " data-level='" + level + "'" +
                " value='" + data[i][prefixValue] + "'>" +
                displayName + "</option>";
        }

        console.log(htmlText);
        $(selector).html(htmlText);

        var optionFormat = function (item) {
            if (!item.id) {
                return item.text;
            }
            var span = document.createElement('span');
            var avatar = item.element.getAttribute('data-user-avatar');
            var level = parseInt(item.element.getAttribute('data-level')) || 0;
            var template = '';

            // Thêm padding dựa trên level
            var paddingLeft = level * 20;
            span.style.paddingLeft = paddingLeft + 'px';

            if (!(avatar === '' || avatar === null || avatar === undefined || avatar === "null")) {
                template += '<img src="' + avatar + '" class="rounded-circle h-20px w-20px me-2" alt="image"/>';
            }

            // Thêm icon folder cho parent categories
            if (level === 0) {
                template += '<i class="fas fa-folder me-2 text-primary"></i>';
            } else {
                template += '<i class="fas fa-folder-open me-2 text-secondary"></i>';
            }

            template += item.text;
            span.innerHTML = template;
            return $(span);
        };

        var optionResult = function (item) {
            if (!item.id) {
                return item.text;
            }
            var span = document.createElement('span');
            var avatar = item.element.getAttribute('data-user-avatar');
            var description = item.element.getAttribute('data-user-description');
            var level = parseInt(item.element.getAttribute('data-level')) || 0;
            var template = '';

            template += '<div class="d-flex align-items-center" style="padding-left: ' + (level * 15) + 'px;">';

            // Icon dựa trên level
            if (level === 0) {
                template += '<i class="fas fa-folder me-2 text-primary"></i>';
            } else {
                template += '<i class="fas fa-folder-open me-2 text-secondary"></i>';
            }

            if (!(avatar === '' || avatar === null || avatar === undefined || avatar === "null")) {
                template += '<img src="' + avatar + '" class="rounded-circle h-40px w-40px me-3" alt="image"/>';
            }

            template += '<div class="d-flex flex-column">';
            template += '<span style="font-weight: bold;color:#ff907a">' + item.text + '</span>';

            if (!(description === '' || description === null || description === undefined || description === "null")) {
                template += '<span class="text-muted small">' + description + '</span>';
            }

            template += '</div>';
            template += '</div>';

            span.innerHTML = template;
            return $(span);
        };

        $(selector).select2({
            templateSelection: optionFormat,
            templateResult: optionResult,
            placeholder: "Chọn danh mục...",
            allowClear: true
        });

    }, true, "text");
}

// Function helper để load category tree
function LoadCategoryTree(selector, valueSelect, textAll) {
    LoadComboboxTreeApiForName(
        selector,
        "/Api/Category/SelectTree",
        true,
        valueSelect,
        window.objAjax,
        textAll || "Danh mục",
        "00000000-0000-0000-0000-000000000000"
    );
}