using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMediaReminder.Data;
using RingoMediaReminder.Entities;
using RingoMediaReminder.Models.Reminders;
using RingoMediaReminder.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RingoMediaReminder.Controllers
{
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailingServices _mailingServices;

        public RemindersController(ApplicationDbContext context, IMailingServices mailingServices)
        {
            _context = context;
            _mailingServices = mailingServices;
        }

        public async Task<IActionResult> Index()
        {
            var reminders = await _context.Reminders.ToListAsync();
            return View(reminders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReminderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reminder = new Reminder
                {
                    Title = model.Title,
                    ReminderDateTime = model.ReminderDateTime,
                    Emails = string.Join(",", model.Emails) // Convert List<string> to comma-separated string
                };

                _context.Add(reminder);
                await _context.SaveChangesAsync();

                // Schedule an email sending job using Hangfire
                BackgroundJob.Schedule(() => _mailingServices.SendEmailAsync(
                    model.Emails,
                    "Reminder: " + model.Title,
                    "This is a reminder for: " + model.Title
                ), model.ReminderDateTime);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            var model = new ReminderViewModel
            {
                Id = reminder.Id,
                Title = reminder.Title,
                ReminderDateTime = reminder.ReminderDateTime,
                Emails = reminder.Emails.Split(',').ToList() // Convert comma-separated string to List<string>
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReminderViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var reminder = await _context.Reminders.FindAsync(id);
                    if (reminder == null)
                    {
                        return NotFound();
                    }

                    reminder.Title = model.Title;
                    reminder.ReminderDateTime = model.ReminderDateTime;
                    reminder.Emails = string.Join(',',model.Emails); // Convert List<string> to comma-separated string
                    _context.Update(reminder);
                    await _context.SaveChangesAsync();

                    // Code to reschedule email sending can be added here

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReminderExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminder = await _context.Reminders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reminder == null)
            {
                return NotFound();
            }

            return View(reminder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReminderExists(int id)
        {
            return _context.Reminders.Any(e => e.Id == id);
        }
    }
}
