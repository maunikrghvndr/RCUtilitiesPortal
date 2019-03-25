var app = angular.module('JobsApp', ["ngSanitize"]);

app.controller('JobsController', function ($scope, $http) {

    $scope.jobs = [];

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
                    if (item.JobName.toLowerCase().indexOf($scope.searchText.toLowerCase()) != -1) {
                        return true;
                    }
                    break;
                case "LastStatusTime":
                    if (item.LastStatusTime.toLowerCase().indexOf($scope.searchText.toLowerCase()) != -1) {
                        return true;
                    }
                    break;
                case "Both":
                    if (item.JobName.toLowerCase().indexOf($scope.searchText.toLowerCase()) != -1 || item.LastStatusTime.toLowerCase().indexOf($scope.searchText.toLowerCase()) != -1) {
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
        url: "/Jobs"
        }).then(function mySuccess(response) {
            $scope.jobs = response.data;
        }, function myError(response) {
            alert(response);
        });
});