


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;

namespace Plateau.Entities
{
    class TEntitySpiritWheel : TileEntity, IInteract, ITickDaily
    {
        private static float SPIN_TIME1 = 4.0f;
        private static float SPIN_TIME2 = 1.5f;
        private static float[] NORMAL_FRAME_LENGTHS = Util.CreateAndFillArray(8, 0.1f);
        private static float[] FAST_FRAME_LENGTHS = Util.CreateAndFillArray(8, 0.025f);
        private float spinTimer;
        private static DialogueNode spinDialogueRoot, smallItemPrize, rareItemPrize, boostPrize, boostJackpot, trilobiteJackpot, goldJackpot, smallTrilobitePrize, bigMiss, spunAlready, notEnoughTrilo;
        private bool spunToday, doneSpinning;
        private EntityPlayer spinner;
        private static List<Item> SMALL_PRIZES = new List<Item> { ItemDict.EGGPLANT, ItemDict.EGG, ItemDict.CACTUS, ItemDict.CACAO_BEAN, ItemDict.VANILLA_EXTRACT, ItemDict.MINT_EXTRACT, ItemDict.MINT_CHOCO_BAR, ItemDict.VANILLA_ICE_CREAM,
             ItemDict.STRAWBERRY, ItemDict.WATERMELON_ICE, ItemDict.WATERMELON_SLICE, ItemDict.IRON_BAR, ItemDict.MYTHRIL_BAR, ItemDict.GOLD_BAR, ItemDict.BANANA, ItemDict.COCONUT, ItemDict.CABBAGE, ItemDict.BUTTERFLY_CHARM, ItemDict.BUTTERFLY_CLIP,
             ItemDict.CAMEL_HAT, ItemDict.BATHROBE, ItemDict.STRIPED_SHIRT, ItemDict.SAILCLOTH, ItemDict.CYCLE_PENDANT, ItemDict.GAIA_PENDANT, ItemDict.STREAMLINE_PENDANT, ItemDict.NEUTRALIZED_PENDANT, ItemDict.FLORAL_RING, ItemDict.GLIMMER_RING, ItemDict.LUMINOUS_RING};
        private static List<Item> BIG_PRIZES = new List<Item> { ItemDict.PEARL, ItemDict.PRISMATIC_FEATHER, ItemDict.GOLDEN_EGG, ItemDict.GOLDEN_COCONUT, ItemDict.GOLDEN_BANANA, ItemDict.GOLDEN_WOOL, ItemDict.ADAMANTITE_BAR, 
            ItemDict.DIAMOND, ItemDict.RUBY, ItemDict.EMERALD, ItemDict.SAPPHIRE, ItemDict.BREWERY_CREST, ItemDict.CLOUD_CREST, ItemDict.COMPRESSION_CREST, ItemDict.DASHING_CREST, ItemDict.FROZEN_CREST,
            ItemDict.MUTATING_CREST, ItemDict.MYTHICAL_CREST, ItemDict.PHILOSOPHERS_CREST, ItemDict.POLYMORPH_CREST, ItemDict.ROYAL_CREST, ItemDict.UNITY_CREST, ItemDict.VAMPYRIC_CREST, ItemDict.ALBINO_WING,
            ItemDict.EMPRESS_BUTTERFLY, ItemDict.TOTEM_OF_THE_CAT, ItemDict.TOTEM_OF_THE_CHICKEN, ItemDict.TOTEM_OF_THE_COW, ItemDict.TOTEM_OF_THE_DOG, ItemDict.TOTEM_OF_THE_PIG, ItemDict.TOTEM_OF_THE_ROOSTER,
            ItemDict.TOTEM_OF_THE_SHEEP, ItemDict.CLONING_MACHINE, ItemDict.SKY_STATUE, ItemDict.DRACONIC_PILLAR, ItemDict.RED_ANGEL, ItemDict.QUEENS_STINGER, ItemDict.PINK_DYE};
        private AnimatedSprite sprite;

        public TEntitySpiritWheel(Vector2 tilePosition, AnimatedSprite sprite) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
            if (spinDialogueRoot == null)
            {
                spinDialogueRoot = new DialogueNode("You pay your 10 trilobites, and spin the wheel with all your might...", DialogueNode.PORTRAIT_SYSTEM);
                smallItemPrize = new DialogueNode("Small win! You win a random item, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                boostPrize = new DialogueNode("Small win! You win a temporary boost, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                rareItemPrize = new DialogueNode("Big win! You win a rare item, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                boostJackpot = new DialogueNode("Jackpot! You win a big boost to all your abilities today, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                trilobiteJackpot = new DialogueNode("Jackpot! You win a ton of trilobites, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                goldJackpot = new DialogueNode("Jackpot! You win a bunch of gold, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                smallTrilobitePrize = new DialogueNode("Small win! You win some trilobites, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                bigMiss = new DialogueNode("Big miss! You don't win anything this time, haiku!", DialogueNode.PORTRAIT_SYSTEM); //change to spirit
                spunAlready = new DialogueNode("You've already spun the Wheel of Spirit today, haiku. Come back again tommorrow if you want to spin again!", DialogueNode.PORTRAIT_SYSTEM);
                notEnoughTrilo = new DialogueNode("You don't have enough trilobites, haiku! Bring 10, then I'll let you spin!", DialogueNode.PORTRAIT_SYSTEM);

            }
            spinTimer = 0;
            spunToday = false;
            doneSpinning = false;
            sprite.AddLoop("spin", 0, 7, true);
            sprite.SetLoop("spin");
            SpinNormal();
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, this.position, Color.White);
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (!spunToday)
            {
                return "Spin";
            }
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }

        private void SpinNormal()
        {
            sprite.SetFrameLengths(NORMAL_FRAME_LENGTHS);
            sprite.Unpause();
        }

        private void SpinFast()
        {
            sprite.SetFrameLengths(FAST_FRAME_LENGTHS);
            sprite.Unpause();
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            ItemStack cost = new ItemStack(ItemDict.TRILOBITE, 10);
            if (!spunToday && player.HasItemStack(cost))
            {
                player.RemoveItemStackFromInventory(cost);
                spinner = player;
                player.SetCurrentDialogue(spinDialogueRoot);
                spinTimer = SPIN_TIME1;
                spunToday = true;
            } else
            {
                spinner.SetCurrentDialogue(spunToday ? spunAlready : notEnoughTrilo);
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {

        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (spinTimer >= SPIN_TIME2)
            {
                spinTimer -= deltaTime;
                SpinFast();
            } else if (spinTimer >= 0)
            {
                spinTimer -= deltaTime;
                SpinNormal();
            }
            else if(spinTimer < 0 && !doneSpinning && spunToday)
            {
                sprite.Pause();
                if (spinner.GetCurrentDialogue() == null)
                {
                    doneSpinning = true;
                    int roll = Util.RandInt(1, 20);
                    if(roll <= 6)
                    {
                        spinner.SetCurrentDialogue(smallItemPrize);
                        Item prize = SMALL_PRIZES[Util.RandInt(0, SMALL_PRIZES.Count - 1)];
                        for (int i = 0; i < Util.RandInt(3, 5); i++)
                        {
                            area.AddEntity(new EntityItem(prize, new Vector2(position.X, position.Y - 10)));
                            if(prize is ClothingItem || prize.HasTag(Item.Tag.ACCESSORY))
                            {
                                break;
                            }
                        }
                    } else if (roll <= 11)
                    {
                        spinner.SetCurrentDialogue(boostPrize);
                        spinner.ApplyEffect(AppliedEffects.LUCK_I, AppliedEffects.LENGTH_VERY_LONG, area);
                        switch(Util.RandInt(1, 6))
                        {
                            case 1:
                                spinner.ApplyEffect(AppliedEffects.CHOPPING_IV, AppliedEffects.LENGTH_VERY_LONG, area);
                                break;
                            case 2:
                                spinner.ApplyEffect(AppliedEffects.FISHING_IV, AppliedEffects.LENGTH_VERY_LONG, area);
                                break;
                            case 3:
                                spinner.ApplyEffect(AppliedEffects.FORAGING_IV, AppliedEffects.LENGTH_VERY_LONG, area);
                                break;
                            case 4:
                                spinner.ApplyEffect(AppliedEffects.BUG_CATCHING_IV, AppliedEffects.LENGTH_VERY_LONG, area);
                                break;
                            case 5:
                                spinner.ApplyEffect(AppliedEffects.MINING_IV, AppliedEffects.LENGTH_VERY_LONG, area);
                                break;
                            case 6:
                            default:
                                spinner.ApplyEffect(AppliedEffects.SPEED_IV, AppliedEffects.LENGTH_VERY_LONG, area);
                                break;
                        }
                            
                    } else if (roll <= 14)
                    {
                        spinner.SetCurrentDialogue(smallTrilobitePrize);
                        for (int i = 0; i < Util.RandInt(12, 20); i++)
                        {
                            area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                        }
                    } else if (roll <= 16)
                    {
                        spinner.SetCurrentDialogue(boostJackpot);
                        spinner.ApplyEffect(AppliedEffects.CHOPPING_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                        spinner.ApplyEffect(AppliedEffects.FISHING_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                        spinner.ApplyEffect(AppliedEffects.FORAGING_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                        spinner.ApplyEffect(AppliedEffects.BUG_CATCHING_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                        spinner.ApplyEffect(AppliedEffects.MINING_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                        spinner.ApplyEffect(AppliedEffects.SPEED_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                        spinner.ApplyEffect(AppliedEffects.LUCK_VI, AppliedEffects.LENGTH_VERY_LONG, area);
                    } else if (roll == 17)
                    {
                        spinner.SetCurrentDialogue(rareItemPrize);
                        Item prize = BIG_PRIZES[Util.RandInt(0, BIG_PRIZES.Count - 1)];
                        area.AddEntity(new EntityItem(prize, new Vector2(position.X, position.Y - 10)));
                        if(prize == ItemDict.SKY_STATUE || prize == ItemDict.DRACONIC_PILLAR)
                        {
                            area.AddEntity(new EntityItem(prize, new Vector2(position.X, position.Y - 10)));
                        }
                    } else if (roll == 18)
                    {
                        spinner.SetCurrentDialogue(trilobiteJackpot);
                        for (int i = 0; i < Util.RandInt(35, 50); i++)
                        {
                            area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                        }
                    } else if (roll == 19)
                    {
                        spinner.SetCurrentDialogue(goldJackpot);
                        spinner.GainGold(Util.RandInt(5000, 15000));

                    } else
                    {
                        spinner.SetCurrentDialogue(bigMiss);
                    }
                }
            }
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            spunToday = false;
            doneSpinning = false;
            SpinNormal();
        }
    }
}
