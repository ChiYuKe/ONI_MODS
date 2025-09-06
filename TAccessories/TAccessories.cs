using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using UnityEngine;

namespace TAccessories
{
    public class KTAccessories
    {
        public static void Register(Accessories accessories, AccessorySlots slots)
        {
            KAnimFile anim = Assets.GetAnim("aete_hulk_head_kanim");
            Assets.GetAnim("aete_hulk_body_kanim");
            KTAccessories.AddAccessories(anim, slots.Hair, accessories);
            KTAccessories.AddAccessories(anim, slots.HatHair, accessories);
            KTAccessories.AddAccessories(anim, slots.Mouth, accessories);
            KTAccessories.AddAccessories(anim, slots.HeadShape, accessories);
            KTAccessories.AddCustomAccessories(Assets.GetAnim("aete_hulk_body_kanim"), accessories, slots);
        }

      
        public static void AddCustomAccessories(KAnimFile anim_file, ResourceSet parent, AccessorySlots slots)
        {
            if (anim_file == null)
            {
                return;
            }
            KAnim.Build build = anim_file.GetData().build;
            for (int i = 0; i < build.symbols.Length; i++)
            {
                string symbol_name = HashCache.Get().Get(build.symbols[i].hash);
                AccessorySlot accessorySlot = slots.resources.Find((AccessorySlot slot) => symbol_name.IndexOf(slot.Id, 0, StringComparison.OrdinalIgnoreCase) != -1);
                if (accessorySlot != null)
                {
                    Accessory accessory = new Accessory(symbol_name, parent, accessorySlot, anim_file.batchTag, build.symbols[i], anim_file, null);
                    accessorySlot.accessories.Add(accessory);
                    HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
                    LogUtil.Log( "added accessory: " + accessory.Id );
                }
            }
        }

      
        public static void AddAccessories(KAnimFile file, AccessorySlot slot, ResourceSet parent)
        {
            KAnim.Build build = file.GetData().build;
            string text = slot.Id.ToLower();
            for (int i = 0; i < build.symbols.Length; i++)
            {
                string text2 = HashCache.Get().Get(build.symbols[i].hash);
                if (text2.StartsWith(text))
                {
                    Accessory accessory = new Accessory(text2, parent, slot, file.batchTag, build.symbols[i], null, null);
                    slot.accessories.Add(accessory);
                    HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
                    LogUtil.Log( "Added accessory: " + accessory.Id );
                }
            }
        }




        public static void ReplaceAllMinionHair(KAnimFile newHairAnim, Color hairColor)
        {
            List<GameObject> minions = KModMinionUtils.GetAllMinionGameObjects();

            foreach (GameObject minion in minions)
            {
                KBatchedAnimController kbac = minion.GetComponent<KBatchedAnimController>();
                if (kbac == null) continue;

                Accessorizer accessorizer = minion.GetComponent<Accessorizer>();
                if (accessorizer == null) continue;

                // 获取当前头发
                Accessory currentHair = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair);
                if (currentHair == null) continue;

                string oldHairSymbol = HashCache.Get().Get(currentHair.symbol.hash); // "hair_013"
                string number = oldHairSymbol.Substring(oldHairSymbol.LastIndexOf('_') + 1).PadLeft(3, '0'); // "013"
                string hairSymbolName = "hair_bleached_" + number;

                // Hair
                KAnim.Build.Symbol newHairSymbol = newHairAnim.GetData().build.GetSymbol(hairSymbolName);
                if (newHairSymbol != null)
                {
                    KAnimHashedString targetHair = Db.Get().AccessorySlots.Hair.targetSymbolId;
                    SymbolOverrideController soc = kbac.GetComponent<SymbolOverrideController>();
                    soc.AddSymbolOverride(targetHair, newHairSymbol, 99);
                    kbac.SetSymbolTint(targetHair, hairColor);
                }
                else
                {
                    LogUtil.LogWarning($"[{minion.name}] 找不到新头发符号: {hairSymbolName}");
                }

                // HatHair
                Accessory hatHair = accessorizer.GetAccessory(Db.Get().AccessorySlots.HatHair);
                if (hatHair != null)
                {
                    string hatHairSymbolName = hairSymbolName;
                    KAnim.Build.Symbol newHatHairSymbol = newHairAnim.GetData().build.GetSymbol(hatHairSymbolName);
                    if (newHatHairSymbol != null)
                    {
                        KAnimHashedString targetHatHair = Db.Get().AccessorySlots.HatHair.targetSymbolId;
                        SymbolOverrideController soc = kbac.GetComponent<SymbolOverrideController>();
                        soc.AddSymbolOverride(targetHatHair, newHatHairSymbol, 99);
                        kbac.SetSymbolTint(targetHatHair, hairColor);
                    }
                    else
                    {
                        LogUtil.LogWarning($"[{minion.name}] 找不到帽子头发符号: {hatHairSymbolName}");
                    }
                }
            }
        }






        public static void PrintAllHairSymbols(KAnimFile hairAnim)
        {
            if (hairAnim == null)
            {
                LogUtil.LogWarning("hairAnim 为 null");
                return;
            }

            KAnim.Build build = hairAnim.GetData().build;
            if (build == null || build.symbols == null)
            {
                LogUtil.LogWarning("没有找到 build 或 symbols");
                return;
            }

            LogUtil.Log("=== Hair symbols in " + hairAnim.name + " ===");

            foreach (var symbol in build.symbols)
            {
                string symbolName = HashCache.Get().Get(symbol.hash); // 转回名字
                LogUtil.Log(symbolName);
            }
        }



        public static readonly int[] allowedHairIds = new int[]
        {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 30,
                36, 37, 43, 44
        };

















    }
}
