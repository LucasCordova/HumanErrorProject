using System.IO;
using System.IO.Compression;
using System.Linq;

namespace HumanErrorProject.Engine.Utilities
{
    public static class EngineFileUtilities
    {
        public static string ExtractZip(string root, string name, byte[] files)
        {
            var folder = Path.Combine(root, name);
            var zip = $"{folder}.zip";
            File.WriteAllBytes(zip, files);
            ZipFile.ExtractToDirectory(zip, folder);
            File.Delete(zip);
            return folder;
        }

        public static bool SameFile(string expected, string actual)
        {
            return new FileInfo(expected).Length == new FileInfo(actual).Length
                && File.ReadAllBytes(expected).SequenceEqual(File.ReadAllBytes(actual));
        }

        public static byte[] ZipFiles(string folder)
        {
            var zip = $"{folder}.zip";
            ZipFile.CreateFromDirectory(folder, zip);
            var bytes = File.ReadAllBytes(zip);
            File.Delete(zip);
            return bytes;
        }

    }
}
