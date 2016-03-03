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

});

theApp.controller('LinksController', function ($scope, $http) {

});