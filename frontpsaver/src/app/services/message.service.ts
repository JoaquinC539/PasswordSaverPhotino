import { Injectable } from '@angular/core';
import { Request } from '../classes/Request';
import { IResponse,PendingEntry } from '../classes/IResponse';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private requestId = 0;
  private pending = new Map<number,PendingEntry>();

  constructor() { 
    window.external.receiveMessage((msg:string)=>{
      let parsedRes;   
          
      try {
        parsedRes=JSON.parse(msg) as IResponse;
        // console.log("Status ress",parsedRes)
      } catch (error) {
        console.error("Failed to parse message from .NET:", msg, error);
        return;
      }      
      
      const id = (parsedRes!.id ?? parsedRes!.Id) as number | undefined;
      if(typeof id === "number" && this.pending.has(id)){
        const entry=this.pending.get(id)!
        const success = parsedRes!.success ?? parsedRes!.Success ?? true;
        if(success){
          entry.resolve(parsedRes.payload ?? parsedRes.Payload );
        }else{
          entry.reject(parsedRes.Error ?? parsedRes!.Error )
        }
      }
    });    
       
  }
  public send<T=any>(type:string,payload?:any):Promise<T>{
    const id = ++this.requestId;
    const req = new Request(id,type,payload);
    window.external.sendMessage(JSON.stringify(req));
    return new Promise<T>((resolve,reject)=>{
      const entry :PendingEntry = {resolve,reject}
      this.pending.set(id,entry);
    })
  }
  
}