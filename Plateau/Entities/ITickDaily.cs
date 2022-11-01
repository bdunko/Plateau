using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    interface ITickDaily
    {
        void TickDaily(World timeData, Area area, EntityPlayer player);
    }
}
