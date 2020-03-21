using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReversiApp.Areas.Identity.Data;

namespace ReversiApp.Models
{
    public enum Kleur { Geen, Wit, Zwart };
    public class Spel : ISpel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public ICollection<User> Spelers { get; set; }
        [NotMapped]
        public Kleur[,] Bord { get; set; }
        [Display(Name = "Spelbord")]
        public string SerializedBord { get => JsonConvert.SerializeObject(Bord); set => Bord = JsonConvert.DeserializeObject<Kleur[,]>(value); }
        public Kleur AandeBeurt { get; set; }

        public Spel()
        {
            // New bord and fill it.
            // geen kleur = 0
            // Wit = 1
            // Zwart = 2

            Bord = new Kleur[8, 8];
            for (int x = 0; x < Bord.GetLength(0); x += 1)
            {
                for (int y = 0; y < Bord.GetLength(1); y += 1)
                {
                    Bord[x, y] = Kleur.Geen;
                }
            }

            Bord[3, 3] = Kleur.Wit;
            Bord[4, 4] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;
        }

        public bool Pas()
        {
            var oppositeColor = AandeBeurt == Kleur.Wit ? Kleur.Zwart : Kleur.Wit;
            int count = 0;
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ZetMogelijk(i,j))
                    {
                        Trace.WriteLine($"y: {i}, x: {j}");
                        count++; ;
                    }
                }
            }

            if (count == 0)
            {
                AandeBeurt = oppositeColor;
                return true;
            }
            return false;
        }

        public bool Afgelopen()
        {
            bool witAf = false;
            bool zwartAf = false;
            var oppositeColor = AandeBeurt == Kleur.Wit ? Kleur.Zwart : Kleur.Wit;

            int countBase = 0;
            int countOpposite = 0;
            int countFull = 0;
            
            // Check if board is full 
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Bord[i, j] == Kleur.Wit || Bord[i, j] == Kleur.Zwart)
                    {
                        countFull++;
                    }
                }
            }

            Console.WriteLine(countFull);
            if (countFull == 64)
            {
                return true;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ZetMogelijk(i,j))
                    {
                        countBase++;
                    }
                }
            }

            AandeBeurt = oppositeColor;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ZetMogelijk(i, j))
                    {
                        countOpposite++;
                    }
                }
            }

            if (countBase == 0 && countOpposite == 0)
            {
                return true;
            }

            return false;
        }

        public Kleur OverwegendeKleur()
        {
            int countGeen = 0;
            int countZwart = 0;
            int countWit = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Bord[i, j] == Kleur.Geen)
                    {
                        countGeen++;
                    }

                    if (Bord[i, j] == Kleur.Zwart)
                    {
                        countZwart++;
                    }

                    if (Bord[i, j] == Kleur.Wit)
                    {
                        countWit++;
                    }
                }
            }

            if (countGeen >= 60)
            {
                return Kleur.Geen;
            }
            if (countZwart >= countWit && countWit < 60)
            {
                return Kleur.Zwart;
            }
            if (countWit >= countZwart && countWit < 60)
            {
                return Kleur.Wit;
            }

            return Kleur.Geen;
        }

        // y, x
        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            int xAs = kolomZet;
            int yAs = rijZet;

            // If not outside boundaries.
            if (yAs > 7 | yAs < 0 | xAs > 7 | xAs < 0)
            {
                return false;
            }

            if (CheckDiagonal(xAs, yAs, AandeBeurt))
            {
                return true;
            }

            return false;
        }

        // y, x
        private Kleur CheckIfInBounds(int x, int y)
        {
            try
            {
                return Bord[y, x];
            }
            catch (Exception e)
            {
                return Kleur.Geen;
            }
        }

        private bool CheckDiagonal(int x, int y, Kleur eigenKleur)
        {
            int xSource = x;
            int ySource = y;
            Kleur colourSource = eigenKleur;
            Kleur oppositeColour = colourSource == Kleur.Wit ? Kleur.Zwart : Kleur.Wit;

            Console.WriteLine($"x: {xSource}, y: {ySource}");

            // x, y 
            Kleur bottom = CheckIfInBounds(x, y + 1);
            Kleur top = CheckIfInBounds(x, y - 1);
            Kleur left = CheckIfInBounds(x - 1, y);
            Kleur right = CheckIfInBounds(x + 1, y);
            
            Kleur bottomLeft = CheckIfInBounds(x - 1, y + 1);
            Kleur bottomRight = CheckIfInBounds(x + 1, y + 1);
            Kleur topleft = CheckIfInBounds(x - 1, y - 1);
            Kleur topRight = CheckIfInBounds(x + 1, y - 1);

            if (Bord[ySource, xSource] == oppositeColour)
            {
                return false;
            }

            // boven
            int count = 0;
            List<int[]> locationsToChange = new List<int[]>();

            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource, ySource - i);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new []{ ySource - i , xSource });
                    count++;
                }
            }

            // onder
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource, ySource + i);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource + i, xSource });
                    count++;
                }
            }

            // links
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource - i, ySource);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource, xSource - i });
                    count++;
                }
            }

            // rechts
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource + i, ySource);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource, xSource + i });
                    count++;
                }
            }

            // linksboven
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource - i, ySource - i);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource - i, xSource - i });
                    count++;
                }
            }

            // rechtsboven
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource + i, ySource - i);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource - i, xSource + i });
                    count++;
                }
            }

            // linksonder
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource - i, ySource + i);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource + i, xSource - i });
                    count++;
                }
            }

            // rechtonder
            count = 0;
            locationsToChange = new List<int[]>();
            for (int i = 1; i < 8; i++)
            {
                Kleur temp = CheckIfInBounds(xSource + i, ySource + i);
                if (count > 0 && temp == Kleur.Geen || i > 0 && temp == Kleur.Geen)
                {
                    break;
                }
                if (count > 0 && colourSource == temp)
                {
                    ChangeLocations(locationsToChange);
                    return true;
                }
                if (temp != colourSource && temp != Kleur.Geen)
                {
                    locationsToChange.Add(new[] { ySource + i, xSource + i });
                    count++;
                }
            }

            return false;
        }

        
        private void ChangeLocations(List<int[]> list)
        {
            if (list.Count > 0)
            {
                foreach (int[] location in list)
                {
                    // y, x
                    Bord[location[0], location[1]] = AandeBeurt;
                }
            }
        }

        // y, x
        public bool DoeZet(int rijZet, int kolomZet)
        {
            var oppositeColor = AandeBeurt == Kleur.Wit ? Kleur.Zwart : Kleur.Wit;
            if (ZetMogelijk(rijZet, kolomZet))
            {
                // y, x
                Bord[rijZet, kolomZet] = AandeBeurt;
                AandeBeurt = oppositeColor;
                return true;
            }

            return false;
        }
    }
}
