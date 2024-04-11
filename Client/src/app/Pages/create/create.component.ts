import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';
import { ContactService } from '../../Services/contact.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './create.component.html',
  styleUrl: './create.component.css'
})
export default class CreateComponent implements OnInit {

  formBuilder = inject(FormBuilder);
  contactService = inject(ContactService);
  toaster = inject(ToastrService);
  router = inject(Router);
  createForm!: FormGroup;


  ngOnInit(): void {
    this.createForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      country: ['', Validators.required],
      postalCode: ['', Validators.required],
    });
  }


  Contacts(){
    if(this.createForm.valid){
      this.contactService.createContacts(this.createForm.value)
      .subscribe({
        next:(res:any)=>{
          //console.log("User",res); 
          if(res.isSuccess){
            // alert("Contact Added Successfully!!")
            this.toaster.success(res.displayMessage, 'Success');
            this.createForm.reset();
            this.router.navigate(['home']);
          }else{
            this.toaster.error("Error While Creating Contacts", 'Error');
          }

        },
        error:(err)=>{
         // console.log("Error",err);
          this.toaster.error(err, 'Error');
        }
      })
  }
    
  }
}
