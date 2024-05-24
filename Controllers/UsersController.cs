using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Data;
using StaffManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StaffManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if(role!="Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var user = _context.Users.Include(d => d.Department);
            return View(await user.ToListAsync());
        }

        // GET: Staffs/Details/5
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
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Users.Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
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
                return RedirectToAction("Index", "Home");
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string StaffNumber, string Name, string PhoneNumber, string Email, DateOnly DOB, string Address, int DepartmentId, string Description)
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
            var user = new User
            {
                Address = Address,
                Name = Name,
                PhoneNumber = PhoneNumber,
                Email = Email,
                DOB = DOB,
                DepartmentId = DepartmentId,
                Description = Description,
                StaffNumber = StaffNumber
            };
            var find = _context.Users.FirstOrDefault(u => u.Name==Name || u.Email==Email || u.StaffNumber==StaffNumber);
            if (ModelState.IsValid && find==null)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();                
                return RedirectToAction(nameof(Index));
            }
            ViewData["Message"] = "User existed with Name or Email or StaffNumber";
            ViewData["Department"] = new SelectList(_context.Departments, "Id", "Name");
            return View(user);
        }

        // GET: Staffs/Edit/5
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

            var staff = await _context.Users.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, string StaffNumber, string Name, string PhoneNumber, string Email, DateOnly DOB, string Address, int DepartmentId, string Description)
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            var staff = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (staff != null)
            {
                staff.Name = Name;
                staff.PhoneNumber = PhoneNumber;
                staff.Description = Description;
                staff.Email = Email;
                staff.DOB = DOB;
                staff.Address = Address;
                staff.DepartmentId = DepartmentId;
                staff.StaffNumber = StaffNumber;
            }
            else
            {
                ViewData["Message"] = "Staff not exist";
                return View(staff);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(staff).State= EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View(staff);
        }

        // GET: Staffs/Delete/5
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
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Users.Include(d=>d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
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
                return RedirectToAction("Index", "Home");
            }
            var staff = await _context.Users.FindAsync(id);
            if (staff != null)
            {
                _context.Users.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
