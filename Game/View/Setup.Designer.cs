namespace Game
{
	partial class Setup
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.playerOptions1 = new Game.PlayerOptions();
			this.playerOptions2 = new Game.PlayerOptions();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(197, 226);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Start!";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// playerOptions1
			// 
			this.playerOptions1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.playerOptions1.Location = new System.Drawing.Point(13, 13);
			this.playerOptions1.Name = "playerOptions1";
			this.playerOptions1.Size = new System.Drawing.Size(259, 99);
			this.playerOptions1.TabIndex = 1;
			// 
			// playerOptions2
			// 
			this.playerOptions2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.playerOptions2.Location = new System.Drawing.Point(13, 121);
			this.playerOptions2.Name = "playerOptions2";
			this.playerOptions2.Size = new System.Drawing.Size(259, 99);
			this.playerOptions2.TabIndex = 2;
			// 
			// Setup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.playerOptions2);
			this.Controls.Add(this.playerOptions1);
			this.Controls.Add(this.button1);
			this.Name = "Setup";
			this.Text = "Setup";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private PlayerOptions playerOptions1;
		private PlayerOptions playerOptions2;
	}
}