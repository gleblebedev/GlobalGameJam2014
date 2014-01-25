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
		private readonly GameOptions options;

		public Setup():this(new GameOptions())
		{
			
		}
		public Setup(GameOptions options)
		{
			this.options = options;
			InitializeComponent();
			playerOptions1.PlayerIndex = 0;
			playerOptions1.Control = ControlType.WASD;
			playerOptions1.Eyes = EyesCount.Spider;
			playerOptions2.PlayerIndex = 0;
			playerOptions2.Control = ControlType.Gamepad2;
			playerOptions2.Eyes = EyesCount.OneEye;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
