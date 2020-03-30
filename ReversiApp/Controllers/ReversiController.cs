using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReversiApp.Areas.Identity.Data;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    public class JsonResultModel
    {
        public int x { get; set; }
        public int y { get; set; }
        public int colour { get; set; }
    }

    [Route("api/")]
    [ApiController]
    public class ReversiController : Controller
    {
        private ReversiAppContext _context { get; set; }
        private UserManager<User> _userManager { get; set; }

        public ReversiController(ReversiAppContext Context, UserManager<User> userManager)
        {
            _context = Context;
            _userManager = userManager;
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

        [HttpPost("spel/{id}/")]
        public async Task<HttpResponseMessage> DoeZet([FromRoute]int id, [FromBody] JsonResultModel jsonResult)
        {
            Spel result = _context.Spel.SingleOrDefault(spel => spel.ID == id);
            
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);

            foreach (var item in _context.User)
            {
                if (item.Spel == result && !result.Spelers.Contains(item))
                {
                    result.Spelers.Add(item);
                }
            }

            if (result.Spelers.Contains(user) && user.Kleur == ((Kleur)jsonResult.colour))
            {
                if (result.AandeBeurt == ((Kleur)jsonResult.colour))
                {
                    if (result.DoeZet(jsonResult.x, jsonResult.y))
                    {
                        _context.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.Accepted);
                    }
                    return new HttpResponseMessage(HttpStatusCode.NotModified);
                }
                return new HttpResponseMessage(HttpStatusCode.NotModified);
            }
            return new HttpResponseMessage(HttpStatusCode.NotModified);
        }
    }
}
