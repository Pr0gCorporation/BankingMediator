using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Internship.TransactionService.API;
using Internship.TransactionService.API.Controllers;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.API.MappingProfiles;
using Internship.TransactionService.Domain.Models;
using Internship.TransactionService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Internship.TransactionService.Service.Tests
{
    public class TransactionControllerTests
    {
        private static IMapper _mapper;
        
        public TransactionControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TransactionCreateToDomainMappingProfile());
                mc.AddProfile(new TransactionDomainToFileMappingProfile());
                mc.AddProfile(new TransactionDomainToReadMappingProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }
        
        [Fact]
        public async Task ShouldReturnListOfTransactions()
        {
            // Arrange
            var mockRepo = new Mock<ITransactionRepository>();
            var mockPublisher = new Mock<IBus>();
            var mockLogger = new Mock<ILogger<Startup>>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestTransactions());
            var controller = new TransactionController(mockRepo.Object,
                _mapper,
                mockPublisher.Object,
                mockLogger.Object);

            // Act
            var transactions = await controller.Get();
            
            // Assert
            if (transactions.Result is OkObjectResult transactionsObjectResult)
            {
                var transactionsReadDto = transactionsObjectResult.Value as IEnumerable<TransactionReadDto>;
                Assert.Equal((transactionsReadDto ?? throw new NullReferenceException()).Count(), (await GetTestTransactions()).Count());
            }
        }

        private async Task<IEnumerable<TransactionModel>> GetTestTransactions()
        {
            var transactionReadDtos = new List<TransactionModel>
            {
                new TransactionModel()
                {
                    Amount = 900,
                    DebtorFirstName = "John",
                    DebtorLastName = "Frilance",
                    DebtorAccountNumber = "54364",
                    DebtorBankId = "4537",
                    CreditorFirstName = "Lol",
                    CreditorLastName = "Kek",
                    CreditorAccountNumber = "90432",
                    CreditorBankId = "3526",
                    TransactionId = new Guid()
                },
                new TransactionModel()
                {
                    Amount = 4376,
                    DebtorFirstName = "Fgf",
                    DebtorLastName = "HFJJ",
                    DebtorAccountNumber = "6547",
                    DebtorBankId = "7666",
                    CreditorFirstName = "Lol",
                    CreditorLastName = "Kek",
                    CreditorAccountNumber = "75685",
                    CreditorBankId = "7658",
                    TransactionId = new Guid()
                }
            };

            return transactionReadDtos;
        }
    }
}
