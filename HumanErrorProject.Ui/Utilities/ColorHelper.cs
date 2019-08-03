using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.Utilities
{
    public class ColorHelper
    {
        public static Random Random = new Random();

        public string RandomColor()
        {
            return "#" + Random.Next(255).ToString("X2") + Random.Next(255).ToString("X2") + Random.Next(255).ToString("X2");
        }
    }
}
