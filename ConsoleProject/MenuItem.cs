using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject
{
    public class MenuItem
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public Action Selected { get; set; }

        public void Display(int counter)
        {
            Console.WriteLine($"[" + counter + "] " + Name);
        }
    }
}
