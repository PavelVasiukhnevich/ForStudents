using ForStudents.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForStudents.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel mainWindowViewModel;
        private PhotoLikesContext photoLikesContext;
        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }
        public LoginPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            photoLikesContext = new PhotoLikesContext();
            this.mainWindowViewModel = mainWindowViewModel;
            currentUser = new User();
            UpdateUsersList();
        }
        public void UpdateUsersList()
        {
            Users = new ObservableCollection<User>(new PhotoLikesContext().Users.ToList());
            if (Users.Count > 0 )
            {
                photoLikesContext.Users.Add(new User()
                {
                    LastDateLogin = DateTime.Now,
                    IsAdmin = true,
                    Login = "admin",
                    Password = "admin",
                    Name = "admin"
                });
                photoLikesContext.SaveChanges();
                UpdateUsersList();
            }
        }
        // команда добавления нового объекта
        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                  (loginCommand = new RelayCommand(obj =>
                  {
                      UpdateUsersList();
                      if (Users.Count > 0)
                      {
                          User us = Users.FirstOrDefault(u => u.Login == currentUser.Login);
                          if (us != null)
                          {
                              if (currentUser.Password == us.Password)
                              {
                                  mainWindowViewModel.CurrentUser = us;
                                  photoLikesContext.Users.First(u => u.Login == currentUser.Login).LastDateLogin = DateTime.Now;
                                  photoLikesContext.SaveChanges();
                                  currentUser.Password = "";
                                  currentUser.Login = "";
                                  mainWindowViewModel.StartUp();
                                  mainWindowViewModel.HomePage();
                                  mainWindowViewModel.MenuWidthAuto();
                                  mainWindowViewModel.CheckAdmin();
                              }
                              else
                              {
                                  MessageBox.Show("Неверный логин или пароль");
                              }
                          }
                          else
                          {
                              MessageBox.Show("Пользователь не найден");
                          }
                      }

                  }, obj => users!=null));
            }
        }

        private RelayCommand registationCommand;
        public RelayCommand RegistationCommand
        {
            get
            {
                return registationCommand ??
                  (registationCommand = new RelayCommand(obj =>
                  {
                      mainWindowViewModel.RegistrationPage();
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
