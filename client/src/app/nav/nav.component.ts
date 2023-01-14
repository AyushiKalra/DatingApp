import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model : any = {}; // create empty object which will contain info what the user enters in username and password fields.
//currentUser$ : Observable<User | null> = of(null); //we need to initialize the observable, using rxjs operator 'of'.
//Inject the services to be used in the component in the argument of the constructor
  constructor(public accountService : AccountService , private router : Router , 
    private toastr : ToastrService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$; 
  }
  //temporary method : getCurrentUser //better is to use async pipe
  /*it is advisable to unsubscribe to an observable when it is no longer in use. Unless it is an HTTP request once they complete
  we are no longer subscribed to that observable.*/
  /*getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      next : user => this.loggedIn = !!user,  //!! turns our user object into boolean. 
      error : error => console.log(error)    })}*/

//create a login method which will be called on click of submit button
//since we are returning observable we need to subscribe to this
login() {
  this.accountService.login(this.model).subscribe({
    next: () => this.router.navigateByUrl('/members'),
    error : error => this.toastr.error(error.error)  
  })
}
logout(){
  this.accountService.logout();//this is going to remove the item from local storage
  this.router.navigateByUrl('/');
}
}
