using ManicureBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ManicureBooking.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly DatabaseContext _context;

        public AppointmentController(DatabaseContext context)
        {
            _context = context;
        }

        // 1. READ: Wyświetlanie listy wizyt
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var appointments = _context.Appointments.Include(a => a.Service).ToList();
            return View(appointments);
        }

        // 2. READ: Szczegóły wizyty
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var appointment = _context.Appointments.Include(a => a.Service).FirstOrDefault(a => a.AppointmentId == id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // 3. CREATE: Dodawanie nowej wizyty (GET - pusty formularz)
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name");
            var defaultAppointment = new Appointment
            {
                AppointmentTime = DateTime.Now.AddDays(1)
            };

            return View(defaultAppointment);
        }

        // 3. CREATE: Zapisywanie do bazy (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment appointment)
        {
            if (appointment.AppointmentTime < DateTime.Now) // sprawdzenie czy data jest z przeszlosci
            {
                ModelState.AddModelError("AppointmentTime", "Nie można zarezerwować wizyty na minioną datę ani godzinę!");
            }

            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // 4. UPDATE: Edycja wizyty (GET - wypełniony formularz)
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var appointment = _context.Appointments.Find(id);
            if (appointment == null) return NotFound();

            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // 4. UPDATE: Zapisanie zmian (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Appointment appointment)
        {
            if (appointment.AppointmentTime < DateTime.Now)
            {
                ModelState.AddModelError("AppointmentTime", "Nie można edytować wizyty na minioną datę!");
            }

            if (ModelState.IsValid)
            {
                _context.Appointments.Update(appointment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", appointment.ServiceId);
            return View(appointment);
        }

        // 5. DELETE: Usuwanie wizyty (GET - ekran potwierdzenia)
        [HttpGet]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var appointment = _context.Appointments.Include(a => a.Service).FirstOrDefault(a => a.AppointmentId == id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // 5. DELETE: Potwierdzenie usunięcia (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteConfirm(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}