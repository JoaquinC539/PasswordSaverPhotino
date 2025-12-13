import { Component, effect, OnInit, signal } from '@angular/core';
import {  Router, RouterOutlet } from '@angular/router';
import { HelloService } from './services/hello.service';
import { PasswordService } from './services/password.service';
import { LoginStateService } from './services/loginState.service';
import { CommonModule } from '@angular/common';



@Component({
  selector: 'app-root',
  imports: [RouterOutlet,CommonModule ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'frontpsaver';
  platform = signal<string>("");
  constructor(private helloService:HelloService,private passwordService:PasswordService,private router:Router,public loginState:LoginStateService){
   
  }


  async getStatus(){
    const res= await this.helloService.getStatus();
    console.log(res);
  }
  async makePost(){
    const res = await this.helloService.makePost();
    console.log(res)
  }
  async getPlatform(){
    const res= await this.passwordService.getPlatform();
    console.log("Platform: "+res);
    this.platform.set(res);
    this.loginState.platform.set(res);
  }
  
  handleLogout(){
    this.loginState.logout()
  }

  async callGreet(){
    const res = await this.helloService.greet();
    console.log(res);
  } 
  async ngOnInit(): Promise<void> {  
    this.getPlatform();
    // this.getStatus();
    // this.makePost();
    // this.callGreet()
    try {
      const count = await this.passwordService.getMasterCount()
    if(!count  && !isNaN(count)){
      this.router.navigate(["/new"])
    }else{
      this.router.navigate(["/login"])
    }
    } catch (error) {
      console.error("Count had an error: "+error);
    }
    
  }
}
