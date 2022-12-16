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
        //default hotkeys
        private static Keys LEFT_DEFAULT = Keys.A;
        private static Keys RIGHT_DEFAULT = Keys.D;
        private static Keys UP_DEFAULT = Keys.W;
        private static Keys DOWN_DEFAULT = Keys.S;
        private static Keys INVENTORY_DEFAULT = Keys.E;
        private static Keys SCRAPBOOK_DEFAULT = Keys.Q;
        private static Keys CRAFTING_DEFAULT = Keys.R;
        private static Keys SETTINGS_DEFAULT = Keys.T;
        private static Keys EDITMODE_DEFAULT = Keys.F;
        private static Keys CYCLE_HOTBAR_DEFAULT = Keys.Tab;
        private static Keys DISCARD_ITEM_DEFAULT = Keys.Z;

        //actual/current hotkeys
        public static Keys LEFT = LEFT_DEFAULT;
        public static Keys RIGHT = RIGHT_DEFAULT;
        public static Keys UP = UP_DEFAULT;
        public static Keys DOWN = DOWN_DEFAULT;
        public static Keys INVENTORY = INVENTORY_DEFAULT;
        public static Keys SCRAPBOOK = SCRAPBOOK_DEFAULT;
        public static Keys CRAFTING = CRAFTING_DEFAULT;
        public static Keys SETTINGS = SETTINGS_DEFAULT;
        public static Keys EDITMODE = EDITMODE_DEFAULT;
        public static Keys CYCLE_HOTBAR = CYCLE_HOTBAR_DEFAULT;
        public static Keys DISCARD_ITEM = DISCARD_ITEM_DEFAULT;

        public static Keys[] SHIFT = { Keys.LeftShift, Keys.RightShift };

        public static Keys ESCAPE = Keys.Escape;
        public static Keys ENTER = Keys.Enter;
        public static Keys[] HOTBAR_SELECT = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };

        public static Keys CONSOLE = Keys.OemTilde;

        //Returns if there are more than 1 hotkeys bound to the given key
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

        public static void ResetAllToDefaults()
        {
            LEFT = LEFT_DEFAULT;
            RIGHT = RIGHT_DEFAULT;
            UP = UP_DEFAULT;
            DOWN = DOWN_DEFAULT;

            INVENTORY = INVENTORY_DEFAULT;
            SCRAPBOOK = SCRAPBOOK_DEFAULT;
            CRAFTING = CRAFTING_DEFAULT;
            SETTINGS = SETTINGS_DEFAULT;
            EDITMODE = EDITMODE_DEFAULT;

            CYCLE_HOTBAR = CYCLE_HOTBAR_DEFAULT;
            DISCARD_ITEM = DISCARD_ITEM_DEFAULT;
    }
    }
}
