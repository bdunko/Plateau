using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;
using Plateau.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class TEntityShrine : TileEntity, IInteract, IHaveHoveringInterface, ITickDaily
    {
        private GameState.ShrineStatus shrineStatus1, shrineStatus2;
        private GameState.ShrineTradeSets shrineTradeSets;
        private string name;
        private AnimatedSprite sprite, incenseSprite;
        private static int TRADE_COMMON_MIN = 5;
        private static int TRADE_COMMON_MAX = 8;
        private static int TRADE_UNCOMMON_MIN = 3;
        private static int TRADE_UNCOMMON_MAX = 5;
        private bool offeredIncenseToday;
        private float timeSinceParticleL, timeSinceParticleR;
        private static Vector2 PARTICLE_OFFSET_L = new Vector2(4, 6);
        private static Vector2 PARTICLE_OFFSET_R = new Vector2(13, 8);
        private static float TIME_BETWEEN_PARTICLES = 2.0f;

        public TEntityShrine(Vector2 tilePosition, AnimatedSprite sprite, AnimatedSprite incenseSprite, string name, GameState.ShrineStatus status1, GameState.ShrineStatus status2, GameState.ShrineTradeSets shrineTradeSets) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
            sprite.AddLoop("stage0", 0, 0, true);
            sprite.AddLoop("stage1", 1, 1, true);
            sprite.AddLoop("stage2", 2, 2, true);
            sprite.SetLoop("stage0");
            this.incenseSprite = incenseSprite;
            incenseSprite.AddLoop("sprite", 0, 0, true);
            incenseSprite.SetLoop("sprite");
            this.name = name;
            this.shrineStatus1 = status1;
            this.shrineStatus2 = status2;
            this.shrineTradeSets = shrineTradeSets;
            this.offeredIncenseToday = false;
            this.timeSinceParticleL = 0.0f;
            this.timeSinceParticleR = 0.0f;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, this.position + new Vector2(0, 1), Color.White, layerDepth);
            if (offeredIncenseToday)
            {
                incenseSprite.Draw(sb, this.position + new Vector2(0, 1), Color.White, layerDepth);
            }
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if(!shrineStatus1.IsComplete())
            {
                sprite.SetLoopIfNot("stage0");
            } else if (!shrineStatus2.IsComplete())
            {
                sprite.SetLoopIfNot("stage1");
            } else
            {
                sprite.SetLoopIfNot("stage2");
            }

            timeSinceParticleL += deltaTime * (1 + Util.RandInt(-5, 5)/10.0f);
            timeSinceParticleR += deltaTime * (1 + Util.RandInt(-5, 5)/10.0f);

            if (offeredIncenseToday && timeSinceParticleL >= TIME_BETWEEN_PARTICLES)
            {
                timeSinceParticleL = 0;

                Color color = Util.RandInt(0, 1) == 0 ? Util.BLACK_SMOKE_SECONDARY.color : Util.BLACK_SMOKE_PRIMARY.color;

                area.AddParticle(ParticleFactory.GenerateParticle(position + PARTICLE_OFFSET_L,
                    ParticleBehavior.DRIFT_UPWARD, ParticleTextureStyle.ONEXONE,
                    color, ParticleFactory.DURATION_LONG));
            }
            if (offeredIncenseToday && timeSinceParticleR >= TIME_BETWEEN_PARTICLES)
            {
                timeSinceParticleR = 0;

                Color color = Util.RandInt(0, 1) == 0 ? Util.BLACK_SMOKE_SECONDARY.color : Util.BLACK_SMOKE_PRIMARY.color;

                area.AddParticle(ParticleFactory.GenerateParticle(position + PARTICLE_OFFSET_R,
                    ParticleBehavior.DRIFT_UPWARD, ParticleTextureStyle.ONEXONE,
                    color, ParticleFactory.DURATION_LONG));
            }
        }

        public virtual string GetLeftClickAction(EntityPlayer player)
        {
            return "Offer";
        }

        public virtual string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual string GetRightClickAction(EntityPlayer player)
        {
            return "Examine";
        }

        public virtual string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            ItemStack selected = player.GetHeldItem();
            GameState.ShrineStatus status = null;
            if (!shrineStatus1.IsComplete())
            {
                status = shrineStatus1;
            } else if (!shrineStatus2.IsComplete())
            {
                status = shrineStatus2;
            }

            bool gaveItem = false;

            if (status != null)
            {
                gaveItem = status.TryGiveItem(selected.GetItem());
                if (gaveItem)
                {
                    if (!player.GetHeldItem().GetItem().HasTag(Item.Tag.NO_TRASH))
                    {
                        player.GetHeldItem().Subtract(1);
                    }
                    if (status.IsComplete())
                    {
                        if (status == shrineStatus1)
                        {
                            GameState.IncrementFlag(GameState.FLAG_NUM_SHRINES_FIRST_STAGE_COMPLETED);
                        } else
                        {
                            GameState.IncrementFlag(GameState.FLAG_NUM_SHRINES_FULLY_COMPLETED);
                        }
                        status.Complete(player, area);
                    }
                }
            }

            bool gaveNotification = false;

            if (!gaveItem)
            {
                Item offering = selected.GetItem();

                if (offering == ItemDict.LAVENDER_INCENSE || offering == ItemDict.COLD_INCENSE || offering == ItemDict.FRESH_INCENSE || offering == ItemDict.SWEET_INCENSE || offering == ItemDict.IMPERIAL_INCENSE)
                {
                    if (offeredIncenseToday)
                    {
                        gaveNotification = true;
                        player.AddNotification(new EntityPlayer.Notification("You have already left an offering today.\nTry again tommorrow.", Color.Black));
                    }
                    else if (!shrineStatus1.IsComplete())
                    {
                        gaveNotification = true;
                        player.AddNotification(new EntityPlayer.Notification("You need to restore the shrine before you can offer incense.", Color.Black));
                    }
                    else
                    {

                        Item reward = ItemDict.NONE;
                        int numRewards = 0;
                        int shrineProgress = GameState.GetFlagValue(GameState.FLAG_NUM_SHRINES_FIRST_STAGE_COMPLETED) + GameState.GetFlagValue(GameState.FLAG_NUM_SHRINES_FULLY_COMPLETED);

                        if (offering == ItemDict.LAVENDER_INCENSE)
                        {
                            switch (Util.RandInt(1, 10))
                            {
                                case 1:
                                case 2:
                                case 3:
                                    offering = ItemDict.COLD_INCENSE;
                                    break;
                                case 4:
                                case 5:
                                case 6:
                                    offering = ItemDict.FRESH_INCENSE;
                                    break;
                                case 7:
                                case 8:
                                case 9:
                                    offering = ItemDict.SWEET_INCENSE;
                                    break;
                                case 10:
                                default:
                                    offering = ItemDict.IMPERIAL_INCENSE;
                                    break;
                            }
                        }

                        if (offering == ItemDict.COLD_INCENSE)
                        {
                            reward = shrineTradeSets.GetCommonItem();
                            numRewards = Util.RandInt(TRADE_COMMON_MIN, TRADE_COMMON_MAX);
                            numRewards += (shrineProgress / 10);
                        }
                        else if (offering == ItemDict.FRESH_INCENSE)
                        {
                            reward = shrineTradeSets.GetUncommonItem();
                            numRewards = Util.RandInt(TRADE_UNCOMMON_MIN, TRADE_UNCOMMON_MAX);
                            numRewards += (shrineProgress / 16);
                        }
                        else if (offering == ItemDict.SWEET_INCENSE)
                        {
                            AppliedEffects.Effect boost = shrineTradeSets.GetEffect();
                            float length = AppliedEffects.LENGTH_MEDIUM;
                            AppliedEffects.Effect luck = AppliedEffects.LUCK_I;

                            if (shrineProgress >= 99)
                            {
                                length = AppliedEffects.LENGTH_VERY_LONG;
                            }
                            else if (shrineProgress >= 50)
                            {
                                length = AppliedEffects.LENGTH_LONG;
                            }

                            if (shrineProgress >= 30)
                            {
                                luck = AppliedEffects.LUCK_II;
                            }
                            else if (shrineProgress >= 60)
                            {
                                luck = AppliedEffects.LUCK_III;
                            }
                            else if (shrineProgress >= 90)
                            {
                                luck = AppliedEffects.LUCK_IV;
                            }

                            player.ApplyEffect(boost, length);
                            player.ApplyEffect(luck, length);

                            player.AddNotification(new EntityPlayer.Notification("The shrine accepts your offering.\nYour abilities and luck have improved.", Color.Black));
                            this.offeredIncenseToday = true;
                            player.GetHeldItem().Subtract(1);
                        }
                        else if (offering == ItemDict.IMPERIAL_INCENSE)
                        {
                            reward = shrineTradeSets.GetRareItem();
                            numRewards = 1;
                            numRewards += (shrineProgress / 80);
                        }

                        if (numRewards != 0 && reward != ItemDict.NONE)
                        {
                            player.AddNotification(new EntityPlayer.Notification("The shrine accepts your offering.", Color.Black));
                            this.offeredIncenseToday = true;
                            player.GetHeldItem().Subtract(1);
                            for (int i = 0; i < numRewards; i++)
                            {
                                area.AddEntity(new EntityItem(reward, new Vector2(position.X, position.Y - 10)));
                            }
                        }

                        gaveItem = true;
                        timeSinceParticleL = Util.RandInt(0, (int)(100 * TIME_BETWEEN_PARTICLES)) / 100.0f;
                        timeSinceParticleR = Util.RandInt(0, (int)(100 * TIME_BETWEEN_PARTICLES)) / 100.0f;
                    }
                } 
            }
            
            if(!gaveItem && !gaveNotification)
            {
                player.AddNotification(new EntityPlayer.Notification("The shrine seems to reject the offering.", Color.Red));
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (!shrineStatus2.IsComplete())
            {
                GameState.ShrineStatus.RequiredItem[] requiredItems;
                if (!shrineStatus1.IsComplete())
                {
                    requiredItems = shrineStatus1.GetRequiredItems();
                }
                else
                {
                    requiredItems = shrineStatus2.GetRequiredItems();
                }
                DialogueNode root = new DialogueNode("This ancient structure has the words\n \"" + name + "\" engraved on the front.", DialogueNode.PORTRAIT_BAD);
                DialogueNode two = new DialogueNode("It feels like I should leave an offering here...\nMaybe if I do that something good will happen?", DialogueNode.PORTRAIT_BAD);
                string threeString = "I still need to offer:\n";
                bool added = false;
                foreach (GameState.ShrineStatus.RequiredItem ri in requiredItems)
                {
                    int needed = ri.amountNeeded - ri.amountHave;
                    if (needed != 0)
                    {
                        threeString += needed.ToString() + " " + ri.item.GetName() + ", ";
                        added = true;
                    }
                }
                if (added)
                {
                    threeString = threeString.Substring(0, threeString.Length - 2);
                }

                DialogueNode three = new DialogueNode(threeString, DialogueNode.PORTRAIT_BAD);
                root.SetNext(two);
                two.SetNext(three);
                player.SetCurrentDialogue(root);
            } else
            {
                DialogueNode root = new DialogueNode("This shrine has been restored!\nIt emits an aura of natural solemnity.", DialogueNode.PORTRAIT_BAD);
                player.SetCurrentDialogue(root);
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public HoveringInterface GetHoveringInterface()
        {
            HoveringInterface baseHI = new HoveringInterface();
            HoveringInterface.Row row = new HoveringInterface.Row();
            GameState.ShrineStatus.RequiredItem[] requiredItems;
            if (!shrineStatus1.IsComplete())
            {
                requiredItems = shrineStatus1.GetRequiredItems();
            } else
            {
                requiredItems = shrineStatus2.GetRequiredItems();
            }

            foreach(GameState.ShrineStatus.RequiredItem ri in requiredItems)
            {
                int needed = ri.amountNeeded - ri.amountHave;
                if(needed != 0)
                {
                    row.AddElement(new HoveringInterface.ItemStackElement(new ItemStack(ri.item, needed)));
                }
            }

            baseHI.AddRow(new HoveringInterface.Row(new HoveringInterface.TextElement(name)));
            baseHI.AddRow(row);
            return baseHI;
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            this.offeredIncenseToday = false;
        }
    }
}
