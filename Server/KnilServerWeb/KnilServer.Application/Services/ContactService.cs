using AutoMapper;
using KnilServer.Application.DTO.Contacts;
using KnilServer.Application.Services.Interfaces;
using KnilServer.Domain.Contracts;
using KnilServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application.Services
{
    public class ContactService : IContactServices
    {
        private readonly IContactsRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactsRepository contactRepository,IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }


        public async Task<ContactsDTO> CreateAsync(CreateContactDTO contact)
        {
            var Entity = _mapper.Map<Contacts>(contact);
            var result = _contactRepository.IsRecordExistsAsync(x => x.PhoneNumber.ToLower().Trim() == contact.PhoneNumber.ToLower().Trim() || x.FirstName.ToLower().Trim() == contact.FirstName.ToLower().Trim());


            if (result)
            {
                return new ContactsDTO();
            }


            var CreatedEntity = await _contactRepository.CreateAsync(Entity);
            var EntityDto = _mapper.Map<ContactsDTO>(CreatedEntity);
            return EntityDto;

        }

        public async Task DeleteAsync(int id)
        {   
            var Entity = await _contactRepository.GetByIdAsync(x =>x.Id == id);
            await _contactRepository.DeleteAsync(Entity);
        }

        public async Task<List<ContactsDTO>> GetAllAsync()
        {
            
            var Contacts = await _contactRepository.GetAllAsync();
            var Entity = _mapper.Map<List<ContactsDTO>>(Contacts);
            var result = Entity.OrderByDescending(x=> x.Id).ToList();    
            return result;
        }

        public async Task<ContactsDTO> GetByIdAsync(int Id)
        {
            var Contact = await _contactRepository.GetByIdAsync(x=>x.Id == Id);
            var Entity = _mapper.Map<ContactsDTO>(Contact);
            return Entity;
        }
      

        public async Task<ContactsDTO> UpdateAsync(UpdateContactDto contactDTo)
        {
            var contact = _mapper.Map<Contacts>(contactDTo);
            var Entity = await _contactRepository.UpdateAsync(contact);
            var EntityDto = _mapper.Map<ContactsDTO>(Entity);
            return EntityDto;
        }
    }
}
