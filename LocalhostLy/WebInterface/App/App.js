var theApp = angular.module('LocalhostLy', ['ui.router']);

theApp.run(
    ['$rootScope', '$state', '$stateParams',
        function ($rootScope, $state, $stateParams) {

        }
    ]
)

// ---- ROUTING ----
theApp.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.when("", "/");

    $stateProvider
        .state("root", {
            url: "/",
            templateUrl: '/App/index.html',
            controller: 'IndexController'
        })
        .state("links", {
            url: "/links",
            templateUrl: '/App/links.html',
            controller: 'LinksController'
        });
})

theApp.controller('IndexController', function ($scope, $http) {
    $scope.newUrl = '';
    $scope.newLink = null;
    $scope.error = null;

    $scope.add = function () {
        $scope.error = null;
        $http.post('/api/links', {}, { params: { OriginalLink: $scope.newUrl } }).success(function (res) {
            if (res.Result.HasErrors) {
                $scope.newLink = null;
                $scope.error = res.Result.AllErrors;
                return;
            }

            $scope.newLink = res.Object;            
        });
    };
});

theApp.controller('LinksController', function ($scope, $http) {

});