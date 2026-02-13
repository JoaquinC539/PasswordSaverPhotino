import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { PasswordService } from '../services/password.service';
import { RouterLink } from '@angular/router';
import { Password, PasswordShow } from '../interfaces/data';
import dayjs from "dayjs";
import { ScreenLoaderComponent } from '../screen-loader/screen-loader.component';
import { LoginStateService } from '../services/loginState.service';

@Component({
  selector: 'app-password-manager',
  imports: [RouterLink,ScreenLoaderComponent],
  templateUrl: './password-manager.component.html',
  styleUrl: './password-manager.component.scss'
})
export class PasswordManagerComponent implements OnInit {
  loading=signal<boolean>(false);
  errorMessage:WritableSignal<string>=signal<string>("");
  message:WritableSignal<string>=signal<string>("");
  passwords:WritableSignal<PasswordShow[]>=signal<PasswordShow[]>([])
  passwordsArray:PasswordShow[]=[];
  passwordToDelete=signal<number>(0);
  siteNameFilter=signal<string>("");

  private searchTimeout:any;
  constructor(private passwordService:PasswordService, private loginstate:LoginStateService){}
  async getPasswords(){
    this.loading.set(true)
    try {
      const passwords = await this.passwordService.getPasswords()
    if(!passwords){
      this.errorMessage.set("Error in getting passwords, restart the app")
       return;
    }
    const dataWithShow=(passwords as Password[]).map((password)=>({...password,show:false,coppied:false}))
      this.passwords.set(dataWithShow);
      this.passwordsArray=(dataWithShow);
      this.loading.set(false);
    } catch (error) {
      this.errorMessage.set("Error in getting passwords, making logout in 3 seconds")
      setTimeout(()=>{
        this.loginstate.logout();
      },3000)

    }finally{
      this.loading.set(false);
    }
    
  }
  ngOnInit(): void {
      window.resizeTo(1450,600)
      this.getPasswords();
      // this.getPlatform();
  }
  async getPlatform(){
    const res= await this.passwordService.getPlatform();
    console.log("Platform: "+res);
  }
  dateParse(date:string|Date){
    return dayjs(date).format("DD/MM/YYYY")
  };
  confirmDelete(id:number){
    this.passwordToDelete.set(id);
  };
  deletePassword(){
    const id=this.passwordToDelete();
    this.loading.set(true)
    const res=this.passwordService.deletePassword(id);
    this.loading.set(false);
    if(!res){this.errorMessage.set("Error deleting password");}
    else this.getPasswords();
    
  }

  filterPasswordOnSearch(e:any){ 
    const search=e.target.value.trim().toLowerCase();
    clearTimeout(this.searchTimeout);
    this.searchTimeout=setTimeout(()=>{
      const passwordsFiltered=this.passwordsArray
      .filter((p)=>{
        const passwordCleaned=p.Name?.trim()?.toLowerCase();
        return passwordCleaned.includes(search)
      })
      this.passwords.set(passwordsFiltered)
    },150)
    
  }
  copyToClipboard(password:PasswordShow){
    navigator.clipboard.writeText(password.PasswordValue).then(()=>{
      this.passwords.update(passwords=>{
        const passwordFind=passwords.find(p=>p.Id===password.Id)
        if(passwordFind){
          passwordFind.coppied=true;
        }
        setTimeout(()=>{
        if(passwordFind){
          passwordFind.coppied=false;
          }
        },1150)
        return passwords;
      })
      
    });
  }
  async makeDump(){
    this.loading.set(true);
    this.errorMessage.set("");
    try {
      const res = await this.passwordService.makeBackup();
      if(!res){
        this.errorMessage.set("Backup wasn't made");
      }else{
        this.message.set("Backup made sucessfully");
        setTimeout(()=>{
          this.message.set("");
        },5000)
      }
    } catch (error) {
      this.errorMessage.set("An error ocurred: "+error);
    }finally{
      this.loading.set(false);
    }
  }
}
