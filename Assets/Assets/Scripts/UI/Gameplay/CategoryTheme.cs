using System.Collections.Generic;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// Maps a level's category to a background tint. The level set spans 50 categories with no
    /// bespoke illustration for each, so categories are bucketed by broad meaning (creatures,
    /// nature, places, people, home objects, food, fun/abstract) and each bucket gets one hue.
    /// </summary>
    public static class CategoryTheme
    {
        private static readonly Color Fallback = new(0.2f, 0.55f, 0.85f, 0.3f);
        private static readonly Dictionary<string, Color> Colors = Build();

        public static Color Resolve(string category)
        {
            if (!string.IsNullOrEmpty(category) && Colors.TryGetValue(category, out var color))
                return color;

            // Unknown category (added after this map was written) still gets *a* tint, just
            // picked deterministically from its name instead of falling back to flat blue.
            return string.IsNullOrEmpty(category) ? Fallback : HashColor(category);
        }

        private static Color HashColor(string category)
        {
            var hash = 0;

            foreach (var c in category)
                hash = hash * 31 + c;

            var hue = (Mathf.Abs(hash) % 360) / 360f;
            var color = Color.HSVToRGB(hue, 0.55f, 0.85f);
            color.a = 0.3f;

            return color;
        }

        private static Dictionary<string, Color> Build()
        {
            var creatures = new Color(0.16f, 0.75f, 0.62f, 0.32f);
            var nature = new Color(0.42f, 0.78f, 0.32f, 0.30f);
            var places = new Color(0.30f, 0.44f, 0.95f, 0.30f);
            var people = new Color(0.98f, 0.55f, 0.42f, 0.30f);
            var home = new Color(0.98f, 0.75f, 0.30f, 0.30f);
            var food = new Color(0.95f, 0.36f, 0.48f, 0.30f);
            var fun = new Color(0.60f, 0.42f, 0.95f, 0.30f);

            return new Dictionary<string, Color>
            {
                { "Animals", creatures }, { "Animals 2", creatures }, { "Birds", creatures },
                { "Fish", creatures }, { "Insects", creatures }, { "Reptiles", creatures },
                { "Sea animals", creatures },

                { "Flowers", nature }, { "Fruits", nature }, { "Fruits 2", nature },
                { "Landscape", nature }, { "Nature", nature }, { "Trees", nature },
                { "Vegetables", nature }, { "Weather", nature },

                { "Buildings", places }, { "Capitals", places }, { "Countries", places },
                { "Countries 2", places }, { "School", places }, { "Space", places },
                { "Space 2", places }, { "Vehicles", places },

                { "Body", people }, { "Clothing", people }, { "Clothing 2", people },
                { "Headwear", people }, { "Professions", people }, { "Professions 2", people },
                { "Shoes", people },

                { "Electronics", home }, { "Everyday objects", home }, { "Fabrics", home },
                { "Furniture", home }, { "Furniture 2", home }, { "Home textile", home },
                { "Jewels", home }, { "Kitchen", home }, { "Kitchen 2", home },
                { "Metals", home }, { "Tools", home },

                { "Drinks", food }, { "Sweets", food },

                { "Colors", fun }, { "Games", fun }, { "Holidays", fun },
                { "Instruments", fun }, { "Sports", fun }, { "Toys", fun }, { "Weapons", fun },
            };
        }
    }
}
