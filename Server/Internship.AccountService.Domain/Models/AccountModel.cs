using System;
using System.Collections.Generic;
using System.Text;

namespace Internship.AccountService.Domain.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IBAN { get; set; }
        public string Email { get; set; }
    }
}
