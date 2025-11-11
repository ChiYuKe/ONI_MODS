using System;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace WireAnywhere
{

    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("https://steamcommunity.com/sharedfiles/filedetails/?id=2923332049", null, false)]
    [RestartRequired]
    public class Config : SingletonOptions<Config>
    {
       
        [Option("STRINGS.OPTIONS.WIRE_REFINED_HIGH_WATTAGE_BASE_DECOR", "", null, Format = "F0")]
        [Limit(-20.0, 200.0)]
        [JsonProperty]
        public float WireRefinedHighWattage_BaseDecor { get; set; }

        [Option("STRINGS.OPTIONS.WIRE_REFINED_HIGH_WATTAGE_BASE_DECOR_RADIUS", "", null, Format = "F0")]
        [Limit(4.0, 20.0)]
        [JsonProperty]
        public float WireRefinedHighWattage_BaseDecorRadius { get; set; }

        [Option("STRINGS.OPTIONS.WIRE_HIGH_WATTAGE_BASE_DECOR", "", null, Format = "F0")]
        [Limit(-20.0, 200.0)]
        [JsonProperty]
        public float WireHighWattage_BaseDecor { get; set; }

        [Option("STRINGS.OPTIONS.WIRE_HIGH_WATTAGE_BASE_DECOR_RADIUS","", null, Format = "F0")]
        [Limit(6.0, 20.0)]
        [JsonProperty]
        public float WireHighWattage_BaseDecorRadius { get; set; }

        public Config()
        {
            // 默认值设置
            this.WireRefinedHighWattage_BaseDecor = -20f;
            this.WireRefinedHighWattage_BaseDecorRadius = 4f;
            this.WireHighWattage_BaseDecor = -25f;
            this.WireHighWattage_BaseDecorRadius = 6f;
        }
    }

}
