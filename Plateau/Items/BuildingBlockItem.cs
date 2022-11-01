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
        private Texture2D placedTexture;
        private BlockType type;

        public BuildingBlockItem(string name, string texturePath, string placedTexturePath, BlockType type, int stackCapacity, string description, int value, Util.RecolorMap recolorMap, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.recolorMap = recolorMap;
            this.placedTexturePath = placedTexturePath;
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
                texture = Util.GenerateRecolor(texture, recolorMap);
                placedTexture = Util.GenerateRecolor(placedTexture, recolorMap);
            }
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

        public override void Draw(SpriteBatch sb, Vector2 position, Color color, float layerDepth)
        {
            base.Draw(sb, position, color, layerDepth);
        }
    }
}
