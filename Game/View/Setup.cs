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
			playerOptions1.Control = options.Players[0].Control;
			playerOptions1.Eyes = options.Players[0].Eyes;
			playerOptions2.PlayerIndex = 1;
			playerOptions2.Control = options.Players[1].Control;
			playerOptions2.Eyes = options.Players[1].Eyes;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			options.Players[0].Control = playerOptions1.Control;
			options.Players[0].Eyes = playerOptions1.Eyes;
			options.Players[1].Control = playerOptions2.Control;
			options.Players[1].Eyes = playerOptions2.Eyes;
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
