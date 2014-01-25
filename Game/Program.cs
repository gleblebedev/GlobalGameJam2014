using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game.Model;

using Newtonsoft.Json;

using OpenTK.Graphics;

namespace Game
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

	        var options = LoadConfig();

			var s = new Setup(options);
			Application.Run(s);

			if (s.DialogResult == DialogResult.OK)
			{
	
				var gameWindow = new GameWindow(options);
				Application.Idle += (sender, e) =>
					{ gameWindow.UpdateGame(); };
				Application.Run(gameWindow);
			}

			SaveConfig(options);
        }

	    private static void SaveConfig(GameOptions options)
	    {
		    var b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(options, Formatting.Indented));
			File.WriteAllBytes(configFileName,b);
	    }

		static string configFileName = "GameOptions.json";
		private static GameOptions LoadConfig()
	    {
		    GameOptions options;
		    string configStr;
		    try
		    {
			    if (File.Exists(configFileName))
			    {
				    configStr = Encoding.UTF8.GetString(File.ReadAllBytes(configFileName));
			    }
			    else
			    {
				    var ms = new MemoryStream();
					using (var manifestResourceStream = typeof(Program).Assembly.GetManifestResourceStream("Game." + configFileName))
				    {
					    manifestResourceStream.CopyTo(ms);
				    }
				    configStr = Encoding.UTF8.GetString(ms.ToArray());
			    }
				return JsonConvert.DeserializeObject<GameOptions>(configStr);
			}
		    catch (Exception)
		    {
				return new GameOptions();
		    }
	    }
    }
}
