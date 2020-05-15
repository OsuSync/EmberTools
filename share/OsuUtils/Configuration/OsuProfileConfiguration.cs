using OsuUtils.Configuration.Profile;
using System;
using System.IO;
using System.Security;

namespace OsuUtils.Configuration
{
    public class OsuProfileConfiguration : IGraphics, IGeneral, IMisc, ISkins
    {
        private readonly RawConfiguration raw;
        /// <summary>
        /// Read osu! profile configuration
        /// </summary>
        /// <param name="path">the configuration file path</param>
        /// <param name="readPassword">should reader read the password information</param>
        /// <exception cref="FileNotFoundException"></exception>
        public OsuProfileConfiguration(string path, bool readPassword = false)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            raw = new RawConfiguration(path, readPassword);
        }

        public FrameSyncMode FrameSync => Enum.Parse<FrameSyncMode>(raw["FrameSync"]);

        public bool FpsCounter => raw["FpsCounter"] == "1";

        public bool CompatibilityContext => raw["CompatibilityContext"] == "1";

        public bool ForceFrameFlush => raw["ForceFrameFlush"] == "1";

        public bool DetectPerformanceIssues => raw["DetectPerformanceIssues"] == "1";

        public int Height => int.Parse(raw["Height"]);

        public int Width => int.Parse(raw["Width"]);

        public bool Fullscreen => raw["Fullscreen"] == "1";

        public string UserName => raw["Username"];

        public SecureString Password => raw.Password;

        public string Language => raw["Language"];

        public string BeatmapDirectory => raw["BeatmapDirectory"];

        public string LastVersion => raw["LastVersion"];

        public string ChatChannels => raw["ChatChannels"];

        public string Skin => raw["Skin"];
    }
}
