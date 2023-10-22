using SKA_Novel.Classes.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SKA_Novel.Classes.Technical
{
    internal class ControlsManager
    {
        public static DockPanel EffectScreen { get; set; }
        public static MainWindow AppMainWindow { get; set; }
        public static MediaElement BackgroundVideo { get; set; }
        public static MediaElement Cutscene { get; set; }
        public static DockPanel DarkScreen { get; set; }
        public static Frame MainMenuFrame { get; set; }
        public static StackPanel OptionPanel { get; set; }
        public static Border MainTextPanel { get; set; }
        public static TextBlock MainText { get; set; }
        public static TextBlock SpeakerName { get; set; }
        public static DockPanel[] HeroPositions { get; set; } = new DockPanel[3];
        public static TypingTimer TypingTimer { get; set; }
        public static DispatcherTimer DarkScreenTimer { get; set; } = new DispatcherTimer()
                                                                            { Interval = TimeSpan.FromMilliseconds(10) };

		public static Boolean IsGameStarted { get; set; } = false;

        public static int KarmaLevel { get; set; } = 0;
        public static List<GameChoiseResult> OptionResults { get; set; } = new List<GameChoiseResult>();

    }
}
