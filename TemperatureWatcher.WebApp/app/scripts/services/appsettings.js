'use strict';

/**
 * @ngdoc service
 * @name temperatureWatcherwebAppApp.AppSettings
 * @description
 * # AppSettings
 * Factory in the temperatureWatcherwebAppApp.
 */
angular.module('temperatureWatcherwebAppApp')
  .factory('AppSettings', function () {
    return {
        apiUrl: "http://localhost:8080/api",
        tokenUrl: "http://localhost:8080/token"
    };
  });
