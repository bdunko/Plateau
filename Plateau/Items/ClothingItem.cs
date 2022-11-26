using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;

namespace Plateau.Items
{
    public class ClothingItem : Item
    {
        private Util.RecolorMap recolorMap;
        private string onBodySpritesheetPath;
        private Texture2D onBodySpritesheet;

        public ClothingItem(string name, string texturePath, int stackCapacity, string description, int value, string onBodySpritesheetPath, Util.RecolorMap recolorMap, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.onBodySpritesheetPath = onBodySpritesheetPath;
            this.recolorMap = recolorMap;
        }

        public override void Load()
        {
            base.Load();
            //load spritesheet
            onBodySpritesheet = PlateauMain.CONTENT.Load<Texture2D>(onBodySpritesheetPath);

            //recolor
            if (recolorMap != null)
            {
                texture = Util.GenerateRecolor(texture, recolorMap);
                onBodySpritesheet = Util.GenerateRecolor(onBodySpritesheet, recolorMap, GetRecolorAdjustment());
            }
        }

        private Util.RecolorAdjustment GetRecolorAdjustment()
        {
            if(this.HasTag(Tag.SHIRT) || this.HasTag(Tag.GLOVES) || this.HasTag(Tag.SHOES))
            {
                return Util.RecolorAdjustment.DARKEN;
            } else if (this.HasTag(Tag.SCARF) || this.HasTag(Tag.SAILCLOTH) )
            {
                return Util.RecolorAdjustment.LIGHTEN;
            } else if (this.HasTag(Tag.OUTERWEAR) || this.HasTag(Tag.SOCKS))
            {
                return Util.RecolorAdjustment.SLIGHT_LIGHTEN;
            }
            else
            {
                return Util.RecolorAdjustment.NORMAL;
            }
        }

        public Texture2D GetSpritesheet()
        {
            if (!this.IsLoaded())
            {
                this.Load();
            }
            return this.onBodySpritesheet;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Color color, float layerDepth)
        {
            base.Draw(sb, position, color, layerDepth);
        }
    }
}
