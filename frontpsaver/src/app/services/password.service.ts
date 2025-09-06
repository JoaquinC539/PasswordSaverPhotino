import { Injectable } from '@angular/core';
import { PasswordDTO, UpdatePayload } from '../interfaces/data';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {
  isElectron:boolean=!!(window && window.electronAPI);
  constructor() { }
  public getMasterCount(){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("get-master-count","")
  }
  public setMasterPassword(password:string){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("setMaster",password)
  }
  public login(password:string){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("login",password);
  }
  public getPasswords(){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("getPasswords")
  }
  public addPassword(password:PasswordDTO){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("addPassword",password);
  }
  public getPassword(id:number){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("getPassword",id);
  }
  public updatePassword(paylaod:UpdatePayload){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("updatePassword",paylaod);
  }
  public deletePassword(id:number){
    // if(!this.isElectron) throw new Error("Not correct platform");
    // return window.electronAPI.invoke("deletePassword",id);
  }
}
