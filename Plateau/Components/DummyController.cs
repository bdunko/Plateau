using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class DummyController : Controller
    {
        public DummyController() : base()
        {
            
        }

        public void Update(MouseState dummyMState, KeyboardState dummyKState)
        {
            textInputHandler.Update();
            kStates.Dequeue();
            mStates.Dequeue();
            kStates.Enqueue(dummyKState);
            mStates.Enqueue(dummyMState);
        }
    }
}
