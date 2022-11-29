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
        private string onBodySpritesheetRecolorPath; //path to body recolor spritesheet
        private string onBodySpritesheetPath; //path to body spritesheet
        private Texture2D onBodySpritesheet; //body spritesheet texture
        private string textureRecolorPath; //path to icon recolor spritesheet
        private Texture2D textureRecolor;  //recolored texture itself

        public ClothingItem(string name, string texturePath, string recolorTexturePath, int stackCapacity, string description, int value, string onBodySpritesheetPath, string onBodySpritesheetRecolorPath, Util.RecolorMap recolorMap, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.onBodySpritesheetPath = onBodySpritesheetPath;
            this.recolorMap = recolorMap;
            this.textureRecolorPath = recolorTexturePath;
            this.onBodySpritesheetRecolorPath = onBodySpritesheetRecolorPath;
        }

        public override void Load()
        {
            base.Load();
            //load spritesheet
            onBodySpritesheet = PlateauMain.CONTENT.Load<Texture2D>(onBodySpritesheetPath);

            //recolor
            if (recolorMap != null)
            {
                textureRecolor = PlateauMain.CONTENT.Load<Texture2D>(textureRecolorPath);
                textureRecolor = Util.GenerateRecolor(textureRecolor, recolorMap, GetRecolorAdjustment());

                //generate the recolor; then for pixels that exist in the original but not the recolor (ie: glasses glass; anything with static color); overlay it on top of the recolor
                onBodySpritesheet = Util.OverlayTextureOnto(onBodySpritesheet, Util.GenerateRecolor(PlateauMain.CONTENT.Load<Texture2D>(onBodySpritesheetRecolorPath), recolorMap, GetRecolorAdjustment()));
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
            } else if (this.HasTag(Tag.BACK))
            {
                return Util.RecolorAdjustment.EXTRA_DARKEN;
            }
            else
            {
                return Util.RecolorAdjustment.NORMAL;
            }
        }

        public string GetBaseSpritesheetPath()
        {
            return this.onBodySpritesheetPath;
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
            if(textureRecolor != null)
                sb.Draw(textureRecolor, position, color);
        }
    }
}
