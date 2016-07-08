namespace TimePro.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TimeProContext : DbContext
    {
        public TimeProContext()
            : base("name=TimeProContext")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.AdminCode)
                .IsFixedLength();

            modelBuilder.Entity<Admin>()
                .Property(e => e.AdminName)
                .IsFixedLength();

            modelBuilder.Entity<Admin>()
                .Property(e => e.AdminPwd)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserCode)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserSex)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserStatus)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserTel)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserEmail)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserAddress)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.UserHobby)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Chat)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.ChatFrom)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Chat1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.ChatFrom)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Note)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Note1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.NoteGetNumber);
        }
    }
}
