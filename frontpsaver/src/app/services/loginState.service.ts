import { effect, Injectable, signal, WritableSignal } from "@angular/core";
import { Router } from "@angular/router";
import { PasswordService } from "./password.service";

@Injectable({
    providedIn: "root"
})
export class LoginStateService {
    private isLogged: WritableSignal<boolean> = signal<boolean>(false);
    public errorMessage: WritableSignal<string> = signal<string>("");
    private readonly maxTime = 1000 * 60 * 15;
    private readonly maxIdleTime = 1000 * 60 * 2;
    private isToCloseStandard: WritableSignal<boolean> = signal<boolean>(false);
    private standardTimeOut: any = null;
    private isToCloseTimeOut: any = null;
    private awayTimeOut: any = null;
    public platform = signal<string>("");
    constructor(private router: Router, private passwordService: PasswordService) {
        effect(() => {
            if (this.IsLogged) {
                this.startSession();                
            }
        })
    }
    public login(): void {
        this.isLogged.set(true);
    }

    public logout(): void {
        this.passwordService.logout()
            .then((res: any) => {
                window.removeEventListener("mousemove",()=>{});
                window.removeEventListener("keydown", ()=>{});
                window.removeEventListener("click", ()=>{});
                window.removeEventListener("scroll", ()=>{});
                window.removeEventListener("touchstart", ()=>{});
                clearTimeout(this.isToCloseTimeOut);
                clearTimeout(this.standardTimeOut);
                clearTimeout(this.awayTimeOut);
                this.isLogged.set(false);
                this.router.navigate(["/login"])
            })
            .catch(() => {
                console.error("Error in logout");
                this.errorMessage.set("Error in logout");
            })

    }

    public get IsLogged(): boolean {
        return this.isLogged();
    }

    public get IsToCloseStandard():boolean{
        return this.isToCloseStandard();
    }
    private startSession():void {
        this.startStandardTimeout();
        this.registerActivityListenersAndAwayTimeout();
    }
    private startStandardTimeout():void {
        this.isToCloseTimeOut = setTimeout(() => {
            this.isToCloseStandard.set(true);
        }, this.maxTime - (1000 * 60 * 1))
        this.standardTimeOut = setTimeout(() => {
            this.isToCloseStandard.set(false);
            this.logout();
        }, this.maxTime)
    }
    private startAwayTimeOut=():void=>{
        this.awayTimeOut=setTimeout(()=>{
            this.logout();
        },this.maxIdleTime);
    }
    private registerActivityListenersAndAwayTimeout():void{
        this.startAwayTimeOut();
        window.addEventListener("mousemove", this.resetAwayTimeOut);
        window.addEventListener("keydown", this.resetAwayTimeOut);
        window.addEventListener("click", this.resetAwayTimeOut);
        window.addEventListener("scroll", this.resetAwayTimeOut);
        window.addEventListener("touchstart", this.resetAwayTimeOut);
        

    }
    private resetAwayTimeOut=():void=>{
        // console.log("Activity restarted");
        clearTimeout(this.awayTimeOut);
        this.startAwayTimeOut();
    }

}