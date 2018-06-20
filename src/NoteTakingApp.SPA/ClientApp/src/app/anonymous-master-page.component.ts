import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AppStore } from './app-store';

@Component({
  templateUrl: './anonymous-master-page.component.html',
  styleUrls: ['./anonymous-master-page.component.css'],
  selector: 'app-anonymous-master-page'
})
export class AnonymousMasterPageComponent {
  constructor(public appStore: AppStore) {

  }
}
