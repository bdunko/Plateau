using Microsoft.Xna.Framework;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public abstract class PlacedEntity : TileEntity
    {
        protected Item itemForm;
        private bool removed = false;
        private bool isRemovable;

        public PlacedEntity(Vector2 tilePosition, Item sourceItem, DrawLayer drawLayer, int tileWidth, int tileHeight) : base(tilePosition, tileWidth, tileHeight, drawLayer)
        {
            this.tilePosition = tilePosition;
            this.itemForm = sourceItem;
            this.position = new Vector2(tilePosition.X * 8, tilePosition.Y * 8);
            this.isRemovable = true;
        }

        public bool IsRemovable()
        {
            return isRemovable;
        }

        public void MarkAsUnremovable()
        {
            this.isRemovable = false;
        }

        public Item GetItemForm()
        {
            return itemForm;
        }

        public virtual void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (!removed)
            {
                removed = true;
                area.AddEntity(new EntityItem(itemForm, this.position - new Vector2(6, 12)));
            }          
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("sourceitem", itemForm.GetName());
            save.AddData("entitytype", EntityType.USE_ITEM.ToString());
            return save;
        }

        public override bool ShouldBeSaved()
        {
            return isRemovable;
        }

    }
}
