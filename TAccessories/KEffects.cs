using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace TAccessories
{
    internal class KEffects
    {
        public static void Register(ModifierSet set)
        {
            Db db = Db.Get();
            Attributes attributes = Db.Get().Attributes;
            string id = db.Attributes.Athletics.Id;
            new EffectBuilder("LuminescenceKingA", 300f, false).Modifier(db.Attributes.Luminescence.Id, 5000f).Add(set);

        }

        public const string CAFFEINATED = "LuminescenceKingA";

    }
}
