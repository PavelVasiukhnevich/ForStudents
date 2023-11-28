using ForStudents.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForStudents.ViewModels
{
    public class RegistationPageViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel mainWindowViewModel;
        PhotoLikesContext photoLikesContext;
        private User user;
        public User User
        {
            get { return user; }
            set { user = value;  OnPropertyChanged("User"); }
        }
        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }

        public ObservableCollection<User> Users
        {
            get { return new ObservableCollection<User>(photoLikesContext.Users.ToList()); }
            set { OnPropertyChanged("Users"); }
        }

        public RegistationPageViewModel(MainWindowViewModel mainWindowViewModel) 
        {
            NewRegistation();
            this.mainWindowViewModel = mainWindowViewModel;
            photoLikesContext = new PhotoLikesContext();
        }

        public void NewRegistation()
        {
            User=new User() { LastDateLogin=DateTime.Now};
            ConfirmPassword = "";
        }

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                  (createCommand = new RelayCommand(obj =>
                  {
                      if (Users.Where(u => u.Login == User.Login).Count() == 0)
                      {
                          if (User.Password == ConfirmPassword)
                          {
                              photoLikesContext.Users.Add(User);
                              photoLikesContext.SaveChanges();
                              mainWindowViewModel.LoginPage();
                          }
                          else
                          {
                              MessageBox.Show("Пароли не совпадают");
                          }
                      }
                      else
                      {
                          MessageBox.Show("Логин занят");
                      }

                  }, obj => User.Login != null && User.Login != "" &&
                  User.Password != null && User.Password != "" &&
                  User.Name != null && User.Name != ""));
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                  (cancelCommand = new RelayCommand(obj =>
                  {
                      mainWindowViewModel.LoginPage();

                  }));
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
