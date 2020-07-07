using EmberKernel.Services.EventBus;
using System.Collections.Generic;

namespace ExamplePlugin.Models.EventSubscription
{
    public class GameStatistic
    {

        public double Accuracy { get; set; }
        public double HP { get; set; }
        /// <summary>
        /// Current score
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// Current combo
        /// </summary>
        public int Combo { get; set; }
        /// <summary>
        /// miss
        /// </summary>
        public int Missing { get; set; }
        /// <summary>
        /// 300
        /// </summary>
        public int Best { get; set; }
        /// <summary>
        /// 100
        /// </summary>
        public int Good { get; set; }
        /// <summary>
        /// 50
        /// </summary>
        public int Bad { get; set; }
        /// <summary>
        /// geki(osu!)/ 300g(mania)
        /// </summary>
        public int Geki { get; set; }
        /// <summary>
        /// katu(osu!)/ 200(mania)
        /// </summary>
        public int Katu { get; set; }
    }

    [EventNamespace("MemoryReader")]
    public class PlayingInfo : Event<PlayingInfo>
    {
        public bool HasValue { get; set; }
        public int RawModInfo { get; set; }
        public int RawListeningModInfo { get; set; }
        public string CurrentPlayerName { get; set; }
        public int PlayingTime { get; set; }
        public List<int> RawUnstableRate { get; set; }
        public GameStatistic GameStatistic { get; set; }
    }

}
