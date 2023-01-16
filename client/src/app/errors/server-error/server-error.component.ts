import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent {
error : any;
  constructor(private router : Router) {
    //the state for server-error is stored in router and is only available in the constructor because beyond that it's gone.
    const navigation = this.router.getCurrentNavigation();
    //optional chaining ?. coz navigation could be undefined
    this.error = navigation?.extras?.state?.['error'];
  }

}
