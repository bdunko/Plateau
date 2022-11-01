using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public interface IInteractTool
    {
        void InteractTool(EntityPlayer player, Area area, World world);
    }
}
