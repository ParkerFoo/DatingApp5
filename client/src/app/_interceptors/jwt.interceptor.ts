import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser:User;

    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);//take(1)  will guarantee you that it will unsub after subsribe
    if (currentUser){ //check if now this variable have user or null
     request=request.clone({
      //  setHeaders:{
      //    Authorization:`Bearer ${currentUser.token}`
      //  }
       headers: request.headers.set("Authorization", `Bearer ${currentUser.token}`)
     })
    
    }
   // console.log('intercepted at jwt');
    return next.handle(request);
  }
}
