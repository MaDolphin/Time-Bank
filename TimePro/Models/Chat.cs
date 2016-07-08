namespace TimePro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chat")]
    public partial class Chat
    {
        public int ChatID { get; set; }

        public int ChatFrom { get; set; }

        public int ChatTo { get; set; }

        public DateTime ChatTime { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string ChatContent { get; set; }

        public int? ChatFlag { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
