import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { apiUrls } from '../api.Urls';
import { ContactsModel } from '../Models/Contacts';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  
  http = inject(HttpClient);
 

  getAllContacts(){
    return this.http.get<any>(`${apiUrls.contactService}Contacts`)
  }
  getContactById(Id:any){
    return this.http.get<any>(`${apiUrls.contactService}Contacts/${Id}`)
  }

  createContacts(CreateObj:ContactsModel){
    return this.http.post<ContactsModel>(`${apiUrls.contactService}Contacts`,CreateObj)
  }

  updateContact(Id:any,updateObj:any){
      return this.http.put<any>(`${apiUrls.contactService}Contacts/${Id}`,updateObj)
  }

  deleteContact(Id:any){
    return this.http.delete<any>(`${apiUrls.contactService}Contacts/${Id}`)
}
}
