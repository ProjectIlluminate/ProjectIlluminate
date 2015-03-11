using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Illuminate_UI
{
   public  class User
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public float Height { get; set; }
    }
   public class Entities : DbContext
   {
       public DbSet<User> Users { get; set; }
       //public Entities() : base("Entities") { }
   }
}
