using KnilServer.Application.DTO.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application.Services.Interfaces
{
    public interface IContactServices
    {

        Task<List<ContactsDTO>> GetAllAsync();
        Task<ContactsDTO> GetByIdAsync(int Id);
        Task<ContactsDTO> CreateAsync(CreateContactDTO contact);
        Task<ContactsDTO> UpdateAsync(UpdateContactDto contact);
        Task DeleteAsync(int id);
      
       
    }
}
