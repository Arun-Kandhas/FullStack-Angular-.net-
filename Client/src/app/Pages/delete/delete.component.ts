import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ContactService } from '../../Services/contact.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-delete',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterModule],
  templateUrl: './delete.component.html',
  styleUrl: './delete.component.css'
})
export default class DeleteComponent implements OnInit {

  formBuilder = inject(FormBuilder);
  contactService = inject(ContactService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  deleteForm!: FormGroup;
  Id: string = '';


  ngOnInit(): void {
    this.Id = this.route.snapshot.params['id'];

    this.deleteForm = this.formBuilder.group({
      id: ['', Validators.required],
      firstName: ['', Validators.required,],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      country: ['', Validators.required],
      postalCode: ['', Validators.required],
    });

    this.getContactById();
  }


  getContactById() {
    this.contactService.getContactById(this.Id).subscribe({
      next: (res) => {
        // console.log('Res', res);
        if (res.isSuccess) {
          this.deleteForm.setValue(res.result);
        }
      },
      error: (err) => {
        console.log('Error', err);
      },
    });
  }

  deleteContacts(){
    this.contactService.deleteContact(this.Id).subscribe({
      next: (res) => {        
        if (res.isSuccess) {
          console.log('Res', res);
          alert("Contact Deleted Successfully!!")
          this.router.navigate(['home'])
        }
      },
      error: (err) => {
        console.log('Error', err);
      },
    });

  }
}
