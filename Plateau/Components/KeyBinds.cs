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
        public static Keys[] HOTBAR_SELECT = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };
        public static Keys OPEN_INVENTORY = Keys.E;
        public static Keys SHIFT = Keys.LeftShift;
        public static Keys OPEN_SCRAPBOOK = Keys.Q;
        public static Keys SETTINGS = Keys.T;
        public static Keys CRAFTING = Keys.R;
        public static Keys ESCAPE = Keys.Escape;
        public static Keys ENTER = Keys.Enter;
        public static Keys CONSOLE = Keys.OemTilde;
        public static Keys EDIT_MODE = Keys.F;
        public static Keys CYCLE_INVENTORY = Keys.Tab;
        public static Keys DROP_ITEM = Keys.Z;
    }
}
