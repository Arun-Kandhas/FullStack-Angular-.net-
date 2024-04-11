using AutoMapper;
using KnilServer.Application.DTO.Contacts;
using KnilServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateContactDTO, Contacts>().ReverseMap();
            CreateMap<ContactsDTO, Contacts>().ReverseMap();
            CreateMap<UpdateContactDto, Contacts>().ReverseMap();
        }
    }
}
