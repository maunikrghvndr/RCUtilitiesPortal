$(document).ready(function () {
    if ($("form").attr("action") == "/Jobs/Create") {
        $("#AppID").change(function () {
            $("#JobName").val(this.options[this.selectedIndex].text);
            $("#JobDescription").val(this.options[this.selectedIndex].text);
        });
    }

    if ($(".jobrun").length) {
        setInterval('jobStatusRefresh()', 5000);

        $("#JobCategoryID").change(function () {
            jobsGetByCategory(this.options[this.selectedIndex].value);
        });
    }
});

function jobsGetByCategory(jobCategoryID) {
    var options = {
        url: window.location + "Jobs/Index?jobCategoryID=" + jobCategoryID,
        type: "GET"
    };
    $.ajax(options).done(function (data) {
        if (data.indexOf("Login") > -1) {
            window.location.replace(logoutLocation);
            alert("Your session has expired or you have logged out. Please login again.");
            return;
        }
        $("#target").html(data);
    });
}

function jobStatusRefresh() {
    var options = {
        url: window.location,
        type: "GET"
    };
    $.ajax(options).done(function (data) {
        if (data.indexOf("Login") > -1) {
            window.location.replace(logoutLocation);
            alert("Your session has expired or you have logged out. Please login again.");
            return;
        }
        $("#target").html(data);
    });
}

function checkJobRun(obj) {
    var jobId = $(obj).attr("data-jobid");
    var options = {
        url: window.location + "Jobs/CheckJobRun?jobID=" + jobId,
        type: "GET"
    };
    $.ajax(options).done(function (checkData) {
        if (checkData.indexOf("Login") > -1) {
            window.location.replace(logoutLocation);
            alert("Your session has expired or you have logged out. Please login again.");
            return;
        }
        $("#checkAlert").html(checkData);
        $("#myModal").modal('show');
    });
}

function runJob(jobId) {
    var options = {
        url: window.location + "Jobs/JobAction?jobID=" + jobId + "&jobAction=Run",
        type: "GET"
    };
    $("#dontRunJob").toggle();
    $("#runJob").toggle();
    $.ajax(options).done(function (checkData) {
        if (checkData.indexOf("Login") > -1) {
            window.location.replace(logoutLocation);
            alert("Your session has expired or you have logged out. Please login again.");
            return;
        }
        $("#myModal").modal('hide');
    });
}