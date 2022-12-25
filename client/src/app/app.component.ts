import { HttpClient } from '@angular/common/http';
import { Component,OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users : any; //turning off Typescript safety here.

  //implementing deoendency injection here to connect to our dotnet http request
  constructor (private http : HttpClient){

  }
  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next : response => this.users = response, //the list of users returned in response.
      error : error => console.log(error) ,
      complete : () => console.log("Request has been completed")
  });
  }
/*appcomponent goes through various lifecycle stages before it displays the content inside our browser. When our component is 
instantiated, constructor also gets created. Constructor is considered too early to go and fetch data from API. so we are going to
implement a lifecycle event inside the component.*/

/*Constructs a GET request that interprets the body as an ArrayBuffer and returns the response in an ArrayBuffer.

@param url — The endpoint URL.

@param options — The HTTP options to send with the request.

@return — An Observable of the response, with the response body as an ArrayBuffer.
observable is a stream of data  which is coming back from our dotnet application.
subscribe the observable as it will force our request to go and get the data.*/

}
