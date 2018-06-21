import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HubClient } from './hub-client';
import { Observable } from 'rxjs';
import { LoginRedirectService } from './redirect.service';
import { AuthService } from './auth.service';

@Injectable()
export class HubClientGuard implements CanActivate {
  constructor(
    private _authService: AuthService,
    private _hubClient: HubClient,
    private _loginRedirectService: LoginRedirectService) { }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise(resolve =>
      this._hubClient.connect().then(() => {
        resolve(true);
      }, () => {
        this._authService.logout().subscribe(() => {
          this._loginRedirectService.lastPath = state.url;
          this._loginRedirectService.redirectToLogin();
        });
      })
    );
  }
}
