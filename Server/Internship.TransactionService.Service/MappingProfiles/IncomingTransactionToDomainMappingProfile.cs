using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;
using System;

namespace Internship.TransactionService.Service.MappingProfiles
{
    public class IncomingTransactionToDomainMappingProfile : Profile
    {
        public IncomingTransactionToDomainMappingProfile()
        {
            CreateMap<IncomingTransactionDto, TransactionModel>();
            CreateMap<TransactionModel, IncomingTransactionDto>();
        }
    }
}
