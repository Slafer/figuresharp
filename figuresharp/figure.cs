using System;
using System.Collections.Generic;
using System.Text;

//Создать графический интерфейс для решения СЛАУ
// 1 вариант: система 3х3, организовать ввод коэффициентов, решить систему, навести красоту в окне, система неоднородная
// 2 вариант: то же самое, но для заданного кол-ва nxn уравнений и неизвестных (DataGridView для матриц)
//
//
namespace figuresharp
{
    abstract class Figure
    {
        protected double[,] dot;

        static bool LineProjIntersection(double a, double b, double c, double d)
        {
            if (a > b)
            {
                double tmp = a;
                a = b;
                b = tmp;
            }
            if (c > d)
            {
                double tmp = c;
                c = d;
                d = tmp;
            }
            return Math.Max(a, b) <= Math.Max(c, d);
        }
        static bool LineIntersection(double[,] dots)
        {
            Triangle tr1 = new Triangle(dots[0, 0], dots[0, 1], dots[1, 0], dots[1, 1], dots[2, 0], dots[2, 1]);
            Triangle tr2 = new Triangle(dots[0, 0], dots[0, 1], dots[1, 0], dots[1, 1], dots[3, 0], dots[3, 1]);
            Triangle tr3 = new Triangle(dots[2, 0], dots[2, 1], dots[3, 0], dots[3, 1], dots[0, 0], dots[0, 1]);
            Triangle tr4 = new Triangle(dots[2, 0], dots[2, 1], dots[3, 0], dots[3, 1], dots[1, 0], dots[1, 1]);
            return LineProjIntersection(dots[0, 0], dots[1, 0], dots[2, 0], dots[3, 0]) && LineProjIntersection(dots[0, 1], dots[1, 1], dots[2, 1], dots[3, 1])
                && ((tr1.SignArea() * tr2.SignArea() <= 0) && (tr3.SignArea() * tr4.SignArea() <= 0));
        }
        public abstract void Move(double dx, double dy);
        public abstract double Area();

        public int GetPtCt()
        {
            return dot.GetLength(0);
        }
        public bool IsIntersect(Figure other)
        {
            for (int i = 0; i < dot.GetLength(0)-1; i++)
                for (int j = 0; j < other.dot.GetLength(0)-1; j++)
                {
                    double[,] dots = new double[4, 2]
                    {
                        {dot[i,0],dot[i,1]},
                        {dot[i+1,0],dot[i+1,1]},
                        {other.dot[j,0],other.dot[j,1]},
                        {other.dot[j+1,0],other.dot[j+1,1]}
                    };
                    if (LineIntersection(dots)) return true;
                }
            return false;
        }
        public abstract bool IsInside(double x, double y);
        public bool IsInclude(Figure other)
        {
            for (int i = 0; i < other.dot.GetLength(0); i++)
            {
                if (!IsInside(other.dot[i, 0], other.dot[i, 1])) return false;
            }
            return true;
        }

        //public abstract int Compare(Figure other);
        public int Compare(Figure other)
        {
            if (Area() == other.Area()) return 0;
            else
            if (Area() > other.Area()) return 1;
            else return -1;
        }
    }
    class Rectangle : Figure
    {
 //       protected double[,] dot = new double[2,2];

        public Rectangle(double[,] dots)
        {
            if (!(dots.GetLength(0) == 2 && dots.GetLength(1) == 2)) throw new Exception("Неправильные размеры массива для создания прямоугольника");
            dot = dots;
        }
        public Rectangle(double x1, double y1, double x2, double y2)
        {
            dot = new double[2, 2]
            {
                {x1,y1},
                {x2,y2}
            };
        }
        public override void Move(double dx, double dy)
        {
            for (int i = 0; i < 2; i++)
            {
                dot[i, 0] += dx;
                dot[i, 1] += dy;
            }
        }
        public override double Area()
        {
            return Math.Abs((dot[0,0]-dot[1,0])*(dot[0,1]-dot[1,1]));
        }
        public override bool IsInside(double x, double y)
        {
            double xmin = Math.Min(dot[0, 0], dot[1, 0]);
            double xmax = Math.Max(dot[0, 0], dot[1, 0]);
            double ymin = Math.Min(dot[0, 1], dot[1, 1]);
            double ymax = Math.Max(dot[0, 1], dot[1, 1]);
            if (x >= xmin && x <= xmax && y <= ymax && y >= ymin) return true;
            return false;
        }
    }
    class Triangle : Figure
    {
 //       protected double[,] dot = new double[3,2];

        public Triangle(double[,] dots)
        {
            if (!(dots.GetLength(0) == 3 && dots.GetLength(1) == 2)) throw new Exception("Неправильный размер массива для того, чтобы создать треугольник");
            dot = dots;
        }
        public Triangle(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            dot = new double[3,2]{ 
                {x1,y1 },
                { x2,y2 },
                { x3,y3 } };
        }
        private double line_length(int d1, int d2)
        {
            return Math.Sqrt((dot[d2, 0] - dot[d1, 0]) * (dot[d2, 0] - dot[d1, 0]) + (dot[d2, 1] - dot[d1, 1]) * (dot[d2, 1] - dot[d1, 1]));
        }
        public double SignArea()
        {
            return (dot[1, 0] - dot[0, 0]) * (dot[2, 1] - dot[0, 1]) - (dot[1, 1] - dot[0, 1]) * (dot[2, 0] - dot[0, 0]);
        }
        public override double Area()
        {
            double a = line_length(0, 1);
            double b = line_length(1, 2);
            double c = line_length(2, 0);
            double p = (a + b + c) / 2;
            return Math.Sqrt(p*(p-a)*(p-b)*(p-c));
        }
        public override bool IsInside(double x, double y)
        {
            double eps = 0.00001;
            Triangle tr1 = new Triangle(dot[0, 0], dot[0, 1], dot[1, 0], dot[1, 1], x, y);
            Triangle tr2 = new Triangle(dot[1, 0], dot[1, 1], dot[2, 0], dot[2, 1], x, y);
            Triangle tr3 = new Triangle(dot[0, 0], dot[0, 1], dot[2, 0], dot[2, 1], x, y);
            if (Math.Abs(Area() - (tr1.Area() + tr2.Area() + tr3.Area())) < eps) return true;
            return false;
        }
        public override void Move(double dx, double dy)
        {
            for (int i = 0; i < 3; i++)
            {
                dot[i, 0] += dx;
                dot[i, 1] += dy;
            }
        }
    }
}
