using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Internship.FileService.Domain.Models;
using Internship.TransactionService.API.DTOs.Account;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Application.Repository.TransactionRepository;
using Internship.TransactionService.Domain.Models;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;

namespace Internship.TransactionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IBus _publisher;

        public TransactionController(
            ITransactionRepository transactionTransactionRepository, IMapper mapper, IBus publisher)
        {
            _transactionRepository = transactionTransactionRepository;
            _mapper = mapper;
            _publisher = publisher;
        }
        
        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionReadDto>>> Get()
        {
            var transactions = await _transactionRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TransactionCreateDto transaction)
        {
            // Instance to insert
            var transactionModel = _mapper.Map<TransactionModel>(transaction);

            // Insert to DB
            await _transactionRepository.Add(transactionModel);
            
            // Instances to publish
            var transactionsFullModel = (await _transactionRepository.GetByTransactionId(transaction.TransactionId));
            
            var transactionRead = _mapper.Map<TransactionReadDto>(transactionsFullModel);
            var transactionFile = _mapper.Map<Transaction>(transactionRead);
            
            // Publish to file endpoint
            await _publisher.Publish(transactionFile);
            
            return Ok();
        }
    }
}