 
import { Injectable } from '@angular/core';
 import { User } from './user.model';
 import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
 @Injectable({
   providedIn: 'root'
 })
 export class UserService {
 constructor(private http: HttpClient) {
 }
   readonly APIUrl = "https://localhost:44347/Api";
 
 formData: User = new User();
    
 Login(val:any): Observable<any> { 
  return this.http.post<any>(this.APIUrl + '/Login/Login', val);
}
getListUser(SearchText:string,StartDate:string,EndDate:string,Gendel:string,CurrentPage:number):Observable<any>{
  return this.http.get<any>(this.APIUrl+'/User/GetListUser?SearchText='+SearchText+'&StartDate='+StartDate+'&EndDate='+EndDate+'&Gendel='+Gendel+'&CurrentPage='+CurrentPage);
   }
 getUser(ID:string):Observable<{ObjData:User}>{
  return this.http.get<any>(this.APIUrl+'/User/GetUser?ID='+ID);
   }
 DeleteUser(ID:string):Observable<any>
{
    return this.http.delete(this.APIUrl+ '/User/DeleteUser?ID='+ID);
}
UpdateUser(val:any): Observable<any>
{
   return this.http.put(this.APIUrl+ '/User/UpdateUser',val);
}
CreateUser(val:any): Observable<any>
{
   return this.http.post(this.APIUrl+ '/User/CreateUser',val);
}
 
}
 
 