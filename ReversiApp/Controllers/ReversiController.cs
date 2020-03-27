﻿using System;
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
    [Route("api")]
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
                overwinnendeKleur = overwinnendeSpeler
            });
        }

        // GET: api/Reversi/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("spel/{id}/{x}/{y}")]
        public void DoeZet(int id, int x, int y)
        {
            Spel result = _context.Spel.SingleOrDefault(x => x.ID == id);
            result.DoeZet(y, x);
            _context.SaveChanges();
        }
    }
}
