using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.TransactionService.API.DTOs.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Internship.TransactionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        // private readonly IUserRepo _repository;
        // private readonly IMapper _mapper;
        //
        // public TransactionController(IUserRepo repository, IMapper mapper)
        // {
        //     this._repository = repository;
        //     this._mapper = mapper;
        // }
        
        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionReadDto>>> Get()
        {
            return Ok(new List<TransactionReadDto>() // Hard code for now
            {
                new TransactionReadDto()
                {
                    Amount = 290,
                    DebtorId = 2940,
                    CreditorId = 4364,
                    TransactionId = new Guid()
                },
                new TransactionReadDto()
                {
                    Amount = 5344,
                    DebtorId = 4643,
                    CreditorId = 2345,
                    TransactionId = new Guid()
                },
                new TransactionReadDto()
                {
                    Amount = 13999,
                    DebtorId = 2355,
                    CreditorId = 1121,
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