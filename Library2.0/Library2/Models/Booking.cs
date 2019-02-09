using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library2.Models
{
    public class Booking : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] props)
        {
            if (PropertyChanged != null)
            {
                foreach (string prop in props)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        private int id;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //to give automaticly new ID
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private string comments;
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }
        public DateTime BorrowDate { get; set; }
        private DateTime? returnDate;
        public DateTime? ReturnDate
        {
            get
            {
                return returnDate;
            }
            set
            {
                returnDate = value;
                OnPropertyChanged("ReturnDate");
            }
        }
        private int reader_id;
        [ForeignKey("Reader")]
        public int Reader_Id
        {
            get
            {
                return reader_id;
            }
            set
            {
                reader_id = value;
                OnPropertyChanged("Reader_Id");
            }
        }
        public Reader Reader { get; set; }
        private int book_id;
        [ForeignKey("Book")]
        public int Book_Id
        {
            get
            {
                return book_id;
            }
            set
            {
                book_id = value;
                OnPropertyChanged("Book_Id");
            }
        }
        public Book Book { get; set; }
    }
}
