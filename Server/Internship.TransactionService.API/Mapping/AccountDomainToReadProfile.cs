using AutoMapper;
using Internship.TransactionService.API.DTOs.Account;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.API.Mapping
{
    public class AccountDomainToReadProfile : Profile
    {
        public AccountDomainToReadProfile()
        {
            CreateMap<AccountModel, AccountReadDto>();
            CreateMap<AccountReadDto, AccountModel>();
        }
    }
}