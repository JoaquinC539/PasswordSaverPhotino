export interface LoginMaster{
    error:boolean;
    authenticated:boolean;
  }

  export interface Password{
    Id:number;
    Name:string;
    Username:string;
    PasswordValue:string;
    Notes:string;
    CreatedAt:Date;
  }

  export interface PasswordShow extends Password{
   
    show:boolean;
    coppied:boolean;
  }
  export interface PasswordDTO{
    name:string;
    username:string;
    passwordValue:string;
    notes?:string;
  }
export interface UpdatePayload extends PasswordDTO{
  id:number;
  
}