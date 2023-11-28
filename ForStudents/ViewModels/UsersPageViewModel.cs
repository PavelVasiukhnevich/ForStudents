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
    public class UsersPageViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel mainWindowViewModel;
        private PhotoLikesContext photoLikesContext;
        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private User newUser;
        public User NewUser
        {
            get { return newUser; }
            set
            {
                newUser = value;
                OnPropertyChanged("NewUser");
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

        private string addMenuVisability;
        public string AddMenuVisability
        {
            get { return addMenuVisability; }
            set
            {
                addMenuVisability = value;
                OnPropertyChanged("AddMenuVisability");
            }
        }
        public void AddMenuVisabilitySwap()
        {
            if (AddMenuVisability == "Hidden")
                AddMenuVisability = "Visible";
            else
                AddMenuVisability = "Hidden";
        }
        public void UpdateUsersList()
        {
            Users = new ObservableCollection<User>(photoLikesContext.Users.ToList());
        }
        public void UpdateUsersList(string parameter)
        {
            Users = new ObservableCollection<User>(photoLikesContext.Users.Where
                (u => u.Name.Contains(parameter) || u.Login.Contains(parameter) 
                || u.Password.Contains(parameter)).ToList());
        }

        private string searchParameter;
        public string SearchParameter
        {
            get { return searchParameter; }
            set
            {
                searchParameter = value;
                OnPropertyChanged("SearchParameter");
            }
        }

        private RelayCommand searchUserCommand;
        public RelayCommand SearchUserCommand
        {
            get
            {
                return searchUserCommand ??
                  (searchUserCommand = new RelayCommand(obj =>
                  {
                      if (SearchParameter != "")
                      {
                          UpdateUsersList(SearchParameter);
                      }
                      else
                      {
                          UpdateUsersList();
                      }
                  }, obj => photoLikesContext.Users.Count() > 0 && SearchParameter != null));
            }
        }

        private RelayCommand deleteUserCommand;
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ??
                  (deleteUserCommand = new RelayCommand(obj =>
                  {
                      User us = SelectedUser;

                      photoLikesContext.Users.Remove(photoLikesContext.Users.FirstOrDefault(u=>u.Login== us.Login) );
                      photoLikesContext.SaveChanges();
                          UpdateUsersList();
                  }, obj => photoLikesContext.Users.Count() > 0 && SelectedUser != null));
            }
        }

        private RelayCommand saveUserCommand;
        public RelayCommand SaveUserCommand
        {
            get
            {
                return saveUserCommand ??
                  (saveUserCommand = new RelayCommand(obj =>
                  {
                      User us = NewUser;
                      photoLikesContext.Users.Add(us);
                      photoLikesContext.SaveChanges();
                      UpdateUsersList();
                      AddMenuVisabilitySwap();
                  }, obj=>NewUser.Login!=""&&NewUser.Password!="" && NewUser.Name != ""));
            }
        }

        private RelayCommand addUserCommand;
        public RelayCommand AddUserCommand
        {
            get
            {
                return addUserCommand ??
                  (addUserCommand = new RelayCommand(obj =>
                  {
                      AddMenuVisabilitySwap();
                  }));
            }
        }
        public UsersPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            photoLikesContext = new PhotoLikesContext();
            NewUser= new User() { Name="",Login="",Password="",LastDateLogin=DateTime.Now};
            SelectedUser = new User();
            AddMenuVisabilitySwap();
            UpdateUsersList();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
