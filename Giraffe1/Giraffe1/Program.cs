using System;

namespace Giraffe1
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Tom Marvollo Riddle";
            int age = 20;
            Console.WriteLine(name+" is of "+age+" years old");
            Console.WriteLine(name.Length);
            Console.WriteLine(name.ToUpper());
            Console.WriteLine(name.Contains("voll"));
            Console.WriteLine(name.Contains("volley"));
            Console.WriteLine(name[0]);
            Console.WriteLine(name.IndexOf("Marvollo"));
            Console.WriteLine(name.Substring(8,3));


            Console.Write("Enter a name:");
            name = Console.ReadLine();
            Console.WriteLine("Hello " + name);


            int num = Convert.ToInt32("45");
            Console.WriteLine(num + 6);

            int[] luckyNumbers = { 4, 5, 3, 7, 8 };
            string[] friends = new string[5];

            friends[0] = "Tony";
            friends[1] = "Steve";
            friends[2] = "Bruce";

            Console.WriteLine(friends.Length);
            Console.WriteLine(luckyNumbers[2]);

            SayHi(name);

            Console.WriteLine(cube(5));

            bool isMale = false;
            bool isTall = true;

            if (isMale && isTall)
            {
                Console.WriteLine("You are tall male.");
            } else if(isMale && !isTall)
            {
                Console.WriteLine("You are short male.");
            } else if (!isMale && isTall)
            {
                Console.WriteLine("You are short but not male.");
            }
            else
            {
                Console.WriteLine("You are not tall and not male.");
            }

            Console.WriteLine(GetMax(2, 10, 87));


            Console.WriteLine("----Building a calculator----");
            Console.Write("Enter first number : ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter a operator : ");
            string op = Console.ReadLine();

            Console.Write("Enter second number : ");
            double num2 = Convert.ToDouble(Console.ReadLine());

            if (op == "+")
            {
                Console.WriteLine(num1 + num2);
            }else if (op == "-")
            {
                Console.WriteLine(num1 - num2);
            }else if (op == "*")
            {
                Console.WriteLine(num1 * num2);
            }else if (op == "/")
            {
                Console.WriteLine(num1 / num2);
            }
            else
            {
                Console.WriteLine("Invalid Operator");
            }

            Console.WriteLine(GetDay(3));



            //Create a 2d array
            int[,] arr = new int[2, 2];
            int[,] arr1 ={ 
                { 1,2},
                { 3,4} };

            Console.WriteLine(arr1[1, 0]);


            //Exception Handling
            try
            {
                Console.Write("Enter first number : ");
                int num3 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter second number : ");
                int num4 = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine(num3 / num4);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Will be executed always");
            }

            //Classes and Objects
            Book book = new Book("Harry Potter", "JK Rowling", 400);

            Console.WriteLine(book.Pages);


            //Inheritance
            Chef c = new Chef();
            c.MakeSpecialDish();

            ItalianChef chef = new ItalianChef();
            chef.MakeChicken();
            chef.MakePasta();
            chef.MakeSpecialDish();


        }

        static void SayHi(string name)
        {
            Console.WriteLine("Hi "+name);
        }

        static int cube(int n)
        {
            int result = n * n * n;
            return result;
        }

        static int GetMax(int n1,int n2,int n3)
        {
            if (n1 > n2 && n1 > n3)
                return n1;
            else if (n2 > n1 && n2 > n3)
                return n2;

            return n3;
        }

        static string GetDay(int dayNum)
        {
            string day;
            switch (dayNum)
            {
                case 0:
                    day = "Sunday";
                    break;
                case 1:
                    day = "Monday";
                    break;
                case 2:
                    day = "Tuesday";
                    break;
                case 3:
                    day = "Wednesday";
                    break;
                case 4:
                    day = "Thursday";
                    break;
                case 5:
                    day = "Friday";
                    break;
                case 6:
                    day = "Saturday";
                    break;
                default:
                    day = "Invald day number";
                    break;
            }

            return day;
        }
    }
}
