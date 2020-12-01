using System;
using System.Diagnostics;
using System.IO;

namespace CryptoSoft
{
    class Program
    {
        /// <summary>
        /// Manage the program sequence
        /// </summary>
        /// <param name="args">Arguments given by the user</param>
        static void Main(string[] args)
        {
            // Parsed arguments
            string[] parsed = {};

            // Check args and exit if wrong
            try
            {
                parsed = ParseArgs(args);
            }
            catch (FileNotFoundException)
            {
                Environment.Exit(-2);
            }
            catch (DirectoryNotFoundException)
            {
                Environment.Exit(-3);
            }

            // Encryption key
            byte[] key = {};

            // Try to get the key and exit if it's wrong
            try
            {
                key = GetKey(parsed[0]);
            }
            catch (ArgumentException)
            {
                Environment.Exit(-4);
            }

            // Ellasped time to return
            int time = 0;

            // Encrypt the file or exit if error
            try
            {
                time = Encrypt(parsed[1], parsed[2], key);
            }
            catch(Exception)
            {
                Environment.Exit(-1);
            }

            Environment.Exit(time);
        }

        /// <summary>
        /// Parse the arguments given by the user
        /// </summary>
        /// <param name="args">Raw arguments</param>
        /// <returns>
        /// Parsed arguments
        /// [0] = key, [1] = source, [2] = destination
        /// </returns>
        public static string[] ParseArgs(string[] args)
        {
            string key = "", source = "", destination = "";

            // Parses the args
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-s" || args[i] == "--source")
                {
                    source = args[i + 1];
                }

                if (args[i] == "-d" || args[i] == "--destination")
                {
                    destination = args[i + 1];
                }

                if (args[i] == "-k" || args[i] == "--key")
                {
                    key = args[i + 1];
                }
            }

            // Check if source file exists
            var sFileInfo = new FileInfo(source);

            if (!sFileInfo.Exists)
            {
                throw new FileNotFoundException();
            }

            // Check if destination directory exists
            var dFileInfo = new FileInfo(destination);
            var path = dFileInfo.Directory.FullName;

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }

            string[] str = {key, source, destination};
            return str;
        }

        /// <summary>
        /// Get the encryption key
        /// </summary>
        /// <param name="path">File where the key is stored</param>
        /// <returns>The key</returns>
        public static byte[] GetKey(string path)
        {
            var fileInfo = new FileInfo(path);

            // Check if key file exists
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException();
            }

            var key = File.ReadAllBytes(path);

            // Ensure the key is 64 bits lenght (8 characters)
            if (key.Length != 8)
            {
                throw new ArgumentException();
            }

            return key;
        }

        /// <summary>
        /// Perform the encryption
        /// </summary>
        /// <param name="source">File to encrypt file</param>
        /// <param name="destination">Result path</param>
        /// <param name="key">Encryption key</param>
        /// <returns>Time spent to encrypt</returns>
        public static int Encrypt(string source, string destination, byte[] key)
        {
            var watch = new Stopwatch();
            watch.Start();

            byte[] file = File.ReadAllBytes(source);
            byte[] crypt = new byte[file.Length];

            // Build crypted file
            for (int i = 0; i < file.Length; i++)
            {
                var j = i % 8;
                byte fByte = file[i];
                byte kByte = key[j];
                byte cByte = (byte)(fByte ^ kByte);

                crypt[i] = cByte;
            }

            File.WriteAllBytes(destination, crypt);

            watch.Stop();

            return (int)watch.ElapsedMilliseconds;
        }
    }
}
