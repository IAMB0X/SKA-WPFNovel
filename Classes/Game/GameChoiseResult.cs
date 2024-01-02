using SKA_Novel.Classes.Technical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SKA_Novel.Classes.Game
{
	[Serializable]
	public class GameChoiseResult
	{
		public string Title { get; set; }
		public string Result { get; set; }

		public GameChoiseResult(string title, string result)
		{
			Title = title;
			Result = result;
        }
	}
}
