using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.classes
{
    class Expence
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
