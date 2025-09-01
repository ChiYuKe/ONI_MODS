using UnityEngine;

namespace ElementExpansion
{
    public class TestElementConfig : IOreConfig
    {
        public SimHashes ElementID => Elements.TestElementHash;

        public GameObject CreatePrefab()
        {
            return EntityTemplates.CreateSolidOreEntity(ElementID, null);
        }
    }
}