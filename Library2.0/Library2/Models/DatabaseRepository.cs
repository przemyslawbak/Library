using Library2.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library2.Models
{
    class DatabaseRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();
        //returns all bookings
        public IEnumerable<Booking> GetBookings()
        {
            var bookings = context.Bookings.
                    Include(i => i.Book).
                    Include(i => i.Reader).
                    AsEnumerable().
                    ToList();
            return bookings;
        }
        //removes active booking from the context
        public void RemoveBooking(Booking booking)
        {
            Booking bookingToRemove = context.Bookings.FirstOrDefault(i => i.Id == booking.Id);
            context.Bookings.Remove(bookingToRemove);
            context.SaveChanges();
        }
        //adds new booking to the context
        public void AddBooking(Booking booking) //add new booking
        {
            context.Bookings.Add(booking);
            context.SaveChanges();
        }
        public void ReturnBooking(Booking booking) //add new booking
        {
            Booking toUpdate = context.Bookings.FirstOrDefault(i => i.Id == booking.Id);
            toUpdate.ReturnDate = booking.ReturnDate;
            context.SaveChanges();
        }
    }
}
