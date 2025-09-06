import { Injectable } from '@angular/core';
import { Request } from '../classes/Request';
import { IResponse } from '../classes/IResponse';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private requestId = 0;
  private pending = new Map<number,(msg:any)=>void>();

  constructor() { 
    window.external.receiveMessage((msg:string)=>{
      const parsedRes=JSON.parse(msg) as IResponse;
      if(parsedRes.Id && this.pending.has(parsedRes.Id)){
        const resolver=this.pending.get(parsedRes.Id)!
        resolver(parsedRes.Payload);
        this.pending.delete(parsedRes.Id)
      }
    });    
      
  }
  public send<T=any>(type:string,payload?:any):Promise<T>{
    const id = ++this.requestId;
    const req = new Request(id,type,payload);
    window.external.sendMessage(JSON.stringify(req));
    return new Promise<T>((resolve)=>{
      this.pending.set(id,resolve);
    })
  }
  
}