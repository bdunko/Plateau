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
    public class EntitySpiritFoodie : EntitySpirit
    {
        private class RequestPair
        {
            public DialogueNode prompt;
            public Item.Tag tag;
            public RequestPair(DialogueNode prompt, Item.Tag tag)
            {
                this.prompt = prompt;
                this.tag = tag;
            }
        }

        private static DialogueNode dislikeDialogue1 = new DialogueNode("This isn't what I ordered, haiku. I guess humans don't make food that matches my request...", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode dislikeDialogue2 = new DialogueNode("The dish is nice and all, but I was hoping for something different, haiku. Sorry!", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode dislikeDialogue3 = new DialogueNode("Eww! This isn't what I asked for at all! I demand to speak with the manager, haiku!", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode likeDialogue1 = new DialogueNode("Wow, this is amazing! Here, take some trilobites as a tip, haiku!", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode likeDialogue2 = new DialogueNode("Perfect! I've never tasted flavors like this before, haiku. Take these as your payment.", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode likeDialogue3 = new DialogueNode("How did you know just what I was thinking of, haiku? Here's your tip!", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode fullLikeDialogue = new DialogueNode("I'm full for today, haiku. I hope you come back tommorrow.", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode fullDislikeDialogue = new DialogueNode("I'm done eating for today. Try to do better next time, haiku.", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode notFoodDialogue = new DialogueNode("I don't think that's edible, haiku...", DialogueNode.PORTRAIT_BAD);
        private static DialogueNode notCuisineDialogue = new DialogueNode("That's boring! I want hand-cooked human cuisine, haiku!", DialogueNode.PORTRAIT_BAD);

        private bool hasEaten;
        private static int TIP_AMOUNT_MIN = 2;
        private static int TIP_AMOUNT_MAX = 5;
        private static RequestPair[] requests = null;
        private RequestPair order;

        public EntitySpiritFoodie(AnimatedSprite sprite, Vector2 position, Element element, bool moves) : base(sprite, position, element, null, moves)
        {
            if(requests == null)
            {
                requests = new RequestPair[]
                {
                    new RequestPair(new DialogueNode("Do you have anything spicy, haiku? I need to burn off some weight!", DialogueNode.PORTRAIT_BAD), Item.Tag.SPICY), 
                    new RequestPair(new DialogueNode("What do I want to eat? Maybe something salty, haiku.", DialogueNode.PORTRAIT_BAD), Item.Tag.SALTY),
                    new RequestPair(new DialogueNode("I'm here on a dare. Give me the most bitter human food you can find, haiku!", DialogueNode.PORTRAIT_BAD), Item.Tag.BITTER),
                    new RequestPair(new DialogueNode("I want something sweet like me, haiku!", DialogueNode.PORTRAIT_BAD), Item.Tag.SWEET),
                    new RequestPair(new DialogueNode("I'm parched, haiku. Do you have any drinks?", DialogueNode.PORTRAIT_BAD), Item.Tag.DRINK),
                    new RequestPair(new DialogueNode("The menu said that breakfast is available 24 hours a day. So get me some breakfast food, haiku!", DialogueNode.PORTRAIT_BAD), Item.Tag.BREAKFAST),
                    new RequestPair(new DialogueNode("You're taking orders, haiku? Find me something meaty!", DialogueNode.PORTRAIT_BAD), Item.Tag.MEATY),
                    new RequestPair(new DialogueNode("Hi, haiku! I'm on a diet, what kinds of foods do humans make with vegetables? I want something like that!", DialogueNode.PORTRAIT_BAD), Item.Tag.VEGGIE),
                    new RequestPair(new DialogueNode("Something made from fruit please, Mr. Waiter!", DialogueNode.PORTRAIT_BAD), Item.Tag.FRUIT),
                    new RequestPair(new DialogueNode("Here's my ID, haiku. I'll take the strongest alcohol you've got.", DialogueNode.PORTRAIT_BAD), Item.Tag.ALCOHOL)
                };
            }
            dialogues = new DialogueNode[1];
            hasEaten = false;
            randomizeOrder();
        }

        private void randomizeOrder()
        {
            order = requests[Util.RandInt(0, requests.Length - 1)];
            dialogues[0] = order.prompt;
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X, position.Y, sprite.GetFrameWidth(), sprite.GetFrameHeight());
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);
        }

        public override string GetLeftClickAction(EntityPlayer player)
        {
            if(player.GetHeldItem().GetItem().HasTag(Item.Tag.FOOD) && !hasEaten)
            {
                return "Feed";
            }
            return "";
        }

        public override string GetRightClickAction(EntityPlayer player)
        {
            return "Talk";
        }

        public override void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (!hasEaten)
            {
                Item food = player.GetHeldItem().GetItem();
                if (food.HasTag(Item.Tag.FOOD))
                {
                    if (food.HasTag(Item.Tag.CUISINE))
                    {
                        player.GetHeldItem().Subtract(1);
                        bool success = food.HasTag(order.tag);
                        hasEaten = true;
                        if (success)
                        {
                            switch (Util.RandInt(1, 3))
                            {
                                case 1:
                                    player.SetCurrentDialogue(likeDialogue1);
                                    break;
                                case 2:
                                    player.SetCurrentDialogue(likeDialogue2);
                                    break;
                                case 3:
                                default:
                                    player.SetCurrentDialogue(likeDialogue3);
                                    break;
                            }

                            for (int i = 0; i < Util.RandInt(TIP_AMOUNT_MIN, TIP_AMOUNT_MAX); i++)
                            {
                                area.AddEntity(new EntityItem(ItemDict.TRILOBITE, new Vector2(position.X, position.Y - 10)));
                            }
                            dialogues[0] = fullLikeDialogue;
                        }
                        else
                        {
                            switch (Util.RandInt(1, 3))
                            {
                                case 1:
                                    player.SetCurrentDialogue(dislikeDialogue1);
                                    break;
                                case 2:
                                    player.SetCurrentDialogue(dislikeDialogue2);
                                    break;
                                case 3:
                                default:
                                    player.SetCurrentDialogue(dislikeDialogue3);
                                    break;
                            }
                            dialogues[0] = fullDislikeDialogue;
                        }
                    }
                    else
                    {
                        player.SetCurrentDialogue(notCuisineDialogue);
                    }
                }
                else
                {
                    player.SetCurrentDialogue(notFoodDialogue);
                }
                TurnToFace(player);
            }
        }

        public override void TickDaily(World world, Area area, EntityPlayer player)
        {
            randomizeOrder();
            hasEaten = false;
            base.TickDaily(world, area, player);
        }
    }
}
