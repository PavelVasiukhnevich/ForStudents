using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ForStudents.Models
{
    public class User : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string login;
        private string password;
        private DateTime lastDateLogin;
        public string DateLogin => LastDateLogin.ToShortDateString();
        private bool isAdmin;
        public string Type => isAdmin?"Admin":"User";

        private ICollection<NoteUser> noteUser;
        public ICollection<NoteUser> NoteUser
        {
            get { return noteUser; }
            set
            {
                noteUser = value;
                OnPropertyChanged("NoteUser");
            }
        }

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public DateTime LastDateLogin
        {
            get { return lastDateLogin; }
            set
            {
                lastDateLogin = value;
                OnPropertyChanged("LastDateLogin");
            }
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
