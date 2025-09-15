using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Klei.AI;

namespace TAccessories
{
    internal class KDb
    {
        public static void Init(Db db)
        {
            // KTAccessories.Register(db.Accessories, db.AccessorySlots);
            KDb.wet = db.effects.Get("SoakingWet");
            KDb.wetFeet = db.effects.Get("WetFeet");
            List<Tuple<Tag, string>> list = new List<Tuple<Tag, string>>();
            foreach (KeyValuePair<Tag, string> keyValuePair in KDb.beverages)
            {
                list.Add(new Tuple<Tag, string>(keyValuePair.Key, keyValuePair.Value));
            }
            WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS = WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.AddRangeToArray(list.ToArray()).ToArray<Tuple<Tag, string>>();
        }


        public const string WET = "SoakingWet";
        public const string WETFEET = "WetFeet";
        public static Effect wet;
        public static Effect wetFeet;
        public static Dictionary<Tag, string> beverages = new Dictionary<Tag, string> {
        {
           SimHashes.Gold.CreateTag(),
            "LuminescenceKingA"
        } };



    }
}
