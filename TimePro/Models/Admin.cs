namespace TimePro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        [StringLength(10)]
        public string AdminCode { get; set; }

        [Required]
        [StringLength(10)]
        public string AdminName { get; set; }

        [Required]
        [StringLength(10)]
        public string AdminPwd { get; set; }
    }
}
