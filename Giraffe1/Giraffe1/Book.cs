using System;
using System.Collections.Generic;
using System.Text;

namespace Giraffe1
{
    class Book
    {
        public string title;
        public string name;
        private int pages;

        public Book(string title,string name,int pages)
        {
            this.title = title;
            this.name = name;
            Pages = pages;
        }

        public int Pages
        {
            get { return pages; }
            set
            {
                pages = value;
            }
        }
    }
}
