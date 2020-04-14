using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Infrastructure.Persistence;

namespace SwaggerAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    public class IncomeTaxApiController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<IncomeTaxApiController> _logger;

        public IncomeTaxApiController(DatabaseContext context, ILogger<IncomeTaxApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/IncomeTax
        [HttpGet]
        public IEnumerable<IncomeTax> Get()
        {
            var entriesList = _context.IncomeTax
                   .OrderBy(t => t.Id)
                   .ToList<IncomeTax>();

            return entriesList;
        }

        // GET: api/IncomeTax/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<IncomeTax> Get(System.Guid id)
        {
            var entity = _context.IncomeTax.Where(x => x.Id == id).ToList();

            return entity;
        }

        // POST: api/IncomeTax
        [HttpPost]
        [Produces("application/json")]
        public IncomeTax Post([FromBody] IncomeTax IncomeTax)
        {
            var entity = new IncomeTax
            {
                Id = IncomeTax.Id,
                IncomeAmount = IncomeTax.IncomeAmount,
                PostalCode = IncomeTax.PostalCode,
                TaxCalculationType = IncomeTax.TaxCalculationType,
                TaxAmount = IncomeTax.TaxAmount,
                CreatedDate = DateTime.UtcNow,
            };

            _context.IncomeTax.Add(entity);
            _context.SaveChanges();

            return IncomeTax;
        }

        // PUT: api/IncomeTax/5
        [HttpPut("{id}")]
        public void Put(System.Guid id, IncomeTax IncomeTax)
        {
            var entity = _context.IncomeTax.Find(id);
           
            entity.IncomeAmount = IncomeTax.IncomeAmount;
            entity.PostalCode = IncomeTax.PostalCode;
            entity.TaxCalculationType = IncomeTax.TaxCalculationType;
            entity.TaxAmount = IncomeTax.TaxAmount;
            entity.CreatedDate = DateTime.UtcNow;

            _context.SaveChanges();
        }

        // DELETE: api/IncomeTax/5
        [HttpDelete("{id}")]
        public void Delete(System.Guid id)
        {
            using (var ctx = _context)
            {
                var entry = ctx.IncomeTax
                    .Where(s => s.Id == id)
                    .FirstOrDefault();

                ctx.Entry(entry).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

    }
}