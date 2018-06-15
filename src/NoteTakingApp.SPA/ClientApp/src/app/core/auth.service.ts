import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map, tap } from 'rxjs/operators';
import { accessTokenKey, baseUrl, currentUserNameKey } from './constants';
import { HubClient } from './hub-client';
import { LocalStorageService } from './local-storage.service';
import { Logger } from './logger.service';

@Injectable()
export class AuthService {
  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _httpClient: HttpClient,
    private _hubClient: HubClient,
    private _localStorageService: LocalStorageService,
    private _loggerService: Logger
  ) {}

  public logout() {
    this._loggerService.trace('AuthService', 'tryToLogout');

    return this._httpClient
      .get(`${this._baseUrl}api/users/signout/${this._localStorageService.get({ name: currentUserNameKey })}`)
      .pipe(tap(() => {
        this._hubClient.disconnect();
        this._localStorageService.put({ name: accessTokenKey, value: null });
        this._localStorageService.put({ name: currentUserNameKey, value: null });
      }));
  }

  public tryToLogin(options: { username: string; password: string }) {
    this._loggerService.trace('AuthService', 'tryToLogin');

    return this._httpClient.post<any>(`${this._baseUrl}api/users/token`, options).pipe(
      map(response => {
        this._localStorageService.put({ name: accessTokenKey, value: response.accessToken });
        this._localStorageService.put({ name: currentUserNameKey, value: options.username });
        return response.accessToken;
      })
    );
  }
}
