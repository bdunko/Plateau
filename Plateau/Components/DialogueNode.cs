using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class DialogueNode
    {
        public string rawText;
        public List<string> dialogueTexts;
        public string decisionUpText, decisionDownText, decisionLeftText, decisionRightText;
        public DialogueNode decisionUpNode, decisionDownNode, decisionLeftNode, decisionRightNode;
        public Texture2D portrait;
        public Action<EntityPlayer, Area, World> onActivation;
        public bool activated;

        public static Texture2D PORTRAIT_BAD;

        private static int LINES_PER_PAGE = 4;

        public static void LoadPortraits()
        {
            PORTRAIT_BAD = PlateauMain.CONTENT.Load<Texture2D>("interface/portrait_test");
        }

        private void Format()
        {
            this.dialogueTexts = new List<string>();
            int nLines = 0;
            string page = "";
            String[] splitPages = rawText.Split('|');
            foreach (string splitPage in splitPages)
            {
                foreach (String line in Util.WrapStringReturnLines(splitPage, Util.DIALOGUE_WRAP_LENGTH))
                {
                    if (nLines == LINES_PER_PAGE)
                    {
                        dialogueTexts.Add(page);
                        page = "";
                        nLines = 0;
                    }
                    page += line;
                    nLines++;
                }
                dialogueTexts.Add(page);
                page = "";
                nLines = 0;
            }
        }

        private Func<EntityPlayer, Area, World, bool> branchCondition;
        private DialogueNode trueBranch, falseBranch;

        public DialogueNode(string dialogueText, Texture2D portrait, Action<EntityPlayer, Area, World> onActivation = null, 
            string decisionUpText = "", DialogueNode decisionUp = null, 
            string decisionLeftText = "", DialogueNode decisionLeft = null,
            string decisionRightText = "", DialogueNode decisionRight = null,
            string decisionDownText = "", DialogueNode decisionDown = null)
        {
            this.rawText = dialogueText;
            Format();

            this.activated = false;

            this.decisionUpText = decisionUpText;
            this.decisionRightText = decisionRightText;
            this.decisionLeftText = decisionLeftText;
            this.decisionDownText = decisionDownText;
            this.decisionUpNode = decisionUp;
            this.decisionDownNode = decisionDown;
            this.decisionLeftNode = decisionLeft;
            this.decisionRightNode = decisionRight;
            this.portrait = portrait;
            this.onActivation = onActivation;
        }

        public DialogueNode(Func<EntityPlayer, Area, World, bool> branchCondition, DialogueNode trueBranch, DialogueNode falseBranch)
        {
            this.rawText = "";
            Format();
            this.activated = false;

            this.branchCondition = branchCondition;
            this.trueBranch = trueBranch;
            this.falseBranch = falseBranch;
        }

        public int NumPages()
        {
            return dialogueTexts.Count;
        }

        public bool IsFinished()
        {
            return decisionUpNode == null;
        }

        public void SetNext(DialogueNode next)
        {
            decisionUpNode = next;
        }

        public DialogueNode GetNext(Area area, World world, EntityPlayer player)
        {
            if(branchCondition != null)
            {
                return branchCondition(player, area, world) ? trueBranch : falseBranch;
            }
            return decisionUpNode;
        }

        public bool Splits()
        {
            return decisionDownNode != null || decisionRightNode != null || decisionLeftNode != null || decisionDownText != "" || decisionRightText != "" || decisionLeftText != "";
        }

        public void OnActivation(EntityPlayer player, Area area, World world)
        {
            //dynamically replace certain strings with player name, etc
            bool reformat = false;
            if(rawText.Contains("<NAME>"))
            {
                rawText = rawText.Replace("<NAME>", player.GetName());
                reformat = true;
            }

            if(reformat)
            {
                Format();
            }

            if (!activated && onActivation != null)
            {
                this.onActivation(player, area, world);
            }
        }

        public string GetText(int numChars, int page)
        {
            string toDraw = "";
            if (numChars > dialogueTexts[page].Length)
            {
                toDraw = dialogueTexts[page];
            } else {
                toDraw = dialogueTexts[page].Substring(0, numChars);
            }

            return toDraw;
        }

    }
}
