using KnilServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Domain.Contracts
{
    public interface IContactsRepository: IGenericRepository<Contacts>
    {
    }
}
