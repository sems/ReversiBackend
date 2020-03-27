using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ReversiController : Controller
    {
        private ReversiAppContext _context { get; set; }
        public ReversiController(ReversiAppContext Context)
        {
            _context = Context;
        }

        [HttpGet("spel/{id}")]
        public JsonResult ReturnGameState(int id)
        {
            Spel result = _context.Spel.SingleOrDefault(x => x.ID == id);

            foreach (var item in _context.User)
            {
                if (item.Spel == result && !result.Spelers.Contains(item))
                {
                    result.Spelers.Add(item);
                }
            }

            var overwinnendeSpeler = result.OverwegendeKleur();
            if (result.Spelers.Count == 1)
            {
                return Json(new
                {
                    Id = result.ID,
                    AanDeBeurd = result.AandeBeurt,
                    Beurt = result.Beurt,
                    bord = result.Bord,
                    omschrijving = result.Omschrijving,
                    speler1 = result.Spelers[0].UserName,
                    speler1Kleur = result.Spelers[0].Kleur,
                    overwinnendeKleur = overwinnendeSpeler,
                    AmountOfBlack = result.AmountOfBlack,
                    AmountOfWhite = result.AmountOfWhite
                });
            }
            return Json(new
            {
                Id = result.ID,
                AanDeBeurd = result.AandeBeurt,
                Beurt = result.Beurt,
                bord = result.Bord,
                omschrijving = result.Omschrijving,
                speler1 = result.Spelers[0].UserName,
                speler1Kleur = result.Spelers[0].Kleur,
                speler2 = result.Spelers[1].UserName, 
                speler2Kleur = result.Spelers[1].Kleur,
                overwinnendeKleur = overwinnendeSpeler,
                AmountOfBlack = result.AmountOfBlack,
                AmountOfWhite = result.AmountOfWhite
            });
        }

        [HttpPost("spel/{id}/{x}/{y}/{colour}")]
        public void Post([FromRoute]int id, [FromRoute]int x, [FromRoute]int y, [FromRoute] int colour)
        {
            Spel result = _context.Spel.SingleOrDefault(spel => spel.ID == id);
            if (result.AandeBeurt == (Kleur) colour)
            {
                result.DoeZet(y, x);
                _context.SaveChanges();
            }
        }
    }
}
