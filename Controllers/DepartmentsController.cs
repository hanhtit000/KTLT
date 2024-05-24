using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Data;
using StaffManagement.Models;
using System.Security.Policy;

namespace StaffManagement.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(await _context.Departments.Include(u => u.User).ToListAsync());
        }

        // GET: Departments/Details/5
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

            var department = await _context.Departments.Include(u=>u.User)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
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
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Code, string Name)
        {
            var role = HttpContext.Session.GetString("Role");
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            var UserID = HttpContext.Session.GetInt32("UserID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (role != "Admin")
            {
                return RedirectToAction("Index");
            }
            var department = new Department
            {
                Code = Code,
                Name = Name,
                UserId = (int)UserID,
                CreatedOn = DateTime.UtcNow
            };
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int DepartmentId, string Code, string Name)
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
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentId == DepartmentId);
            department.Code = Code;
            department.Name = Name;
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Entry(department).State=EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
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
            return View(department);
        }

        // GET: Departments/Delete/5
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

            var department = await _context.Departments.Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
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
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
