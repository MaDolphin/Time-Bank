namespace TimePro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Note")]
    public partial class Note
    {
        public int NoteID { get; set; }

        [Required]
        public string NoteTitle { get; set; }

        public string NoteLabel { get; set; }

        public int? UserID { get; set; }

        public int? NoteTime { get; set; }

        [Column(TypeName = "ntext")]
        public string NoteContent { get; set; }

        public int? NoteGetNumber { get; set; }

        public DateTime? NoteCreateTime { get; set; }

        public int? NoteFlag { get; set; }

        public DateTime? NoteFinishTime { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
