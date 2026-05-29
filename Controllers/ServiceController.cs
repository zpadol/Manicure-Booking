using ManicureBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManicureBooking.Controllers
{
    public class ServiceController : Controller
    {
        private readonly DatabaseContext _context;

        public ServiceController(DatabaseContext context)
        {
            _context = context;
        }

        // 1. READ: Wyświetlanie listy usług (Cennik)
        [HttpGet]
        public IActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        // 2. CREATE: Dodawanie nowej usługi (GET - pusty formularz)
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Service());
        }

        // 2. CREATE: Zapisywanie do bazy (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // 3. UPDATE: Edycja usługi (GET - wypełniony formularz)
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();
            return View(service);
        }

        // 3. UPDATE: Zapisanie zmian (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Update(service);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // 4. DELETE: Usuwanie usługi (GET - ekran potwierdzenia)
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();
            return View(service);
        }

        // 4. DELETE: Potwierdzenie usunięcia (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int id)
        {
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        // 5. READ: Szczegóły usługi (Details)
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();
            return View(service);
        }
    }
}