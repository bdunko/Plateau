using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class AppliedEffects
    {
        public static float LENGTH_VERY_SHORT = 60.0f; // 1 hour
        public static float LENGTH_SHORT = 120.0f; // 2 hour
        public static float LENGTH_MEDIUM = 240.0f; // 4 hours
        public static float LENGTH_LONG = 360.0f; // 6 hours
        public static float LENGTH_VERY_LONG = 480.0f; //8 hours
        public static float LENGTH_INFINITE = LENGTH_VERY_LONG * 1000.0f;

        public class Effect
        {
            public string name, description;
            private Texture2D icon, modifier, frame;
            private bool removable;

            public Effect(string name, string description, Texture2D icon, Texture2D frame, Texture2D modifier, bool removable = true)
            {
                this.name = name;
                this.description = description;
                this.icon = icon;
                this.frame = frame;
                this.modifier = modifier;
                this.removable = removable;
            }

            public void DrawIcon(SpriteBatch sb, Vector2 position)
            {
                sb.Draw(frame, position, Color.White);
                sb.Draw(modifier, position, Color.White * 0.75f);
                sb.Draw(icon, position, Color.White);
            }

            public bool CanRemove()
            {
                return removable;
            }
        }

        public static Effect CHOPPING_I, CHOPPING_II, CHOPPING_III, CHOPPING_IV,
            CHOPPING_V, CHOPPING_VI, CHOPPING_I_AUTUMN, CHOPPING_II_SPRING, CHOPPING_III_AUTUMN;
        public static Effect FISHING_I, FISHING_II, FISHING_III, FISHING_IV, FISHING_II_OCEAN, FISHING_III_CLOUD, FISHING_III_FRESHWATER, FISHING_III_LAVA, FISHING_III_OCEAN, FISHING_IV_CAVE, FISHING_IV_CLOUD, FISHING_IV_FRESHWATER,
            FISHING_IV_LAVA, FISHING_IV_OCEAN,
            FISHING_V_OCEAN, FISHING_VI_LAVA, FISHING_VI_CLOUD, FISHING_VI_CAVE, FISHING_V, FISHING_III_AUTUMN, FISHING_VI,
            FISHING_VI_SUMMER, FISHING_II_SUMMER;
        public static Effect FORAGING_I, FORAGING_II, FORAGING_III, FORAGING_IV, FORAGING_II_MUSHROOMS, FORAGING_II_SPRING, FORAGING_II_SUMMER, FORAGING_II_WINTER, FORAGING_III_AUTUMN, FORAGING_III_SUMMER, FORAGING_III_SPRING, FORAGING_IV_BEACH, FORAGING_IV_FLOWERS,
            FORAGING_IV_MUSHROOMS, FORAGING_IV_SUMMER, FORAGING_IV_WINTER, FORAGING_II_FLOWERS, FORAGING_II_BEACH,
            FORAGING_I_SPRING, FORAGING_I_WINTER, FORAGING_I_AUTUMN, FORAGING_I_MUSHROOMS, FORAGING_II_AUTUMN, FORAGING_III_BEACH, FORAGING_III_FLOWERS, FORAGING_III_MUSHROOMS,
            FORAGING_IV_SPRING, FORAGING_V_WINTER, FORAGING_V_FLOWERS, FORAGING_VI_SUMMER, FORAGING_VI_AUTUMN, FORAGING_VI_SPRING, FORAGING_VI_MUSHROOMS, FORAGING_V, FORAGING_VI;
        public static Effect GATHERING_BOAR, GATHERING_CHICKEN, GATHERING_COW, GATHERING_SHEEP, GATHERING_PIG;
        public static Effect BUG_CATCHING_I, BUG_CATCHING_II, BUG_CATCHING_III, BUG_CATCHING_IV_MORNING, BUG_CATCHING_IV_NIGHT, BUG_CATCHING_IV, 
            BUG_CATCHING_VI_NIGHT, BUG_CATCHING_VI_AUTUMN, BUG_CATCHING_V, BUG_CATCHING_I_SUMMER, BUG_CATCHING_I_SPRING, BUG_CATCHING_I_AUTUMN,
            BUG_CATCHING_IV_SUMMER, BUG_CATCHING_IV_SPRING, BUG_CATCHING_II_SUMMER, BUG_CATCHING_VI,
            BUG_CATCHING_VI_SUMMER, BUG_CATCHING_VI_SPRING, BUG_CATCHING_III_SUMMER, BUG_CATCHING_III_SPRING;
        public static Effect MINING_I, MINING_II, MINING_III, MINING_IV, MINING_V, MINING_VI, MINING_III_AUTUMN, MINING_VI_AUTUMN;
        public static Effect SPEED_I, SPEED_I_SPRING, SPEED_I_SUMMER, SPEED_I_AUTUMN, SPEED_II, SPEED_III, SPEED_IV, SPEED_I_MORNING, SPEED_II_MORNING, SPEED_III_AUTUMN, 
            SPEED_III_MORNING, SPEED_III_SPRING, SPEED_III_SUMMER, SPEED_III_WINTER, SPEED_IV_AUTUMN, SPEED_IV_SPRING, SPEED_IV_WINTER,
            SPEED_V_WINTER, SPEED_VI, SPEED_V, SPEED_IV_MORNING, SPEED_II_SUMMER, SPEED_II_SPRING, SPEED_II_AUTUMN;
        public static Effect DIZZY, BLESSED, BEWITCHED, WISHBOAT_HEALTH, WISHBOAT_LOVE, WISHBOAT_FORTUNE;
        public static Effect LUCK_I, LUCK_II, LUCK_III, LUCK_IV, LUCK_V, LUCK_VI;
        public static Effect PERFUME_AUTUMNS_KISS, PERFUME_BIZARRE_PERFUME, PERFUME_BLISSFUL_SKY, PERFUME_FLORAL_PERFUME, PERFUME_OCEAN_GUST, PERFUME_RED_ANGEL, PERFUME_SUMMERS_GIFT, PERFUME_SWEET_BREEZE, PERFUME_WARM_MEMORIES;

        public static Effect[] PERFUMES;

        public static void Initialize(ContentManager content)
        {
            Texture2D frame_i = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_I);
            Texture2D frame_ii = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_II);
            Texture2D frame_iii = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_III);
            Texture2D frame_iv = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_IV);
            Texture2D frame_v = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_V);
            Texture2D frame_vi = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_VI);
            Texture2D frame_special_gold = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_SPECIAL_GOLD);
            Texture2D frame_special_black = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_SPECIAL_BLACK);
            Texture2D frame_special_pink = content.Load<Texture2D>(Paths.INTERFACE_ICON_FRAME_SPECIAL_PINK);
            Texture2D modifier_spring = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_SPRING);
            Texture2D modifier_summer = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_SUMMER);
            Texture2D modifier_autumn = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_AUTUMN);
            Texture2D modifier_winter = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_WINTER);
            Texture2D modifier_morning = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_MORNING);
            Texture2D modifier_night = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_NIGHT);
            Texture2D modifier_sunny = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_SUNNY);
            Texture2D modifier_rainy = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_RAINY);
            Texture2D modifier_cloudy = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_CLOUDY);
            Texture2D modifier_snowy = content.Load<Texture2D>(Paths.INTERFACE_ICON_MODIFIER_SNOWY);
            Texture2D icon_boar = content.Load<Texture2D>(Paths.INTERFACE_ICON_BOAR);
            Texture2D icon_chicken = content.Load<Texture2D>(Paths.INTERFACE_ICON_CHICKEN);
            Texture2D icon_chopping = content.Load<Texture2D>(Paths.INTERFACE_ICON_CHOPPING);
            Texture2D icon_cow = content.Load<Texture2D>(Paths.INTERFACE_ICON_COW);
            Texture2D icon_fishing = content.Load<Texture2D>(Paths.INTERFACE_ICON_FISHING);
            Texture2D icon_fishing_cave = content.Load<Texture2D>(Paths.INTERFACE_ICON_FISHING_CAVE);
            Texture2D icon_fishing_cloud = content.Load<Texture2D>(Paths.INTERFACE_ICON_FISHING_CLOUD);
            Texture2D icon_fishing_freshwater = content.Load<Texture2D>(Paths.INTERFACE_ICON_FISHING_FRESHWATER);
            Texture2D icon_fishing_ocean = content.Load<Texture2D>(Paths.INTERFACE_ICON_FISHING_OCEAN);
            Texture2D icon_fishing_lava = content.Load<Texture2D>(Paths.INTERFACE_ICON_FISHING_LAVA);
            Texture2D icon_foraging = content.Load<Texture2D>(Paths.INTERFACE_ICON_FORAGING);
            Texture2D icon_foraging_beach = content.Load<Texture2D>(Paths.INTERFACE_ICON_FORAGING_BEACH);
            Texture2D icon_foraging_flower = content.Load<Texture2D>(Paths.INTERFACE_ICON_FORAGING_FLOWER);
            Texture2D icon_foraging_mushroom = content.Load<Texture2D>(Paths.INTERFACE_ICON_FORAGING_MUSHROOM);
            Texture2D icon_insect_catching = content.Load<Texture2D>(Paths.INTERFACE_ICON_INSECT_CATCHING);
            Texture2D icon_mining = content.Load<Texture2D>(Paths.INTERFACE_ICON_MINING);
            Texture2D icon_pig = content.Load<Texture2D>(Paths.INTERFACE_ICON_PIG);
            Texture2D icon_sheep = content.Load<Texture2D>(Paths.INTERFACE_ICON_SHEEP);
            Texture2D icon_speed = content.Load<Texture2D>(Paths.INTERFACE_ICON_SPEED);
            Texture2D icon_dizzy = content.Load<Texture2D>(Paths.INTERFACE_ICON_DIZZY);
            Texture2D icon_luck = content.Load<Texture2D>(Paths.INTERFACE_ICON_LUCK);

            Texture2D icon_blessed = content.Load<Texture2D>(Paths.INTERFACE_ICON_BLESSED);
            Texture2D icon_bewitched = content.Load<Texture2D>(Paths.INTERFACE_ICON_BEWITCHED);
            Texture2D icon_wishboat_health = content.Load<Texture2D>(Paths.INTERFACE_ICON_WISHBOAT_HEALTH);
            Texture2D icon_wishboat_love = content.Load<Texture2D>(Paths.INTERFACE_ICON_WISHBOAT_LOVE);
            Texture2D icon_wishboat_fortune = content.Load<Texture2D>(Paths.INTERFACE_ICON_WISHBOAT_FORTUNE);

            Texture2D icon_perfume_autumns_kiss = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_AUTUMNS_KISS);
            Texture2D icon_perfume_bizarre_perfume = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_BIZARRE_PERFUME);
            Texture2D icon_perfume_blissful_sky = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_BLISSFUL_SKY);
            Texture2D icon_perfume_floral_perfume = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_FLORAL_PERFUME);
            Texture2D icon_perfume_ocean_gust = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_OCEAN_GUST);
            Texture2D icon_perfume_red_angel = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_RED_ANGEL);
            Texture2D icon_perfume_summers_gift = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_SUMMERS_GIFT);
            Texture2D icon_perfume_sweet_breeze = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_SWEET_BREEZE);
            Texture2D icon_perfume_warm_memories = content.Load<Texture2D>(Paths.INTERFACE_ICON_PERFUME_WARM_MEMORIES);

            Texture2D none = content.Load<Texture2D>(Paths.ITEM_NONE);

            //increases wood drops by 1 per level. 
            //if chopping a tree, increases tree-specific drops by 0.25 per level.
            CHOPPING_I = new Effect("Chopping I", "Slightly increases both the speed of woodcutting and the quantity of items acquired.", icon_chopping, frame_i, none);
            CHOPPING_I_AUTUMN = new Effect("Chopping I (Autumn", "Slightly increases both the speed of woodcutting and the quantity of items acquired in Autumn.", icon_chopping, frame_i, modifier_autumn);

            CHOPPING_II = new Effect("Chopping II", "Increases both the speed of woodcutting and the quantity of items acquired.", icon_chopping, frame_ii, none);
            CHOPPING_II_SPRING = new Effect("Chopping II (Spring)", "Increases both the speed of woodcutting and the quantity of items acquired in Spring.", icon_chopping, frame_ii, modifier_spring);

            CHOPPING_III = new Effect("Chopping III", "Greatly increases both the speed of woodcutting and the quantity of items acquired.", icon_chopping, frame_iii, none);
            CHOPPING_III_AUTUMN = new Effect("Chopping III (Autumn)", "Greatly increases both the speed of woodcutting and the quantity of items acquired in Autumn.", icon_chopping, frame_iii, modifier_autumn);

            CHOPPING_IV = new Effect("Chopping IV", "Hugely increases both the speed of woodcutting and the quantity of items acquired.", icon_chopping, frame_iv, none);

            CHOPPING_V = new Effect("Chopping V", "Massively increases both the speed of woodcutting and the quantity of items acquired.", icon_chopping, frame_v, none);

            CHOPPING_VI = new Effect("Chopping VI", "Absurdly increases both the speed of woodcutting and the quantity of items acquired.", icon_chopping, frame_vi, none);

            //when rolling fish; rolls an extra 0.5x per level, and gives the item with the highest value out of all rolled fish.
            FISHING_I = new Effect("Fishing I", "Slightly decreases the time before a bite and increases the rarity of items acquired while fishing.", icon_fishing, frame_i, none);

            FISHING_II = new Effect("Fishing II", "Decreases the time before a bite and increases the rarity of items acquired while fishing.", icon_fishing, frame_ii, none);
            FISHING_II_SUMMER = new Effect("Fishing II (Summer)", "Decreases the time before a bite and increases the rarity of items acquired while fishing in summer.", icon_fishing, frame_ii, modifier_summer);
            FISHING_II_OCEAN = new Effect("Fishing II (Ocean)", "Decreases the time before a bite and increases the rarity of items acquired while fishing in the ocean.", icon_fishing_ocean, frame_ii, none);

            FISHING_III = new Effect("Fishing III", "Greatly decreases the time before a bite and increases the rarity of items acquired while fishing.", icon_fishing, frame_iii, none);
            FISHING_III_CLOUD = new Effect("Fishing III (Cloud)", "Greatly decreases the time before a bite and increases the rarity of items acquired while fishing in clouds.", icon_fishing_cloud, frame_iii, none);
            FISHING_III_FRESHWATER = new Effect("Fishing III (Freshwater)", "Greatly decreases the time before a bite and increases the rarity of items acquired while fishing in freshwater.", icon_fishing_freshwater, frame_iii, none);
            FISHING_III_LAVA = new Effect("Fishing III (Lava)", "Greatly decreases the time before a bite and increases the rarity of items acquired while fishing in magma.", icon_fishing_lava, frame_iii, none);
            FISHING_III_OCEAN = new Effect("Fishing III (Ocean)", "Greatly decreases the time before a bite and increases the rarity of items acquired while fishing in the ocean.", icon_fishing_ocean, frame_iii, none);
            FISHING_III_AUTUMN = new Effect("Fishing III (Autumn)", "Greatly decreases the time before a bite and increases the rarity of items acquired while fishing in Autumn.", icon_fishing_ocean, frame_iii, modifier_autumn);

            FISHING_IV = new Effect("Fishing IV", "Hugely decreases the time before a bite and increases the rarity of items acquired while fishing.", icon_fishing, frame_iv, none);
            FISHING_IV_CAVE = new Effect("Fishing IV (Cave)", "Hugely decreases the time before a bite and increases the rarity of items acquired while fishing in caves.", icon_fishing_cave, frame_iv, none);
            FISHING_IV_CLOUD = new Effect("Fishing IV (Cloud)", "Hugely decreases the time before a bite and increases the rarity of items acquired while fishing in clouds.", icon_fishing_cloud, frame_iv, none);
            FISHING_IV_FRESHWATER = new Effect("Fishing IV (Freshwater)", "Hugely decreases the time before a bite and increases the rarity of items acquired while fishing in freshwater.", icon_fishing_freshwater, frame_iv, none);
            FISHING_IV_LAVA = new Effect("Fishing IV (Lava)", "Hugely decreases the time before a bite and increases the rarity of items acquired while fishing in magma.", icon_fishing_lava, frame_iv, none);
            FISHING_IV_OCEAN = new Effect("Fishing IV (Ocean)", "Hugely decreases the time before a bite and increases the rarity of items acquired while fishing in the ocean.", icon_fishing_ocean, frame_iv, none);

            FISHING_V = new Effect("Fishing V", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing.", icon_fishing, frame_v, none);
            FISHING_V_OCEAN = new Effect("Fishing V (Ocean)", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing in the ocean.", icon_fishing_ocean, frame_v, none);

            FISHING_VI = new Effect("Fishing VI (Lava)", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing.", icon_fishing, frame_vi, none);
            FISHING_VI_LAVA = new Effect("Fishing VI (Lava)", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing in magma.", icon_fishing_lava, frame_vi, none);
            FISHING_VI_CAVE = new Effect("Fishing VI (Cave)", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing in caves.", icon_fishing_cave, frame_vi, none);
            FISHING_VI_CLOUD = new Effect("Fishing VI (Cloud)", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing in clouds.", icon_fishing_cloud, frame_vi, none);
            FISHING_VI_SUMMER = new Effect("Fishing VI (Summer)", "Massively decreases the time before a bite and increases the rarity of items acquired while fishing in Summer.", icon_fishing, frame_vi, modifier_summer);


            //increases forage by 0.5x per level
            //mushroom/flower forage: multiplies found mushrooms/flowers by 0.5x per level
            FORAGING_I = new Effect("Foraging I", "Slightly increases the amount of items found while foraging.", icon_foraging, frame_i, none);
            FORAGING_I_SPRING = new Effect("Foraging I (Spring)", "Slightly increases the amount of items found while foraging in Spring.", icon_foraging, frame_i, modifier_spring);
            FORAGING_I_WINTER = new Effect("Foraging I (Winter)", "Slightly increases the amount of items found while foraging in Winter.", icon_foraging, frame_i, modifier_winter);
            FORAGING_I_AUTUMN = new Effect("Foraging I (Autumn)", "Slightly increases the amount of items found while foraging in Autumn.", icon_foraging, frame_i, modifier_autumn);
            FORAGING_I_MUSHROOMS = new Effect("Foraging I (Mushrooms)", "Slightly increases the amount of mushrooms found while foraging.", icon_foraging_mushroom, frame_i, none);

            FORAGING_II = new Effect("Foraging II", "Increases the amount of items found while foraging.", icon_foraging, frame_ii, none);
            FORAGING_II_MUSHROOMS = new Effect("Foraging II (Mushrooms)", "Increases the amount of mushrooms found while foraging.", icon_foraging_mushroom, frame_ii, none);
            FORAGING_II_SPRING = new Effect("Foraging II (Spring)", "Increases the amount of items found while foraging in Spring.", icon_foraging, frame_ii, modifier_spring);
            FORAGING_II_SUMMER = new Effect("Foraging II (Summer)", "Increases the amount of items found while foraging in Summer.", icon_foraging, frame_ii, modifier_summer);
            FORAGING_II_WINTER = new Effect("Foraging II (Winter)", "Increases the amount of items found while foraging in Winter.", icon_foraging, frame_ii, modifier_winter);
            FORAGING_II_FLOWERS = new Effect("Foraging II (Flowers)", "Increases the amount of flowers found while foraging.", icon_foraging_flower, frame_i, none);
            FORAGING_II_AUTUMN = new Effect("Foraging II (Autumn)", "Increases the amount of items found while foraging in Autumn.", icon_foraging, frame_ii, modifier_autumn);

            FORAGING_III = new Effect("Foraging III", "Greatly increases the amount of items found while foraging.", icon_foraging, frame_iii, none);
            FORAGING_III_AUTUMN = new Effect("Foraging III (Autumn)", "Greatly increases the amount of items found while foraging in Autumn.", icon_foraging, frame_iii, modifier_autumn);
            FORAGING_III_SPRING = new Effect("Foraging III (Spring)", "Greatly increases the amount of items found while foraging in Spring.", icon_foraging, frame_iii, modifier_spring);
            FORAGING_III_SUMMER = new Effect("Foraging III (Summer)", "Greatly increases the amount of items found while foraging in Spring.", icon_foraging, frame_iii, modifier_spring);
            FORAGING_III_BEACH = new Effect("Foraging III (Beach)", "Greatly increases the amount of items found while foraging on the beach.", icon_foraging_beach, frame_iii, none);
            FORAGING_III_FLOWERS = new Effect("Foraging III (Flowers)", "Greatly increases the amount of flowers found while foraging.", icon_foraging_flower, frame_iii, none);
            FORAGING_III_MUSHROOMS = new Effect("Foraging III (Mushrooms)", "Greatly increases the amount of mushrooms found while foraging.", icon_foraging_mushroom, frame_iii, none);

            FORAGING_IV = new Effect("Foraging IV", "Hugely increases the amount of items found while foraging.", icon_foraging, frame_iv, none);
            FORAGING_IV_BEACH = new Effect("Foraging IV (Beach)", "Hugely increases the amount of items found while foraging on the beach.", icon_foraging_beach, frame_iv, none);
            FORAGING_IV_FLOWERS = new Effect("Foraging IV (Flowers)", "Hugely increases the amount of flowers found while foraging.", icon_foraging_flower, frame_iv, none);
            FORAGING_IV_MUSHROOMS = new Effect("Foraging IV (Mushrooms)", "Hugely increases the amount of mushrooms found while foraging.", icon_foraging_mushroom, frame_iv, none);
            FORAGING_IV_SUMMER = new Effect("Foraging IV (Summer)", "Hugely increases the amount of items found while foraging in Summer.", icon_foraging, frame_iv, modifier_summer);
            FORAGING_IV_WINTER = new Effect("Foraging IV (Winter)", "Hugely increases the amount of items found while foraging in Winter.", icon_foraging, frame_iv, modifier_winter);
            FORAGING_IV_SPRING = new Effect("Foraging IV (Spring)", "Hugely increases the amount of items found while foraging in Spring.", icon_foraging, frame_iv, modifier_spring);

            FORAGING_V = new Effect("Foraging V", "Massively increases the amount of items found while foraging.", icon_foraging, frame_v, none);
            FORAGING_V_WINTER = new Effect("Foraging V (Winter)", "Massively increases the amount of items found while foraging in Winter.", icon_foraging, frame_v, modifier_winter);
            FORAGING_V_FLOWERS = new Effect("Foraging V (Flowers)", "Massively increases the amount of flowers found while foraging.", icon_foraging_flower, frame_v, modifier_winter);

            FORAGING_VI = new Effect("Foraging VI", "Absurdly increases the amount of items found while foraging.", icon_foraging, frame_vi, none);
            FORAGING_VI_SPRING = new Effect("Foraging VI (Spring)", "Absurdly increases the amount of items found while foraging in Spring.", icon_foraging, frame_vi, modifier_spring);
            FORAGING_VI_SUMMER = new Effect("Foraging VI (Summer)", "Absurdly increases the amount of items found while foraging in Summer.", icon_foraging, frame_vi, modifier_summer);
            FORAGING_VI_AUTUMN = new Effect("Foraging VI (Autumn)", "Absurdly increases the amount of items found while foraging in Autumn.", icon_foraging, frame_vi, modifier_autumn);
            FORAGING_VI_MUSHROOMS = new Effect("Foraging VI (Mushrooms)", "Absurdly increases the amount of mushrooms found while foraging.", icon_foraging_mushroom, frame_vi, none);

            //increases gathered items by 1-2
            GATHERING_BOAR = new Effect("Gathering (Boar)", "Boosts the quantity of items recieved when gathering from boar traps.", icon_boar, frame_v, none);
            GATHERING_CHICKEN = new Effect("Gathering (Chicken)", "Boosts the quantity of items recieved when gathering from chickens.", icon_chicken, frame_v, none);
            GATHERING_COW = new Effect("Gathering (Cow)", "Boosts the quantity of items recieved when milking cows.", icon_cow, frame_v, none);
            GATHERING_SHEEP = new Effect("Gathering (Sheep)", "Boosts the quantity of items recieved when shearing sheep.", icon_sheep, frame_v, none);
            GATHERING_PIG = new Effect("Gathering (Pig)", "Boosts the quantity of items produced by pigs.", icon_pig, frame_v, none);

            //increases gathered insects by 0.5x per level
            BUG_CATCHING_I = new Effect("Bug Catching I", "Slightly increases the amount of insects caught.", icon_insect_catching, frame_i, none);
            BUG_CATCHING_I_SPRING = new Effect("Bug Catching I (Spring)", "Slightly increases the amount of insects caught in Spring.", icon_insect_catching, frame_i, modifier_spring);
            BUG_CATCHING_I_SUMMER = new Effect("Bug Catching I (Summer)", "Slightly increases the amount of insects caught in Summer.", icon_insect_catching, frame_i, modifier_summer);
            BUG_CATCHING_I_AUTUMN = new Effect("Bug Catching I (Autumn)", "Slightly increases the amount of insects caught in Autumn.", icon_insect_catching, frame_i, modifier_autumn);

            BUG_CATCHING_II = new Effect("Bug Catching II", "Increases the amount of insects caught.", icon_insect_catching, frame_ii, none);
            BUG_CATCHING_II_SUMMER = new Effect("Bug Catching II (Summer)", "Increases the amount of insects caught in Summer.", icon_insect_catching, frame_ii, modifier_summer);

            BUG_CATCHING_III = new Effect("Bug Catching III", "Greatly increases the amount of insects caught.", icon_insect_catching, frame_iii, none);
            BUG_CATCHING_III_SUMMER = new Effect("Bug Catching III (Summer)", "Greatly increases the amount of insects caught in Summer.", icon_insect_catching, frame_iii, modifier_summer);
            BUG_CATCHING_III_SPRING = new Effect("Bug Catching III (Spring)", "Greatly increases the amount of insects caught in Spring.", icon_insect_catching, frame_iii, modifier_spring);

            BUG_CATCHING_IV = new Effect("Bug Catching IV", "Hugely increases the amount of insects caught.", icon_insect_catching, frame_iv, none);
            BUG_CATCHING_IV_MORNING = new Effect("Bug Catching IV (Morning)", "Hugely increases the amount of insects caught in the morning.", icon_insect_catching, frame_iv, modifier_morning);
            BUG_CATCHING_IV_NIGHT = new Effect("Bug Catching IV (Night)", "Hugely increases the amount of insects caught during nighttime.", icon_insect_catching, frame_iv, modifier_night);
            BUG_CATCHING_IV_SUMMER = new Effect("Bug Catching IV (Summer)", "Hugely increases the amount of insects caught in Summer.", icon_insect_catching, frame_iv, modifier_summer);
            BUG_CATCHING_IV_SPRING = new Effect("Bug Catching IV (Spring)", "Hugely increases the amount of insects caught in Spring.", icon_insect_catching, frame_iv, modifier_spring);

            BUG_CATCHING_V = new Effect("Bug Catching V", "Massively increases the amount of insects caught.", icon_insect_catching, frame_v, none);

            BUG_CATCHING_VI = new Effect("Bug Catching VI", "Absurdly increases the amount of insects caught.", icon_insect_catching, frame_vi, none);
            BUG_CATCHING_VI_NIGHT = new Effect("Bug Catching VI (Night)", "Absurdly increases the amount of insects caught during nighttime.", icon_insect_catching, frame_vi, modifier_night);
            BUG_CATCHING_VI_AUTUMN = new Effect("Bug Catching VI (Autumn)", "Absurdly increases the amount of insects caught in Autumn.", icon_insect_catching, frame_vi, modifier_autumn);
            BUG_CATCHING_VI_SPRING = new Effect("Bug Catching VI (Spring)", "Absurdly increases the amount of insects caught in Spring.", icon_insect_catching, frame_vi, modifier_spring);
            BUG_CATCHING_VI_SUMMER = new Effect("Bug Catching VI (Summer)", "Absurdly increases the amount of insects caught in Summer.", icon_insect_catching, frame_vi, modifier_summer);

            //increases mined items by 0.5x per level
            MINING_I = new Effect("Mining I", "Slightly increases both the speed of mining and the quantity of items acquired.", icon_mining, frame_i, none);

            MINING_II = new Effect("Mining II", "Increases both the speed of mining and the quantity of items acquired.", icon_mining, frame_ii, none);

            MINING_III = new Effect("Mining III", "Greatly increases both the speed of mining and the quantity of items acquired.", icon_mining, frame_iii, none);
            MINING_III_AUTUMN = new Effect("Mining III (Autumn)", "Greatly increases both the speed of mining and the quantity of items acquired in Autumn.", icon_mining, frame_iii, modifier_autumn);

            MINING_IV = new Effect("Mining IV", "Hugely increases both the speed of mining and the quantity of items acquired.", icon_mining, frame_iv, none);

            MINING_V = new Effect("Mining V", "Massively increases both the speed of mining and the quantity of items acquired.", icon_mining, frame_v, none);

            MINING_VI = new Effect("Mining VI", "Absurdly increases both the speed of mining and the quantity of items acquired.", icon_mining, frame_vi, none);
            MINING_VI_AUTUMN = new Effect("Mining VI (Autumn)", "Absurdly increases both the speed of mining and the quantity of items acquired in Autumn.", icon_mining, frame_vi, modifier_autumn);

            //increases maximum movement speed
            SPEED_I = new Effect("Speed I", "Slightly increases movement speed.", icon_speed, frame_ii, none);
            SPEED_I_MORNING = new Effect("Speed I (Morning)", "Slightly increases movement speed during the morning.", icon_speed, frame_i, modifier_morning);
            SPEED_I_SPRING = new Effect("Speed I", "Slightly increases movement speed during Spring.", icon_speed, frame_i, modifier_spring);
            SPEED_I_SUMMER = new Effect("Speed I", "Slightly increases movement speed during Summer.", icon_speed, frame_i, modifier_summer);
            SPEED_I_AUTUMN = new Effect("Speed I", "Slightly increases movement speed during Autumn.", icon_speed, frame_i, modifier_autumn);

            SPEED_II = new Effect("Speed II", "Increases movement speed.", icon_speed, frame_ii, none);
            SPEED_II_MORNING = new Effect("Speed II (Morning)", "Increases movement speed during the morning.", icon_speed, frame_ii, modifier_morning);
            SPEED_II_SUMMER = new Effect("Speed II (Summer)", "Increases movement speed during Summer.", icon_speed, frame_ii, modifier_summer);
            SPEED_II_SPRING = new Effect("Speed II (Spring)", "Increases movement speed during Spring.", icon_speed, frame_ii, modifier_spring);
            SPEED_II_AUTUMN = new Effect("Speed II (Autumn)", "Increases movement speed during Autumn.", icon_speed, frame_ii, modifier_autumn);

            SPEED_III = new Effect("Speed III", "Greatly increases movement speed.", icon_speed, frame_iii, none);
            SPEED_III_SPRING = new Effect("Speed III (Spring)", "Greatly increases movement speed during Spring", icon_speed, frame_iii, modifier_spring);
            SPEED_III_SUMMER = new Effect("Speed III (Summer)", "Greatly increases movement speed during Summer.", icon_speed, frame_iii, modifier_summer);
            SPEED_III_AUTUMN = new Effect("Speed III (Autumn)", "Greatly increases movement speed during Autumn.", icon_speed, frame_iii, modifier_autumn);
            SPEED_III_WINTER = new Effect("Speed III (Winter)", "Greatly increases movement speed during Winter.", icon_speed, frame_iii, modifier_winter);
            SPEED_III_MORNING = new Effect("Speed III (Morning)", "Greatly increases movement speed in the morning.", icon_speed, frame_iii, modifier_morning);
           
            SPEED_IV = new Effect("Speed IV", "Hugely increases movement speed.", icon_speed, frame_iv, none);
            SPEED_IV_AUTUMN = new Effect("Speed IV (Autumn)", "Hugely increases movement speed during Autumn.", icon_speed, frame_iv, modifier_autumn);
            SPEED_IV_SPRING = new Effect("Speed IV (Spring)", "Hugely increases movement speed during Spring.", icon_speed, frame_iv, modifier_spring);
            SPEED_IV_WINTER = new Effect("Speed IV (Winter)", "Hugely increases movement speed during Winter.", icon_speed, frame_iv, modifier_winter);
            SPEED_IV_MORNING = new Effect("Speed IV (Morning)", "Hugely increases movement speed in the morning.", icon_speed, frame_iv, modifier_morning);

            SPEED_V = new Effect("Speed V", "Massively increases movement speed.", icon_speed, frame_v, none);
            SPEED_V_WINTER = new Effect("Speed V (Winter)", "Massively increases movement speed during Winter.", icon_speed, frame_v, modifier_winter);
            
            SPEED_VI = new Effect("Speed VI", "Absurdly increases movement speed.", icon_speed, frame_vi, none);

            //luck
            //chop, mine, insect, fish, forage - attempts to rerolls  lowest value item 0.5xLevel times, keeping if higher value
            LUCK_I = new Effect("Luck I", "Slightly increases the rarity of items found when doing anything!", icon_luck, frame_i, none);
            LUCK_II = new Effect("Luck II", "Increases the rarity of items found when doing anything!", icon_luck, frame_ii, none);
            LUCK_III = new Effect("Luck III", "Greatly increases the rarity of items found when doing anything!", icon_luck, frame_iii, none);
            LUCK_IV = new Effect("Luck IV", "Hugely increases the rarity of items found when doing anything!", icon_luck, frame_iv, none);
            LUCK_V = new Effect("Luck V", "Massively increases the rarity of items found when doing anything!", icon_luck, frame_v, none);
            LUCK_VI = new Effect("Luck VI", "Absurdly increases the rarity of items found when doing anything!", icon_luck, frame_vi, none);

            //dizzy - makes you worse at everything
            DIZZY = new Effect("Dizzy", "Makes you extremely ineffective when using tools.", icon_dizzy, frame_special_black, none);

            //blessed - one time use, gurantees the rarest drop
            BLESSED = new Effect("Blessed", "You feel exceedingly fortunate at the moment.", icon_blessed, frame_special_gold, none);
            
            //bewitched - gives a random V boost at all times
            BEWITCHED = new Effect("Bewitched", "Your abilities seem to be shifting unpredictably...", icon_bewitched, frame_special_black, none);

            //wishboat: growth - increases crop growth rate/quality
            //wishboat: love - increases points given when gifting
            //wishboat: health - increases damage dealt with all tools
            WISHBOAT_HEALTH = new Effect("Wish for Health", "Your wish has come true!\nIncreases crop growth rate and health.\nIncreases aptitude with all tools.", icon_wishboat_health, frame_special_gold, none, false);
            WISHBOAT_LOVE = new Effect("Wish for Love", "Your wish has come true!\nYour friendships will strengthen quicker!", icon_wishboat_love, frame_special_gold, none, false);
            WISHBOAT_FORTUNE = new Effect("Wish for Fortune", "Your wish has come true!\nYou'll be just a bit more lucky with anything you do.", icon_wishboat_fortune, frame_special_gold, none, false);

            PERFUME_AUTUMNS_KISS = new Effect("Perfumed - Autumnal", "Your scent is reminiscent of wood and leaves.", icon_perfume_autumns_kiss, frame_special_pink, none, false);
            PERFUME_BIZARRE_PERFUME = new Effect("Perfumed - Bizarre", "Your scent is reminiscent of... something.", icon_perfume_bizarre_perfume, frame_special_pink, none, false);
            PERFUME_BLISSFUL_SKY = new Effect("Perfumed - Sky", "Your scent is reminiscent of drifting clouds.", icon_perfume_blissful_sky, frame_special_pink, none, false);
            PERFUME_FLORAL_PERFUME = new Effect("Perfumed - Floral", "Your scent is reminiscent of soft flowers.", icon_perfume_floral_perfume, frame_special_pink, none, false);
            PERFUME_OCEAN_GUST = new Effect("Perfumed - Nautical", "Your scent is reminiscent of a salty breeze.", icon_perfume_ocean_gust, frame_special_pink, none, false);
            PERFUME_RED_ANGEL = new Effect("Perfumed - Divine", "Your scent is reminiscent of sacred perfection.", icon_perfume_red_angel, frame_special_pink, none, false);
            PERFUME_SUMMERS_GIFT = new Effect("Perfumed - Estival", "Your scent is reminiscent of summer wildflowers.", icon_perfume_summers_gift, frame_special_pink, none, false);
            PERFUME_SWEET_BREEZE = new Effect("Perfumed - Sweet", "Your scent is reminiscent of sugar and candy.", icon_perfume_sweet_breeze, frame_special_pink, none, false);
            PERFUME_WARM_MEMORIES = new Effect("Perfumed - Memorable", "Your scent is reminiscent of warmer times.", icon_perfume_warm_memories, frame_special_pink, none, false);

            PERFUMES = new Effect[] { PERFUME_AUTUMNS_KISS, PERFUME_BIZARRE_PERFUME, PERFUME_BLISSFUL_SKY, PERFUME_FLORAL_PERFUME, PERFUME_OCEAN_GUST, PERFUME_RED_ANGEL, PERFUME_SUMMERS_GIFT, PERFUME_SWEET_BREEZE, PERFUME_WARM_MEMORIES };
        }
    }
}
