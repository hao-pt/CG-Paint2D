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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.openGLControl = new SharpGL.OpenGLControl();
			this.bt_Line = new System.Windows.Forms.Button();
			this.Icon_List = new System.Windows.Forms.ImageList(this.components);
			this.bt_Circle = new System.Windows.Forms.Button();
			this.bt_Palette = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.lb_Time = new System.Windows.Forms.Label();
			this.tb_Time = new System.Windows.Forms.TextBox();
			this.bt_Flood_Fill = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.bt_Rec = new System.Windows.Forms.Button();
			this.bt_Triangle = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openGLControl
			// 
			this.openGLControl.Cursor = System.Windows.Forms.Cursors.Cross;
			this.openGLControl.DrawFPS = false;
			this.openGLControl.Location = new System.Drawing.Point(0, 109);
			this.openGLControl.Name = "openGLControl";
			this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
			this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
			this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
			this.openGLControl.Size = new System.Drawing.Size(1368, 642);
			this.openGLControl.TabIndex = 0;
			this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
			this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
			this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
			this.openGLControl.Load += new System.EventHandler(this.openGLControl1_Load);
			this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrl_OpenGLControl_MouseDown);
			this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrl_OpenGLControl_MouseMove);
			this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrl_OpenGLControl_MouseUp);
			// 
			// bt_Line
			// 
			this.bt_Line.ForeColor = System.Drawing.Color.Black;
			this.bt_Line.ImageIndex = 0;
			this.bt_Line.ImageList = this.Icon_List;
			this.bt_Line.Location = new System.Drawing.Point(12, 10);
			this.bt_Line.Name = "bt_Line";
			this.bt_Line.Size = new System.Drawing.Size(30, 30);
			this.bt_Line.TabIndex = 1;
			this.bt_Line.UseVisualStyleBackColor = true;
			this.bt_Line.Click += new System.EventHandler(this.bt_Line_Click);
			// 
			// Icon_List
			// 
			this.Icon_List.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icon_List.ImageStream")));
			this.Icon_List.TransparentColor = System.Drawing.Color.Transparent;
			this.Icon_List.Images.SetKeyName(0, "icons8-line-96.png");
			this.Icon_List.Images.SetKeyName(1, "if_button_shape_triangle_352894.png");
			this.Icon_List.Images.SetKeyName(2, "icons8-rectangular-100.png");
			this.Icon_List.Images.SetKeyName(3, "if_check-circle-outline-blank_326565.png");
			this.Icon_List.Images.SetKeyName(4, "Actions-fill-color-icon.png");
			// 
			// bt_Circle
			// 
			this.bt_Circle.ImageIndex = 3;
			this.bt_Circle.ImageList = this.Icon_List;
			this.bt_Circle.Location = new System.Drawing.Point(48, 10);
			this.bt_Circle.Name = "bt_Circle";
			this.bt_Circle.Size = new System.Drawing.Size(30, 30);
			this.bt_Circle.TabIndex = 2;
			this.bt_Circle.UseVisualStyleBackColor = true;
			// 
			// bt_Palette
			// 
			this.bt_Palette.Location = new System.Drawing.Point(1279, 7);
			this.bt_Palette.Name = "bt_Palette";
			this.bt_Palette.Size = new System.Drawing.Size(75, 23);
			this.bt_Palette.TabIndex = 3;
			this.bt_Palette.Text = "Palette";
			this.bt_Palette.UseVisualStyleBackColor = true;
			this.bt_Palette.Click += new System.EventHandler(this.bt_Palette_Click);
			// 
			// lb_Time
			// 
			this.lb_Time.AutoSize = true;
			this.lb_Time.Location = new System.Drawing.Point(1215, 84);
			this.lb_Time.Name = "lb_Time";
			this.lb_Time.Size = new System.Drawing.Size(36, 13);
			this.lb_Time.TabIndex = 4;
			this.lb_Time.Text = "Time: ";
			// 
			// tb_Time
			// 
			this.tb_Time.HideSelection = false;
			this.tb_Time.Location = new System.Drawing.Point(1254, 81);
			this.tb_Time.Name = "tb_Time";
			this.tb_Time.Size = new System.Drawing.Size(100, 20);
			this.tb_Time.TabIndex = 7;
			this.tb_Time.Text = "00:00.000000";
			this.tb_Time.TextChanged += new System.EventHandler(this.openGLControl1_Load);
			// 
			// bt_Flood_Fill
			// 
			this.bt_Flood_Fill.BackColor = System.Drawing.SystemColors.ControlLight;
			this.bt_Flood_Fill.ForeColor = System.Drawing.Color.Black;
			this.bt_Flood_Fill.ImageIndex = 4;
			this.bt_Flood_Fill.ImageList = this.Icon_List;
			this.bt_Flood_Fill.Location = new System.Drawing.Point(298, 10);
			this.bt_Flood_Fill.Name = "bt_Flood_Fill";
			this.bt_Flood_Fill.Size = new System.Drawing.Size(45, 45);
			this.bt_Flood_Fill.TabIndex = 8;
			this.bt_Flood_Fill.UseVisualStyleBackColor = false;
			this.bt_Flood_Fill.Click += new System.EventHandler(this.bt_Flood_Fill_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.bt_Rec);
			this.panel1.Controls.Add(this.bt_Triangle);
			this.panel1.Controls.Add(this.bt_Line);
			this.panel1.Controls.Add(this.bt_Palette);
			this.panel1.Controls.Add(this.tb_Time);
			this.panel1.Controls.Add(this.lb_Time);
			this.panel1.Controls.Add(this.bt_Flood_Fill);
			this.panel1.Controls.Add(this.bt_Circle);
			this.panel1.Location = new System.Drawing.Point(0, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1368, 110);
			this.panel1.TabIndex = 9;
			// 
			// bt_Rec
			// 
			this.bt_Rec.ImageIndex = 2;
			this.bt_Rec.ImageList = this.Icon_List;
			this.bt_Rec.Location = new System.Drawing.Point(84, 10);
			this.bt_Rec.Name = "bt_Rec";
			this.bt_Rec.Size = new System.Drawing.Size(30, 30);
			this.bt_Rec.TabIndex = 12;
			this.bt_Rec.UseVisualStyleBackColor = true;
			this.bt_Rec.Click += new System.EventHandler(this.bt_Rec_Click);
			// 
			// bt_Triangle
			// 
			this.bt_Triangle.ImageIndex = 1;
			this.bt_Triangle.ImageList = this.Icon_List;
			this.bt_Triangle.Location = new System.Drawing.Point(120, 10);
			this.bt_Triangle.Name = "bt_Triangle";
			this.bt_Triangle.Size = new System.Drawing.Size(30, 30);
			this.bt_Triangle.TabIndex = 11;
			this.bt_Triangle.UseVisualStyleBackColor = true;
			this.bt_Triangle.Click += new System.EventHandler(this.bt_Triangle_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1366, 745);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.openGLControl);
			this.Name = "Form1";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenGLControl openGLControl;
		private System.Windows.Forms.Button bt_Line;
		private System.Windows.Forms.Button bt_Circle;
		private System.Windows.Forms.Button bt_Palette;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Label lb_Time;
		private System.Windows.Forms.TextBox tb_Time;
		private System.Windows.Forms.Button bt_Flood_Fill;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ImageList Icon_List;
		private System.Windows.Forms.Button bt_Triangle;
		private System.Windows.Forms.Button bt_Rec;
	}
}

