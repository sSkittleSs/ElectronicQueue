using ElectronicQueue.DataBase.Models;
using System.Data.Entity;

namespace ElectronicQueue.DataBase
{
    class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection") { }

        public DbSet<Adres> Adreses { set; get; }
        public DbSet<Cabinet> Cabinets { set; get; }
        public DbSet<Doctor> Doctors { set; get; }
        public DbSet<Polyclinic> Polyclinics { set; get; }
        public DbSet<Role> Roles { set; get; }
        public DbSet<Speciality> Specialities { set; get; }
        public DbSet<Ticket> Tickets { set; get; }
        public DbSet<User> Users { set; get; }
    }
}
