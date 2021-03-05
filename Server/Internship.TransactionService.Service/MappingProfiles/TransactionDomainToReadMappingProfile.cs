using AutoMapper;
using Internship.Shared.Files;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.API.MappingProfiles
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