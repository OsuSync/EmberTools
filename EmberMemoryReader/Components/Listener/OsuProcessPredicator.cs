﻿using Microsoft.Extensions.Logging;
using OsuUtils.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Listener
{
    public class OsuProcessPredicator : IProcessPredicator<OsuProcessMatchedEvent>
    {
        public string WindowsPathStrip(string entry)
        {
            StringBuilder builder = new StringBuilder(entry);
            foreach (char c in Path.GetInvalidFileNameChars())
                builder.Replace(c.ToString(), string.Empty);
            builder.Replace(".", string.Empty);
            return builder.ToString();
        }

        public string FilterProcessName => "osu!";

        /// <summary>
        /// <para>When obtain a process named 'osu!'</para>
        /// <para>Search 'songs' and try to get player username</para>
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public OsuProcessMatchedEvent MatchProcess(Process process)
        {
            Console.WriteLine("Start find osu! process");
            var osuFolder = Path.GetDirectoryName(process.MainModule.FileName);
            var configPath = Path.Combine(osuFolder, $"osu!.{WindowsPathStrip(Environment.UserName)}.cfg");
            if (!File.Exists(configPath)) return null;

            var config = new OsuProfileConfiguration(configPath);
            // Exist 'song folder'
            var beatmapFolder = Path.Combine(osuFolder, string.IsNullOrWhiteSpace(config.BeatmapDirectory) ? "Songs" : config.BeatmapDirectory);
            if (!Directory.Exists(beatmapFolder)) return null;

            return new OsuProcessMatchedEvent()
            {
                BeatmapDirectory = beatmapFolder,
                GameDirectory = osuFolder,
                LatestVersion = config.LastVersion,
                ProcessId = process.Id,
                UserName = config.UserName,
            };
        }
    }
}