import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PasswordService } from '../services/password.service';
import { LoginMaster } from '../interfaces/data';
import { Router } from '@angular/router';
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule,ScreenLoaderComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  errorMessage=signal<string>("")
  loading=signal<boolean>(false);
  loginForm=new FormGroup({
    password:new FormControl<string>("",[Validators.required]),
    
  })
  constructor(private passwordService:PasswordService,private router:Router){}

  onSubmit(event:SubmitEvent){
    event.preventDefault();
    this.loading.set(true)
    this.errorMessage.set("");
    if(this.loginForm.invalid){
      this.errorMessage.set("Password is required");
    }
    if(this.loginForm.value.password!==undefined && this.loginForm.value.password!==null ){
      this.passwordService.login(this.loginForm.value.password)
      // .then((res)=>{
        
      //   if((res as LoginMaster).error){this.errorMessage.set("An error looking for the password happened, reset the program");return;}
      //   if(!(res as LoginMaster).authenticated){this.errorMessage.set("Incorrect password");return;}
      //   this.loading.set(false)
      //   this.router.navigate(["/manager"])
        
      // })
      
    }
    



  }
}
