using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.WEB.PANEL.ADMIN.ViewModels.ContactViewModels
{
    public class IndexViewModel : ViewModel
    {
        public IEnumerable<ContactResource> Contacts { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
        {
            Contacts = await DbContext.Contacts.Select(x => new ContactResource
            {
                Id = x.Id,
                ResponsibleName = x.ResponsibleName,
                BusinessName = x.BusinessName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToListAsync();
        }
    }
}
