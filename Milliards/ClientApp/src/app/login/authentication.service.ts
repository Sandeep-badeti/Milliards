import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

import { User } from '../login/users';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {

    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient, private router: Router) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    login(data: User) {
        return this.http.post<any>(environment.base_url + `auth/login`, data)
            .pipe(map(user => {
                // login successful if there's a token in the response
                if (user && user.token) {
                    console.log(user);
                    // store user details and token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    this.currentUserSubject.next(user);
                    //this.router.navigate(['/patron-list']);
                }
                return user;
            }));
    }

    // isLoggedIn() {
    //     if (this.currentUserValue && this.currentUserValue.token) {
    //         return this.http.get(environment.base_url + `auth/isLoggedIn`, { params: { token: this.currentUserValue.token } })
    //             .pipe(map(tokenStatus => {
    //                 if (tokenStatus['success'] === false && (tokenStatus['message'] === 'INVALID_TOKEN')) {
    //                     this.clearStorage();
    //                 }
    //                 return (tokenStatus['success'] && (tokenStatus['message'] === 'VALID_TOKEN') ? true : false);
    //             }));
    //     }
    // }

    logout() {
        return this.http.get(environment.base_url + `auth/logout`)
            .pipe(map(user => {
                this.clearStorage();
            }));
    }

    clearStorage() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
        this.router.navigate(['/login']);
    }
}
