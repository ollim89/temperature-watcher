'use strict';

/**
 * @ngdoc overview
 * @name temperatureWatcherwebAppApp
 * @description
 * # temperatureWatcherwebAppApp
 *
 * Main module of the application.
 */
angular
  .module('temperatureWatcherwebAppApp', [
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'frapontillo.bootstrap-switch'
  ])
  .config(function ($routeProvider) {
    $routeProvider
      .when('/', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .otherwise({
        redirectTo: '/'
      });
  });
