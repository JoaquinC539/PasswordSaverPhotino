import { Component, OnInit } from '@angular/core';
import {  Router, RouterOutlet } from '@angular/router';
import { HelloService } from './services/hello.service';
import { PasswordService } from './services/password.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet,],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'frontpsaver';
  constructor(private helloService:HelloService,private passwordService:PasswordService,private router:Router){

  }
  
  callGreet(){
    
    this.helloService.greet()
    .then((res)=>{
      console.log("Response got in component: ",res);
    })
    
  }

  async ngOnInit(): Promise<void> {  
    this.callGreet();
   
    const count = await this.passwordService.getMasterCount()
    if(!count){
      this.router.navigate(["/new"])
    }else{
      this.router.navigate(["/login"])
    }
    
  }
}
