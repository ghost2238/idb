using lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace html
{
    class Program
    {

        static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            while(true)
            {
                if (File.Exists(dir + "/template.html"))
                    break;
                dir = Path.GetFullPath($"{dir}/../");
            }

            var idb = new IDBLib(dir);
            var s = new List<string>();
            var names = idb.GetNames($"{dir}/f2_res.idb").ToList();
            foreach (var n in names)
                s.Add($"<tr><td>{n.ToHex()}</td><td>{n.name}</td></tr>");
            var template = File.ReadAllText($"{dir}/template.html");
            File.WriteAllText($"{dir}/index.html", template.Replace("%ROWS%", string.Join("\n", s))); 
        }
    }
}
