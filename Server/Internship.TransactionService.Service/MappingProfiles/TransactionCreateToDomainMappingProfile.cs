using System;
using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.Service.MappingProfiles
{
    public class TransactionCreateToDomainMappingProfile : Profile
    {
        public TransactionCreateToDomainMappingProfile()
        {
            CreateMap<TransactionCreateDto, TransactionModel>()
                .ForMember(dest => dest.TransactionId, 
                    opt => 
                        opt.MapFrom(src =>  Guid.NewGuid()));
            CreateMap<TransactionModel, TransactionCreateDto>();
        }
    }
}