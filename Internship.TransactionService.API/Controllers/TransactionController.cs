using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;
using Internship.TransactionService.API.DTOs.Account;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Application.Repository.TransactionRepository;
using Microsoft.AspNetCore.Mvc;

namespace Internship.TransactionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        
        public TransactionController(
            ITransactionRepository transactionTransactionRepository)
        {
             _transactionRepository = transactionTransactionRepository;
        }
        
        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionReadDto>>> Get()
        {
            return Ok(new List<TransactionReadDto>() // Hard code for now
            {
                new TransactionReadDto()
                {
                    Amount = 290,
                    Debtor = new AccountReadDto()
                    {
                        AccountNumber = "432523",
                        BankId = "4422",
                        FirstName = "John",
                        LastName = "Smith"
                    },
                    Creditor = new AccountReadDto()
                    {
                        AccountNumber = "432444",
                        BankId = "3324",
                        FirstName = "Will",
                        LastName = "Brendon"
                    },
                    TransactionId = new Guid()
                },
                new TransactionReadDto()
                {
                    Amount = 5344,
                    Debtor = new AccountReadDto()
                    {
                        AccountNumber = "432655",
                        BankId = "3342",
                        FirstName = "Deni",
                        LastName = "Washington"
                    },
                    Creditor = new AccountReadDto()
                    {
                        AccountNumber = "782444",
                        BankId = "3334",
                        FirstName = "Will",
                        LastName = "Cargo"
                    },
                    TransactionId = new Guid()
                },
                new TransactionReadDto()
                {
                    Amount = 13999,
                    Debtor = new AccountReadDto()
                    {
                        AccountNumber = "434544",
                        BankId = "8777",
                        FirstName = "John",
                        LastName = "Mankind"
                    },
                    Creditor = new AccountReadDto()
                    {
                        AccountNumber = "343423",
                        BankId = "1265",
                        FirstName = "Brad",
                        LastName = "Smith"
                    },
                    TransactionId = new Guid()
                }
            });
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TransactionCreateDto transaction)
        {
            //
            // TODO: 
            // 1) Insert to DB
            // 2) Publish to the File Service Endpoint
            //
            
            return Ok();
        }
    }
}