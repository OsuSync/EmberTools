using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;

namespace OsuUtils.Configuration.Profile
{
    public class RawConfiguration
    {
        public string Path { get; }
        public bool ReadPassword { get; }
        public SecureString Password { get; } = new SecureString();
        private readonly Dictionary<string, string> config = new Dictionary<string, string>(250);
        private void Read()
        {
            using var stream = File.OpenRead(Path);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length == 0 || line[0] == '#') continue;

                var equalIndex = line.IndexOf('=');
                var valueLength = line.Length - equalIndex - 1;
                var key = line.Substring(0, equalIndex - 1).Trim();
                var value = line.Substring(equalIndex + 1, valueLength).Trim();
                if (key.ToLower() == "password")
                {
                    if (!ReadPassword) { continue; }
                    foreach (var chr in value) Password.AppendChar(chr);
                }
                else
                {
                    config.Add(key, value);
                }
            }

        }

        public string this[string index]
        {
            get => config[index];
        }

        public RawConfiguration(string path, bool readPassword)
        {
            this.Path = path;
            this.ReadPassword = readPassword;
            Read();
        }
    }
}
