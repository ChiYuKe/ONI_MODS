using System.Collections.Generic;
using UnityEngine;

namespace ElementExpansion
{
    public static class Elements
    {
        public static void RegisterAllElements()
        {
            var algaeMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Algae).material;

            // 固体元素
            TestElement = ElementUtil.CreateSolidElement(
                "TestElement",
                "Test_element_kanim",
                new Color32(255, 255, 255, 255),
                null
                "TestElementTexture",
                1f
            );

            // 液体元素
            TestElement1 = ElementUtil.CreateLiquidElement(
                "TestElement1",
                new Color32(255, 100, 0, 255)
            );

            // 气体元素
            TestElement2 = ElementUtil.CreateGasElement(
                "TestElement2",
                new Color32(115, 20, 0, 255)
            );
        }

        

        #region Element References

        public static Substance TestElement { get; private set; }
        public static Substance TestElement1 { get; private set; }
        public static Substance TestElement2 { get; private set; }

        public static SimHashes TestElementHash => (SimHashes)Hash.SDBMLower("TestElement");
        public static SimHashes TestElement1Hash => (SimHashes)Hash.SDBMLower("TestElement1");
        public static SimHashes TestElement2Hash => (SimHashes)Hash.SDBMLower("TestElement2");

        #endregion
    }
}
