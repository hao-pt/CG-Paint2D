namespace SharpGL
{
	partial class Form1
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
			this.openGLControl = new SharpGL.OpenGLControl();
			this.bt_Line = new System.Windows.Forms.Button();
			this.bt_Circle = new System.Windows.Forms.Button();
			this.bt_Palette = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
			this.SuspendLayout();
			// 
			// openGLControl
			// 
			this.openGLControl.DrawFPS = false;
			this.openGLControl.Location = new System.Drawing.Point(0, 41);
			this.openGLControl.Name = "openGLControl";
			this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
			this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
			this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
			this.openGLControl.Size = new System.Drawing.Size(751, 374);
			this.openGLControl.TabIndex = 0;
			this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
			this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
			this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
			this.openGLControl.Load += new System.EventHandler(this.openGLControl1_Load);
			this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrl_OpenGLControl_MouseDown);
			this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrl_OpenGLControl_MouseUp);
			// 
			// bt_Line
			// 
			this.bt_Line.Location = new System.Drawing.Point(12, 9);
			this.bt_Line.Name = "bt_Line";
			this.bt_Line.Size = new System.Drawing.Size(75, 23);
			this.bt_Line.TabIndex = 1;
			this.bt_Line.Text = "Line";
			this.bt_Line.UseVisualStyleBackColor = true;
			this.bt_Line.Click += new System.EventHandler(this.bt_Line_Click);
			// 
			// bt_Circle
			// 
			this.bt_Circle.Location = new System.Drawing.Point(93, 9);
			this.bt_Circle.Name = "bt_Circle";
			this.bt_Circle.Size = new System.Drawing.Size(75, 23);
			this.bt_Circle.TabIndex = 2;
			this.bt_Circle.Text = "Circle";
			this.bt_Circle.UseVisualStyleBackColor = true;
			// 
			// bt_Palette
			// 
			this.bt_Palette.Location = new System.Drawing.Point(664, 9);
			this.bt_Palette.Name = "bt_Palette";
			this.bt_Palette.Size = new System.Drawing.Size(75, 23);
			this.bt_Palette.TabIndex = 3;
			this.bt_Palette.Text = "Palette";
			this.bt_Palette.UseVisualStyleBackColor = true;
			this.bt_Palette.Click += new System.EventHandler(this.bt_Palette_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(751, 415);
			this.Controls.Add(this.bt_Palette);
			this.Controls.Add(this.bt_Circle);
			this.Controls.Add(this.bt_Line);
			this.Controls.Add(this.openGLControl);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenGLControl openGLControl;
		private System.Windows.Forms.Button bt_Line;
		private System.Windows.Forms.Button bt_Circle;
		private System.Windows.Forms.Button bt_Palette;
		private System.Windows.Forms.ColorDialog colorDialog1;
	}
}

