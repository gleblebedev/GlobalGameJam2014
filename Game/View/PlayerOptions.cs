using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game.Model;

namespace Game
{
	public partial class PlayerOptions : UserControl
	{
		private int playerIndex;

		private ControlType control;

		public PlayerOptions()
		{
			InitializeComponent();
		}

		public int PlayerIndex
		{
			get
			{
				return this.playerIndex;
			}
			set
			{
				this.playerIndex = value;
				this.groupBox1.Text = "Player " + (value + 1);
			}
		}

		public ControlType Control
		{
			get
			{
				return (ControlType)controlSelection.SelectedIndex ;
			}
			set
			{
				controlSelection.SelectedIndex = (int)value;
			}
		}
	}
}
