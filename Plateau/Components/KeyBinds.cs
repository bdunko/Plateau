using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class KeyBinds
    {
        public static Keys LEFT = Keys.A;
        public static Keys RIGHT = Keys.D;
        public static Keys UP = Keys.W;
        public static Keys DOWN = Keys.S;

        public static Keys INVENTORY = Keys.E;
        public static Keys SCRAPBOOK = Keys.Q;
        public static Keys CRAFTING = Keys.R;
        public static Keys SETTINGS = Keys.T;
        public static Keys EDITMODE = Keys.F;

        public static Keys CYCLE_HOTBAR = Keys.Tab;
        public static Keys DISCARD_ITEM = Keys.Z;

        public static Keys[] SHIFT = { Keys.LeftShift, Keys.RightShift };

        public static Keys ESCAPE = Keys.Escape;
        public static Keys ENTER = Keys.Enter;
        public static Keys[] HOTBAR_SELECT = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };

        public static Keys CONSOLE = Keys.OemTilde;

        public static bool HasOverlap(Keys key)
        {
            List<Keys> allKeys = new List<Keys>() { LEFT, RIGHT, UP, DOWN, INVENTORY, SCRAPBOOK, CRAFTING, SETTINGS, EDITMODE, CYCLE_HOTBAR, DISCARD_ITEM, ESCAPE, ENTER, CONSOLE };
            foreach (Keys k in HOTBAR_SELECT)
                allKeys.Add(k);
            foreach (Keys k in SHIFT)
                allKeys.Add(k);

            int instances = 0;

            foreach (Keys k in allKeys)
                if (key == k)
                    instances++;

            return instances > 1;
        }
    }
}
