import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{ //OnInit comes after the constructor
  title = 'The Dating App';
  users : any;

  //below is dependency injection
  constructor(private http: HttpClient, private accountService: AccountService) {} //private variable of http with the type of httpclient which will perform http requests

  ngOnInit() {
 // this.getUsers();
  this.setCurrentUser();
   console.log('setuser from app')

  }

setCurrentUser(){
  const user: User = JSON.parse(localStorage.getItem('user'));
  console.log('user: '+ user);
  this.accountService.setCurrentUser(user);
}




  // getUsers(){
  //   //this.http.get() will return observables. But without subsribe(), nothing will happen as observable is lazy.
  //   this.http.get('https://localhost:5001/api/users').subscribe(
  //     response => { this.users=response; },
  //     error    => { console.log(error);  }
  //   );
  // }


}
