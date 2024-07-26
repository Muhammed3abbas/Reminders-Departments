namespace RingoMediaReminder.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reminder
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Date-Time is required")]
        public DateTime ReminderDateTime { get; set; }

        public string Emails { get; set; } // This should be a string

    }


}
