using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class PEntityTotem : PlacedEntity, ITickDaily
    {
        private EntityFarmAnimal linkedAnimal;
        private static float ANIMATION_DELAY = 5.0f;
        private float timeSinceAnimation;
        protected PartialRecolorSprite sprite;
        private EntityFarmAnimal.Type animalType;
        private bool animalCanSpawn;

        public PEntityTotem(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer, EntityFarmAnimal.Type animalType) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            this.itemForm = sourceItem;
            this.animalType = animalType;
            this.linkedAnimal = null;
            this.animalCanSpawn = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, position, Color.White);
        }

        public override SaveState GenerateSave()
        {
            SaveState state = base.GenerateSave();
            state.AddData("animalExists", (linkedAnimal != null || animalCanSpawn).ToString());
            //state.AddData("animalHarvestable", (linkedAnimal == null ? false.ToString() : linkedAnimal.IsHarvestable().ToString()));
            return state;
        }

        public override void LoadSave(SaveState state)
        {
            base.LoadSave(state);
            bool animalExists = state.TryGetData("animalExists", false.ToString()) == true.ToString();
            //bool animalHarvestable = state.TryGetData("animalHarvestable", false.ToString()) == true.ToString();
            if(animalExists)
            {
                animalCanSpawn = true;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            if(animalCanSpawn)
            {
                SpawnAnimal(area);
            }

            timeSinceAnimation += deltaTime;
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoopIfNot("idle");
                if(animalType == EntityFarmAnimal.Type.PIG)
                {
                    sprite.SetLoop("anim");
                }
            }
            if (timeSinceAnimation >= ANIMATION_DELAY && sprite.IsCurrentLoop("idle"))
            {
                sprite.SetLoopIfNot("anim");
                timeSinceAnimation = 0;
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (linkedAnimal != null)
            {
                area.RemoveEntity(linkedAnimal);
            }
            base.OnRemove(player, area, world);
        }

        private Vector2 FindSpawnPosition(Area area)
        {
            int animalWidth = 0, animalHeight = 0;

            switch(animalType)
            {
                case EntityFarmAnimal.Type.CHICKEN:
                    animalWidth = 2;
                    animalHeight = 2;
                    break;
                case EntityFarmAnimal.Type.COW:
                    animalWidth = 3;
                    animalHeight = 2;
                    break;
                case EntityFarmAnimal.Type.SHEEP:
                    animalWidth = 2;
                    animalHeight = 2;
                    break;
                case EntityFarmAnimal.Type.PIG:
                    animalWidth = 3;
                    animalHeight = 2;
                    break;
            }

            List<Area.XYTile> checkSpots = new List<Area.XYTile>();
            for(int x = -6; x < 6; x++)
            {
                for(int y = -6; y < 6; y++)
                {
                    Vector2 spotToCheck = new Vector2(tilePosition.X + x, tilePosition.Y + y);
                    checkSpots.Add(new Area.XYTile((int)spotToCheck.X, (int)spotToCheck.Y));
                }
            }
            checkSpots = Util.ShuffleList<Area.XYTile>(checkSpots);

            bool spotFound = false;
            int currentTile = 0;

            while(!spotFound && currentTile < checkSpots.Count - 1)
            {
                bool noSolidInHitbox = true; 
                Area.XYTile toCheck = checkSpots[currentTile];
                
                //make sure none of spawn is covered by solid tiles
                for(int width = 0; width < animalWidth; width++)
                {
                    for(int height = 0; height < animalHeight; height++)
                    {
                        if(area.GetCollisionTypeAt(toCheck.tileX+width, toCheck.tileY + height) == Area.CollisionTypeEnum.SOLID ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + height) == Area.CollisionTypeEnum.WATER ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + height) == Area.CollisionTypeEnum.DEEP_WATER ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + height) == Area.CollisionTypeEnum.TOP_WATER ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + height) == Area.CollisionTypeEnum.BOUNDARY ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + height) == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + height) == Area.CollisionTypeEnum.BRIDGE)
                        {
                            noSolidInHitbox = false;
                        }
                    }
                }

                if (noSolidInHitbox)
                {
                    //check to see if ground is on tiles below
                    for (int width = 0; width < animalWidth; width++) {
                        if (area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + animalHeight) == Area.CollisionTypeEnum.SOLID ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + animalHeight) == Area.CollisionTypeEnum.BRIDGE ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + animalHeight) == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                            area.GetCollisionTypeAt(toCheck.tileX + width, toCheck.tileY + animalHeight) == Area.CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                        {
                            spotFound = true;
                        }
                    }
                }

                if(!spotFound)
                {
                    currentTile++;
                }
            }

            if(spotFound)
            {
                return new Vector2(checkSpots[currentTile].tileX, checkSpots[currentTile].tileY);
            } 

            return new Vector2(-100000, -100000);
        }

        private void SpawnAnimal(Area area)
        {
            Vector2 spawnPosition = FindSpawnPosition(area);
            if(spawnPosition.X == -100000 && spawnPosition.Y == -100000)
            {
                return;
            }

            switch (animalType)
            {
                case EntityFarmAnimal.Type.CHICKEN:
                    linkedAnimal = (EntityFarmAnimal)EntityFactory.GetEntity(EntityType.CHICKEN, null, spawnPosition, area);
                    break;
                case EntityFarmAnimal.Type.COW:
                    linkedAnimal = (EntityFarmAnimal)EntityFactory.GetEntity(EntityType.COW, null, spawnPosition, area);
                    break;
                case EntityFarmAnimal.Type.PIG:
                    linkedAnimal = (EntityFarmAnimal)EntityFactory.GetEntity(EntityType.PIG, null, spawnPosition, area);
                    break;
                case EntityFarmAnimal.Type.SHEEP:
                    linkedAnimal = (EntityFarmAnimal)EntityFactory.GetEntity(EntityType.SHEEP, null, spawnPosition, area);
                    break;
            }
            
            animalCanSpawn = false;
            area.AddEntity(linkedAnimal);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            if(linkedAnimal == null)
            {
                animalCanSpawn = true;
            }

            if(itemForm == ItemDict.TOTEM_OF_THE_CAT || itemForm == ItemDict.TOTEM_OF_THE_DOG)
            {
                for (int x = -20; x <= 20; x++)
                {
                    for (int y = -10; y <= 10; y++)
                    {
                        TileEntity got = area.GetTileEntity((int)(this.position.X / 8) + x, (int)(this.position.Y / 8) + y);

                        if (got is TEntityFarmable)
                        {
                            TEntityFarmable toWater = (TEntityFarmable)got;
                            toWater.WaterGeyser(area);
                        }
                    }
                }
            }
        }
    }
}
