using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Items
{
    public class EdibleItem : Item
    {
        private AppliedEffects.Effect[] whenEaten;
        private string onEatDescription;
        private float effectLength;

        public EdibleItem(string name, string texturePath, int stackCapacity, string description, AppliedEffects.Effect effect, float effectLength, string onEatDescription, int value, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.whenEaten = new AppliedEffects.Effect[] { effect };
            this.effectLength = effectLength;
            this.onEatDescription = onEatDescription;
        }

        public EdibleItem(string name, string texturePath, int stackCapacity, string description, AppliedEffects.Effect[] whenEaten, float effectLength, string onEatDescription, int value, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.whenEaten = whenEaten;
            this.effectLength = effectLength;
            this.onEatDescription = onEatDescription;
        }

        public override String GetDescription()
        {
            String fullDescription = "" + description;
            if (value > 0)
            {
                fullDescription += "\nValue: " + value;
            }
            fullDescription += "\n\nWhen eaten, gain: ";
            foreach (AppliedEffects.Effect foodEffect in whenEaten)
            {
                fullDescription += "\n+" + foodEffect.name + " (" + effectLength + " min)";
            }
            return fullDescription;
        }

        public AppliedEffects.Effect[] GetEffect()
        {
            return this.whenEaten;
        }

        public float GetEffectLength()
        {
            return this.effectLength;
        }

        public string GetOnEatDescription()
        {
            return this.onEatDescription;
        }
    }
}
