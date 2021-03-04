using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.API.Mapping
{
    public class TransactionDomainToReadProfile : Profile
    {
        public TransactionDomainToReadProfile()
        {
            CreateMap<TransactionModel, TransactionReadDto>();
            CreateMap<TransactionReadDto, TransactionModel>();
        }
    }
}