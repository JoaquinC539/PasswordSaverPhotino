import { Component, effect, OnInit } from '@angular/core';
import {  Router, RouterOutlet } from '@angular/router';
import { HelloService } from './services/hello.service';
import { PasswordService } from './services/password.service';
import { LoginStateService } from './services/loginState.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'frontpsaver';
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
  
  handleLogout(){
    this.loginState.logout()
  }

  async callGreet(){
    const res = await this.helloService.greet();
    console.log(res);
  } 
  async ngOnInit(): Promise<void> {  
    // this.getStatus();
    // this.makePost();
    // this.callGreet()
    const count = await this.passwordService.getMasterCount()
    console.log("Count en appcomponent",);
    if(!count  && !isNaN(count)){
      this.router.navigate(["/new"])
    }else{
      this.router.navigate(["/login"])
    }
  }
}
