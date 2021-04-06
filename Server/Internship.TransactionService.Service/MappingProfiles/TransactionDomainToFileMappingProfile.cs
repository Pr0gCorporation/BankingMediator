using System;
using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.Service.MappingProfiles
{
    public class TransactionDomainToFileMappingProfile : Profile
    {
        public TransactionDomainToFileMappingProfile()
        {
            CreateMap<TransactionModel, TransactionToFileDto>()
                .ForMember(dest => dest.Date,
                    opt => 
                        opt.MapFrom(src => DateTime.Now));
            
            CreateMap<TransactionToFileDto, TransactionModel>();
        }
    }
}
