import { BehaviorSubject } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable()
export class AppStore {
  public isBusy$: BehaviorSubject<boolean> = new BehaviorSubject(false);
}
