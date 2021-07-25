import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
constructor (private accountService: AccountService, private toastr: ToastrService){}
  
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe( //dont even need to subscribe as Auth Guard will do it automatically if you access the observable property
      map(user => {
        if(user) return true;  //returns observable of type boolean. If user is not null, return true
          this.toastr.error('You shall not pass!');
          return false;
      })
    )
  }
  
}
