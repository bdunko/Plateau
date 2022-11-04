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
    public class EntitySpiritArchaeologist : EntitySpirit
    {
        private static int SHARDS_PER_TRILO = 5;
        private static int TRILO_PER_BONE = 10;
        private static int TRILO_PER_SHELL = 25;

        private static DialogueNode trilobiteDialogue = new DialogueNode("Oh, I have plenty of these already, haiku. But I'll trade some of mine for any other fossils you find!", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode shardsDialogue = new DialogueNode("Fossil shards! I'll take " + SHARDS_PER_TRILO + " of those for this trilobite, ok? Thanks for trading, haiku!", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode multiShardsDialogue = new DialogueNode("You have a whole bunch of fossil shards, haiku! I'll take all of those off your hands, for a trilobite per 5. Thanks for helping my research!", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode shardsNotEnoughDialogue = new DialogueNode("Oh, you have some fossil shards? Hmm, I don't think it would be a fair trade unless you had, say, at least 5, right haiku? Come back with more shards!", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode multiShellDialogue = new DialogueNode("Wow, where did you find this many shells? I'll take all those off your hands. Here's some trilobites, haiku!", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode shellDialogue = new DialogueNode("Wow, what a perfectly preserved shell! I'll give you " + TRILO_PER_SHELL + " trilobites for that, sound fair?", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode boneDialogue = new DialogueNode("This bone is very well preserved. How does " + TRILO_PER_BONE + " trilobites sound? Thanks for the exchange, haiku!", DialogueNode.PORTRAIT_SYSTEM);
        private static DialogueNode multiBoneDialogue = new DialogueNode("You want to swap all those bones for trilobites? Here you go haiku, thanks for the swap!", DialogueNode.PORTRAIT_SYSTEM);

        public EntitySpiritArchaeologist(AnimatedSprite sprite, Vector2 position, Element element, bool moves) : base(sprite, position, element, null, moves)
        {
            dialogues = new DialogueNode[1];
            dialogues[0] = new DialogueNode("Hi human! I'm an archaeologist studying the past of this region. If you have any rare fossils, I'll trade you Trilobites for them! Oh, and I'll also take fossil shards off your hands for trilobites, maybe at a " + SHARDS_PER_TRILO + "-to-1 ratio, haiku?", DialogueNode.PORTRAIT_SYSTEM);
            
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X, position.Y, sprite.GetFrameWidth(), sprite.GetFrameHeight());
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);
        }

        public override string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "Give All";
        }

        public override string GetLeftClickAction(EntityPlayer player)
        {
            return "Give";
        }

        public override string GetRightClickAction(EntityPlayer player)
        {
            return "Talk";
        }

        public override void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            Item heldItem = player.GetHeldItem().GetItem();
            if (heldItem == ItemDict.FOSSIL_SHARDS)
            {
                if (player.GetHeldItem().GetQuantity() >= 5)
                {
                    player.SetCurrentDialogue(multiShardsDialogue);
                    while (player.GetHeldItem().GetQuantity() >= 5)
                    {
                        player.GetHeldItem().Subtract(5);
                        area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                    }
                }
                else
                {
                    player.SetCurrentDialogue(shardsNotEnoughDialogue);
                }
            }
            else if (heldItem == ItemDict.PRIMORDIAL_SHELL)
            {
                player.SetCurrentDialogue(multiShellDialogue);
                while (player.GetHeldItem().GetQuantity() > 0)
                {
                    player.GetHeldItem().Subtract(1);
                    for (int i = 0; i < TRILO_PER_SHELL; i++)
                    {
                        area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                    }
                }
            }
            else if (heldItem == ItemDict.OLD_BONE)
            {
                player.SetCurrentDialogue(multiBoneDialogue);
                while (player.GetHeldItem().GetQuantity() > 0) {
                    player.GetHeldItem().Subtract(1);
                    for (int i = 0; i < TRILO_PER_BONE; i++)
                    {
                        area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                    }
                }
            }
            else if (heldItem == ItemDict.TRILOBITE)
            {
                player.SetCurrentDialogue(trilobiteDialogue);
            }
        }

        public override void InteractLeft(EntityPlayer player, Area area, World world)
        {
            Item heldItem = player.GetHeldItem().GetItem();
            if (heldItem == ItemDict.FOSSIL_SHARDS)
            {
                if (player.GetHeldItem().GetQuantity() >= 5)
                {
                    player.GetHeldItem().Subtract(5);
                    player.SetCurrentDialogue(shardsDialogue);
                    area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                } else
                {
                    player.SetCurrentDialogue(shardsNotEnoughDialogue);
                }
            } else if (heldItem == ItemDict.PRIMORDIAL_SHELL)
            {
                player.GetHeldItem().Subtract(1);
                player.SetCurrentDialogue(shellDialogue);
                for(int i = 0; i < TRILO_PER_SHELL; i++)
                {
                    area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                }
            } else if (heldItem == ItemDict.OLD_BONE)
            {
                player.GetHeldItem().Subtract(1);
                player.SetCurrentDialogue(boneDialogue);
                for (int i = 0; i < TRILO_PER_BONE; i++) 
                {
                    area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                }
            } else if (heldItem == ItemDict.TRILOBITE)
            {
                player.SetCurrentDialogue(trilobiteDialogue);
            }
        }
    }
}
