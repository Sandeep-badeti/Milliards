import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from './login/users';
import { AuthenticationService } from './login/authentication.service';
import { tap } from 'rxjs/operators';
@Injectable()
export class AppInterceptorService implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization/accessToken header
        const currentUser: User = <any>this.authenticationService.currentUserValue;

        if (currentUser && currentUser.token) {
            request = request.clone({
                setHeaders: {
                    'access-token': `${currentUser.token}`
                }
            });
        }
        return next.handle(request).pipe(tap((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {
                // do stuff with response if you want
            }
        },
            (err: any) => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status === 401) {
                        this.authenticationService.clearStorage();
                    }
                }
            }
        ));
    }
}
