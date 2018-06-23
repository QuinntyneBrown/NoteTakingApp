import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { accessTokenKey } from '../core/constants';
import { LocalStorageService } from '../core/local-storage.service';
import { LoginRedirectService } from './redirect.service';
import { HubClient } from './hub-client';

@Injectable()
export class UnauthorizedResponseInterceptor implements HttpInterceptor {
  constructor(
    private _hubClient: HubClient,
    private _localStorageService: LocalStorageService,
    private _loginRedirectService: LoginRedirectService
  ) {}
  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      tap(
        (httpEvent: HttpEvent<any>) => {
          return httpEvent;
        },
        error => {
          if ((error instanceof HttpErrorResponse && error.status === 401) || error.status == 0) {
            this._hubClient.disconnect();
            this._localStorageService.put({ name: accessTokenKey, value: null });
            this._loginRedirectService.redirectToLogin();
          }
        }
      )
    );
  }
}
