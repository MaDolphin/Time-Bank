namespace TimePro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Chat = new HashSet<Chat>();
            Chat1 = new HashSet<Chat>();
            Note = new HashSet<Note>();
            Note1 = new HashSet<Note>();
        }

        public int UserID { get; set; }

        [Required]
        [StringLength(10)]
        public string UserCode { get; set; }

        [Required]
        [StringLength(10)]
        public string UserName { get; set; }

        [Column(TypeName = "image")]
        public byte[] UserPhoto { get; set; }

        public int? UserAge { get; set; }

        [StringLength(10)]
        public string UserSex { get; set; }

        [Required]
        public string UserPwd { get; set; }

        [StringLength(10)]
        public string UserStatus { get; set; }

        [StringLength(10)]
        public string UserTel { get; set; }

        [Required]
        [StringLength(10)]
        public string UserEmail { get; set; }

        [StringLength(10)]
        public string UserAddress { get; set; }

        [StringLength(10)]
        public string UserHobby { get; set; }

        public DateTime? UserRegisterTime { get; set; }

        public int? UserRole { get; set; }

        [StringLength(50)]
        public string ImageType { get; set; }

        public int? UserTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chat1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Note> Note { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Note> Note1 { get; set; }
    }
}
