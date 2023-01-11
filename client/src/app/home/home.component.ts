import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
registerMode = false;
users : any; //turning off Typescript safety here.
/**
 *
 */
constructor(private http : HttpClient) {
}

ngOnInit() : void{
  this.getUsers();
}

registerToggle(){
  this.registerMode = !this.registerMode;
}
getUsers(){
  this.http.get('https://localhost:5001/api/users').subscribe({
    next : response => this.users = response, //the list of users returned in response.
    error : error => console.log(error) ,
    complete : () => console.log("Request has been completed")
});
}
//the event value is being emitted from the child component (Register)
cancelRegisterMode(event: boolean){
  this.registerMode = event;
}
}
