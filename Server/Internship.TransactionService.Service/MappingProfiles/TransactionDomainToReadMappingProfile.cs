using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.Service.MappingProfiles
{
    public class TransactionDomainToReadMappingProfile : Profile
    {
        public TransactionDomainToReadMappingProfile()
        {
            CreateMap<TransactionModel, TransactionReadDto>();
            CreateMap<TransactionReadDto, TransactionModel>();
        }
    }
}