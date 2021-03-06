﻿using System;
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
			this.controlSelection.Items.Add("GamePad 1 (" + (OpenTK.Input.GamePad.GetState(0).IsConnected ? "Connected" : "Not connected") + ")");
			this.controlSelection.Items.Add("GamePad 2 (" + (OpenTK.Input.GamePad.GetState(1).IsConnected ? "Connected" : "Not connected") + ")");
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

		public EyesCount Eyes
		{
			get
			{
				return (EyesCount)(comboBox1.SelectedIndex);
			}
			set
			{
				comboBox1.SelectedIndex = (int)value;
			}
		}
	}
}
