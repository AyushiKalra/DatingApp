import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable  } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
//injecting router so that we can redirect the app when we encounter an error from the API
constructor(private router : Router , private toastr : ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error : HttpErrorResponse) =>
      {
        if(error){
          switch(error.status){
            case 400:
              if(error.error.errors){
                const modelStateErrors = [];
                for(const key in error.error.errors){
                  if(error.error.errors[key]){
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();//flat() method flattens an array into a single element by concatenating other elements.
                //we want the component to deal with the errors, we are just formatting it in an array
              }
              else{
                this.toastr.error(error.error , error.status.toString()); 
              }
              break;
            case 401:
              this.toastr.error("Unauthorized", error.status.toString());
              break;
            case 404:
              this.router.navigateByUrl("/not-found");
              break;
            case 500:
              const navigationExtras : NavigationExtras = {state : {error : error.error}}
              //our router is capable of receiving states. and we will send our apiexception
              this.router.navigateByUrl("/server-error", navigationExtras);
              break;
            default:
              this.toastr.error("Something unexpected went wrong.");
              console.log(error);
              break;
          }
        }
        throw error;
      })
      
      )
}
}
