using ManicureBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public IActionResult Create()
        {
            return View(new Service());
        }

        // 2. CREATE: Zapisywanie do bazy (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Service service, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                service.ImagePath = fileName;
            }
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(Service service, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                service.ImagePath = fileName;
            }

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
        [Authorize]
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
        [Authorize]
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