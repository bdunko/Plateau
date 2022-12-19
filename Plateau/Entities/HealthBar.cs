using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class HealthBar
    {
        private static Vector2 SECTION_START_OFFSET = new Vector2(8, 1);
        private static float TIME_VISIBLE_AFTER_DAMAGE = 2.0f;

        private static int NUM_SECTIONS = 8;
        private Texture2D outline, section;
        private Vector2 position;
        private int maxHealth, currentHealth;
        private float timeSinceDamage;

        public HealthBar(int maxHealth, Texture2D outline, Texture2D section)
        {
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
            this.outline = outline;
            this.section = section;
            this.timeSinceDamage = 1000000;
            this.position = new Vector2(-100, -100);
        }

        public int GetWidth()
        {
            return outline.Width;
        }

        public int GetHeight()
        {
            return outline.Height;
        }

        public bool IsDepleted()
        {
            return (currentHealth) <= 0;
        }

        public void Damage(int damageAmount)
        {
            currentHealth -= damageAmount;
            timeSinceDamage = 0f;
        }

        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            timeSinceDamage = 0;
        }

        public void Reset()
        {
            currentHealth = maxHealth;
        }

        public void Update(float deltaTime)
        {
            timeSinceDamage += deltaTime;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Draw(SpriteBatch sb)
        {
            if (timeSinceDamage < TIME_VISIBLE_AFTER_DAMAGE)
            {
                float opacity = 1.0f;
                Vector2 bonusOffset = new Vector2(0, 0);
                if(timeSinceDamage < TIME_VISIBLE_AFTER_DAMAGE * 0.02f)
                {
                    bonusOffset = new Vector2(1, 1);
                } else if (timeSinceDamage < TIME_VISIBLE_AFTER_DAMAGE * 0.04f)
                {
                    bonusOffset = new Vector2(0, 0);
                } else if (timeSinceDamage < TIME_VISIBLE_AFTER_DAMAGE * 0.06f)
                {
                    bonusOffset = new Vector2(-1, -1);
                }

                if (timeSinceDamage > TIME_VISIBLE_AFTER_DAMAGE * 0.96f)
                {
                    opacity = 0.3f;
                } else if (timeSinceDamage > TIME_VISIBLE_AFTER_DAMAGE * 0.93f)
                {
                    opacity = 0.6f;
                } else if (timeSinceDamage > TIME_VISIBLE_AFTER_DAMAGE * 0.9f)
                {
                    opacity = 0.8f;
                }

                int sectionsFilled = (int)((currentHealth / (float)maxHealth) * NUM_SECTIONS);

                sb.Draw(outline, position + bonusOffset, Color.White * opacity);

                Vector2 offset = new Vector2(0, 0);
                while (sectionsFilled > 0)
                {
                    sb.Draw(section, position + SECTION_START_OFFSET + offset + bonusOffset, section.Bounds, Color.White * opacity);
                    offset.X -= 1;
                    sectionsFilled--;
                }
            }
        }

    }
}
