using ForStudents.Models;
using ForStudents.Views;
using ForStudents.Views.AdminViews;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ForStudents.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
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
        private string adminVisability;
        public string AdminVisability
        {
            get { return adminVisability; }
            set
            {
                adminVisability = value;
                OnPropertyChanged("AdminVisability");
            }
        }

        public void CheckAdmin()
        {
            if (CurrentUser.IsAdmin)
            {
                AdminVisability = "Visible";
            }
            else
            {
                AdminVisability = "Hidden";
            }
        }

        private string menuWidth;
        public string MenuWidth
        {
            get { return menuWidth; }
            set
            {
                menuWidth = value;
                OnPropertyChanged("MenuWidth");
            }
        }
        public void MenuWidthAuto()
        {
            MenuWidth = "auto"; // 200d;
        }
        public void MenuWidth0()
        {
            MenuWidth = "0";// 0d;
        }

        private Page homePage;
        private HomePageViewModel homePageViewModel;

        private Page loginPage;
        private LoginPageViewModel loginPageViewModel;

        private Page notePage;
        private NotePageViewModel notePageViewModel;

        private RegistationPageViewModel registationPageViewModel;
        private Page registrationPage;

        private Page usersPage;
        private UsersPageViewModel usersPageViewModel;

        private Page createNotePage;
        private CreateNotePageViewModel createNotePageViewModel;

        private Page currentPage;
        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public MainWindowViewModel()//Конструктор
        {
            StartUp();

            LoginPage();
            MenuWidth0();
        }

        public void StartUp()
        {
            homePageViewModel = new HomePageViewModel(this);
            homePage = new HomePage() { DataContext = homePageViewModel };

            loginPageViewModel = new LoginPageViewModel(this);
            loginPage = new LoginPage() { DataContext = loginPageViewModel };

            notePageViewModel = new NotePageViewModel(this);
            notePage = new NotePage() { DataContext = notePageViewModel };


            usersPageViewModel = new UsersPageViewModel(this);
            usersPage = new UsersPage() { DataContext = usersPageViewModel };

            createNotePageViewModel = new CreateNotePageViewModel(this);
            createNotePage = new CreateNotePage() { DataContext = createNotePageViewModel };

            registationPageViewModel = new RegistationPageViewModel(this);
            registrationPage = new RegistrationPage() { DataContext = registationPageViewModel };

        }

        private RelayCommand usersPageCommand;
        public RelayCommand UsersPageCommand
        {
            get
            {
                return usersPageCommand ??
                  (usersPageCommand = new RelayCommand(obj =>
                  {
                      UsersPage();
                  }, obj => CurrentUser != null));
            }
        }

        private RelayCommand homePageCommand;
        public RelayCommand HomePageCommand
        {
            get
            {
                return homePageCommand ??
                  (homePageCommand = new RelayCommand(obj =>
                  {
                      HomePage();
                  }, obj => CurrentUser != null));
            }
        }

        private RelayCommand loginPageCommand;
        public RelayCommand LoginPageCommand
        {
            get
            {
                return loginPageCommand ??
                  (loginPageCommand = new RelayCommand(obj =>
                  {
                      CurrentPage = loginPage;
                      MenuWidth0();
                  }));
            }
        }

        private RelayCommand createNotePageCommand;
        public RelayCommand CreateNotePageCommand
        {
            get
            {
                return createNotePageCommand ??
                  (createNotePageCommand = new RelayCommand(obj =>
                  {
                      CreateNotePage();
                  }));
            }
        }
        


        private RelayCommand counterCommand;
        public RelayCommand CounterCommand
        {
            get
            {
                return counterCommand ??
                  (counterCommand = new RelayCommand(obj =>
                  {
                      MessageBox.Show(CurrentUser.Name);
                  }));
            }
        }



        public void HomePage()
        {
            homePageViewModel.UpdateNotesList();
            CurrentPage = homePage;
        }
        public void LoginPage()
        {
            CurrentPage = loginPage;
        }
        public void NotePage(NoteForUser noteForUser)
        {
            notePageViewModel.StartUp(noteForUser);
            CurrentPage = notePage;
        }
        public void RegistrationPage()
        {
            registationPageViewModel.NewRegistation();
            CurrentPage = registrationPage;
        }
        public void UsersPage()
        {
            CurrentPage = usersPage;
        }
        public void CreateNotePage()
        {
            createNotePageViewModel.Start();
            CurrentPage = createNotePage;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
