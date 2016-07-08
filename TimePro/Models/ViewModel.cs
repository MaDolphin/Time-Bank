using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimePro.Models
{
    public class ViewModel
    {
        public IEnumerable<Note> NoteModel { get; set; }
        public IEnumerable<Chat> ChatModel { get; set; }
        public User UserModel { get; set; }
        public int temp { set; get; }
    }

    public class ViewModel1
    {
        public Note NoteModel { get; set; }
        public User UserModel { get; set; }
        public Chat ChatModel { get; set; }
    }
}