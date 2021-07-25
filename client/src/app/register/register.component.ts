import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
@Input() usersFromHomeComponent//input decorator
@Output() cancelRegister= new EventEmitter(); 

  model:any={};


  constructor(private accountService:AccountService, private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    //console.log(this.model);
    this.accountService.register(this.model).subscribe(response=>{
        console.log(response);
        this.cancel();
    },
    error=>{ //this error in the parameter is actually HttpErrorResponse
      console.log(error)
      this.toastr.error(error.error); //this will be HttpErrorResponse.errors
    }
    );
    
  }
  cancel(){
    //console.log("cancelled");
    this.cancelRegister.emit(false); //send or emit false value to home component 
  }

}
