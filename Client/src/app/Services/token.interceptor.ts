import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const Service = inject(AuthService);
  const toaster = inject(ToastrService);
  const router = inject(Router);
  const Token = Service.getToken();

  if (Token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${Token}`,
      },
    });
  }

  return next(req).pipe(
    catchError((err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401 && Token) {
          // alert('Token is Expired, Please Login Again!!!');
          //console.log("Check");
          console.log('Token is Expired, Please Login Again!!!')
          localStorage.clear();
          Service.isLoggedIn$.next(false)
          
          router.navigate(['login'])
        }
      }
      return throwError(()=> new Error("Some Other Error Occured"))
    })
  );
};
