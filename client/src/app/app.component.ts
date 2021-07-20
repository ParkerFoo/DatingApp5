import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{ //OnInit comes after the constructor
  title = 'The Dating App';
  users : any;

  //below is dependency injection
  constructor(private http: HttpClient) {} //private variable of http with the type of httpclient which will performs http requests

  ngOnInit() {
  this.getUsers();
  }


  getUsers(){
    //this.http.get() will return observables. But without subsribe(), noting will happen as observable is lazy.
    this.http.get('https://localhost:5001/api/users').subscribe(
      response => { this.users=response; },
      error    => { console.log(error);  }
    );
  }


}
