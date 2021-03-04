using System;
using AutoMapper;
using Internship.FileService.Domain.Models;
using Internship.TransactionService.API.DTOs.Transaction;

namespace Internship.TransactionService.API.Mapping
{
    public class TransactionReadToFileProfile : Profile
    {
        public TransactionReadToFileProfile()
        {
            CreateMap<TransactionReadDto, Transaction>()
                .ForMember(dest => dest.Date,
                    opt =>
                        opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Debtor,
                    opt =>
                        opt.MapFrom(src => $"{src.Debtor.FirstName} {src.Debtor.LastName}"))
                .ForMember(dest => dest.Creditor,
                    opt =>
                        opt.MapFrom(src => $"{src.Creditor.FirstName} {src.Creditor.LastName}"));
        }
    }
}