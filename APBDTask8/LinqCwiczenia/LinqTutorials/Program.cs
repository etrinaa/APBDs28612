using System;

namespace LinqTutorials
{
    class Program
    {
        static void Main(string[] args)
        {


            foreach (var emp in LinqTasks.Task1())
            {
                Console.WriteLine(emp);
            }



            foreach (var emp in LinqTasks.Task2())
            {
                Console.WriteLine(emp);
            }



            LinqTasks.ShowTask3Result();


            foreach (var emp in LinqTasks.Task4())
            {
                Console.WriteLine(emp);
            }



            foreach (var obj in LinqTasks.Task5())
            {
                Console.WriteLine(obj);
            }



            foreach (var item in LinqTasks.Task6())
            {
                Console.WriteLine(item);
            }



            foreach (var item in LinqTasks.Task7())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(LinqTasks.Task8());



            var recentFrontend = LinqTasks.Task9();
            if (recentFrontend != null)
            {
                Console.WriteLine(recentFrontend);
            }
            else
            {
                Console.WriteLine("No frontend programmers found.");
            }



            foreach (var item in LinqTasks.Task10())
            {
                Console.WriteLine(item);
            }


            foreach (var item in LinqTasks.Task11())
            {
                Console.WriteLine(item);
            }


            foreach (var emp in LinqTasks.Task12())
            {
                Console.WriteLine(emp);
            }

            var numbers = new int[] { 1, 1, 2, 2, 2 };
            Console.WriteLine(LinqTasks.Task13(numbers));



            foreach (var dept in LinqTasks.Task14())
            {
                Console.WriteLine(dept);
            }


            foreach (var item in LinqTasks.Task15())
            {
                Console.WriteLine(item);
            }


            foreach (var item in LinqTasks.Task16())
            {
                Console.WriteLine($"Employee: {item.emp.Ename}, Department: {item.dept.Dname}");
            }

        }
    }
}