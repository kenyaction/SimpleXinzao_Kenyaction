using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK.Events;

namespace TestEloBuddy
{
    class Program
    {
        static void Main(string[] args)
        {
            Addon a = new Addon();
            Loading.OnLoadingComplete += a.start;
        }
    }
}
