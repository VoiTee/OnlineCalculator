﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p1 = new Parser("-1*2,3+43,8+21+9,7-17");
            Console.WriteLine(p1.execute());
        }
    }
}