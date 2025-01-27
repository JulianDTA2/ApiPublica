using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPJApiPublica.Models
{
    public class Car
    {
        public int Id { get; set; } // ID para SQLite
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
    }

}
