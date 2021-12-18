using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworkDashboard.Data;
using NetworkDashboard.Models;

namespace NetworkDashboard.Controllers
{
    public class MonitorTypesController : Controller
    {
        private readonly DBContext _context;

        public MonitorTypesController(DBContext context)
        {
            _context = context;
        }

        // GET: MonitorTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MonitorTypes.ToListAsync());
        }

        // GET: MonitorTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitorTypes = await _context.MonitorTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitorTypes == null)
            {
                return NotFound();
            }

            return View(monitorTypes);
        }

        // GET: MonitorTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MonitorTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeTitle,TypeDescription,ping")] MonitorTypes monitorTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monitorTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monitorTypes);
        }

        // GET: MonitorTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitorTypes = await _context.MonitorTypes.FindAsync(id);
            if (monitorTypes == null)
            {
                return NotFound();
            }
            return View(monitorTypes);
        }

        // POST: MonitorTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeTitle,TypeDescription,ping")] MonitorTypes monitorTypes)
        {
            if (id != monitorTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monitorTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonitorTypesExists(monitorTypes.Id))
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
            return View(monitorTypes);
        }

        // GET: MonitorTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitorTypes = await _context.MonitorTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitorTypes == null)
            {
                return NotFound();
            }

            return View(monitorTypes);
        }

        // POST: MonitorTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monitorTypes = await _context.MonitorTypes.FindAsync(id);
            _context.MonitorTypes.Remove(monitorTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonitorTypesExists(int id)
        {
            return _context.MonitorTypes.Any(e => e.Id == id);
        }
    }
}
