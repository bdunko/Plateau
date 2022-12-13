using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class SaveState
    {
        public enum Identifier
        {
            PLAYER, PLACEABLE, AREA, WORLD, GAMESTATE, BUILDING_BLOCK, WALL_ENTITY, VENDORENTITY, SHIPPING_BIN, CUTSCENES, CHARACTER, SOULCHEST, CONFIG
        }

        private Identifier StringToIdentifier(string str)
        {
            foreach(Identifier id in Enum.GetValues(typeof(Identifier)))
            {
                if(id.ToString() == str)
                {
                    return id;
                }
            }

            throw new Exception("No identifier found for string.");
        }

        private Dictionary<string, string> saveData;
        private Identifier id;

        public SaveState(Identifier id)
        {
            this.id = id;
            this.saveData = new Dictionary<string, string>();
        }

        public SaveState(string data)
        {
            this.saveData = new Dictionary<string, string>();
            string[] spData = data.Split('{');
            this.id = StringToIdentifier(spData[0]);
            string[] dataPieces = data.Split('{')[1].Split('[');
            for(int i = 1; i < dataPieces.Length; i++)
            {
                string[] pair = dataPieces[i].Split(',');
                AddData(pair[0], pair[1].Substring(0, pair[1].IndexOf("]")));
            }
        }

        public Identifier GetIdentifier()
        {
            return id;
        }

        public Dictionary<string, string>.KeyCollection KeySet()
        {
            return saveData.Keys;
        }

        public void AddData(string name, string data)
        {
            saveData[name] = data;
        }

        public bool ContainsData(string name)
        {
            return saveData.ContainsKey(name);
        }

        public string TryGetData(string name, string defaultValue)
        {
            if(saveData.ContainsKey(name))
            {
                return saveData[name];
            }
            return defaultValue;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(id.ToString());
            sb.Append("{");
            foreach(string key in saveData.Keys)
            {
                sb.Append("[").Append(key).Append(",").Append(saveData[key]).Append("]");
            }
            sb.Append("}");
            return sb.ToString();
        }

    }
}
