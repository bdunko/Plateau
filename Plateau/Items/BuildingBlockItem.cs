using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Items
{
    public class BuildingBlockItem : Item
    {
        private Util.RecolorMap recolorMap;

        private string placedTexturePath;
        private string placedTextureRecolorPath;
        private Texture2D placedTexture;

        private string textureRecolorPath; 
        private Texture2D textureRecolor; //the recolored inventory icon, if this has one\

        private BlockType type;

        public BuildingBlockItem(string name, string texturePath, string textureRecolorPath, string placedTexturePath, string placedTextureRecolorPath, BlockType type, int stackCapacity, string description, int value, Util.RecolorMap recolorMap, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.recolorMap = recolorMap;
            this.placedTexturePath = placedTexturePath;
            this.placedTextureRecolorPath = placedTextureRecolorPath;
            this.textureRecolorPath = textureRecolorPath;
            this.type = type;
        }

        public override void Load()
        {
            base.Load();
            //load spritesheet
            placedTexture = PlateauMain.CONTENT.Load<Texture2D>(placedTexturePath);

            //recolor
            if(recolorMap != null)
            {
                textureRecolor = Util.GenerateRecolor(PlateauMain.CONTENT.Load<Texture2D>(textureRecolorPath), recolorMap);
                placedTexture = Util.OverlayTextureOnto(placedTexture, Util.GenerateRecolor(PlateauMain.CONTENT.Load<Texture2D>(placedTextureRecolorPath), recolorMap));
            }
        }

        public string GetBasePlacedPath()
        {
            return placedTexturePath;
        }

        public Texture2D GetPlacedTexture()
        {
            if (!this.IsLoaded())
            {
                this.Load();
            }
            return this.placedTexture;
        }

        public BlockType GetBlockType()
        {
            return this.type;
        }

        public BuildingBlock GenerateBuildingBlock(Vector2 tilePositionPlaced)
        {
            return new BuildingBlock(this, tilePositionPlaced, this.placedTexture, type);
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Color color)
        {
            base.Draw(sb, position, color);
            if (textureRecolor != null)
                sb.Draw(textureRecolor, position, color);
        }
    }
}
