import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {

  authService = inject(AuthService);
  router = inject(Router);
  isLoggedIn:boolean= true


  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe(res=>{
      this.isLoggedIn = this.authService.isLoggedIn()
    })
  }
 

  Logout(){
    localStorage.removeItem("Token");
    this.authService.isLoggedIn$.next(false)
    this.router.navigate(['login'])
  }

}
