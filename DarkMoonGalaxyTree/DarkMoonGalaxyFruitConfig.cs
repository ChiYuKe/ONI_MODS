using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI = STRINGS.UI;
using UnityEngine;

namespace DarkMoonGalaxy
{
    public class DarkMoonGalaxyFruitConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity(DarkMoonGalaxyFruitConfig.Id, UI.FormatAsLink(STRINGS.SPECIES.DARKMOONGALAXYFRUIT.NAME, DarkMoonGalaxyFruitConfig.Id.ToUpperInvariant()), STRINGS.SPECIES.DARKMOONGALAXYFRUIT.DESC, 1f, false, Assets.GetAnim("DarkMoonGalaxyFruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, 0, SimHashes.Creature, null);
            EdiblesManager.FoodInfo foodInfo = new EdiblesManager.FoodInfo(DarkMoonGalaxyFruitConfig.Id, 1000f * 5000f, 6, 255.15f, 277.15f, 4800f, true, null, null);

            GameObject gameObject2 = EntityTemplates.ExtendEntityToFood(gameObject, foodInfo);

            Sublimates sublimates = gameObject2.AddOrGet<Sublimates>();
            sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
            sublimates.info = new Sublimates.Info(0.001f, 0f, 0.8f, 0f, SimHashes.Oxygen, byte.MaxValue, 0);
            return gameObject2;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }


        public string[] GetDlcIds()
        {
            return null;
        }

        public const string Id = "DarkMoonGalaxyFruit";
    }
}
