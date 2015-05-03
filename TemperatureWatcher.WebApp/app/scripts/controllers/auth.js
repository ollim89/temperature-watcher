'use strict';

/**
 * @ngdoc function
 * @name temperatureWatcherwebAppApp.controller:AuthCtrl
 * @description
 * # AuthCtrl
 * Controller of the temperatureWatcherwebAppApp
 */
angular.module('temperatureWatcherwebAppApp')
  .controller('AuthCtrl', function ($scope, AuthService, $location, $route) {
      //Get user from auth service
      AuthService.getUser().then(function(user) {
        $scope.username = user.username;
        $scope.password1 = user.password; 
      });
      
      //Remove user details from cookie and refresh page
      $scope.cleanCache = function() {
        AuthService.removeCookie();
        $route.reload();
      };
      
      $scope.goBack = function() {
          $location.path('/');
      };
      
      //Save user details
      $scope.save = function(){
          //If passwords match
          if($scope.password1 != $scope.password2) {
              $scope.message = "Lösenorden överensstämmer inte";
              $("#messageDialog").modal();
              return;
          }
          
          //Test credentials and save if successfull
          AuthService.updateUser($scope.username, $scope.password1).then(function(user){
              $scope.message = "Uppgifterna lagrade";
              $("#messageDialog").modal();
          }, function(reason) {
              var errorMsg = reason != undefined && reason.error_description != undefined ? reason.error_description : "Anropet misslyckades";
              $scope.message = "Kunde inte lagra uppgifter, orsak: " + errorMsg;
              $("#messageDialog").modal();
          });
      };
  });
