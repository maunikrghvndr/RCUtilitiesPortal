var currentUrl = window.location.href;

$(document).ready(function () {
    if ($('#consoleAppSpecific').length > 0) {
        $('#consoleAppSpecific').hide();
        if ($('#IsConsoleApp').is(':checked')) {
            $('#classLibSpecific').hide();
            $('#consoleAppSpecific').show();
        }
        else {
            $('#classLibSpecific').show();
            $('#consoleAppSpecific').hide();
        }
    }

    $("#msg").hide();

    $("#appForm").submit(ValidateAndPost);
});

var ajaxFormSubmit = function () {
    var options = {
        url: currentUrl,
        type: "POST",
        data: $("#appForm").serialize()
    };

    $.ajax(options).done(function (data) {
        if (data.indexOf("Login") > -1) {
            window.location.replace(logoutLocation);
            alert("Your session has expired or you have logged out. Please login again.");
            return;
        }
        $("#msg").show(1000);
        $("#create").prop("disabled", true);
        if (currentUrl.toUpperCase().indexOf("CREATE") > -1)
            $("#newJob").html(data);
        else {
            window.location = data;
        }
    }).fail(function (data) {
        alert(JSON.stringify(data));
    });
};

function ValidateAndPost() {
    var valid = true;
    if ($("#IsConsoleApp").prop("checked")) {
        var tb = $("#consoleAppSpecific :input[type=text]");
        $.each(tb, function myfunction() {
            if ($(this).val().length == 0) {
                var lbl = $('label[for="' + $(this).attr('name') + '"]').html();
                if (typeof (lbl) === "undefined")
                    lbl = "This";
                $(this).nextAll('.field-validation-error:first').html(lbl + " Field is required")
                valid = false;
            }
            else {
                $(this).nextAll('.field-validation-error:first').html("");
            }
        });
    }
    else {
        var tb = $("#classLibSpecific :input[type=text]");
        $.each(tb, function myfunction() {
            if ($(this).val().length == 0) {
                var lbl = $('label[for="' + $(this).attr('name') + '"]').html();
                if (typeof (lbl) === "undefined")
                    lbl = "This";
                $(this).nextAll('.field-validation-error:first').html(lbl + " Field is required")
                valid = false;
            }
            else {
                $(this).nextAll('.field-validation-error:first').html("");
            }
        });
    }

    if (valid) {
        var options = {
            url: currentUrl.replace("Create", "AppNameExists").replace("Edit", "AppNameExists"),
            type: "POST",
            data: $("#appForm").serialize()
        };

        $.ajax(options).done(function (AppNameExists) {
            if (AppNameExists.indexOf("Login") > -1) {
                window.location.replace(logoutLocation);
                alert("Your session has expired or you have logged out. Please login again.");
                return;
            }
            if (AppNameExists == "True") {
                valid = false;
                alert("The App Name you are using is already taken. Use another name.");
            }
            else {
                ajaxFormSubmit();
            }
        }).fail(function (data) {
            alert(JSON.stringify(data));
        });
    }
    return false;
}
function ToggleForm(checked) {
    if (checked) {
        $('#consoleAppSpecific').show();
        $('#classLibSpecific').hide();
    }
    else {
        $('#consoleAppSpecific').hide();
        $('#classLibSpecific').show();
    }
}