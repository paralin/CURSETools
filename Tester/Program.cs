using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CURSETools.Mod;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var mod = Parser.FromUrl("http://www.curse.com/ksp-mods/kerbal/220221-mechjeb");
            var json = JObject.FromObject(mod);
            var jstring = json.ToString(Formatting.None);
        }
    }
}
