import { Message } from "./Message";
import { IRequest } from "./IRequest";

export class Request implements IRequest{
    id:number;
    type:string;
    payload?:any

    constructor(id:number,type:string, payload?:any){
        this.id=id;
        this.type=type;
        this.payload=payload ;
    }

    
}