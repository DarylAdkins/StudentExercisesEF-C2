using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExercisesEF.Data;
using StudentExercisesEF.Models;
using StudentExercisesEF.Models.ViewModels;

namespace StudentExercisesEF.Controllers
{
    public class StudentsController : Controller
    {
        // Private field to store user manager
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        // Inject user manager into constructor
        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Private method to get current user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Students
        public async Task<IActionResult> Index(string searchQuery)
        {
            
            ApplicationUser loggedInUser = await GetCurrentUserAsync();
            List<Student> students = await _context.Student.Include(s => s.Cohort).Where(s => s.User == loggedInUser).ToListAsync();

            if(searchQuery != null)
            {
                students = students.Where(student => student.FirstName.Contains(searchQuery) || student.LastName.Contains(searchQuery)).ToList();
            }


            return View(students);
        }

        // GET: Students/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();

            var student = await _context.Student
                .Include(s => s.Cohort)
                .FirstOrDefaultAsync(m => m.Id == id && m.User== user);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            //ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Id");
            CreateStudentViewModel vm = new CreateStudentViewModel();
            vm.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            vm.Cohorts.Insert(0, new SelectListItem()
            {
                Value = "0",
                Text = "Please choose a cohort"
            });
            return View(vm);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();
                vm.Student.UserId = currentUser.Id;
                _context.Add(vm.Student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.Cohorts = _context.Cohort.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(vm);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Id", student.CohortId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,SlackHandle,CohortId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await GetCurrentUserAsync();
                    student.UserId = currentUser.Id;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["CohortId"] = new SelectList(_context.Cohort, "Id", "Id", student.CohortId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Cohort)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
