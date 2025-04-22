using STRINGS;


namespace KModTool
{
    public static class KModStringUtils
    {

        public static void Add_New_PlantStrings(string plantId, string name, string description, string domesticatedDescription)
        {
            Strings.Add(new string[]
            {
            "STRINGS.CREATURES.SPECIES." + plantId.ToUpperInvariant() + ".NAME",
            UI.FormatAsLink(name, plantId)
            });
            Strings.Add(new string[]
            {
            "STRINGS.CREATURES.SPECIES." + plantId.ToUpperInvariant() + ".DESC",
            description
            });
            Strings.Add(new string[]
            {
            "STRINGS.CREATURES.SPECIES." + plantId.ToUpperInvariant() + ".DOMESTICATEDDESC",
            domesticatedDescription
            });
        }


        public static void Add_New_PlantSeedStrings(string plantId, string name, string description)
        {
            Strings.Add(new string[]
            {
            "STRINGS.CREATURES.SPECIES.SEEDS." + plantId.ToUpperInvariant() + ".NAME",
            UI.FormatAsLink(name, plantId)
            });
            Strings.Add(new string[]
            {
            "STRINGS.CREATURES.SPECIES.SEEDS." + plantId.ToUpperInvariant() + ".DESC",
            description
            });
        }


        public static void Add_New_FoodStrings(string foodId, string name, string description, string recipeDescription = null)
        {
            Strings.Add(new string[]
            {
            "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".NAME",
            UI.FormatAsLink(name, foodId)
            });
            Strings.Add(new string[]
            {
            "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".DESC",
            description
            });
            bool flag = recipeDescription != null;
            bool flag2 = flag;
            if (flag2)
            {
                Strings.Add(new string[]
                {
                "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".RECIPEDESC",
                recipeDescription
                });
            }
        }

        public static void Add_New_BuildStrings(string plantId, string name, string description, string effect)
        {
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + plantId.ToUpperInvariant() + ".NAME",
                name
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + plantId.ToUpperInvariant() + ".DESC",
                description
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + plantId.ToUpperInvariant() + ".EFFECT",
                effect
            });
        }







        public static void Add_New_CustomEffectBuilder_Strings(string pillId, string name, string description)
        {
            Strings.Add(new string[]
            {
            "STRINGS.DUPLICANTS.MODIFIERS." + pillId.ToUpperInvariant() + ".NAME",
            UI.FormatAsLink(name, pillId)
            });
            Strings.Add(new string[]
            {
            "STRINGS.DUPLICANTS.MODIFIERS." + pillId.ToUpperInvariant() + ".DESCRIPTION",
            description
            });
        }
        public static void Add_New_Deaths_Strings(string pillId, string name, string description)
        {
            Strings.Add(new string[]
            {
            "STRINGS.DUPLICANTS.DEATHS." + pillId.ToUpperInvariant() + ".NAME",
            UI.FormatAsLink(name, pillId)
            });
            Strings.Add(new string[]
            {
            "STRINGS.DUPLICANTS.DEATHS." + pillId.ToUpperInvariant() + ".DESCRIPTION",
            description
            });
        }
        public static void Add_New_Attributes_Strings(string pillId, string name, string description)
        {
            Strings.Add(new string[]
            {
            "MISSING.STRINGS.CREATURES.ATTRIBUTES" + pillId.ToUpperInvariant() + "DELTA" + ".NAME",
            name
            });
            Strings.Add(new string[]
            {
            "MISSING.STRINGS.CREATURES.ATTRIBUTES." + pillId.ToUpperInvariant() + "DELTA" + ".DESC",
            description
            });
        }
    }
}