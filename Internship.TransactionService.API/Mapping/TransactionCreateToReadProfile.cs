using System;
using AutoMapper;
using Internship.FileService.Domain.Models;
using Internship.TransactionService.API.DTOs.Transaction;

namespace Internship.TransactionService.API.Mapping
{
    public class TransactionCreateToReadProfile : Profile
    {
        public TransactionCreateToReadProfile()
        {
            CreateMap<TransactionCreateDto, TransactionReadDto>();
        }
    }
}