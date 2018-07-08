import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarRef } from '@angular/material';
import { ErrorListComponent } from '../shared/error-list.component';

@Injectable()
export class ErrorService {
  constructor(private _snackBar: MatSnackBar) { }

  public errors: Error[] = [];

  public handle(httpErrorResponse: HttpErrorResponse): MatSnackBarRef<ErrorListComponent> {
    const ref = this._snackBar.openFromComponent(ErrorListComponent, { duration: 0 });
    this.errors = [httpErrorResponse, ...this.errors];
    ref.instance.errors$.next(this.errors);
    return ref;
  }  
}
