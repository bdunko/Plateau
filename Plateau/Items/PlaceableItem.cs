using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Entities;

namespace Plateau.Items
{
    public class PlaceableItem : Item
    {
        public enum PlacementType {
            NORMAL, WALL, WALLPAPER, FLOOR, CEILING
        }

        private EntityType placedType;
        private PlacementType placementType;
        private string textureRecolorPath, placedTexturePath, placedTextureRecolorPath;
        private Texture2D placedTextureRecolor, placedTexture, textureRecolor, previewTexture;
        private int placeableHeight, placeableWidth;
        private Util.RecolorMap recolorMap;

        public PlaceableItem(string name, string texturePath, string textureRecolorPath, string placedTextureRecolorPath, string placedTexturePath, 
            int placeableWidth, int placeableHeight, int stackCapacity, string description, int value, EntityType placedType, PlacementType placementType, Util.RecolorMap recolor, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.placedTextureRecolorPath = placedTextureRecolorPath;
            this.placedTexturePath = placedTexturePath;
            this.placeableHeight = placeableHeight;
            this.placeableWidth = placeableWidth;
            this.placedType = placedType;
            this.recolorMap = recolor;
            this.placementType = placementType;
            this.textureRecolorPath = textureRecolorPath;
        }

        public Color GetBaseColor()
        {
            if(recolorMap == null)
            {
                return Color.White;
            }
            return recolorMap.GetRecolorForGreyscale(Util.GREYSCALE_225.color);
        }

        public override void Load()
        {
            base.Load();
            //load icon recolor
            textureRecolor = PlateauMain.CONTENT.Load<Texture2D>(textureRecolorPath);
            //load placed recolor/normal
            placedTexture = PlateauMain.CONTENT.Load<Texture2D>(placedTexturePath);
            placedTextureRecolor = PlateauMain.CONTENT.Load<Texture2D>(placedTextureRecolorPath);

            if(recolorMap != null)
            {
                textureRecolor = Util.GenerateRecolor(textureRecolor, recolorMap);
                placedTextureRecolor = Util.GenerateRecolor(placedTextureRecolor, recolorMap);
            }

            //load preview texture
            Rectangle sourceRectangle = new Rectangle(0, 0, placeableWidth * 8, placeableHeight * 8);
            Color[] recolorData = new Color[sourceRectangle.Width * sourceRectangle.Height];
            Color[] nonrecolorData = new Color[sourceRectangle.Width * sourceRectangle.Height];
            Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
            placedTextureRecolor.GetData(0, sourceRectangle, recolorData, 0, recolorData.Length);
            placedTexture.GetData(0, sourceRectangle, nonrecolorData, 0, nonrecolorData.Length);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = nonrecolorData[i];
            }
            for (int i = 0; i < data.Length; i++)
            {
                if (recolorData[i].A != 0)
                {
                    data[i] = recolorData[i];
                }
            }

            previewTexture = new Texture2D(PlateauMain.GRAPHICS.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
            previewTexture.SetData<Color>(data);
        }

        public int GetPlaceableHeight()
        {
            return placeableHeight;
        }

        public int GetPlaceableWidth()
        {
            return placeableWidth;
        }

        public PlacementType GetPlacementType()
        {
            return placementType;
        }

        public Texture2D GetPlacedTextureRecolor()
        {
            if (!this.IsLoaded())
            {
                this.Load();
            }
            return placedTextureRecolor;
        }

        public Texture2D GetPlacedTexture()
        {
            if (!this.IsLoaded())
            {
                this.Load();
            }
            return placedTexture;
        }

        public string GetPlacedTexturePath()
        {
            return placedTexturePath;
        }

        public Texture2D GetPreviewTexture()
        {
            if (!this.IsLoaded())
            {
                this.Load();
            }
            return previewTexture;
        }

        public EntityType GetPlacedEntityType()
        {
            return placedType;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Color color, float layerDepth)
        {
            base.Draw(sb, position, color, layerDepth);
            sb.Draw(textureRecolor, position, color);
        }
    }
}
