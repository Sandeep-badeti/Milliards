import { TestBed } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { User } from './users';
describe('AuthenticationService', () => {

  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientModule, 
    RouterTestingModule.withRoutes(
        [{path: 'login', component: DummyLogin}]
    )],
    providers: [AuthenticationService],
    declarations: [DummyLogin]
    
  }));

  it('should be created', () => {
    const service: AuthenticationService = TestBed.get(AuthenticationService);
    expect(service).toBeTruthy();
  });

  it('should login', (done) => {
    const service: AuthenticationService = TestBed.get(AuthenticationService);
    service.login({ "userName": 'kiran', "password": "milliards@2020", "id": 0, "firstName": "", "lastName": "", "emailAddress": "", "token": "" })
      .subscribe((resp: User) => {
        console.log(resp.token)
        expect(resp.token).toBeDefined();
        done();
      })
  }, 20000);

  it('should logout', (done) => {
    const service: AuthenticationService = TestBed.get(AuthenticationService);
    if (service.currentUserValue) {
      service.logout().subscribe((resp) => {
        console.log(resp)
        expect(resp['logout']).toBeTruthy();
        done();
      })
    } else {
      expect(true).toBeTruthy();
      done();
    }
  }, 20000);

  it('should clear All values', (done) => {
    const service: AuthenticationService = TestBed.get(AuthenticationService);
    if (service.currentUserValue) {
      service.clearStorage();
      expect(true).toBeTruthy();
      done();
    }
  });

});
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-name',
  template: ``
})
export class DummyLogin {
  constructor() { }
}


