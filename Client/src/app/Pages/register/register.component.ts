import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export default class RegisterComponent implements OnInit {
  formBuilder = inject(FormBuilder);
  Service = inject(AuthService);
  router = inject(Router);
  registerForm!: FormGroup;

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }


  Register(){
    if(this.registerForm.valid){
        this.Service.registerService(this.registerForm.value)
        .subscribe({
          next:(res)=>{
            // console.log("User",res);            
            alert("User Added")
            this.registerForm.reset();
            this.router.navigate(['login'])
          },
          error:(err)=>{
            console.log("Error",err);
          }
        })
    }
      
      
  }
}
