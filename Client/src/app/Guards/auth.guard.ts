import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
    const service = inject(AuthService);
    const router = inject(Router);

  if(service.isLoggedIn()){
    return true;
  }else
  {
    router.navigate(['login'])
    return false
  }
  
};
