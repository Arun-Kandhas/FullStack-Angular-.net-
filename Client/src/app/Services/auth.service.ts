import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { apiUrls } from '../api.Urls';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http = inject(HttpClient);
  isLoggedIn$ = new BehaviorSubject<boolean>(false);

  registerService(register:any){
    return this.http.post<any>(`${apiUrls.authService}Register`,register)
  }
  
  loginService(loginObj:any)
  {
    return this.http.post<any>(`${apiUrls.authService}Login`,loginObj)
  }

  isLoggedIn():boolean
  {
    return !!localStorage.getItem("Token")
  }

  getToken():any
  {
    return localStorage.getItem("Token")
  }

}
