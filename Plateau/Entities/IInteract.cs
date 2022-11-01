using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public interface IInteract
    {
        string GetLeftShiftClickAction(EntityPlayer player);
        string GetRightShiftClickAction(EntityPlayer player);
        string GetLeftClickAction(EntityPlayer player);
        string GetRightClickAction(EntityPlayer player);
        void InteractRight(EntityPlayer player, Area area, World world);
        void InteractLeft(EntityPlayer player, Area area, World world);
        void InteractRightShift(EntityPlayer player, Area area, World world);
        void InteractLeftShift(EntityPlayer player, Area area, World world);
    }
}
