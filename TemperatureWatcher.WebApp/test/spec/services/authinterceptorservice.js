'use strict';

describe('Service: authInterceptorService', function () {

  // load the service's module
  beforeEach(module('temperatureWatcherwebAppApp'));

  // instantiate service
  var authInterceptorService;
  beforeEach(inject(function (_authInterceptorService_) {
    authInterceptorService = _authInterceptorService_;
  }));

  it('should do something', function () {
    expect(!!authInterceptorService).toBe(true);
  });

});
