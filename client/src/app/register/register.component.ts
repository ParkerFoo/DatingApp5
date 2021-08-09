import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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

//model:any={};
registerForm: FormGroup
maxDate:Date;
validationErrors: string[]=[];

  constructor(private accountService:AccountService, private toastr:ToastrService, private fb: FormBuilder,private router:Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate=new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
  }

  initializeForm(){
    // this.registerForm=new FormGroup({
    //   username:new FormControl('', Validators.required),
    //   password: new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]),
    //   confirmPassword:new FormControl('',[Validators.required,this.matchValues('password')])
    // })

    this.registerForm = this.fb.group({
      gender:['male'],
      username:  ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
  }

  matchValues(matchTo:string):ValidatorFn{
    return (control: AbstractControl) => { //All FormCOntrol derived from AbstractControl
      console.log(control?.value === control?.parent?.controls[matchTo].value);
      console.log(control?.value === control?.parent?.controls[matchTo].value ? 'same' : 'not same' );
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: false } //if match, then null    
    }

    //if both password and confirm password matches, but if you change password , code below will show invalid
    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })

  }

  register(){
    console.log(this.registerForm.value);
    //console.log(this.model);
    this.accountService.register(this.registerForm.value).subscribe(response=>{
        console.log(response);
       this.router.navigateByUrl('/members');
    },
    error=>{ //this error in the parameter is actually HttpErrorResponse
      console.log(error)
     // this.toastr.error(error.error); //this will be HttpErrorResponse.errors
     this.validationErrors=error;
    }
    );    
  }

  cancel(){
    //console.log("cancelled");
    this.cancelRegister.emit(false); //send or emit false value to home component 
  }

}
