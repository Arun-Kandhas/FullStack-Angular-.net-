import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ContactService } from '../../Services/contact.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterModule],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.css',
})
export default class EditComponent implements OnInit {
  formBuilder = inject(FormBuilder);
  contactService = inject(ContactService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  toaster = inject(ToastrService);
  editForm!: FormGroup;
  Id: string = '';

  ngOnInit(): void {
    this.Id = this.route.snapshot.params['id'];

    this.editForm = this.formBuilder.group({
      id: ['', Validators.required],
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

    this.getContactById();
  }

  getContactById() {
    this.contactService.getContactById(this.Id).subscribe({
      next: (res) => {
        // console.log('Res', res);
        if (res.isSuccess) {
          this.editForm.setValue(res.result);
        }
      },
      error: (err) => {
        console.log('Error', err);
      },
    });
  }

  UpdateContacts(){
    this.contactService.updateContact(this.Id,this.editForm.value)
    .subscribe({
      next: (res) => {
        // console.log('Res', res);
        if (res.isSuccess) {
          // alert("Contact Updated Successfully!!")
          this.toaster.success("Contact Updated Successfully", 'Success');
          this.router.navigate(['home'])
          
        }else{
          this.toaster.error("Error While Updating Contacts", 'Error');
        }
      },
      error: (err) => {
        console.log('Error', err);
        this.toaster.error(err, 'Error');
      },
    });
  }

}
