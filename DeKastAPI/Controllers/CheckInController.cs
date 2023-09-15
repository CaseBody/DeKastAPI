using DeKastAPI.DataBase;
using DeKastAPI.Entities;
using DeKastAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DeKastAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckInController : ControllerBase
    {
        private readonly DeKastContext _deKastContext;

        public CheckInController(DeKastContext deKastContext)
        {
            _deKastContext = deKastContext;
        }

        [HttpPut]
        public async Task<ActionResult<CheckInResultModel>> Get([FromQuery] int AbonnementId)
        {
            var abonnement = _deKastContext.Abonnementen.FirstOrDefault(x => x.Id == AbonnementId && x.IsActive);

            if (abonnement == null)
                return NotFound(new CheckInResultModel
                {
                    Success = false,
                    Status = "Abonnement is niet gevonden.",
                });

            var weekNumber = ISOWeek.GetWeekOfYear(DateTime.UtcNow);
            var uses = await _deKastContext.AbonnementUses.Where(x => x.AbonnementId == AbonnementId && x.WeekNumber == weekNumber).CountAsync();

            var allowedUses = abonnement.Type switch
            {
                AbonnementType.Weekly => 1,
                AbonnementType.DoubleWeekly => 2,
                AbonnementType.Unlimited => 9999,
                _ => 0
            };

            if (uses + 1 > allowedUses)
            {
                return BadRequest(new CheckInResultModel
                {
                    Success = false,
                    Status = "Weekelijks gebruikslimiet is bereikt.",
                });
            }
            else
            {
                var newUse = new AbonnementUse
                {
                    AbonnementId = AbonnementId,
                    WeekNumber = weekNumber
                };

                _deKastContext.AbonnementUses.Add(newUse);
                await _deKastContext.SaveChangesAsync();

                return Ok(new CheckInResultModel
                {
                    Success = true,
                    Status = "Succesvol ingecheckt"
                });
            }

        }
    }
}