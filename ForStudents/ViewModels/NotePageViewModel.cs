using ForStudents.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForStudents.ViewModels
{
    public class NotePageViewModel : INotifyPropertyChanged
    {
        public int GetIdCurrentUser => mainWindowViewModel.CurrentUser.Id;
        private MainWindowViewModel mainWindowViewModel;
        private PhotoLikesContext photoLikesContext;

        private NoteForUser notesForUser;
        public NoteForUser NoteForUser
        {
            get { return notesForUser; }
            set { notesForUser = value; OnPropertyChanged("NoteForUser"); }
        }

        public NotePageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            photoLikesContext = new PhotoLikesContext();
            NoteForUser = new NoteForUser(mainWindowViewModel);
        }
        public void StartUp(NoteForUser noteForUser)
        {
            NoteForUser=noteForUser;
        }

        private RelayCommand openPictureCommand;
        public RelayCommand OpenPictureCommand
        {
            get
            {
                return openPictureCommand ??
                  (openPictureCommand = new RelayCommand(obj =>
                  {
                      Process.Start(new ProcessStartInfo() { FileName = NoteForUser.Note1.FilePath, UseShellExecute = true });
                  }));
            }
        }
        private RelayCommand backHomeCommand;
        public RelayCommand BackHomeCommand
        {
            get
            {
                return backHomeCommand ??
                  (backHomeCommand = new RelayCommand(obj =>
                  {
                      mainWindowViewModel.HomePage();
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
