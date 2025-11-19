import { Injectable } from '@angular/core';
import { Request } from '../classes/Request';
import { IResponse, PendingEntry } from '../classes/IResponse';
import { HttpClient } from '@angular/common/http';
import { apiUrl } from './apiUrl';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private requestId = 0;
  private pending = new Map<number, PendingEntry>();

  constructor(private http: HttpClient) {


  }
  public send<T = any>(type: string, payload?: any): Promise<T> {
    const id = ++this.requestId;
    const req = new Request(id, type, payload);
    const timer = type ==="dbLocation" ? 1000*420 :1000* 45;
    const timeoutId = setTimeout(() => {
      const entry = this.pending.get(id);
      if(entry){
        this.pending.delete(id);
        entry.reject("Timeout rejection");
      }else{
        throw new Error("Timeout exception");
      }
      
      
    }, timer);
    return new Promise<T>((resolve, reject) => {
      const entry: PendingEntry = { resolve, reject, timeoutId };
      this.pending.set(id, entry);
      this.http.post(apiUrl.passwordsUrl, req).subscribe({
        next: (response) => this.handleResponse(response, id),
        error: (err) => {
          console.log("Error",err.error.Error);
          const stringError=err.error.Error
          const entry = this.pending.get(id)!;
          this.pending.delete(id)
          clearTimeout(entry.timeoutId);
          entry.reject(stringError);
          
          

          throw new Error("An error ocurred at rquest", err)
        }
      })

    })
  }
  private handleResponse(response: IResponse, trackedId: number): void {
    console.log("Response from server", response);
    const id = response.Id ?? response.id!
    try {
      if(response.Success ?? response.success ){
        if(this.pending.has(id)){
          const entry = this.pending.get(id);
          this.pending.delete(id);
          clearTimeout(entry?.timeoutId)
          entry?.resolve(response.Payload);
          return;
        }else{
          const entry = this.pending.get(trackedId);
          clearTimeout(entry?.timeoutId)
          this.pending.delete(trackedId);
          entry?.reject("Id not tracked");
          return;
        }
      }else{
        if(this.pending.has(id)){
          const entry = this.pending.get(id);
          this.pending.delete(id);
          clearTimeout(entry?.timeoutId)
          entry?.reject(response.Error ?? response.error);
          return;
        }else{
          const entry = this.pending.get(trackedId);
          clearTimeout(entry?.timeoutId)
          this.pending.delete(trackedId);
          entry?.reject("Id not tracked and error response: "+(response.Error ?? response.error));
          return;
        }
      }
    } catch (error) {
      const entry = this.pending.get(trackedId);
      clearTimeout(entry?.timeoutId)
      this.pending.delete(trackedId);
      throw new Error("An error ocurred at handling response: " + error);
    }

  }

}