import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

/** @ represents a decorator. Injectable decorator represents that angular services can be injected into our components
 *  or into other servvices. 'providedIn : root' represents the app.module.ts providers array*/
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http : HttpClient) { 
    //injecting HTTPClient into the constructor.
      }
  
      login(model:any){
        return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
          map((response : User) => {
            const user = response;
            if(user){
              //store the user info in browser local storage, it acccepts key value pair in terms of strings (user is an object)
              localStorage.setItem("user" , JSON.stringify(user)); 
              this.currentUserSource.next(user);  
            }
          })
        );
      }

      register(model:any){
        /*using pipe method which gives us access to rxjs operators so that
        we can transform the observable before the component subscribes to it*/
        return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
          map(user => {
            if(user){
              localStorage.setItem("user", JSON.stringify(user));
              this.currentUserSource.next(user);
            }
          })
        );
      }
      setCurrentUser(user : User){
        this.currentUserSource.next(user);
      }
      logout(){
        localStorage.removeItem('user');
        this.currentUserSource.next(null);  
      }
}
