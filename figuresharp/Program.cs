using System;

namespace figuresharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Rectangle a = new Rectangle(0, 4, 2, 6);
            Console.WriteLine(a.Area());
            Triangle b = new Triangle(0, 0, 0, 6, 10, 6);
            Console.WriteLine(a.IsInclude(b));
            Console.WriteLine(b.IsInclude(a));
            Triangle c = new Triangle(0, 0, 0, 6, 10, 8);
            Console.WriteLine(b.IsIntersect(c));
            Triangle d = new Triangle(-1, -1, -1, -6, -10, -8);
            Console.WriteLine(b.IsIntersect(d));

        }
    }
}
