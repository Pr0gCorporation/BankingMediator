using System;
using AutoMapper;
using Internship.Shared.Files;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.API.MappingProfiles
{
    public class TransactionDomainToFileMappingProfile : Profile
    {
        public TransactionDomainToFileMappingProfile()
        {
            CreateMap<TransactionModel, TransactionToFile>()
                .ForMember(dest => dest.Debtor, 
                    opt => 
                        opt.MapFrom(src => new AccountToFile
                        {
                            FirstName = src.DebtorFirstName,
                            LastName = src.DebtorLastName,
                            AccountNumber = src.DebtorAccountNumber,
                            BankId = src.DebtorBankId,
                        }))
                .ForMember(dest => dest.Creditor, 
                    opt => 
                        opt.MapFrom(src => new AccountToFile
                        {
                            FirstName = src.CreditorFirstName,
                            LastName = src.CreditorLastName,
                            AccountNumber = src.CreditorAccountNumber,
                            BankId = src.CreditorBankId,
                        }))
                .ForMember(dest => dest.Date,
                    opt => 
                        opt.MapFrom(src => DateTime.Now));
            
            CreateMap<TransactionToFile, TransactionModel>();
        }
    }
}
