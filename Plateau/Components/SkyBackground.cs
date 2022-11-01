/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class SkyBackground
    {
        private class BackgroundLayer
        {
            public Texture2D texture;
            public World.TimeOfDay whenShown;

            public BackgroundLayer(Texture2D texture, World.TimeOfDay whenShown)
            {
                this.texture = texture;
                this.whenShown = whenShown;
            }
        }

        private class Cloud
        {
            public Texture2D cloudTex;
            public Vector2 position;
            public float speed;

            public Cloud(Texture2D cloudTex, Vector2 position, float speed)
            {
                this.cloudTex = cloudTex;
                this.position = position;
                this.speed = speed;
            }

            public void Draw(SpriteBatch sb, Vector2 topLeft, Color color)
            {
                sb.Draw(cloudTex, Vector2.Add(topLeft, position), color);
            } 

            public void Update(float deltaTime, EntityPlayer player)
            {
                if(player.GetVelocity().X < 0)
                {
                    position.X += (speed * 1.5f + -player.GetVelocity().X) * deltaTime;
                }
                else if (player.GetVelocity().X > 0)
                {
                    position.X += (speed * 0.5f - player.GetVelocity().X) * deltaTime;
                }
                else
                {
                    position.X += speed * deltaTime;
                }
            }

            public bool IsDone()
            {
                return position.X > Plateau.RESOLUTION_X + 80 || position.Y < -80 || position.Y > Plateau.RESOLUTION_Y + 80;
            }
        }

        private Texture2D sun, moon;
        private Texture2D[] smClouds;
        private Texture2D[] mdClouds;
        private Texture2D[] lgClouds;
        private List<Cloud>[] cloudLayers;

        private List<BackgroundLayer> backgroundLayers;

        //layers are drawn in the order 3 -> 2 -> 1


        private static int NUMBER_CLOUDS_LAYER1 = 60;
        private static int NUMBER_CLOUDS_LAYER2 = 35;
        private static int NUMBER_CLOUDS_LAYER3 = 15;

        public SkyBackground()
        {
            backgroundLayers = new List<BackgroundLayer>();
            cloudLayers = new List<Cloud>[3];
        }

        public void LoadContent(ContentManager Content)
        {
            backgroundLayers.Add(new BackgroundLayer(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_MORNING), World.TimeOfDay.MORNING));
            backgroundLayers.Add(new BackgroundLayer(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_DAY), World.TimeOfDay.DAY));
            backgroundLayers.Add(new BackgroundLayer(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_EVENING), World.TimeOfDay.EVENING));
            backgroundLayers.Add(new BackgroundLayer(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_NIGHT), World.TimeOfDay.NIGHT));
            sun = Content.Load<Texture2D>(Paths.BACKGROUND_SUN);
            moon = Content.Load<Texture2D>(Paths.BACKGROUND_MOON);
            smClouds = new Texture2D[4];
            smClouds[0] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM1);
            smClouds[1] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM2);
            smClouds[2] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM3);
            smClouds[3] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM4);
            mdClouds = new Texture2D[5];
            mdClouds[0] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD1);
            mdClouds[1] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD2);
            mdClouds[2] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD3);
            mdClouds[3] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD4);
            mdClouds[4] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD5);
            lgClouds = new Texture2D[3];
            lgClouds[0] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG1);
            lgClouds[1] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG2);
            lgClouds[2] = Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG3);

            for (int layer = 0; layer < cloudLayers.Length; layer++)
            {
                cloudLayers[layer] = new List<Cloud>();
                int numClouds = 0;
                switch (layer)
                {
                    case 0:
                        numClouds = NUMBER_CLOUDS_LAYER1;
                        break;
                    case 1:
                        numClouds = NUMBER_CLOUDS_LAYER2;
                        break;
                    case 2:
                        numClouds = NUMBER_CLOUDS_LAYER3;
                        break;
                }
                for(int j = 0; j < numClouds; j++)
                {
                    cloudLayers[layer].Add(GenerateCloud(layer, true));
                }
            }
        }

        private Texture2D GetSmCloudTexture()
        {
            return smClouds[Util.RandInt(0, smClouds.Length - 1)];
        }

        private Texture2D GetMdCloudTexture()
        {
            return mdClouds[Util.RandInt(0, mdClouds.Length - 1)];
        }

        private Texture2D GetLgCloudTexture()
        {
            return lgClouds[Util.RandInt(0, lgClouds.Length-1)];
        }

        private Cloud GenerateCloud(int layer, bool initialize)
        {
            int xPos = -110;
            int yPos = Util.RandInt(-80, Plateau.RESOLUTION_Y - 40);
            if (initialize)
            {
                xPos = Util.RandInt(-80, Plateau.RESOLUTION_X + 110);
            }
            if (layer == 0)
            {
                return new Cloud(GetSmCloudTexture(), new Vector2(xPos, yPos), Util.RandInt(6, 8));
            }
            else if (layer == 1)
            {
                return new Cloud(GetMdCloudTexture(), new Vector2(xPos, yPos), Util.RandInt(4, 7));
            }
            else
            {
                return new Cloud(GetLgCloudTexture(), new Vector2(xPos, yPos), Util.RandInt(2, 4));
            }
        }

        private static float BLEND_MINUTES = 60.0f;

        public void Update(float deltaTime, EntityPlayer player)
        {
            for(int i = 0; i < cloudLayers.Length; i++)
            {
                for (int j = 0; j < cloudLayers[i].Count; j++)
                {
                    cloudLayers[i][j].Update(deltaTime, player);
                    if (cloudLayers[i][j].IsDone())
                    {
                        cloudLayers[i][j] = GenerateCloud(i, false);
                    }
                }
            }
        }

        private Color GetColorForTimeAndLayer(int layer, World.TimeOfDay time)
        {
            switch(time)
            {
                case World.TimeOfDay.MORNING:
                case World.TimeOfDay.DAY:
                    switch(layer)
                    {
                        case 0:
                            return LAYER1COLORDAY;
                        case 1:
                            return LAYER2COLORDAY;
                        case 2:
                            return LAYER3COLORDAY;
                    }
                    break;
                case World.TimeOfDay.EVENING:
                    switch (layer)
                    {
                        case 0:
                            return LAYER1COLOREVENING;
                        case 1:
                            return LAYER2COLOREVENING;
                        case 2:
                            return LAYER3COLOREVENING;
                    }
                    break;
                case World.TimeOfDay.NIGHT:
                    switch (layer)
                    {
                        case 0:
                            return LAYER1COLORNIGHT;
                        case 1:
                            return LAYER2COLORNIGHT;
                        case 2:
                            return LAYER3COLORNIGHT;
                    }
                    break;
            }
            return Color.Green;
        }

        public void Draw(SpriteBatch sb, RectangleF boundingBox, World.TimeData timeData)
        {
            World.TimeOfDay mainTime = timeData.timeOfDay;
            World.TimeOfDay blendTime = World.NextTimeOfDay(mainTime);
            int minsTillTransition = World.MinutesUntilTransition(timeData.hour, timeData.minute);

            Vector2 topLeft = Util.ConvertFromAbsoluteToCameraVector(boundingBox, new Vector2(0, 0));
            foreach(BackgroundLayer layer in backgroundLayers)
            {
                if (layer.whenShown == mainTime)
                {
                    sb.Draw(layer.texture, topLeft, Color.White);
                } 
            }

            if(minsTillTransition <= BLEND_MINUTES)
            {
                foreach(BackgroundLayer layer in backgroundLayers)
                {
                    if(layer.whenShown == blendTime)
                    {
                        sb.Draw(layer.texture, topLeft, Color.White * (1.0f-(minsTillTransition/ BLEND_MINUTES)));
                    }
                }
            }

            if (timeData.hour < World.SUN_END_HOUR)
            {
                float sunCurrentTime = (60 * (timeData.hour - World.SUN_START_HOUR)) + timeData.minute;
                float sunLength = 60 * (World.SUN_END_HOUR - World.SUN_START_HOUR);
                float Xmultiplier = (sunCurrentTime / sunLength);
                float Ymultiplier = -Xmultiplier;
                if(Xmultiplier >= 0.5f)
                {
                    Ymultiplier = Xmultiplier - 1;
                }
                sb.Draw(sun, new Vector2(topLeft.X + (Plateau.RESOLUTION_X * Xmultiplier), topLeft.Y + (30 * Ymultiplier)), Color.White);
            } else
            {
                float moonCurrentTime = (60 * (timeData.hour - World.MOON_START_HOUR)) + timeData.minute;
                float moonLength = 60 * (24 - World.MOON_START_HOUR);
                float Xmultiplier = (moonCurrentTime / moonLength);

                sb.Draw(moon, new Vector2(topLeft.X + ((Plateau.RESOLUTION_X * Xmultiplier)/2) - 75, topLeft.Y + 18 + (15 * -Xmultiplier)), Color.White);
            }

            for(int i = cloudLayers.Length-1; i >= 0; i--)
            {
                Color color = GetColorForTimeAndLayer(i, timeData.timeOfDay);
                if (minsTillTransition < BLEND_MINUTES)
                {
                    Color blendColor = GetColorForTimeAndLayer(i, World.NextTimeOfDay(timeData.timeOfDay));
                    float blendAmount = 1.0f - (minsTillTransition / BLEND_MINUTES);
                    color = Util.BlendColors(color, blendColor, blendAmount);
                }

                for (int j = 0; j < cloudLayers[i].Count; j++)
                {
                    cloudLayers[i][j].Draw(sb, topLeft, color);
                }
            }
        }
    }
}*/
