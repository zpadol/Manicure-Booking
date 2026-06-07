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
            var appointments = _context.Appointments.Include(a => a.Service).ToList().OrderBy(a => a.AppointmentTime); ;
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
                AppointmentTime = DateTime.Today.AddDays(1).AddHours(8) // defaultowo 8 rano jutro
            };

            return View(defaultAppointment);
        }

        // 3. CREATE: Zapisywanie do bazy (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment appointment)
        {
            appointment.Status = "Oczekująca";
            ModelState.Remove("Status");
            if (appointment.AppointmentTime < DateTime.Now) // sprawdzenie czy data jest z przeszlosci
            {
                ModelState.AddModelError("AppointmentTime", "Nie można zarezerwować wizyty na minioną datę ani godzinę!");
            }

            var requestedService = _context.Services.Find(appointment.ServiceId);

            if (requestedService != null)
            {
                DateTime newStart = appointment.AppointmentTime;
                DateTime newEnd = newStart.AddMinutes(requestedService.DurationInMinutes);

                var appointmentsOnThatDay = _context.Appointments
                    .Include(a => a.Service)
                    .Where(a => a.AppointmentTime.Date == newStart.Date)
                    .ToList();
                bool isTimeTaken = appointmentsOnThatDay.Any(a =>
                {
                    DateTime existingStart = a.AppointmentTime;
                    DateTime existingEnd = existingStart.AddMinutes(a.Service!.DurationInMinutes);

                    return newStart < existingEnd && newEnd > existingStart;
                });

                if (isTimeTaken)
                {
                    ModelState.AddModelError("AppointmentTime", "Termin zajęty.");
                }
            }


            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Wizyta została pomyślnie zarezerwowana! Do zobaczenia w ManiSpa";
                return RedirectToAction("Index", "Home");
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
            var requestedService = _context.Services.Find(appointment.ServiceId);

            if (requestedService != null)
            {
                DateTime newStart = appointment.AppointmentTime;
                DateTime newEnd = newStart.AddMinutes(requestedService.DurationInMinutes);

                var appointmentsOnThatDay = _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Service)
                    .Where(a => a.AppointmentTime.Date == newStart.Date)
                    .ToList();

                bool isTimeTaken = appointmentsOnThatDay.Any(a =>
                {
                    if (a.AppointmentId == appointment.AppointmentId)
                    {
                        return false;
                    }

                    DateTime existingStart = a.AppointmentTime;
                    DateTime existingEnd = existingStart.AddMinutes(a.Service!.DurationInMinutes);

                    return newStart < existingEnd && newEnd > existingStart;
                });

                if (isTimeTaken)
                {
                    ModelState.AddModelError("AppointmentTime", "Termin zajęty przez inną wizytę.");
                }
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