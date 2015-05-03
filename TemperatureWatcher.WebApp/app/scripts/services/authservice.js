'use strict';

/**
 * @ngdoc service
 * @name temperatureWatcherwebAppApp.AuthService
 * @description
 * # AuthService
 * Service in the temperatureWatcherwebAppApp.
 */
angular.module('temperatureWatcherwebAppApp')
    .service('AuthService', function (AppSettings, $cookies, $q, $http, $cookieStore) {
        var user;

        var getUser = function() {
            var deferred = $q.defer();
            
            //Get from cookie
            if(user == undefined) {
                var cookie = $cookies['temperatureWatcher'];
                //Cookie exists
                if(cookie != undefined) {
                    try {
                        user = JSON.parse(cookie);
                    }
                    catch(err) {
                        $cookieStore.remove('temperatureWatcher');
                        deferred.reject();
                    }
                }
            }
         
            //Check token expirition
            if(user != undefined) {
                //Token expired
                if(moment() >= moment(user.token.expires)) {
                        refreshToken().then(
                            //Token refreshed successfully
                            function() {
                                deferred.resolve(user); 
                            },
                            //Token could not be refreshed
                            function(reason) {
                                deferred.reject(reason);
                            }
                        );
                }
                //Token not expired, return user
                else {
                    deferred.resolve(user);
                }
            }
            //No cookie
            else {
                deferred.reject(user);
            }
            
            return deferred.promise;
        };

        var updateUser = function(username, password) {
            return refreshToken(username, password);
        };
        
        var removeCookie = function() {
          user = null;
          $cookieStore.remove('temperatureWatcher');
        };
        
        function refreshToken(username, password) {
            var deferred = $q.defer();
            $http.post(AppSettings.tokenUrl, {
                grant_type: 'password',
                username: username,
                password: password
            }, {
                headers: {'Content-Type': 'application/x-www-form-urlencoded'},
                transformRequest: function(obj) {
                    var str = [];
                    for(var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                }
            })
            .success(function(data){
                user = {
                    username: username,
                    password: password,
                    token: {
                        token: data.access_token,
                        expires: moment().add(data.expires_in, 'seconds').format()
                    }
                }
                
                $cookies['temperatureWatcher'] = JSON.stringify(user);

                deferred.resolve(user);
            })
            .error(function(data, status, headers, config){
                //Remove 
                $cookieStore.remove('temperatureWatcher');
                deferred.reject(data);
            });

            return deferred.promise;
        }

        return {
            getUser: getUser,
            updateUser: updateUser,
            removeCookie: removeCookie
        }
  });
