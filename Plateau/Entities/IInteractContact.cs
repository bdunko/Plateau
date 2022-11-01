using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    interface IInteractContact
    {
        void OnContact(EntityPlayer player, Area area);
    }
}
