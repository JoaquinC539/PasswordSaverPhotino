import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { PasswordService } from '../services/password.service';
import { RouterLink } from '@angular/router';
import { Password, PasswordShow } from '../interfaces/data';
import dayjs from "dayjs";
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';

@Component({
  selector: 'app-password-manager',
  imports: [RouterLink,ScreenLoaderComponent],
  templateUrl: './password-manager.component.html',
  styleUrl: './password-manager.component.scss'
})
export class PasswordManagerComponent implements OnInit {
  loading=signal<boolean>(false);
  errorMessage:WritableSignal<string>=signal<string>("");
  passwords:WritableSignal<PasswordShow[]>=signal<PasswordShow[]>([])
  passwordToDelete=signal<number>(0);


  constructor(private passwordService:PasswordService){}
  async getPasswords(){
    this.loading.set(true)
    const passwords = await this.passwordService.getPasswords()

    
    
    if(!passwords){
      this.errorMessage.set("Error in getting passwords, restart the app")
       return;
    }
    const dataWithShow=(passwords as Password[]).map((password)=>({...password,show:false}))
      this.passwords.set(dataWithShow);
      this.loading.set(false);
      
  }
  ngOnInit(): void {
      window.resizeTo(1450,600)
      this.getPasswords();
  }
  dateParse(date:string|Date){
    return dayjs(date).format("DD/MM/YYYY")
  }
  confirmDelete(id:number){
    this.passwordToDelete.set(id);
  }
  deletePassword(){
    const id=this.passwordToDelete();
    this.loading.set(true)
    this.passwordService.deletePassword(id)
    // .then((res)=>{
    //   if(res.error) this.errorMessage.set("Error deleting password");
    //   else this.getPasswords();

    // })
  }

}
