import { Component, OnInit } from '@angular/core';
import {  Router, RouterOutlet } from '@angular/router';
import { HelloService } from './services/hello.service';
import { PasswordService } from './services/password.service';
import { MessageService } from './services/message.service';
import { Message } from './classes/Message';
import { Request } from './classes/Request';

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
      console.log(res);
      console.log("Response got in component: ",res);
    })
    
  }

  ngOnInit(): void {  
    // this.callGreet();
    /* this.helloService.greet("Peter")
    .then(val=>{
      console.log(val);
    }) */
    //   this.passwordService.getMasterCount()
    // .then((res)=>{      
    //   if(!res){
    //     this.router.navigate(["/new"])
    //   }else{
    //     this.router.navigate(["/login"])
    //   }
    // })
  }
}
