'use strict';

/**
 * @ngdoc function
 * @name temperatureWatcherwebAppApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the temperatureWatcherwebAppApp
 */
angular.module('temperatureWatcherwebAppApp')
  .controller('MainCtrl', function ($scope, $http, AppSettings) {
    updateCurrentTemperature();
    getExecutingState();
      
    $scope.isActive = true;
    $scope.hour = 6;
    $scope.minute = 0;
    $scope.currentTemperature = "0.0";
    $scope.isExecuting = false;

    $scope.save = function() {
        $http.post(AppSettings.apiUrl + "/SetSchedule/", {
           Hour: $scope.hour,
           Minute: $scope.minute,
           ActiveState: $scope.isActive
        });
    };

    $scope.controlExecutable = function(sendStartSignal, minutesToKeepRunning) {
        $http.post(AppSettings.apiUrl + "/ControlExecutable/", {
            SendOnFlags: sendStartSignal,
            MinutesToKeepRunning: minutesToKeepRunning
        }).success(function(){
            getExecutingState();
        });
    };
    
    function updateCurrentTemperature() {
        $http.get(AppSettings.apiUrl + "/GetCurrentTemperature/")
            .success(function(data) {
                $scope.currentTemperature = data.temperature;
            });
    };
    
    function getExecutingState() {
        $http.get(AppSettings.apiUrl + "/GetExecutingState/")
            .success(function(data) {
                $scope.isExecuting = data.isExecuting;
            });
    };
  });
