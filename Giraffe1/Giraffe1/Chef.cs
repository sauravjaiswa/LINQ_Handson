using System;
using System.Collections.Generic;
using System.Text;

namespace Giraffe1
{
    class Chef
    {
        public void MakeChicken()
        {
            Console.WriteLine("Making chicken");
        }

        public void MakeSalad()
        {
            Console.WriteLine("Making salad");
        }

        public virtual void MakeSpecialDish()
        {
            Console.WriteLine("Making bbq chicken");
        }
    }
}
