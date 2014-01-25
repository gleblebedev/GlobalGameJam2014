using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game.Model;

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

			var s = new Setup();
			Application.Run(s);

			if (s.DialogResult == DialogResult.OK)
			{
	
				var gameWindow = new GameWindow();
				Application.Idle += (sender, e) =>
					{ gameWindow.UpdateGame(); };
				Application.Run(gameWindow);
			}
        }

    }
}
