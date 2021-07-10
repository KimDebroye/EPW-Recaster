
namespace EPW_Recaster
{
    partial class MainGui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGui));
            this.seeThroughRegion = new System.Windows.Forms.PictureBox();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.lvConditions = new System.Windows.Forms.ListView();
            this.btnAddCondition = new MetroFramework.Controls.MetroButton();
            this.numAmount = new System.Windows.Forms.NumericUpDown();
            this.lblAmount = new MetroFramework.Controls.MetroLabel();
            this.cbTerms = new System.Windows.Forms.ComboBox();
            this.captureRegion = new System.Windows.Forms.PictureBox();
            this.btnRetain = new MetroFramework.Controls.MetroButton();
            this.btnNew = new MetroFramework.Controls.MetroButton();
            this.btnReproduce = new MetroFramework.Controls.MetroButton();
            this.lblCaptureRegion = new System.Windows.Forms.Label();
            this.lblConditionsNote = new MetroFramework.Controls.MetroLabel();
            this.gbConditions = new System.Windows.Forms.GroupBox();
            this.gbSubConditions = new System.Windows.Forms.GroupBox();
            this.cbSubTerms = new System.Windows.Forms.ComboBox();
            this.lblSubConditionsNote = new MetroFramework.Controls.MetroLabel();
            this.lvSubConditions = new System.Windows.Forms.ListView();
            this.btnAddSubCondition = new MetroFramework.Controls.MetroButton();
            this.numSubAmount = new System.Windows.Forms.NumericUpDown();
            this.lblSubAmount = new MetroFramework.Controls.MetroLabel();
            this.toolTip = new MetroFramework.Components.MetroToolTip();
            ((System.ComponentModel.ISupportInitialize)(this.seeThroughRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.captureRegion)).BeginInit();
            this.gbConditions.SuspendLayout();
            this.gbSubConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSubAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // seeThroughRegion
            // 
            this.seeThroughRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seeThroughRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.seeThroughRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.seeThroughRegion.Location = new System.Drawing.Point(23, 77);
            this.seeThroughRegion.Margin = new System.Windows.Forms.Padding(10);
            this.seeThroughRegion.MinimumSize = new System.Drawing.Size(250, 150);
            this.seeThroughRegion.Name = "seeThroughRegion";
            this.seeThroughRegion.Size = new System.Drawing.Size(253, 213);
            this.seeThroughRegion.TabIndex = 2;
            this.seeThroughRegion.TabStop = false;
            this.seeThroughRegion.Resize += new System.EventHandler(this.seeThroughRegion_Resize);
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = null;
            this.metroStyleManager.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lvConditions
            // 
            this.lvConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConditions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvConditions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvConditions.HideSelection = false;
            this.lvConditions.Location = new System.Drawing.Point(12, 26);
            this.lvConditions.Name = "lvConditions";
            this.lvConditions.Size = new System.Drawing.Size(250, 38);
            this.lvConditions.TabIndex = 3;
            this.lvConditions.UseCompatibleStateImageBehavior = false;
            this.lvConditions.Enter += new System.EventHandler(this.lvConditions_Enter);
            // 
            // btnAddCondition
            // 
            this.btnAddCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCondition.Location = new System.Drawing.Point(232, 71);
            this.btnAddCondition.Name = "btnAddCondition";
            this.btnAddCondition.Size = new System.Drawing.Size(30, 21);
            this.btnAddCondition.TabIndex = 4;
            this.btnAddCondition.Text = "➕";
            this.btnAddCondition.UseCustomForeColor = true;
            this.btnAddCondition.UseSelectable = true;
            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);
            // 
            // numAmount
            // 
            this.numAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAmount.Location = new System.Drawing.Point(17, 71);
            this.numAmount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new System.Drawing.Size(33, 21);
            this.numAmount.TabIndex = 12;
            this.numAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAmount.ValueChanged += new System.EventHandler(this.numAmount_ValueChanged);
            this.numAmount.Enter += new System.EventHandler(this.numAmount_Enter);
            // 
            // lblAmount
            // 
            this.lblAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAmount.Location = new System.Drawing.Point(52, 71);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(54, 21);
            this.lblAmount.TabIndex = 14;
            this.lblAmount.Text = "x";
            // 
            // cbTerms
            // 
            this.cbTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTerms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTerms.FormattingEnabled = true;
            this.cbTerms.Location = new System.Drawing.Point(68, 71);
            this.cbTerms.Name = "cbTerms";
            this.cbTerms.Size = new System.Drawing.Size(158, 21);
            this.cbTerms.TabIndex = 15;
            this.cbTerms.DropDownClosed += new System.EventHandler(this.cbTerms_DropDownClosed);
            // 
            // captureRegion
            // 
            this.captureRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.captureRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.captureRegion.Location = new System.Drawing.Point(148, 77);
            this.captureRegion.Name = "captureRegion";
            this.captureRegion.Size = new System.Drawing.Size(128, 160);
            this.captureRegion.TabIndex = 16;
            this.captureRegion.TabStop = false;
            // 
            // btnRetain
            // 
            this.btnRetain.Enabled = false;
            this.btnRetain.Location = new System.Drawing.Point(42, 253);
            this.btnRetain.Name = "btnRetain";
            this.btnRetain.Size = new System.Drawing.Size(10, 10);
            this.btnRetain.TabIndex = 17;
            this.btnRetain.UseSelectable = true;
            // 
            // btnNew
            // 
            this.btnNew.Enabled = false;
            this.btnNew.Location = new System.Drawing.Point(246, 253);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(10, 10);
            this.btnNew.TabIndex = 18;
            this.btnNew.UseSelectable = true;
            // 
            // btnReproduce
            // 
            this.btnReproduce.Enabled = false;
            this.btnReproduce.Location = new System.Drawing.Point(143, 266);
            this.btnReproduce.Name = "btnReproduce";
            this.btnReproduce.Size = new System.Drawing.Size(10, 10);
            this.btnReproduce.TabIndex = 19;
            this.btnReproduce.UseSelectable = true;
            // 
            // lblCaptureRegion
            // 
            this.lblCaptureRegion.AutoSize = true;
            this.lblCaptureRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblCaptureRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaptureRegion.Location = new System.Drawing.Point(145, 221);
            this.lblCaptureRegion.Name = "lblCaptureRegion";
            this.lblCaptureRegion.Size = new System.Drawing.Size(116, 16);
            this.lblCaptureRegion.TabIndex = 20;
            this.lblCaptureRegion.Text = "( Capture Region )";
            // 
            // lblConditionsNote
            // 
            this.lblConditionsNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConditionsNote.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblConditionsNote.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblConditionsNote.Location = new System.Drawing.Point(12, 48);
            this.lblConditionsNote.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblConditionsNote.Name = "lblConditionsNote";
            this.lblConditionsNote.Size = new System.Drawing.Size(243, 19);
            this.lblConditionsNote.TabIndex = 22;
            this.lblConditionsNote.Text = "Add at least one condition to be matched:";
            this.lblConditionsNote.UseCustomBackColor = true;
            this.lblConditionsNote.UseCustomForeColor = true;
            // 
            // gbConditions
            // 
            this.gbConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConditions.Controls.Add(this.cbTerms);
            this.gbConditions.Controls.Add(this.lblConditionsNote);
            this.gbConditions.Controls.Add(this.lvConditions);
            this.gbConditions.Controls.Add(this.btnAddCondition);
            this.gbConditions.Controls.Add(this.numAmount);
            this.gbConditions.Controls.Add(this.lblAmount);
            this.gbConditions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbConditions.ForeColor = System.Drawing.SystemColors.Control;
            this.gbConditions.Location = new System.Drawing.Point(289, 71);
            this.gbConditions.Margin = new System.Windows.Forms.Padding(15);
            this.gbConditions.Name = "gbConditions";
            this.gbConditions.Padding = new System.Windows.Forms.Padding(10);
            this.gbConditions.Size = new System.Drawing.Size(275, 105);
            this.gbConditions.TabIndex = 23;
            this.gbConditions.TabStop = false;
            this.gbConditions.Text = "Must have at least:";
            this.gbConditions.Paint += new System.Windows.Forms.PaintEventHandler(this.gbConditions_Paint);
            // 
            // gbSubConditions
            // 
            this.gbSubConditions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSubConditions.Controls.Add(this.cbSubTerms);
            this.gbSubConditions.Controls.Add(this.lblSubConditionsNote);
            this.gbSubConditions.Controls.Add(this.lvSubConditions);
            this.gbSubConditions.Controls.Add(this.btnAddSubCondition);
            this.gbSubConditions.Controls.Add(this.numSubAmount);
            this.gbSubConditions.Controls.Add(this.lblSubAmount);
            this.gbSubConditions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbSubConditions.ForeColor = System.Drawing.SystemColors.Control;
            this.gbSubConditions.Location = new System.Drawing.Point(289, 185);
            this.gbSubConditions.Margin = new System.Windows.Forms.Padding(15);
            this.gbSubConditions.Name = "gbSubConditions";
            this.gbSubConditions.Padding = new System.Windows.Forms.Padding(10);
            this.gbSubConditions.Size = new System.Drawing.Size(275, 105);
            this.gbSubConditions.TabIndex = 24;
            this.gbSubConditions.TabStop = false;
            this.gbSubConditions.Text = "AND must also have at least:";
            this.gbSubConditions.Paint += new System.Windows.Forms.PaintEventHandler(this.gbSubConditions_Paint);
            // 
            // cbSubTerms
            // 
            this.cbSubTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSubTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubTerms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSubTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSubTerms.FormattingEnabled = true;
            this.cbSubTerms.Location = new System.Drawing.Point(68, 71);
            this.cbSubTerms.Name = "cbSubTerms";
            this.cbSubTerms.Size = new System.Drawing.Size(158, 21);
            this.cbSubTerms.TabIndex = 15;
            this.cbSubTerms.DropDownClosed += new System.EventHandler(this.cbSubTerms_DropDownClosed);
            // 
            // lblSubConditionsNote
            // 
            this.lblSubConditionsNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubConditionsNote.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSubConditionsNote.Location = new System.Drawing.Point(12, 27);
            this.lblSubConditionsNote.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblSubConditionsNote.Name = "lblSubConditionsNote";
            this.lblSubConditionsNote.Size = new System.Drawing.Size(243, 19);
            this.lblSubConditionsNote.TabIndex = 22;
            this.lblSubConditionsNote.Text = "( default | any other stat combination )";
            this.lblSubConditionsNote.UseCustomBackColor = true;
            this.lblSubConditionsNote.UseCustomForeColor = true;
            // 
            // lvSubConditions
            // 
            this.lvSubConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSubConditions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSubConditions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSubConditions.HideSelection = false;
            this.lvSubConditions.Location = new System.Drawing.Point(12, 26);
            this.lvSubConditions.Name = "lvSubConditions";
            this.lvSubConditions.Size = new System.Drawing.Size(250, 38);
            this.lvSubConditions.TabIndex = 3;
            this.lvSubConditions.UseCompatibleStateImageBehavior = false;
            this.lvSubConditions.Enter += new System.EventHandler(this.lvSubConditions_Enter);
            // 
            // btnAddSubCondition
            // 
            this.btnAddSubCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSubCondition.Location = new System.Drawing.Point(232, 71);
            this.btnAddSubCondition.Name = "btnAddSubCondition";
            this.btnAddSubCondition.Size = new System.Drawing.Size(30, 21);
            this.btnAddSubCondition.TabIndex = 4;
            this.btnAddSubCondition.Text = "➕";
            this.btnAddSubCondition.UseCustomForeColor = true;
            this.btnAddSubCondition.UseSelectable = true;
            this.btnAddSubCondition.Click += new System.EventHandler(this.btnAddSubCondition_Click);
            // 
            // numSubAmount
            // 
            this.numSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numSubAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSubAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSubAmount.Location = new System.Drawing.Point(17, 71);
            this.numSubAmount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSubAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubAmount.Name = "numSubAmount";
            this.numSubAmount.Size = new System.Drawing.Size(33, 21);
            this.numSubAmount.TabIndex = 12;
            this.numSubAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSubAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubAmount.ValueChanged += new System.EventHandler(this.numSubAmount_ValueChanged);
            this.numSubAmount.Enter += new System.EventHandler(this.numSubAmount_Enter);
            // 
            // lblSubAmount
            // 
            this.lblSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubAmount.Location = new System.Drawing.Point(52, 71);
            this.lblSubAmount.Name = "lblSubAmount";
            this.lblSubAmount.Size = new System.Drawing.Size(54, 21);
            this.lblSubAmount.TabIndex = 14;
            this.lblSubAmount.Text = "x";
            // 
            // toolTip
            // 
            this.toolTip.Style = MetroFramework.MetroColorStyle.Blue;
            this.toolTip.StyleManager = null;
            this.toolTip.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // MainGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 320);
            this.Controls.Add(this.gbSubConditions);
            this.Controls.Add(this.lblCaptureRegion);
            this.Controls.Add(this.btnReproduce);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnRetain);
            this.Controls.Add(this.captureRegion);
            this.Controls.Add(this.seeThroughRegion);
            this.Controls.Add(this.gbConditions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(585, 320);
            this.Name = "MainGui";
            this.Opacity = 0.85D;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.Text = "EPW Recaster";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainGui_FormClosing);
            this.LocationChanged += new System.EventHandler(this.MainGui_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.MainGui_SizeChanged);
            this.Resize += new System.EventHandler(this.MainGui_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.seeThroughRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.captureRegion)).EndInit();
            this.gbConditions.ResumeLayout(false);
            this.gbSubConditions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSubAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox seeThroughRegion;
        private MetroFramework.Controls.MetroButton btnAddCondition;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.ComboBox cbTerms;
        internal MetroFramework.Controls.MetroLabel lblAmount;
        private System.Windows.Forms.PictureBox captureRegion;
        internal System.Windows.Forms.ListView lvConditions;
        private MetroFramework.Controls.MetroButton btnRetain;
        private MetroFramework.Controls.MetroButton btnNew;
        private MetroFramework.Controls.MetroButton btnReproduce;
        private System.Windows.Forms.Label lblCaptureRegion;
        internal MetroFramework.Controls.MetroLabel lblConditionsNote;
        private System.Windows.Forms.GroupBox gbConditions;
        private System.Windows.Forms.GroupBox gbSubConditions;
        private System.Windows.Forms.ComboBox cbSubTerms;
        internal MetroFramework.Controls.MetroLabel lblSubConditionsNote;
        internal System.Windows.Forms.ListView lvSubConditions;
        private MetroFramework.Controls.MetroButton btnAddSubCondition;
        private System.Windows.Forms.NumericUpDown numSubAmount;
        internal MetroFramework.Controls.MetroLabel lblSubAmount;
        private MetroFramework.Components.MetroToolTip toolTip;
        internal MetroFramework.Components.MetroStyleManager metroStyleManager;
    }
}

