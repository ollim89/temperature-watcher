'use strict';

/**
 * @ngdoc function
 * @name temperatureWatcherwebAppApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the temperatureWatcherwebAppApp
 */
angular.module('temperatureWatcherwebAppApp')
  .controller('MainCtrl', function ($scope, $http, AppSettings, $interval) {
    updateCurrentTemperature();
    getExecutingState(false);
    
    var interval;
    if(!angular.isDefined(interval)) {
        interval = $interval(function() {
            getExecutingState(true);
        }, 10 * 1000);
    }
    
    $scope.isActive = true;
    $scope.hour = "0";
    $scope.minute = "0";
    $scope.currentTemperature = "0.0";
    $scope.isExecuting = false;

    $scope.save = function() {
        $http.post(AppSettings.apiUrl + "/SetSchedule/", {
           Hour: $scope.hour,
           Minute: $scope.minute,
           ActiveState: $scope.isActive
        }).success(function() {
            $scope.message = "Tiden är uppdaterad";
            $("#messageDialog").modal();
        })
        .error(printError);
    };

    $scope.controlExecutable = function(sendStartSignal, minutesToKeepRunning) {
        $http.post(AppSettings.apiUrl + "/ControlExecutable/", {
            SendOnFlags: sendStartSignal,
            MinutesToKeepRunning: minutesToKeepRunning
        }).success(function(){
            getExecutingState(true);
        })
        .error(printError);
    };
    
    $scope.$watchGroup(["hour","minute"], function(newValues, oldValues, scope) {
       for(var i = 0; i < newValues.length; i++) {
           if(intval(newValues[i].toString()) < 10) {
               newValues[i] = '0' + newValues[i];
           }
       } 
    });
    
    function printError(data, status) {
        $scope.message = "Ett fel uppstod, felmeddelande: " + data;
        $("#messageDialog").modal();
    };
    
    function updateCurrentTemperature() {
        $http.get(AppSettings.apiUrl + "/GetCurrentTemperature/")
            .success(function(data) {
                $scope.currentTemperature = data.temperature;
            });
    };
    
    function getExecutingState(onlyUpdateExecution) {
        $http.get(AppSettings.apiUrl + "/GetExecutingState/")
            .success(function(data) {
                $scope.isExecuting = data.isExecuting;
                if(!onlyUpdateExecution) {
                    $scope.hour = data.hour;
                    $scope.minute = data.minute;
                    $scope.isActive = data.isActive;
                }
            });
    };
  });
