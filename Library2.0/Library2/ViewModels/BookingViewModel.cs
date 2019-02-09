using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Library2.Models;

namespace Library2.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private ApplicationDbContext context = new ApplicationDbContext(); //<---zm na repo
        private DialogBox dialogBox = new DialogBox();
        private DatabaseRepository repository = new DatabaseRepository();
        public static ObservableCollection<Booking> BookingList { get; set; } = new ObservableCollection<Booking>();
        public static List<Reader> Readers { get; set; } = new List<Reader>();
        public static List<Book> Books { get; set; } = new List<Book>();
        private Booking model = new Booking();
        public BookingViewModel()
        {
            LoadBookings();
            LoadBooks();
            LoadReaders();
        }
        private void LoadBookings()
        {
            BookingList.Clear();
            IEnumerable<Booking> bookings = repository.GetBookings();
            foreach(Booking item in bookings)
            {
                BookingList.Add(item);
            }
        }
        private void LoadReaders()
        {
            Readers.Clear();
            IEnumerable<Reader> readers = context.Readers.AsEnumerable().ToList();
            foreach (Reader item in readers)
            {
                Readers.Add(item);
            }
        }
        private void LoadBooks()
        {
            Books.Clear();
            IEnumerable<Book> books = context.Books.AsEnumerable().ToList();
            foreach (Book item in books)
            {
                Books.Add(item);
                foreach (Booking booking in BookingList)
                {
                    if (item.Id == booking.Book_Id && booking.ReturnDate == null) Books.Remove(item);
                }
            }
        }
        //model
        public int Id
        {
            get
            {
                return model.Id;
            }
            set
            {
                model.Id = value;
            }
        }
        public string Comments
        {
            get
            {
                return model.Comments;
            }
            set
            {
                model.Comments = value;
            }
        }
        public DateTime BorrowDate
        {
            get
            {
                return model.BorrowDate;
            }
            set
            {
                model.BorrowDate = value;
            }
        }
        public DateTime? ReturnDate
        {
            get
            {
                return model.ReturnDate;
            }
            set
            {
                model.ReturnDate = value;
            }
        }
        public int Reader_Id
        {
            get
            {
                return model.Reader_Id;
            }
            set
            {
                model.Reader_Id = value;
            }
        }
        public int Book_Id
        {
            get
            {
                return model.Book_Id;
            }
            set
            {
                model.Book_Id = value;
            }
        }
        public Book Book
        {
            get
            {
                return model.Book;
            }
            set
            {
                model.Book = value;
            }
        }
        public Reader Reader
        {
            get
            {
                return model.Reader;
            }
            set
            {
                model.Reader = value;
            }
        }
        //create new booking
        private string newComments;
        public string NewComments
        {
            get
            {
                return newComments;
            }
            set
            {
                newComments = value;
            }
        }
        private Book newbook;
        public Book NewBook
        {
            get
            {
                return newbook;
            }
            set
            {
                newbook = value;
            }
        }
        private Reader newreader;
        public Reader NewReader
        {
            get
            {
                return newreader;
            }
            set
            {
                newreader = value;
            }
        }
        //return bookings
        private ICommand returnBooking;
        public ICommand ReturnBooking
        {
            get
            {
                if (returnBooking == null)
                    returnBooking = new RelayCommand(
                    o =>
                    {
                        int bookingIndex = (int)o;
                        Booking booking = BookingList[bookingIndex];
                        if (booking.ReturnDate == null)
                        {
                            booking.ReturnDate = DateTime.Now;
                            var openWindow = new WindowDialog();
                            openWindow.ShowWindow(new CommentViewModel(booking), 195, 250, "Comments");
                            repository.ReturnBooking(booking);
                            dialogBox.ShowDialog("Return updated", "Message");
                        }
                        else
                        {
                            dialogBox.ShowDialog("This book is already returned", "Message");
                        }
                    },
                    o =>
                    {
                        if (o == null) return false;
                        int bookingIndex = (int)o;
                        return bookingIndex >= 0;
                    });
                return returnBooking;
            }
        }
        private ICommand deleteBooking;
        public ICommand DeleteBooking
        {
            get
            {
                if (deleteBooking == null)
                    deleteBooking = new RelayCommand(
                    o =>
                    {
                        int bookingIndex = (int)o;
                        Booking booking = BookingList[bookingIndex];
                        BookingList.Remove(booking);
                        repository.RemoveBooking(booking);
                        dialogBox.ShowDialog("Record deleted", "Message");
                    },
                    o =>
                    {
                        if (o == null) return false;
                        int bookingIndex = (int)o;
                        return bookingIndex >= 0;
                    });
                return deleteBooking;
            }
        }
        private ICommand addBooking;
        public ICommand AddBooking
        {
            get
            {
                Booking newBooking = new Booking();
                if (addBooking == null)
                    addBooking = new RelayCommand(
                    o =>
                    {
                        var openWindow = new WindowDialog();
                        openWindow.ShowWindow(new BookingViewModel(), 315, 250, "Booking");
                    });
                return addBooking;
            }
        }
        private ICommand confirmBooking;
        public ICommand ConfirmBooking
        {
            get
            {
                Booking newBooking = new Booking();
                if (confirmBooking == null)
                    confirmBooking = new RelayCommand(
                    o =>
                    {
                        if (NewReader != null && NewBook != null)
                        {
                            newBooking.Reader_Id = NewReader.Id;
                            newBooking.Book_Id = NewBook.Id;
                            newBooking.Comments = NewComments;
                            newBooking.BorrowDate = DateTime.Now;
                            newBooking.ReturnDate = null;
                            repository.AddBooking(newBooking);
                            newBooking.Book = NewBook; //adds book for the collection
                            newBooking.Reader = NewReader; //adds reader for the collection
                            BookingList.Add(newBooking);
                            dialogBox.ShowDialog("Booking added", "Message");
                        }
                        else
                        {
                            dialogBox.ShowDialog("You need to pick up reader and book from the lists", "Message");
                        }
                    });
                return confirmBooking;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] props)
        {
            if (PropertyChanged != null)
            {
                foreach (string prop in props)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
