'use strict';

describe('Service: AuthService', function () {

  // load the service's module
  beforeEach(module('temperatureWatcherwebAppApp'));

  // instantiate service
  var AuthService;
  beforeEach(inject(function (_AuthService_) {
    AuthService = _AuthService_;
  }));

  it('should do something', function () {
    expect(!!AuthService).toBe(true);
  });

});
