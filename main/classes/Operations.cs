using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.classes
{
    class Operations
    {
        public static bool IsNumericInput(string input)
        {
            return int.TryParse(input, out _); // Check if the input can be parsed as an integer
        }
    }
}
