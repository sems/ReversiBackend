using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ReversiApp.Areas.Identity.Data;
using ReversiApp.Data;
using ReversiApp.Models;

namespace MvcScaffolding.Controllers
{
    [Authorize(Roles = "Mod, Admin")]
    public class UsersController : Controller
    {
        private readonly ReversiAppContext _context;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController(ReversiAppContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<User, Spel> mvcScaffoldingContext;
            if (User.IsInRole("Mod"))
            {
                mvcScaffoldingContext = _context.User
                    .Include(u => u.Spel);
            }
            else
            {
                mvcScaffoldingContext = _context.User
                    .Include(u => u.Spel);
            }
            return View(await mvcScaffoldingContext.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Spel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["SpelId"] = new SelectList(_context.Spel, "ID", "ID");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid().ToString();
                user.Token = Guid.NewGuid().ToString();
                user.Archived = false;
                user.NormalizedEmail = user.Email.ToUpper();
                user.NormalizedUserName = user.UserName.ToUpper();
                user.SpelId = null;
                user.PasswordHash = Guid.NewGuid().ToString();
                user.AccessFailedCount = 0;
                user.LockoutEnabled = true;
                user.LockoutEnd = null;
                user.TwoFactorEnabled = false;
                user.PhoneNumberConfirmed = false;
                user.Kleur = Kleur.Geen;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpelId"] = new SelectList(_context.Spel, "ID", "ID", user.SpelId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["SpelId"] = new SelectList(_context.Spel, "ID", "ID", user.SpelId);
            return View(user);
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Token,SpelId,Id,UserName,Email,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.NormalizedEmail = user.Email.ToUpper();
                    user.NormalizedUserName = user.UserName.ToUpper();
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpelId"] = new SelectList(_context.Spel, "ID", "ID", user.SpelId);
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Spel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        // POST: Users/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            user.Archived = true;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
