using AutoMapper;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.API.Mapping
{
    public class TransactionCreateToDomainProfile : Profile
    {
        public TransactionCreateToDomainProfile()
        {
            CreateMap<TransactionCreateDto, TransactionModel>()
                .ForMember(dest => dest.Debtor,
                    opt => 
                        opt.MapFrom(src => new AccountModel()
                        {
                            Id = src.DebtorId
                        }))
                .ForMember(dest => dest.Creditor,
                    opt => 
                        opt.MapFrom(src => new AccountModel()
                        {
                            Id = src.CreditorId
                        }));
            
            CreateMap<TransactionModel, TransactionCreateDto>();
        }
    }
}