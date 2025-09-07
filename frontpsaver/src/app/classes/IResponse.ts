import { IRequest } from "./IRequest";

export interface IResponse {
    id?: number;
    Id?: number;
    type?: string;
    Type?: string;
    payload?: any;
    Payload?: any;
    success?: boolean;
    Success?: boolean;
    error?: string | null;
    Error?: string | null;
}
export  type PendingEntry = {
  resolve: (v: any) => void;
  reject: (err: any) => void;
  timeoutId?: number;
};