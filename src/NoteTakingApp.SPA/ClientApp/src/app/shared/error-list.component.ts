import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { MatSnackBarConfig } from "@angular/material";

@Component({
  templateUrl: "./error-list.component.html",
  styleUrls: ["./error-list.component.css"],
  selector: "app-error-list"
})
export class ErrorListComponent {   
  public errors$: BehaviorSubject<any> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  public displayedColumns: string[] = ['message'];
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
