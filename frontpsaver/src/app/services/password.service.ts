import { Injectable } from '@angular/core';
import { PasswordDTO, UpdatePayload } from '../interfaces/data';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {
  
  constructor(private messageService:MessageService) { }
  public getMasterCount(){
   
    return this.messageService.send("count")
    // 
  }
  public setMasterPassword(password:string){
    return this.messageService.send("setMaster",password);
  }
  public login(password:string){    
    return this.messageService.send("login",password)
  }
  public getPasswords(){
    
    return this.messageService.send("getPasswords")
  }
  public addPassword(password:PasswordDTO){
    
    return this.messageService.send("addPassword",password);
  }
  public getPassword(id:number){
    
    return this.messageService.send("getPassword",id);
  }
  public updatePassword(paylaod:UpdatePayload){
    
    return this.messageService.send("updatePassword",paylaod);
  }
  public deletePassword(id:number){
    
    return this.messageService.send("deletePassword",id);
  }
  public logout(){
    return this.messageService.send("logout");
  }
  public changeDB(filePath:string){
    return this.messageService.send("dbLocation",filePath);    
  }
}
