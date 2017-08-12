using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    public static class SharedCode
    {
        public static readonly string PathOsu;

        static SharedCode()
        {
            //find osu! installation
            if (!Directory.Exists(PathOsu = $@"C:\Users\{Environment.UserName}\AppData\Local\osu!\")    //current install dir
                && !Directory.Exists(PathOsu = @"C:\Program Files (x86)\osu!\")     //old install dir (x64-based)
                && !Directory.Exists(PathOsu = @"C:\Program Files\osu!\")) {        //old install dir (x86-based)
                PathOsu = string.Empty; //if none of the previous paths exist
            }
            //TODO: dev to create file with custom location
        }

        public static void PreTestCheck()
        {
            if (string.IsNullOrEmpty(PathOsu))
                Assert.Inconclusive("osu! installation directory not found.");
        }

        public static string GetRelativeFile(string rel, bool shouldError = false)
        {
            string absolute = Path.Combine(PathOsu, rel);
            if (!File.Exists(absolute)) {
                if (shouldError)
                    Assert.Fail($"File does not exist: {absolute}");
                else
                    Assert.Inconclusive($"File does not exist: {absolute}");
            }
            return absolute;
        }

        public static string GetRelativeDirectory(string rel, bool shouldError = false)
        {
            string absolute = Path.Combine(PathOsu, rel);
            if (!Directory.Exists(absolute))
            {
                if (shouldError)
                    Assert.Fail($"Directory does not exist: {absolute}");
                else
                    Assert.Inconclusive($"Directory does not exist: {absolute}");
            }
            return absolute;
        }

        public static bool VerifyFileChecksum(string path, string md5)
        {
            string hash = string.Join(string.Empty, MD5.Create().ComputeHash(File.OpenRead(path)).Select(a => a.ToString("X2")));
            return hash == md5.ToUpper();
        }
    }
}
