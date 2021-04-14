using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTextValidation
{
    public static class Sanity
    {
        public static void Requires(bool valid, string message = "")
        {
            if (!valid)
                throw new MyException(message);
        }
    }

    public class MyException : Exception
    {
        public MyException() : base() { }
        public MyException(string message) : base(message) { }
    }
}
