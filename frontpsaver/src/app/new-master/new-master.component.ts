import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms"
import { PasswordService } from '../services/password.service';
import { Router } from '@angular/router';
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';
import { LoginStateService } from '../services/loginState.service';
@Component({
  selector: 'app-new-master',
  imports: [ReactiveFormsModule,ScreenLoaderComponent],
  templateUrl: './new-master.component.html',
  styleUrl: './new-master.component.scss'
})
export class NewMasterComponent {
  loading=signal<boolean>(false);
  errorMessage:string="";
  successMessage:string="";
  newPasswordForm=new FormGroup({
    newPassword:new FormControl("",[Validators.required,Validators.pattern(/^[a-zA-Z0-9#$%]+$/)]),
    retypedPassWord:new FormControl("",[Validators.required,Validators.pattern(/^[a-zA-Z0-9#$%]+$/)])
  })

  constructor(private passwordService:PasswordService,private router:Router,private loginState:LoginStateService){}

  async onSubmit(event:SubmitEvent):Promise<void>{
    this.loading.set(true)
    this.errorMessage="";
    this.successMessage=""
    event.preventDefault();
    const formValue=this.newPasswordForm.value;
    if(this.newPasswordForm.invalid){
      this.errorMessage="Password(s) are invalid only allowed letters, numbers and #$% characters and must not be empty";
      this.loading.set(false)
      return;
    }
    if(formValue.newPassword!==formValue.retypedPassWord){
      this.errorMessage="Passwords are not the same";
      this.loading.set(false)
      return;
    }
    if(formValue?.newPassword){
      const res= await this.passwordService.setMasterPassword(formValue?.newPassword )
      console.log(res);
      if(res){
          this.successMessage="Master password added sucessfully. \n Keep very safe this password since it is not recoverable and if it is loss the data wont be recoverable. \n This password will be asked everytime the program is opened \n Redirecting to password manager in 10 seconds";          
          this.newPasswordForm.setValue({newPassword:"",retypedPassWord:""});
          setTimeout(()=>{
            this.loginState.login();
            this.router.navigate(["/manager"]);
          },10000)
      }else{
        this.errorMessage="Master password could not be added close the program and retry it. Delete .db file is that doesn't work"
      }
      this.loading.set(false);
      
    }
    
    
  }
}
