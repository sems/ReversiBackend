using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReversiApp.Models
{
    public enum Kleur { Geen, Wit, Zwart };
    public class Speler
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
        public string Token { get; set; }
        public Kleur Kleur { get; set; }

    }
}
