using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomyWayAPI.Models;
using HomyWayAPI.DTO;

namespace HomyWayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentInfoesController : ControllerBase
    {
        private readonly HomyWayContext _context;

        public PaymentInfoesController(HomyWayContext context)
        {
            _context = context;
        }

        // GET: api/PaymentInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentInfo>>> GetPaymentInfos()
        {
            return await _context.PaymentInfos.ToListAsync();
        }

        // GET: api/PaymentInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentInfo>> GetPaymentInfo(int id)
        {
            var paymentInfo = await _context.PaymentInfos.FindAsync(id);

            if (paymentInfo == null)
            {
                return NotFound();
            }

            return paymentInfo;
        }

        // PUT: api/PaymentInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentInfo(int id, PaymentInfo paymentInfo)
        {
            if (id != paymentInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentInfo>> PostPaymentInfo(PaymentDTO paymentDTO)
        {
            var paymentInfo = new PaymentInfo
            {
                BookingId = paymentDTO.BookingId,
                PaymentMethod = paymentDTO.PaymentMethod,
                PaymentId = paymentDTO.PaymentId,
                CreatedDate = paymentDTO.CreatedDate,
            };
            _context.PaymentInfos.Add(paymentInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentInfo", new { id = paymentInfo.Id }, paymentInfo);
        }

        // DELETE: api/PaymentInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentInfo(int id)
        {
            var paymentInfo = await _context.PaymentInfos.FindAsync(id);
            if (paymentInfo == null)
            {
                return NotFound();
            }

            _context.PaymentInfos.Remove(paymentInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentInfoExists(int id)
        {
            return _context.PaymentInfos.Any(e => e.Id == id);
        }
    }
}
