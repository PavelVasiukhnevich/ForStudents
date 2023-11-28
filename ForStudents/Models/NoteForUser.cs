using ForStudents.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForStudents.Models
{
    public class NoteForUser : INotifyPropertyChanged
    {
        private PhotoLikesContext photoLikesContext;
        private MainWindowViewModel mainViewModel;
        private int noteId;
        public int NoteId
        {
            get { return noteId; } set { noteId = value; OnPropertyChanged("NoteId"); }
        }
        public Note Note1 => photoLikesContext.Notes.FirstOrDefault(n=>n.Id== NoteId);
        public string IsLiked
        {
            get { if (mainViewModel.CurrentUser != null)
                    return photoLikesContext.NotesUsers.Where(nu => nu.IsLiked && nu.Note.Id == NoteId &&
                    nu.User.Id == mainViewModel.CurrentUser.Id).ToList().Count() > 0 ? "Red" : "White";
                else return "White";
            }
            set
            {
                OnPropertyChanged("IsLiked");
            }
        }

        private string likeZise;
        public string LikeSize
        {
            get { return likeZise; }
            set
            {
                likeZise = value;
                OnPropertyChanged("LikeSize");
            }
        }

        public void LikeSizeSwapper()
        {
            if (likeZise == "30") 
            {
            LikeSize = "33";
            }
            else
            {
                LikeSize = "30";
            }
        }
        public string Creator
        { 
            get 
            {
                if (photoLikesContext.NotesUsers.Where(nu => nu.Note.Id == NoteId && nu.IsCreater == true).Count() > 0)
                {
                    return photoLikesContext.Users.FirstOrDefault(u=>u.Id==photoLikesContext.NotesUsers.FirstOrDefault
                        (nu => nu.Note.Id == NoteId && nu.IsCreater == true).User.Id).Name;
                }
                else
                {
                    return "Not find";
                }
            }        
            set
            {
                OnPropertyChanged("Creator");
            }
        }
        public int LikesCount
        {
            get { return photoLikesContext.NotesUsers.Where(nu => nu.IsLiked && nu.Note.Id == NoteId).Count(); }
            set { OnPropertyChanged("LikesCount"); }
        }

        public NoteForUser(MainWindowViewModel mainViewModel)
        {
            LikeSize = "30";
            photoLikesContext = new PhotoLikesContext();
            this.mainViewModel = mainViewModel;
        }

        private RelayCommand likeItCommand;
        public RelayCommand LikeItCommand
        {
            get
            {
                return likeItCommand ??
                  (likeItCommand = new RelayCommand(obj =>
                  {
                      if (IsLiked == "White")
                      {
                          if (photoLikesContext.NotesUsers.Where(nu => nu.Note.Id == NoteId &&
                          nu.User.Id == mainViewModel.CurrentUser.Id).Count() == 0)
                          {
                              photoLikesContext.NotesUsers.Add(new NoteUser()
                              {
                                  IsCreater = false,
                                  IsLiked = true,
                                  Note = photoLikesContext.Notes.FirstOrDefault(n => n.Id == NoteId),
                                  User = photoLikesContext.Users.FirstOrDefault(u => u.Id == mainViewModel.CurrentUser.Id)
                              });
                          }
                          else
                          {
                              photoLikesContext.NotesUsers.FirstOrDefault(nu => nu.User.Id == mainViewModel.CurrentUser.Id &&
                              nu.Note.Id == NoteId).IsLiked = true;
                          }
                      }
                      else
                      {
                          photoLikesContext.NotesUsers.FirstOrDefault(nu => nu.User.Id == mainViewModel.CurrentUser.Id &&
                              nu.Note.Id == NoteId).IsLiked = false;
                      }
                      photoLikesContext.SaveChanges();
                      IsLiked = "";
                      LikesCount = 0;
                  }));
            }
        }

        private RelayCommand likeMouseCommand;
        public RelayCommand LikeMouseCommand
        {
            get
            {
                return likeMouseCommand ??
                  (likeMouseCommand = new RelayCommand(obj =>
                  {
                      LikeSizeSwapper();
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
