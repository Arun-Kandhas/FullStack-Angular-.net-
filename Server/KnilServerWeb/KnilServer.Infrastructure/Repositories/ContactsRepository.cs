using KnilServer.Domain.Contracts;
using KnilServer.Domain.Models;
using KnilServer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Infrastructure.Repositories
{
    public class ContactsRepository : GenericRepository<Contacts>, IContactsRepository
    {
        
        public ContactsRepository(ApplicationDbContext applicationDbContext):base(applicationDbContext)
        {
          
        }
    }
}
