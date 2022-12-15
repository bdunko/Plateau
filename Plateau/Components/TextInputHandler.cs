using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class TextInputHandler
    {
        private bool isActive;
        private string input;
        private bool acceptSpecialKeys;

        public TextInputHandler()
        {
            isActive = false;
            input = "";
            previous = new Keys[0];
            acceptSpecialKeys = false;
        }

        //Activates this text input handler; and sets if special keys (arrow keys and tab) are accepted
        public void Activate(bool acceptSpecialKeys)
        {
            isActive = true;
            this.acceptSpecialKeys = acceptSpecialKeys;
        }

        public void Deactivate()
        {
            isActive = false;
            acceptSpecialKeys = false;
        }

        private Keys[] previous;

        private bool IsShiftDown(Keys[] current)
        {
            return current.Contains(Keys.LeftShift) || current.Contains(Keys.RightShift) || current.Contains(Keys.LeftShift) || current.Contains(Keys.RightShift);
        }

        public void Update()
        {
            if (isActive) {
                KeyboardState keyboardState = Keyboard.GetState();
                Keys[] current = keyboardState.GetPressedKeys();
                foreach (Keys key in current)
                {
                    if (key == Keys.LeftAlt || key == Keys.LeftControl || key == Keys.LeftShift || key == Keys.RightAlt || key == Keys.RightControl || key == Keys.LeftShift
                        || key == Keys.OemTilde || key == Keys.Enter || key == Keys.CapsLock)
                    {
                        continue;
                    } 
                    if(!previous.Contains(key))
                    {
                        string keyValue = key.ToString();
                        if (keyValue == Keys.Back.ToString())
                        {
                            if (input.Length > 0)
                            {
                                input = input.Substring(0, input.Length - 1);
                            }
                        } else if (keyValue == Keys.Space.ToString())
                        {
                            input += " ";
                        } else if (keyValue == Keys.OemPeriod.ToString())
                        {
                            input += (IsShiftDown(current) ? ">" : ".");
                        } else if (keyValue == Keys.D1.ToString())
                        {
                            input += (IsShiftDown(current) ? "!" : "1");
                        } else if (keyValue == Keys.D2.ToString())
                        {
                            input += (IsShiftDown(current) ? "@" : "2");
                        } else if (keyValue == Keys.D3.ToString())
                        {
                            input += (IsShiftDown(current) ? "#" : "3");
                        } else if (keyValue == Keys.D4.ToString())
                        {
                            input += (IsShiftDown(current) ? "$" : "4");
                        } else if (keyValue == Keys.D5.ToString())
                        {
                            input += (IsShiftDown(current) ? "%" : "5");
                        } else if (keyValue == Keys.D6.ToString())
                        {
                            input += (IsShiftDown(current) ? "^" : "6");
                        } else if (keyValue == Keys.D7.ToString())
                        {
                            input += (IsShiftDown(current) ? "&" : "7");
                        } else if (keyValue == Keys.D8.ToString())
                        {
                            input += (IsShiftDown(current) ? "*" : "8");
                        } else if (keyValue == Keys.D9.ToString())
                        {
                            input += (IsShiftDown(current) ? "(" : "9");
                        } else if (keyValue == Keys.D0.ToString())
                        {
                            input += (IsShiftDown(current) ? ")" : "0");
                        }else if (keyValue == Keys.OemMinus.ToString())
                        {
                            input += (IsShiftDown(current) ? "_" : "-");
                        }else if (keyValue == Keys.OemPlus.ToString())
                        {
                            input += (IsShiftDown(current) ? "+" : "=");
                        } else if (keyValue == Keys.OemSemicolon.ToString())
                        {
                            input += (IsShiftDown(current) ? ":" : ";");
                        }
                        else if (keyValue == Keys.OemComma.ToString())
                        {
                            input += (IsShiftDown(current) ? "<" : ".");
                        }
                        else if (keyValue == Keys.OemQuestion.ToString())
                        {
                            input += (IsShiftDown(current) ? "?" : "/");
                        }
                        else if (keyValue == Keys.OemQuotes.ToString())
                        {
                            input += (IsShiftDown(current) ? "\"" : "\'");
                        }
                        else if (keyValue == Keys.OemOpenBrackets.ToString())
                        {
                            input += (IsShiftDown(current) ? "{" : "[");
                        }
                        else if (keyValue == Keys.OemCloseBrackets.ToString())
                        {
                            input += (IsShiftDown(current) ? "}" : "]");
                        }
                        else if (acceptSpecialKeys && keyValue == Keys.Left.ToString() || keyValue == Keys.Right.ToString() || keyValue == Keys.Up.ToString() || keyValue == Keys.Down.ToString() || keyValue == Keys.Tab.ToString())
                        {
                            input += keyValue;
                        }
                        else //all other cases like letters...
                        {
                            keyValue = (!IsShiftDown(current) ? keyValue.ToLower() : keyValue);
                            if (keyValue.Length == 1) {
                                input += keyValue;
                            } 
                        }
                    }
                }
                previous = current;
            }
        }

        public string GetInputString()
        {
            return input;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void ClearInputString()
        {
            input = "";
            previous = new Keys[0];
        }
    }
}
