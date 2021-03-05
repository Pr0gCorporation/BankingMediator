using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Internship.Shared.DTOs.Transaction;
using Internship.Shared.Files;
using Internship.TransactionService.API.DTOs.Transaction;
using Internship.TransactionService.Domain.Models;
using Internship.TransactionService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Internship.TransactionService.API.Controllers
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
                return Ok(_mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
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
            try
            {
                // Instance to insert
                var transactionModel = _mapper.Map<TransactionModel>(transaction);
                _logger.LogInformation($"Verb: POST, Desc: Post transaction, param: transaction = {transaction.TransactionId}");

                // Insert to DB
                await _transactionRepository.Add(transactionModel);
                _logger.LogInformation($"Insert to the database the transaction: {transaction.TransactionId}");

                // Instance to publish
                var transactionFile = _mapper.Map<TransactionToFile>(transactionModel);

                // Publish to file endpoint
                await _publisher.Publish(transactionFile);
                _logger.LogInformation($"Publish the transaction: {transaction.TransactionId}");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while posting a transaction {transaction.TransactionId}: {e}");
                return BadRequest("Either incorrect data or an error with the server occured.");
            }
        }
    }
}