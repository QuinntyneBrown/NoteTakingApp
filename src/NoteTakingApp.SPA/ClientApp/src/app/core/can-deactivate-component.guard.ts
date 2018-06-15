import { Injectable } from "@angular/core";
import { CanDeactivate } from "@angular/router";
import { Observable } from "rxjs";
import { IDeactivatable } from "./deactivatable";
import { LocalStorageService } from "./local-storage.service";
import { accessTokenKey } from "./constants";

@Injectable()
export class CanDeactivateComponentGuard implements CanDeactivate<IDeactivatable> {
  constructor(private _localStorageService: LocalStorageService) {
  }

  canDeactivate(
    component: IDeactivatable
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (this._localStorageService.get({ name: accessTokenKey }) == null) return true;

    return component.canDeactivate ? component.canDeactivate() : true;
  }
}
