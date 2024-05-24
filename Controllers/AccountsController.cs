using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StaffManagement.Data;
using StaffManagement.Models;

namespace StaffManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var applicationDbContext = _context.Accounts.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Username, string Password, bool IsAdmin, bool IsActive, int UserId)
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            var account = new Account
            {
                Username = Username,
                Password = Password,
                IsActive = IsActive,
                IsAdmin = IsAdmin,
                UserId = UserId
            };
            var find = _context.Accounts.FirstOrDefault(a => (a.Username == account.Username || a.UserId == UserId));
            if (ModelState.IsValid && find==null)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Message"] = "Account with Username or User is existed";
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", account.User.Id);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, string Username, string Password, bool IsAdmin, bool IsActive)
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            var account = _context.Accounts.FirstOrDefault(d => d.Id == Id);
            if (account != null)
            {
                account.Username = Username;
                account.Password = Password;
                account.IsActive = IsActive;
                account.IsAdmin = IsAdmin;
            }
            else
            {
                ViewData["Message"] = "Account not exist";
                return View(account);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(account).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
