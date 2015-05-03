'use strict';

/**
 * @ngdoc function
 * @name temperatureWatcherwebAppApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the temperatureWatcherwebAppApp
 */
angular.module('temperatureWatcherwebAppApp')
  .controller('MainCtrl', function ($scope, $http, AppSettings, $interval, AuthService, $location, $q) {
    //Controls if authentication should be used when contacting the service
    var authUsed = false;
    var interval;
    
    //Check if auth should be used when contacting service
    configureAuth();
    
    //Initial values
    $scope.isActive = true;
    $scope.currentTemperature = "0.0";
    $scope.isExecuting = false;
    $scope.hour = "00";
    $scope.minute = "00";
    
    $scope.goToAuth = function() {
        $interval.cancel(interval);
        interval = undefined;
        $location.path('/auth');  
    };

    //Save time to leave
    $scope.save = function() {
        getRequestHeaders().then(function(header){
            $http.post(AppSettings.apiUrl + "/SetSchedule/", {
               Hour: $scope.hour,
               Minute: $scope.minute,
               ActiveState: $scope.isActive
            }, { headers: header }).success(function() {
                $scope.message = "Tiden är uppdaterad";
                $("#messageDialog").modal();
            })
            .error(printError);
        });
    };

    //Send requests for instant start/stop
    $scope.controlExecutable = function(sendStartSignal, minutesToKeepRunning) {
        getRequestHeaders().then(function(header){
            $http.post(AppSettings.apiUrl + "/ControlExecutable/", {
                SendOnFlags: sendStartSignal,
                MinutesToKeepRunning: minutesToKeepRunning
            }, { headers: header }).success(function(){
                getExecutingState(true);
            })
            .error(printError);
        });
    };
    
    //Gets the auth settings of the service
    function configureAuth() {
        $http.get(AppSettings.apiUrl + "/AuthUsed/")
            .success(function(result) {
                if(result == true) {        
                    authUsed = true;
                    $("#auth-button-container").show();
            
                    AuthService.getUser().then(function(user) {
                        //Do nothing, every thing is configured properly
                    }, function() {
                       $interval.cancel(interval);
                       interval = undefined;
                       $location.path('/auth');
                       return;
                    });
                }
        
                init();
            })
            .error(printError);
    };
    
    //Initializes values
    function init() {
        //Get status from service
        updateCurrentTemperature();
        getExecutingState(false);

        //Set interval to get status every 10 seconds
        if(!angular.isDefined(interval)) {
            interval = $interval(function() {
                getExecutingState(true);
            }, 10 * 1000);
        }
    };
    
    //Helper to print errors in message dialog
    function printError(data, status) {
        $scope.message = "Ett fel uppstod, felmeddelande: " + data.message;
        $("#messageDialog").modal();
    };
    
    //Gets the current temperature
    function updateCurrentTemperature() {
        getRequestHeaders().then(function(header){
            $http.get(AppSettings.apiUrl + "/GetCurrentTemperature/", { headers: header })
            .success(function(data) {
                $scope.currentTemperature = data.temperature;
            }); 
        });
    };
    
    //Gets execution status
    function getExecutingState(onlyUpdateExecution) {
        getRequestHeaders().then(function(header){
            $http.get(AppSettings.apiUrl + "/GetExecutingState/", { headers: header })
                .success(function(data) {
                    $scope.isExecuting = data.isExecuting;
                    if(!onlyUpdateExecution) {
                        $scope.hour = data.hour;
                        $scope.minute = data.minute;
                        $scope.isActive = data.isActive;
                    }
                });
        });
    };
    
    function getRequestHeaders() {
        var deferred = $q.defer();
        
        if(authUsed) {
            AuthService.getUser().then(function(user) {
                deferred.resolve({
                   Authorization: 'Bearer ' + user.token.token 
                });
            }, function() {
                $location.path('/auth');
                deferred.reject();
            });
        }
        else {
            deferred.resolve({});
        }
        
        return deferred.promise;
    }
  });
