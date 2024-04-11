import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { ContactService } from '../../Services/contact.service';
import { CommonModule } from '@angular/common';
import { ContactsModel } from '../../Models/Contacts';
import { RouterModule } from '@angular/router';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { MatSort,Sort, MatSortModule } from '@angular/material/sort';
import { BrowserModule } from '@angular/platform-browser';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,RouterModule,MatTableModule,MatSortModule,MatPaginator],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export default class HomeComponent implements OnInit {
  displayedColumns: string[] = ['index', 'firstName', 'lastName', 'email', 'phoneNumber','address', 'city', 'state', 'country','postalCode', 'actions'];
  dataSource :any;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  Contacts:ContactsModel[] = [];
  service = inject(ContactService);

  ngOnInit(): void {
    this.GetContactList();
  }

  GetContactList() {
    this.service.getAllContacts().subscribe({
      next: (res) => {
        if (res.isSuccess)
           this.Contacts = res.result;
          this.dataSource = new MatTableDataSource<ContactsModel>(this.Contacts);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        // console.log("Deata",this.Contacts);
      },
      error: (err) => {
        console.log('Error', err);
      },
    });
  }
}
