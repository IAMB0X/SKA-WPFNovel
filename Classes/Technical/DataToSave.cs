using SKA_Novel.Classes.Game;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKA_Novel.Classes.Technical
{
	[Serializable]
	public class DataToSave
	{
		public readonly string SaveName;
		public readonly string File;
		public readonly string Background;
		public readonly int Line;
		public readonly int KarmaLevel;
		public readonly DateTime Time;
		public readonly List<GameChoiseResult> Results;

		public DataToSave(string saveName, string fileName, string background,
						  int line, int karma, DateTime time,
						  List<GameChoiseResult> results)
		{
			SaveName = saveName;
			File = fileName;
			Background = background;
			Line = line;
			KarmaLevel = karma;
			Time = time;
			Results = results;
		}


	}
}
