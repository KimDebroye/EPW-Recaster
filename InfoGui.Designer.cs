
namespace EPW_Recaster
{
    partial class InfoGui
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
            this.rTxtBoxInfo = new System.Windows.Forms.RichTextBox();
            this.btnOcr = new MetroFramework.Controls.MetroButton();
            this.btnChainForms = new MetroFramework.Controls.MetroButton();
            this.chkbxPreviewCapture = new MetroFramework.Controls.MetroCheckBox();
            this.numMaxRolls = new System.Windows.Forms.NumericUpDown();
            this.lblMaxRolls = new MetroFramework.Controls.MetroLabel();
            this.btnLogFolder = new MetroFramework.Controls.MetroButton();
            this.toolTip = new MetroFramework.Components.MetroToolTip();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRolls)).BeginInit();
            this.SuspendLayout();
            // 
            // rTxtBoxInfo
            // 
            this.rTxtBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTxtBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rTxtBoxInfo.Location = new System.Drawing.Point(25, 71);
            this.rTxtBoxInfo.Margin = new System.Windows.Forms.Padding(15, 30, 15, 15);
            this.rTxtBoxInfo.Name = "rTxtBoxInfo";
            this.rTxtBoxInfo.ReadOnly = true;
            this.rTxtBoxInfo.Size = new System.Drawing.Size(250, 115);
            this.rTxtBoxInfo.TabIndex = 2;
            this.rTxtBoxInfo.Text = "";
            this.rTxtBoxInfo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rTxtBoxInfo_MouseClick);
            this.rTxtBoxInfo.TextChanged += new System.EventHandler(this.rTxtBoxInfo_TextChanged);
            // 
            // btnOcr
            // 
            this.btnOcr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOcr.Location = new System.Drawing.Point(220, 203);
            this.btnOcr.Name = "btnOcr";
            this.btnOcr.Size = new System.Drawing.Size(55, 23);
            this.btnOcr.TabIndex = 3;
            this.btnOcr.Text = "Start";
            this.btnOcr.UseSelectable = true;
            this.btnOcr.Click += new System.EventHandler(this.btnOcr_Click);
            // 
            // btnChainForms
            // 
            this.btnChainForms.FontWeight = MetroFramework.MetroButtonWeight.Light;
            this.btnChainForms.Location = new System.Drawing.Point(0, 5);
            this.btnChainForms.Name = "btnChainForms";
            this.btnChainForms.Size = new System.Drawing.Size(50, 25);
            this.btnChainForms.TabIndex = 4;
            this.btnChainForms.Text = "▉ 🧲 ▉";
            this.btnChainForms.UseSelectable = true;
            this.btnChainForms.Click += new System.EventHandler(this.btnChainForms_Click);
            // 
            // chkbxPreviewCapture
            // 
            this.chkbxPreviewCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkbxPreviewCapture.AutoSize = true;
            this.chkbxPreviewCapture.Location = new System.Drawing.Point(23, 207);
            this.chkbxPreviewCapture.Name = "chkbxPreviewCapture";
            this.chkbxPreviewCapture.Size = new System.Drawing.Size(64, 15);
            this.chkbxPreviewCapture.TabIndex = 5;
            this.chkbxPreviewCapture.Text = "Preview";
            this.chkbxPreviewCapture.UseSelectable = true;
            this.chkbxPreviewCapture.CheckStateChanged += new System.EventHandler(this.chkbxPreviewCapture_CheckStateChanged);
            // 
            // numMaxRolls
            // 
            this.numMaxRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numMaxRolls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numMaxRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMaxRolls.Location = new System.Drawing.Point(165, 204);
            this.numMaxRolls.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numMaxRolls.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxRolls.Name = "numMaxRolls";
            this.numMaxRolls.Size = new System.Drawing.Size(49, 21);
            this.numMaxRolls.TabIndex = 13;
            this.numMaxRolls.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numMaxRolls.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMaxRolls.ValueChanged += new System.EventHandler(this.numMaxRolls_ValueChanged);
            // 
            // lblMaxRolls
            // 
            this.lblMaxRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxRolls.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblMaxRolls.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblMaxRolls.Location = new System.Drawing.Point(93, 203);
            this.lblMaxRolls.Name = "lblMaxRolls";
            this.lblMaxRolls.Size = new System.Drawing.Size(72, 22);
            this.lblMaxRolls.TabIndex = 15;
            this.lblMaxRolls.Text = "Roll Limit";
            this.lblMaxRolls.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLogFolder
            // 
            this.btnLogFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFolder.Location = new System.Drawing.Point(277, 5);
            this.btnLogFolder.Name = "btnLogFolder";
            this.btnLogFolder.Size = new System.Drawing.Size(23, 23);
            this.btnLogFolder.TabIndex = 16;
            this.btnLogFolder.Text = "📁";
            this.btnLogFolder.UseSelectable = true;
            this.btnLogFolder.Click += new System.EventHandler(this.btnLogFolder_Click);
            // 
            // toolTip
            // 
            this.toolTip.Style = MetroFramework.MetroColorStyle.Blue;
            this.toolTip.StyleManager = null;
            this.toolTip.Theme = MetroFramework.MetroThemeStyle.Default;
            // 
            // InfoGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 255);
            this.ControlBox = false;
            this.Controls.Add(this.btnLogFolder);
            this.Controls.Add(this.numMaxRolls);
            this.Controls.Add(this.lblMaxRolls);
            this.Controls.Add(this.chkbxPreviewCapture);
            this.Controls.Add(this.btnChainForms);
            this.Controls.Add(this.btnOcr);
            this.Controls.Add(this.rTxtBoxInfo);
            this.Movable = false;
            this.Name = "InfoGui";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.InfoGui_Load);
            this.LocationChanged += new System.EventHandler(this.InfoGui_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.InfoGui_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRolls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.RichTextBox rTxtBoxInfo;
        internal MetroFramework.Controls.MetroButton btnOcr;
        internal MetroFramework.Controls.MetroButton btnChainForms;
        internal MetroFramework.Controls.MetroCheckBox chkbxPreviewCapture;
        internal MetroFramework.Controls.MetroLabel lblMaxRolls;
        internal System.Windows.Forms.NumericUpDown numMaxRolls;
        internal MetroFramework.Controls.MetroButton btnLogFolder;
        internal MetroFramework.Components.MetroToolTip toolTip;
    }
}