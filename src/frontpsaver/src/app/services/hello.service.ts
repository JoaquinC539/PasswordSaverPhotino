import { Injectable } from '@angular/core';
import { MessageService } from './message.service';
import { Message } from '../classes/Message';
import { HttpClient } from '@angular/common/http';
import { apiUrl } from './apiUrl';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HelloService {
  
  constructor(private messageService:MessageService, private http:HttpClient) { }

  public greet(){
    const mes =new Message("Hello from view")
      return  this.messageService.send("greet",mes)
  }

  public getStatus(){
    return firstValueFrom( this.http.get(apiUrl.getStatusUrl));
  }

  public makePost(){
    return firstValueFrom(this.http.post(apiUrl.makePostUrl,{Message:"Hello from Js",Error:false}))
  }

 
}
