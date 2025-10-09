import { Injectable, signal, WritableSignal } from "@angular/core";
import { Router } from "@angular/router";
import { PasswordService } from "./password.service";

@Injectable({
    providedIn:"root"
})
export class LoginStateService{
    

    private isLogged:WritableSignal<boolean>=signal<boolean>(false);
    public errorMessage:WritableSignal<string>=signal<string>("");

    constructor(private router:Router,private passwordService:PasswordService){}

    public login():void{
        this.isLogged.set(true);
    }

    public logout():void{
        this.passwordService.logout()
        .then((res:any)=>{
            console.log(res)
            this.isLogged.set(false);
            this.router.navigate(["/login"])
        })
        .catch(()=>{
            console.error("Error in logout");
            this.errorMessage.set("Error in logout");
        })        
        
    }

    public get IsLogged():boolean{
        return this.isLogged();
    }

}