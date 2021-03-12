using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Enums;
using Internship.TransactionService.Domain.Enums.EnumExtentions;
using Internship.TransactionService.Domain.Interfaces;
using Internship.TransactionService.Domain.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Internship.TransactionService.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IBus _publisher;
        private readonly ILogger<Startup> _logger;

        public TransactionController(
            ITransactionRepository transactionTransactionRepository, IMapper mapper, IBus publisher, ILogger<Startup> logger)
        {
            _transactionRepository = transactionTransactionRepository;
            _mapper = mapper;
            _publisher = publisher;
            _logger = logger;
        }
        
        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionReadDto>>> Get()
        {
            try
            {
                var transactions = await _transactionRepository.GetAll();
                _logger.LogInformation("Verb: GET, Desc: Get all the transactions from the database");
                var transactionsRead = _mapper.Map<IEnumerable<TransactionReadDto>>(transactions);
                return Ok(transactionsRead);
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting all the transactions from the repo: " + e);
                return NotFound("Either no data found or an error with the server occured.");
            }
        }
        
        // GET: api/Transactions/{id}
        [HttpGet("/id")]
        public async Task<ActionResult<IEnumerable<TransactionReadDto>>> Get(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetById(id);
                _logger.LogInformation($"Verb: GET, Desc: Get transaction by id from the database, param: id = {id}");
                return Ok(_mapper.Map<TransactionReadDto>(transaction));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while getting transaction by id {id} from the repo: {e}");
                return NotFound("Either no data found or an error with the server occured.");
            }
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TransactionCreateDto transaction)
        {
            const TransactionStatus transactionStatus = TransactionStatus.Created;
            try
            {
                // Instance to insert
                var transactionModel = _mapper.Map<TransactionModel>(transaction);
                _logger.LogInformation($"Verb: POST, Desc: Post transaction, param: transaction = {transactionModel.TransactionId}");

                // Insert to DB
                await _transactionRepository.Add(transactionModel);
                _logger.LogInformation($"Insert to the database the transaction: {transactionModel.TransactionId}");

                // Insert to DB (status of the transaction is created)
                await _transactionRepository.UpdateStatus(new TransactionStatusModel()
                {
                    Status = transactionStatus.ToFriendlyString(),
                    Reason = "",
                    Date = DateTime.Now,
                    TransactionId = transactionModel.TransactionId
                });
                _logger.LogInformation($"Insert to the database the transaction status: {transactionModel.TransactionId}, {transactionStatus}");
                
                // Instance to publish
                var transactionFile = _mapper.Map<TransactionToFileDto>(transactionModel);

                // Publish to file endpoint
                await _publisher.Publish(transactionFile);
                _logger.LogInformation($"Publish the transaction: {transactionModel.TransactionId}");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while posting a transaction: {e}");
                return BadRequest("Either incorrect data or an error with the server occured.");
            }
        }
    }
}