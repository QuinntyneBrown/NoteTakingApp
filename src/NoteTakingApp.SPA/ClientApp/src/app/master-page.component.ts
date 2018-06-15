import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './core/auth.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.css'],
  selector: 'app-master-page'
})
export class MasterPageComponent {
  constructor(private _router: Router, private _authService: AuthService) { }

  public tryToLogout() {
    this._authService
      .logout()
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => this._router.navigateByUrl("/login"))
  }

  ngOnDestroy() { this.onDestroy.next(); }

  public onDestroy: Subject<void> = new Subject();
}
