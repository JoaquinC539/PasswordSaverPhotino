import { Component, OnInit, signal } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Password, PasswordDTO, UpdatePayload } from '../interfaces/data';
import { PasswordService } from '../services/password.service';
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';

@Component({
  selector: 'app-edit-password',
  imports: [RouterLink,ReactiveFormsModule,ScreenLoaderComponent],
  templateUrl: './edit-password.component.html',
  styleUrl: './edit-password.component.scss'
})
export class EditPasswordComponent  implements OnInit{
  passwordId=signal<number>(0);
  loading=signal<boolean>(false);
  errorMessage=signal<string>("");
  passworldField=signal<string>("password");  
  editPasswordForm=new FormGroup({
    name:new FormControl( "",[Validators.required]),
    username:new FormControl( "",[Validators.required]),
    password:new FormControl( "",[Validators.required]),
    notes:new FormControl(""),
  })

  constructor(private passwordService:PasswordService,private router:Router,private route:ActivatedRoute){}

  async ngOnInit(): Promise<void> {
    this.loading.set(true)
    const id=this.route.snapshot.paramMap.get("id")
    this.passwordId.set(Number(id))
    if(id){
      try {
        const password =await this.passwordService.getPassword(Number(id)) as Password;      
        this.editPasswordForm.patchValue({
          name:password.Name,
          username:password.Username,
          password:password.PasswordValue,
          notes:password.Notes
        })
      } catch (error) {
        this.errorMessage.set("Error getting password: "+ error)
      }      
      this.loading.set(false);
      
    }
    
   
  }

  togglePassword(){
    this.passworldField.update((val:string)=>val==="password"?"text":"password")
  }
  async onSubmit(event:SubmitEvent){
    event.preventDefault();
    this.errorMessage.set("");
    const formValue=this.editPasswordForm.value;
    if(this.editPasswordForm.invalid){
      this.errorMessage.set("Form invalid only allowed letters and numbers in name or first three fields empty")
      this.loading.set(false);
      return;
    }
    const password:PasswordDTO={
      name: formValue.name || "",
      username: formValue.username||'',
      passwordValue:  formValue.password||'',
      notes:formValue.notes ?? ""
    }
    const payload:UpdatePayload={
      id: this.passwordId(),
      ...password
    }
    this.editPasswordForm.reset();

    try {
      const updated = await this.passwordService.updatePassword(payload);
      if(updated){
        this.router.navigate(["/manager"])
      }else{
        this.errorMessage.set("Error adding password reboot the app");
      }
    } catch (error) {
      this.errorMessage.set("Error updating the password: "+error);
    }
    
    
    // .then((res)=>{
    //   if(res.error){
    //     this.errorMessage.set("Error adding password reboot the app");
    //   }else{
    //     this.router.navigate(["/manager"])
    //   }

    // })
    // .catch((err)=>this.errorMessage.set("Error updating the password: "+err))
    
  }
}
