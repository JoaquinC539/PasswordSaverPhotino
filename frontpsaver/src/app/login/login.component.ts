import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PasswordService } from '../services/password.service';
import { Router, RouterLink } from '@angular/router';
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';
import { LoginStateService } from '../services/loginState.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule,ScreenLoaderComponent,RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  errorMessage=signal<string>("")
  loading=signal<boolean>(false);
  loginForm=new FormGroup({
    password:new FormControl<string>("",[Validators.required]),    
  })
  dbForm=new FormGroup({
    dbFile:new FormControl<string>("")
  })
  constructor(private passwordService:PasswordService,private router:Router,private loginState:LoginStateService){}

  async onSubmit(event:SubmitEvent){
    event.preventDefault();
    this.loading.set(true)
    this.errorMessage.set("");
    if(this.loginForm.invalid){
      this.errorMessage.set("Password is required");
    }
    if(this.loginForm.value.password!==undefined && this.loginForm.value.password!==null ){
      try {
        const res= await this.passwordService.login(this.loginForm.value.password);
        this.loading.set(false);
        if(!res){
          this.errorMessage.set("Incorrect password");
          return;
        }
        this.loginState.login();
        this.router.navigate(["/manager"])
      } catch (error) {
        this.errorMessage.set("An error looking for the password happened, reset the program");
        return;
      }
    }
  }
  async selectDB(event:SubmitEvent):Promise<void>{
    event.preventDefault();
    const filePath=this.dbForm.value.dbFile;
    if(filePath){
      const res = await this.passwordService.changeDB(filePath);
      console.log(res);
    }else{
      this.errorMessage.set("DB path cannot be empty");
    }
  }

}
