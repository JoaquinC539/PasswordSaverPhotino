import { Injectable } from '@angular/core';
import { MessageService } from './message.service';
import { Message } from '../classes/Message';

@Injectable({
  providedIn: 'root'
})
export class HelloService {
  
  constructor(private messageService:MessageService) { }

  public greet(){
    const mes =new Message("Hello from view")
      return  this.messageService.send("greet",mes)
  }


 
}
