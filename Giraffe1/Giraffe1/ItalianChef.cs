using System;
using System.Collections.Generic;
using System.Text;

namespace Giraffe1
{
    class ItalianChef : Chef
    {
        public void MakePasta()
        {
            Console.WriteLine("Making pasta");
        }

        public override void MakeSpecialDish()
        {
            Console.WriteLine("Making bbq salad");
        }
    }
}
