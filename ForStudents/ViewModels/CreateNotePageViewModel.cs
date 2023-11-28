using ForStudents.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ForStudents.ViewModels
{
    public class CreateNotePageViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel mainWindowViewModel;
        private PhotoLikesContext photoLikesContext;
        public int GetIdCurrentUser => mainWindowViewModel.CurrentUser.Id;
        private Note note;
        public Note Note
        { 
            get { return note; }
            set { note = value;
                OnPropertyChanged("Note");
            }
        }

        private RelayCommand selectCommand;
        public RelayCommand SelectCommand
        {
            get
            {
                return selectCommand ??
                  (selectCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          OpenFileDialog op = new OpenFileDialog();
                          op.Title = "Select a picture";
                          op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                            "Portable Network Graphic (*.png)|*.png";
                          if (op.ShowDialog() == DialogResult.OK)
                          {
                              Note.FilePath= op.FileName;
                          }
                      }
                      catch { }
                  }));
            }
        }

        private RelayCommand openPictureCommand;
        public RelayCommand OpenPictureCommand
        {
            get
            {
                return openPictureCommand ??
                  (openPictureCommand = new RelayCommand(obj =>
                  {
                      Process.Start(new ProcessStartInfo() { FileName = Note.FilePath, UseShellExecute = true });
                  }));
            }
        }

        private RelayCommand openFolderCommand;
        public RelayCommand OpenFolderCommand
        {
            get
            {
                return openFolderCommand ??
                  (openFolderCommand = new RelayCommand(obj =>
                  {
                      Process.Start("explorer.exe", GetPath(Note.FilePath));
                  }));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      photoLikesContext.Notes.Add(Note);
                      photoLikesContext.SaveChanges();
                      photoLikesContext.NotesUsers.Add(new NoteUser()
                      {
                          Note = Note,
                          User = photoLikesContext.Users.FirstOrDefault(u => u.Id == GetIdCurrentUser),
                          IsCreater = true
                      });
                      photoLikesContext.SaveChanges();
                      mainWindowViewModel.HomePage();
                  }, obj=> Note.FilePath!=null && Note.Description!=null));
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
                      mainWindowViewModel.HomePage();
                  }));
            }
        }

        private string GetPath(string path)
        {
            return path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal));
        }
        public void Start()
        {
            Note = new Note();
        }


        public CreateNotePageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            photoLikesContext= new PhotoLikesContext();
            this.mainWindowViewModel = mainWindowViewModel;
            Start();
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
