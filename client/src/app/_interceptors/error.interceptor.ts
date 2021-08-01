import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor { //error interceptor will intercept before it runs the code within subscribes at the test-errors.component

  constructor(private router: Router, private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> { //intecepts the request that goes out or the response that comes back
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {  //this is for the 400 validation error 
                const modalStateErrors = []; //empty array
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modalStateErrors.flat();  //flattens the multilevel array into an array
              } else { //this is the usual 400 bad request
                this.toastr.error(error.statusText, error.status);
              }
              break;
            case 401:
              this.toastr.error(error.statusText, error.status);
              break;
            case 404:
              console.log('4044');
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state:{error:error.error}}; //get the details the error return from the API
              console.log( navigationExtras);
              this.router.navigateByUrl('/server-error',navigationExtras);
              break;
            default:
              this.toastr.error('something unexpected went wrong');
              console.log(error);
              break;
          }
        }
        return throwError(error);
      }
      )
    )
  }
}
