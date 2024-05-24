using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Data;
using StaffManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StaffManagement.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(await _context.Schedules.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var schedule = await _context.Schedules.Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
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
            var list = _context.Users.Include(u=>u.Account).Where(u => u.Account.IsAdmin == false).ToList();
            ViewData["UserId"] = new SelectList(list, "Id", "Name");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UserId, string Name, string Description, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime)
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
            var list = _context.Users.Include(u => u.Account).Where(u => u.Account.IsAdmin == false).ToList();
            ViewData["UserId"] = new SelectList(list, "Id", "Name");
            var schedule = new Schedule
            {
                UserId = UserId,
                Name = Name,
                Description = Description,
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime,
                IsAccept=false
            };
            if(StartTime>EndTime)
            {
                ViewData["Message"] = "Start Time must before End Time";
                return View(schedule);
            }
            if (Date<DateOnly.FromDateTime(DateTime.Now))
            {
                ViewData["Message"] = "Date must not in the past";
                return View(schedule);
            }
            var scheduleList = _context.Schedules.Where(s => s.Date == Date && ((s.StartTime.CompareTo(StartTime)<1 && StartTime.CompareTo(s.EndTime)<0) || (s.StartTime.CompareTo(EndTime)<0 && EndTime.CompareTo(s.EndTime)<1) || (s.StartTime.CompareTo(StartTime)<1 && EndTime.CompareTo(s.EndTime)<1)) && s.UserId==UserId).ToList();
            if (ModelState.IsValid && scheduleList.Count==0)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["Message"] = "Start Time and End Time conflict with others schedule with selected staff";
                return View(schedule);
            }
            return View(schedule);
        }

        // GET: Schedules/Edit/5
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

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            var list = _context.Users.Include(u => u.Account).Where(u => u.Account.IsAdmin == false).ToList();
            ViewData["UserId"] = new SelectList(list, "Id", "Name");
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, int UserId, string Name, string Description, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime)
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
            var list = _context.Users.Include(u => u.Account).Where(u => u.Account.IsAdmin == false).ToList();
            ViewData["UserId"] = new SelectList(list, "Id", "Name"); 
            var schedule = _context.Schedules.FirstOrDefault(s => s.Id == Id);
            if (schedule != null)
            {
                schedule.UserId = UserId;
                schedule.Name = Name;
                schedule.Description = Description;
                schedule.Date = Date;
                schedule.StartTime = StartTime;
                schedule.EndTime = EndTime;
            }
            else
            {
                ViewData["Message"] = "Schedule not exist";
                return View(schedule);
            }
            if (StartTime > EndTime)
            {
                ViewData["Message"] = "Start Time must before End Time";
                return View(schedule);
            }
            if (Date < DateOnly.FromDateTime(DateTime.Now))
            {
                ViewData["Message"] = "Date must not in the past";
                return View(schedule);
            }
            var scheduleList = _context.Schedules.Where(s => s.Date == Date && ((s.StartTime.CompareTo(StartTime) < 1 && StartTime.CompareTo(s.EndTime) < 0) || (s.StartTime.CompareTo(EndTime) < 0 && EndTime.CompareTo(s.EndTime) < 1) || (s.StartTime.CompareTo(StartTime) < 1 && EndTime.CompareTo(s.EndTime) < 1)) && s.UserId == UserId).ToList();
            
            if (ModelState.IsValid && scheduleList.Count == 0)
            {
                try
                {
                    _context.Entry(schedule).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            return View(schedule);
        }

        // GET: Schedules/Delete/5
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

            var schedule = await _context.Schedules.Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Accept(int id)
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                schedule.IsAccept = true;
                _context.Entry(schedule).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Cancel(int id)
        {
            var AccountID = HttpContext.Session.GetInt32("AccountID");
            if (AccountID == null || AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
