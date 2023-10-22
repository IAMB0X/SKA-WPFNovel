using SKA_Novel.Classes.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKA_Novel.Classes.Game
{
    internal class Character
    {
        public string FullName { get; }
        public string ShortName { get; }

        public Character(string characterName)
        {
            FullName = characterName;
            ShortName = characterName.Substring(0, 3).Trim().ToUpper();
        }
    }
}
