using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game.Model;

namespace Game
{
	public partial class Setup : Form
	{
		public Setup()
		{
			InitializeComponent();
			playerOptions1.PlayerIndex = 0;
			playerOptions1.Control = ControlType.WASD;
			playerOptions2.PlayerIndex = 0;
			playerOptions2.Control = ControlType.Gamepad2;
		}
	}
}
