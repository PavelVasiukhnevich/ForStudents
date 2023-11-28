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
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public int GetIdCurrentUser => mainWindowViewModel.CurrentUser.Id;
        private MainWindowViewModel mainWindowViewModel;
        private PhotoLikesContext photoLikesContext;
        private ObservableCollection<Note> notes;
        public ObservableCollection<Note> Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }
        private ObservableCollection<NoteForUser> notesForUser;
        public ObservableCollection<NoteForUser> NotesForUsers
        {
            get { return notesForUser; }
            set
            {
                notesForUser = value; 
                OnPropertyChanged("NotesForUsers");
            }
        }
        private NoteForUser selectedNoteForUser;
        public NoteForUser SelectedNoteForUser
        { 
        get { return selectedNoteForUser; }
            set { selectedNoteForUser = value; OnPropertyChanged("SelectedNoteForUser"); }
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

        private RelayCommand searchNotesCommand;
        public RelayCommand SearchNotesCommand
        {
            get
            {
                return searchNotesCommand ??
                  (searchNotesCommand = new RelayCommand(obj =>
                  {
                      if (SearchParameter != "")
                      {
                          UpdateNotesList(SearchParameter);
                      }
                      else
                      {
                          UpdateNotesList();
                      }
                  }, obj => NotesForUsers!=null && SearchParameter != null));
            }
        }


        public HomePageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            photoLikesContext = new PhotoLikesContext();
            this.mainWindowViewModel = mainWindowViewModel;
            UpdateNotesList();
        }
        public void UpdateNotesList()
        {
            Notes = new ObservableCollection<Note>(photoLikesContext.Notes.ToList());
            NotesForUsers = new ObservableCollection<NoteForUser>();
            Notes.ToList().ForEach(note => NotesForUsers.Add(new NoteForUser(mainWindowViewModel) 
            { NoteId = note.Id}) );
        }
        public void UpdateNotesList(string search_parameter)
        {
            Notes = new ObservableCollection<Note>(photoLikesContext.Notes.ToList());
            NotesForUsers = new ObservableCollection<NoteForUser>();
            Notes.ToList().ForEach(note => NotesForUsers.Add(new NoteForUser(mainWindowViewModel)
            { NoteId = note.Id }));
            var search = new ObservableCollection<NoteForUser>
                (NotesForUsers.Where(n => n.Creator.ToLower().Contains(search_parameter.ToLower())).ToList());
            NotesForUsers = search;
        }

        private RelayCommand openNoteCommand;
        public RelayCommand OpenNoteCommand
        {
            get
            {
                return openNoteCommand ??
                  (openNoteCommand = new RelayCommand(obj =>
                  {
                      mainWindowViewModel.NotePage(SelectedNoteForUser);
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
                      MessageBox.Show(NotesForUsers.Count().ToString());
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
