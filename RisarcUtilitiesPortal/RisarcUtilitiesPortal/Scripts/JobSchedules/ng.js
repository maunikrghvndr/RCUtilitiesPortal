var baseUrl = window.location.href;
var app = angular.module('JobSchedulesApp', ["ngSanitize"]);

app.controller('JobSchedulesController', function ($scope, $http) {
    $scope.jobSchedules = [];

    $scope.xDate = Date.now();

    $scope.narrowSearch = function (item) {
        if ($scope.searchText == undefined || $scope.searchText.length == 0) {
            return true;
        }
        else {
            var ns = document.getElementsByName("rbNarrowSearch");
            var selected;
            for (var i = 0; i < ns.length; i++) {
                if (ns[i].checked) {
                    selected = ns[i];
                    break;
                }
            }

            switch (selected.value) {
                case "JobName":
                    if (item.JobDescription.toLowerCase().indexOf($scope.searchText.toLowerCase()) != -1) {
                        return true;
                    }
                    break;
                case "NextStartDate":
                    if (item.NextStartDateTimeString.indexOf($scope.searchText) != -1) {
                        return true;
                    }
                    break;
                case "Both":
                    if (item.JobDescription.toLowerCase().indexOf($scope.searchText.toLowerCase()) != -1 || item.NextStartDateTimeString.indexOf($scope.searchText) != -1) {
                        return true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    $http({
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        method: "GET",
        url: baseUrl
    }).then(function mySuccess(response) {
        $scope.jobSchedules = response.data;
        }, function myError(response) {
            alert(response);
        });
});

(function ($) {
    $(function () {
        $('input.datetime-picker').datetimepicker({
            timeFormat: 'hh:mm tt'
        });
    });
})(jQuery);