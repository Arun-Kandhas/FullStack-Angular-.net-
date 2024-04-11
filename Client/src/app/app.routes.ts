import { Routes } from '@angular/router';
import { authGuard } from './Guards/auth.guard';

export const routes: Routes = [
    {path:"", redirectTo:'login',pathMatch:'full'},
    {path:"login",loadComponent:()=>import('./Pages/login/login.component')},
    {path:"Register",loadComponent:()=>import('./Pages/register/register.component')},
    {path:"home",loadComponent:()=>import('./Pages/home/home.component'),canActivate:[authGuard]},
    {path:"create",loadComponent:()=>import('./Pages/create/create.component'),canActivate:[authGuard]},
    {path:"edit/:id",loadComponent:()=>import('./Pages/edit/edit.component'),canActivate:[authGuard]},
    {path:"delete/:id",loadComponent:()=>import('./Pages/delete/delete.component'),canActivate:[authGuard]},
];
