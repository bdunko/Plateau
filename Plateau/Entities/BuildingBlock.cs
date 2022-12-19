using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public enum BlockType
    {
        SCAFFOLDING, PLATFORM, BLOCK, PLATFORM_FARM
    }

    public class BuildingBlock : IPersist
    {
        private BuildingBlockItem sourceItem;
        protected Vector2 position;
        protected Vector2 tilePosition;
        protected Texture2D texture;
        protected BlockType type;

        public BuildingBlock(BuildingBlockItem sourceItem, Vector2 tilePosition, Texture2D texture, BlockType type)
        {
            this.sourceItem = sourceItem;
            this.tilePosition = tilePosition;
            this.position = tilePosition * new Vector2(8, 8);
            this.texture = texture;
            this.type = type;
        }

        public BlockType GetBlockType()
        {
            return this.type;
        }

        public Vector2 GetTilePosition()
        {
            return tilePosition;
        }

        public void OnRemove(EntityPlayer player, Area area, World world)
        {
            area.AddEntity(new EntityItem(sourceItem, this.position - new Vector2(6, 12)));

            if (area.GetTileEntity((int)tilePosition.X, (int)tilePosition.Y - 1) != null)
            {
                area.RemoveTileEntity(player, (int)tilePosition.X, (int)tilePosition.Y - 1, world);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.position, texture.Bounds, Color.White);
        }

        public SaveState GenerateSave()
        {
            SaveState save = new SaveState(SaveState.Identifier.BUILDING_BLOCK);

            save.AddData("sourceitem", sourceItem.GetName());
            save.AddData("tileX", ((int)tilePosition.X).ToString());
            save.AddData("tileY", ((int)tilePosition.Y).ToString());


            return save;
        }

        public Item GetItemForm()
        {
            return this.sourceItem;
        }

        public void LoadSave(SaveState state)
        {
            //loads nothing   
        }

        public bool ShouldBeSaved()
        {
            return true;
        }
    }
}
