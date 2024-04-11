import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export default class LoginComponent implements OnInit {
  formBuilder = inject(FormBuilder);
  toaster = inject(ToastrService);
  Service = inject(AuthService);
  router = inject(Router);
  loginForm!: FormGroup;

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  Login() {
    if (this.loginForm.valid) {
      this.Service.loginService(this.loginForm.value).subscribe({
        next: (res) => {
          // alert("Login SuccessFully")
          if (res.isSuccess) {
            this.toaster.success(res.displayMessage, 'Success');
            localStorage.setItem('Token', res.result.token);
            this.Service.isLoggedIn$.next(true);
            this.loginForm.reset();
            this.router.navigate(['home']);
          }else{
            this.toaster.error("Login Failed", 'Error');
          }
        },
        error: (err) => {
          console.log('Error', err);
          this.toaster.error(err, 'Error');
        },
      });
    }
  }
}
