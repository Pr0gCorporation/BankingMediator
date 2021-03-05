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
                .ForMember(dest => dest.Date,
                    opt => 
                        opt.MapFrom(src => DateTime.Now));
            
            CreateMap<TransactionToFile, TransactionModel>();
        }
    }
}
