using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.API.MappingProfiles
{
    public class TransactionCreateToDomainMappingProfile : Profile
    {
        public TransactionCreateToDomainMappingProfile()
        {
            CreateMap<TransactionCreateDto, TransactionModel>();
            CreateMap<TransactionModel, TransactionCreateDto>();
        }
    }
}