import { Injectable, NgZone, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@aspnet/signalr';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, baseUrl } from './constants';
import { filter } from 'rxjs/operators';
import { Logger } from './logger.service';
import { LoginRedirectService } from './redirect.service';

@Injectable()
export class HubClient {
  private _connection: HubConnection;
  private _connect: Promise<any>;

  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _logger: Logger,
    private _loginRedirectService: LoginRedirectService,
    private _storage: LocalStorageService,
    private _ngZone: NgZone
  ) {}
  
  public messages$: Subject<any> = new Subject();

  public connect(): Promise<any> {    
    if (this._connect) return this._connect;

    this._connect = new Promise((resolve, reject) => {
      var options: IHttpConnectionOptions = {
        logger: this._logger,
        accessTokenFactory: () => this._storage.get({
          name: accessTokenKey
        }),
        logMessageContent: true
      };

      this._connection =
        this._connection || new HubConnectionBuilder()
          .withUrl(`${this._baseUrl}hub`, options)
          .build();

      this._connection.on('message', value =>
        this._ngZone.run(() => this.messages$.next(value)));

      this._connection.start().then(() => resolve(), () =>
        reject());

      this._connection.onclose((error) => {
        this._storage.put({ name: accessTokenKey, value: null });
        this._loginRedirectService.redirectToLogin()
      });
    });

    return this._connect;
  }

  public disconnect() {
    if (this._connection) {
      this._connection.stop();
      this._connect = null;
      this._connection = null;
    }
  }
}
