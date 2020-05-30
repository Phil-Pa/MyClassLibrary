using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Math
{
    public static class Logic
    {

        public static bool Implication(bool a, bool b)
        {
            if (!a)
                return true;

            return !a || b;
        }

    }
}
