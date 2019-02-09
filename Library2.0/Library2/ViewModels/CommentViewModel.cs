using Library2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Library2.ViewModels
{
    public class CommentViewModel : INotifyPropertyChanged
    {
        DialogBox dialogBox = new DialogBox();
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] props)
        {
            if (PropertyChanged != null)
            {
                foreach (string prop in props)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        private string newComments;
        public string NewComments
        {
            get
            {
                return newComments = updateBoking.Comments;
            }
            set
            {
                newComments = value;
            }
        }
        Booking updateBoking = new Booking();
        public CommentViewModel(Booking booking)
        {
            updateBoking = booking;
        }
        private ICommand addComment;
        public ICommand AddComment
        {
            get
            {
                if (addComment == null)
                    addComment = new RelayCommand(
                        o =>
                        {
                            updateBoking.Comments = NewComments;
                            dialogBox.ShowDialog("Comment updated", "Message");
                            NewComments = "";
                        });
                return addComment;
            }
        }
    }
}
