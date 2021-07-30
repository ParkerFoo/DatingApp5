import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  
  model:any={} //created an empty object called model.
  //loggedIn: boolean=false;


  constructor(public accountService : AccountService, private router:Router, private toastr:ToastrService) { }

  ngOnInit(): void {
    //this.getCurrentUser();   
    console.log('getuser from nav')
  }


  login(){
   // console.log(this.model);
   this.accountService.login(this.model).subscribe(response=>{
    console.log(response);
    console.log('response');
   // this.loggedIn=true;
   this.router.navigateByUrl('/members')
   }
   //,error=>{
  //    console.log(error);
  //    this.toastr.error(error.error);
  //  }
   )
  }


  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/')
    //this.loggedIn=false;
  }


  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe(user=>{
  //    // this.loggedIn=!!user; //!! will turn null into false or if there is value at user, it will be true.
  //   },error=>{
  //     console.log(error);
  //   })
  // }

}
