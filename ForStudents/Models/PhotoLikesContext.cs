using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForStudents.Models
{
    public class PhotoLikesContext:DbContext
    {
        public PhotoLikesContext() : base("dbContext")
        { }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NoteUser> NotesUsers { get; set; }
    }
}
