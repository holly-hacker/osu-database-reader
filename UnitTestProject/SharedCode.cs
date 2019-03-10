using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

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
            //TODO: allow dev to create file with custom location
        }

        public static void PreTestCheck()
        {
            Skip.If(string.IsNullOrEmpty(PathOsu), "osu! installation directory not found.");
        }

        public static string GetRelativeFile(string rel, bool shouldError = false)
        {
            string absolute = Path.Combine(PathOsu, rel);
            if (shouldError)
                Assert.True(File.Exists(absolute), $"File does not exist: {absolute}");
            else
                Skip.IfNot(File.Exists(absolute), $"File does not exist: {absolute}");
            return absolute;
        }

        public static string GetRelativeDirectory(string rel, bool shouldError = false)
        {
            string absolute = Path.Combine(PathOsu, rel);
            if (shouldError)
                Assert.True(Directory.Exists(absolute), $"Directory does not exist: {absolute}");
            else
                Skip.IfNot(Directory.Exists(absolute), $"Directory does not exist: {absolute}");
            return absolute;
        }

        public static bool VerifyFileChecksum(string path, string md5)
        {
            string hash = string.Join(string.Empty, MD5.Create().ComputeHash(File.OpenRead(path)).Select(a => a.ToString("X2")));
            return hash == md5.ToUpper();
        }
    }
}
