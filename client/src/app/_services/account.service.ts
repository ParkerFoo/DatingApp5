import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({ //injectable means it can be injected to other services and components
  providedIn: 'root' 
})
export class AccountService { //services is a singleton,only when user close the application it will get destroyed; 
  //component is not, it will get destroyed as soon as they are not in use
  baseUrl=environment.apiUrl;
  private _currentUserSource = new ReplaySubject<User>(1); //ReplaySubject is kind of like a buffer object. it is going to store the value inside here.And any time a subscriber subscribes to this observable, it's going to emit the last value inside it. 1 is the size of our buffer object
 
currentUser$=this._currentUserSource.asObservable(); //returns observable with the type of User

  constructor(private http: HttpClient) { }

  login(model:any){
    return this.http.post(this.baseUrl+'account/login',model).pipe(   //post model to the api endpoint
      map((response: User) => {
        const user=response;
        if (user){//check if we received user or not from the API
           localStorage.setItem('user',JSON.stringify(user));  //create a local storage with the name called user
           this._currentUserSource.next(user);
        }
      })
    )
  }


 register(model:any){
  return this.http.post(this.baseUrl+'account/register',model).pipe(
    map((response:User)=>{
      const user=response;
      if (user){
        localStorage.setItem('user',JSON.stringify(user));
        this._currentUserSource.next(user);
      }
      return user;
    })
  )
}



setCurrentUser(user:User){
  this._currentUserSource.next(user);
    console.log('_currentUserSource: ' + this._currentUserSource);
}

logout(){
  localStorage.removeItem('user');
  this._currentUserSource.next(null);
}




}
