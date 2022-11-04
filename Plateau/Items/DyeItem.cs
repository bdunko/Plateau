using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Plateau.Components.Util;

namespace Plateau.Items
{
    public class DyeItem : Item
    {
        private RecolorMap color, houseColor;

        public DyeItem(string name, string texturePath, int stackCapacity, RecolorMap color, RecolorMap houseColor, string description, int value, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.color = color;
            this.houseColor = houseColor;
        }

        public RecolorMap GetRecolorMap()
        {
            return this.color;
        }

        public RecolorMap GetHouseRecolorMap()
        {
            return this.houseColor;
        }

        public string GetDyedNameAdjustment()
        {
            return " (" + color.name + ")";
        }
    }
}
