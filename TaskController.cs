using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using Task = TaskManagement.Models.Task;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskDbContext _context;

        public TaskController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: Task
        //public async Task<IActionResult> Index()
        //{
        //    var tasks = await _context.Tasks.ToListAsync();
        //    return View(tasks);
        //}
        public IActionResult Index()
        {
            var tasks = _context.Tasks.OrderBy(t => t.Priority).ToList();
            if (tasks == null)
            {
                // Log the error or throw an exception
                throw new Exception("Tasks list is null");
            }
            return View(tasks);
        }


        [HttpPost]
        public IActionResult UpdateTaskPriority(int[] taskIds)
        {
            for (int i = 0; i < taskIds.Length; i++)
            {
                var task = _context.Tasks.Find(taskIds[i]);
                if (task != null)
                {
                    task.Priority = i + 1;
                    _context.Update(task);
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("Task/Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Task/Create")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,Priority")] TaskManagement.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }


        //// POST: Task/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,Priority")] TaskManagement.Models.Task task)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(task);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(task);
        //}

        // GET: Tasks/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,Priority")] TaskManagement.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }


        // GET: Tasks/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


        // GET: Tasks/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }


    }
}
