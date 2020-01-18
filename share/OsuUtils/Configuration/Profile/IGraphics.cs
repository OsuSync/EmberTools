using System;
using System.Collections.Generic;
using System.Text;

namespace OsuUtils.Configuration.Profile
{
    public enum FrameSyncMode
    {
        VSync,
        PowerSaving,
        Optimal,
        Unlimited,
    }

    public interface IGraphics
    {
        public FrameSyncMode FrameSync { get; }
        public bool FpsCounter { get; }
        /// <summary>
        /// Compatibility mode
        /// </summary>
        public bool CompatibilityContext { get; }
        /// <summary>
        /// ForceFrameFlush
        /// </summary>
        public bool ForceFrameFlush { get; }
        public bool DetectPerformanceIssues { get; }
        public int Height { get; }
        public int Width { get; }
        public bool Fullscreen { get; }
    }
}
