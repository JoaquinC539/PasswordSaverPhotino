import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { PasswordService } from '../services/password.service';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { PasswordDTO } from '../interfaces/data';
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';

@Component({
  selector: 'app-new-password',
  imports: [RouterLink,ReactiveFormsModule,ScreenLoaderComponent],
  templateUrl: './new-password.component.html',
  styleUrl: './new-password.component.scss'
})
export class NewPasswordComponent {
  errorMessage=signal<string>("");
  loading=signal<boolean>(false);
  passworldField=signal<string>("password");
  newPasswordForm=new FormGroup({
    name:new FormControl("",[Validators.required]),
    username:new FormControl("",[Validators.required]),
    password:new FormControl("",[Validators.required]),
    notes:new FormControl(""),
  })

  constructor(private passwordService:PasswordService,private router:Router){}

  togglePassword(){
    this.passworldField.update((val:string)=>val==="password"?"text":"password")
  }
  onSubmit(event:SubmitEvent){
    event.preventDefault();
    this.loading.set(true);
    this.errorMessage.set("");
    const formValue=this.newPasswordForm.value;
    if(this.newPasswordForm.invalid){
      this.errorMessage.set("Form invalid only allowed letters and numbers in name or first three fields empty")
      this.loading.set(false);
      return;
    }
    const password:PasswordDTO={
      name: formValue.name || "",
      username: formValue.username||'',
      password:  formValue.password||'',
      notes:formValue.notes ?? ""
    }
    this.newPasswordForm.reset();
    this.passwordService.addPassword(password)
    // .then((res)=>{
    //   if(res.error){
    //     this.loading.set(false);
    //     this.errorMessage.set("Error adding password reboot the app");
    //   }else{
    //     this.loading.set(false);
    //     this.router.navigate(["/manager"])
    //   }
      
    // })
    
  }
}
