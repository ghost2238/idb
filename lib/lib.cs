using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace lib
{
    public class Symbol
    {
        public int address;
        public string name;
        public string ToHex() => "0x"+address.ToString("X");
    }

    public class IDBLib
    {
        string bin;

        public IDBLib(string dir)
        {
            bin = Path.Combine(dir, "idbtool.exe");
        }

        public IEnumerable<Symbol> GetNames(string db)
        {
            var proc = RunArgs($"--names {db}");
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (line.IndexOf(':') == -1 || line.Contains("==>"))
                    continue;

                var spl = line.Split(':');

                yield return new Symbol
                {
                    address = Convert.ToInt32(spl[0], 16),
                    name = spl[1]
                };
            }
        }

        public void GetEnums(string db)
        {
            var proc = RunArgs($"--enums {db}");
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
        }

        private Process RunArgs(string args)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = bin,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            return proc;
        }
    }
}
