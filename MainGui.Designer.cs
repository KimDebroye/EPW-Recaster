
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
            this.cbSubSubSubTerms = new System.Windows.Forms.ComboBox();
            this.cbSubSubTerms = new System.Windows.Forms.ComboBox();
            this.cbSubTerms = new System.Windows.Forms.ComboBox();
            this.chkbxAnyAmount = new MetroFramework.Controls.MetroCheckBox();
            this.numSubSubAmount = new System.Windows.Forms.NumericUpDown();
            this.numSubAmount = new System.Windows.Forms.NumericUpDown();
            this.lblSubAmount = new MetroFramework.Controls.MetroLabel();
            this.dgConditions = new System.Windows.Forms.DataGridView();
            this.lblSubSubAmount = new MetroFramework.Controls.MetroLabel();
            this.lblSubSubSubAmount = new MetroFramework.Controls.MetroLabel();
            this.numSubSubSubAmount = new System.Windows.Forms.NumericUpDown();
            this.toolTip = new MetroFramework.Components.MetroToolTip();
            this.rbList1 = new MetroFramework.Controls.MetroRadioButton();
            this.rbList2 = new MetroFramework.Controls.MetroRadioButton();
            this.rbList3 = new MetroFramework.Controls.MetroRadioButton();
            this.rbList4 = new MetroFramework.Controls.MetroRadioButton();
            this.rbList5 = new MetroFramework.Controls.MetroRadioButton();
            this.lblCurrentList = new System.Windows.Forms.Label();
            this.cmExportImport = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.seeThroughRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.captureRegion)).BeginInit();
            this.gbConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSubSubAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSubAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgConditions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSubSubSubAmount)).BeginInit();
            this.cmExportImport.SuspendLayout();
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
            this.seeThroughRegion.Size = new System.Drawing.Size(268, 243);
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
            // btnAddCondition
            // 
            this.btnAddCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCondition.Location = new System.Drawing.Point(229, 114);
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
            this.numAmount.Location = new System.Drawing.Point(14, 114);
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
            this.lblAmount.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblAmount.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblAmount.Location = new System.Drawing.Point(50, 117);
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
            this.cbTerms.Location = new System.Drawing.Point(65, 114);
            this.cbTerms.Name = "cbTerms";
            this.cbTerms.Size = new System.Drawing.Size(158, 21);
            this.cbTerms.TabIndex = 15;
            this.cbTerms.SelectedIndexChanged += new System.EventHandler(this.cbTerms_SelectedIndexChanged);
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
            this.lblConditionsNote.Location = new System.Drawing.Point(12, 91);
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
            this.gbConditions.Controls.Add(this.cbSubSubSubTerms);
            this.gbConditions.Controls.Add(this.cbSubSubTerms);
            this.gbConditions.Controls.Add(this.cbSubTerms);
            this.gbConditions.Controls.Add(this.cbTerms);
            this.gbConditions.Controls.Add(this.chkbxAnyAmount);
            this.gbConditions.Controls.Add(this.numSubSubAmount);
            this.gbConditions.Controls.Add(this.numSubAmount);
            this.gbConditions.Controls.Add(this.lblSubAmount);
            this.gbConditions.Controls.Add(this.lblConditionsNote);
            this.gbConditions.Controls.Add(this.btnAddCondition);
            this.gbConditions.Controls.Add(this.numAmount);
            this.gbConditions.Controls.Add(this.lblAmount);
            this.gbConditions.Controls.Add(this.dgConditions);
            this.gbConditions.Controls.Add(this.lblSubSubAmount);
            this.gbConditions.Controls.Add(this.lblSubSubSubAmount);
            this.gbConditions.Controls.Add(this.numSubSubSubAmount);
            this.gbConditions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbConditions.ForeColor = System.Drawing.SystemColors.Control;
            this.gbConditions.Location = new System.Drawing.Point(304, 71);
            this.gbConditions.Margin = new System.Windows.Forms.Padding(15);
            this.gbConditions.Name = "gbConditions";
            this.gbConditions.Padding = new System.Windows.Forms.Padding(10);
            this.gbConditions.Size = new System.Drawing.Size(275, 249);
            this.gbConditions.TabIndex = 23;
            this.gbConditions.TabStop = false;
            this.gbConditions.Text = "Must have :";
            this.gbConditions.Paint += new System.Windows.Forms.PaintEventHandler(this.gbDotted_Paint);
            // 
            // cbSubSubSubTerms
            // 
            this.cbSubSubSubTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSubSubSubTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubSubSubTerms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSubSubSubTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSubSubSubTerms.FormattingEnabled = true;
            this.cbSubSubSubTerms.Location = new System.Drawing.Point(65, 195);
            this.cbSubSubSubTerms.Name = "cbSubSubSubTerms";
            this.cbSubSubSubTerms.Size = new System.Drawing.Size(158, 21);
            this.cbSubSubSubTerms.TabIndex = 32;
            this.cbSubSubSubTerms.SelectedIndexChanged += new System.EventHandler(this.cbSubSubSubTerms_SelectedIndexChanged);
            this.cbSubSubSubTerms.DropDownClosed += new System.EventHandler(this.cbSubSubSubTerms_DropDownClosed);
            // 
            // cbSubSubTerms
            // 
            this.cbSubSubTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSubSubTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubSubTerms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSubSubTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSubSubTerms.FormattingEnabled = true;
            this.cbSubSubTerms.Location = new System.Drawing.Point(65, 168);
            this.cbSubSubTerms.Name = "cbSubSubTerms";
            this.cbSubSubTerms.Size = new System.Drawing.Size(158, 21);
            this.cbSubSubTerms.TabIndex = 28;
            this.cbSubSubTerms.SelectedIndexChanged += new System.EventHandler(this.cbSubSubTerms_SelectedIndexChanged);
            this.cbSubSubTerms.DropDownClosed += new System.EventHandler(this.cbSubSubTerms_DropDownClosed);
            // 
            // cbSubTerms
            // 
            this.cbSubTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSubTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubTerms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSubTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSubTerms.FormattingEnabled = true;
            this.cbSubTerms.Location = new System.Drawing.Point(65, 141);
            this.cbSubTerms.Name = "cbSubTerms";
            this.cbSubTerms.Size = new System.Drawing.Size(158, 21);
            this.cbSubTerms.TabIndex = 15;
            this.cbSubTerms.SelectedIndexChanged += new System.EventHandler(this.cbSubTerms_SelectedIndexChanged);
            this.cbSubTerms.DropDownClosed += new System.EventHandler(this.cbSubTerms_DropDownClosed);
            // 
            // chkbxAnyAmount
            // 
            this.chkbxAnyAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbxAnyAmount.Location = new System.Drawing.Point(14, 222);
            this.chkbxAnyAmount.Name = "chkbxAnyAmount";
            this.chkbxAnyAmount.Size = new System.Drawing.Size(250, 15);
            this.chkbxAnyAmount.TabIndex = 29;
            this.chkbxAnyAmount.Text = "Any combination of selected stats only ?";
            this.chkbxAnyAmount.UseSelectable = true;
            this.chkbxAnyAmount.Visible = false;
            this.chkbxAnyAmount.CheckedChanged += new System.EventHandler(this.chkbxAnyAmount_CheckedChanged);
            this.chkbxAnyAmount.MouseHover += new System.EventHandler(this.chkbxAnyAmount_MouseHover);
            // 
            // numSubSubAmount
            // 
            this.numSubSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numSubSubAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSubSubAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSubSubAmount.Location = new System.Drawing.Point(14, 168);
            this.numSubSubAmount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSubSubAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubSubAmount.Name = "numSubSubAmount";
            this.numSubSubAmount.Size = new System.Drawing.Size(33, 21);
            this.numSubSubAmount.TabIndex = 26;
            this.numSubSubAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSubSubAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubSubAmount.ValueChanged += new System.EventHandler(this.numSubSubAmount_ValueChanged);
            this.numSubSubAmount.Enter += new System.EventHandler(this.numSubSubAmount_Enter);
            // 
            // numSubAmount
            // 
            this.numSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numSubAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSubAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSubAmount.Location = new System.Drawing.Point(14, 141);
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
            this.lblSubAmount.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSubAmount.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblSubAmount.Location = new System.Drawing.Point(50, 144);
            this.lblSubAmount.Name = "lblSubAmount";
            this.lblSubAmount.Size = new System.Drawing.Size(54, 21);
            this.lblSubAmount.TabIndex = 14;
            this.lblSubAmount.Text = "x";
            // 
            // dgConditions
            // 
            this.dgConditions.AllowDrop = true;
            this.dgConditions.AllowUserToAddRows = false;
            this.dgConditions.AllowUserToDeleteRows = false;
            this.dgConditions.AllowUserToResizeColumns = false;
            this.dgConditions.AllowUserToResizeRows = false;
            this.dgConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgConditions.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgConditions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgConditions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgConditions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgConditions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgConditions.ColumnHeadersVisible = false;
            this.dgConditions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgConditions.Location = new System.Drawing.Point(1, 19);
            this.dgConditions.Name = "dgConditions";
            this.dgConditions.ReadOnly = true;
            this.dgConditions.RowHeadersVisible = false;
            this.dgConditions.ShowCellToolTips = false;
            this.dgConditions.Size = new System.Drawing.Size(271, 80);
            this.dgConditions.TabIndex = 25;
            this.dgConditions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgConditions_CellClick);
            this.dgConditions.SelectionChanged += new System.EventHandler(this.dgConditions_SelectionChanged);
            this.dgConditions.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgConditions_DragDrop);
            this.dgConditions.DragOver += new System.Windows.Forms.DragEventHandler(this.dgConditions_DragOver);
            this.dgConditions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgConditions_MouseDown);
            this.dgConditions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgConditions_MouseMove);
            this.dgConditions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGrid_MouseUp);
            // 
            // lblSubSubAmount
            // 
            this.lblSubSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubSubAmount.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSubSubAmount.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblSubSubAmount.Location = new System.Drawing.Point(50, 171);
            this.lblSubSubAmount.Name = "lblSubSubAmount";
            this.lblSubSubAmount.Size = new System.Drawing.Size(54, 21);
            this.lblSubSubAmount.TabIndex = 27;
            this.lblSubSubAmount.Text = "x";
            // 
            // lblSubSubSubAmount
            // 
            this.lblSubSubSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubSubSubAmount.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSubSubSubAmount.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblSubSubSubAmount.Location = new System.Drawing.Point(50, 198);
            this.lblSubSubSubAmount.Name = "lblSubSubSubAmount";
            this.lblSubSubSubAmount.Size = new System.Drawing.Size(54, 21);
            this.lblSubSubSubAmount.TabIndex = 31;
            this.lblSubSubSubAmount.Text = "x";
            // 
            // numSubSubSubAmount
            // 
            this.numSubSubSubAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numSubSubSubAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSubSubSubAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSubSubSubAmount.Location = new System.Drawing.Point(14, 195);
            this.numSubSubSubAmount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSubSubSubAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubSubSubAmount.Name = "numSubSubSubAmount";
            this.numSubSubSubAmount.Size = new System.Drawing.Size(33, 21);
            this.numSubSubSubAmount.TabIndex = 30;
            this.numSubSubSubAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSubSubSubAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubSubSubAmount.ValueChanged += new System.EventHandler(this.numSubSubSubAmount_ValueChanged);
            this.numSubSubSubAmount.Enter += new System.EventHandler(this.numSubSubSubAmount_Enter);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 0;
            this.toolTip.Style = MetroFramework.MetroColorStyle.Blue;
            this.toolTip.StyleManager = null;
            this.toolTip.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // rbList1
            // 
            this.rbList1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbList1.AutoSize = true;
            this.rbList1.Checked = true;
            this.rbList1.Location = new System.Drawing.Point(397, 42);
            this.rbList1.Name = "rbList1";
            this.rbList1.Size = new System.Drawing.Size(29, 15);
            this.rbList1.TabIndex = 0;
            this.rbList1.TabStop = true;
            this.rbList1.Text = "1";
            this.rbList1.UseSelectable = true;
            this.rbList1.Click += new System.EventHandler(this.AllRadioButtons_Click);
            this.rbList1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllRadioButtons_MouseUp);
            // 
            // rbList2
            // 
            this.rbList2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbList2.AutoSize = true;
            this.rbList2.Location = new System.Drawing.Point(433, 42);
            this.rbList2.Name = "rbList2";
            this.rbList2.Size = new System.Drawing.Size(29, 15);
            this.rbList2.TabIndex = 1;
            this.rbList2.Text = "2";
            this.rbList2.UseSelectable = true;
            this.rbList2.Click += new System.EventHandler(this.AllRadioButtons_Click);
            this.rbList2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllRadioButtons_MouseUp);
            // 
            // rbList3
            // 
            this.rbList3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbList3.AutoSize = true;
            this.rbList3.Location = new System.Drawing.Point(469, 42);
            this.rbList3.Name = "rbList3";
            this.rbList3.Size = new System.Drawing.Size(29, 15);
            this.rbList3.TabIndex = 2;
            this.rbList3.Text = "3";
            this.rbList3.UseSelectable = true;
            this.rbList3.Click += new System.EventHandler(this.AllRadioButtons_Click);
            this.rbList3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllRadioButtons_MouseUp);
            // 
            // rbList4
            // 
            this.rbList4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbList4.AutoSize = true;
            this.rbList4.Location = new System.Drawing.Point(505, 42);
            this.rbList4.Name = "rbList4";
            this.rbList4.Size = new System.Drawing.Size(29, 15);
            this.rbList4.TabIndex = 3;
            this.rbList4.Text = "4";
            this.rbList4.UseSelectable = true;
            this.rbList4.Click += new System.EventHandler(this.AllRadioButtons_Click);
            this.rbList4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllRadioButtons_MouseUp);
            // 
            // rbList5
            // 
            this.rbList5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbList5.AutoSize = true;
            this.rbList5.Location = new System.Drawing.Point(541, 42);
            this.rbList5.Name = "rbList5";
            this.rbList5.Size = new System.Drawing.Size(29, 15);
            this.rbList5.TabIndex = 4;
            this.rbList5.Text = "5";
            this.rbList5.UseSelectable = true;
            this.rbList5.Click += new System.EventHandler(this.AllRadioButtons_Click);
            this.rbList5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AllRadioButtons_MouseUp);
            // 
            // lblCurrentList
            // 
            this.lblCurrentList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentList.AutoSize = true;
            this.lblCurrentList.ForeColor = System.Drawing.SystemColors.Control;
            this.lblCurrentList.Location = new System.Drawing.Point(308, 43);
            this.lblCurrentList.Name = "lblCurrentList";
            this.lblCurrentList.Size = new System.Drawing.Size(82, 13);
            this.lblCurrentList.TabIndex = 24;
            this.lblCurrentList.Text = "[ Condition List ]";
            // 
            // cmExportImport
            // 
            this.cmExportImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clearListToolStripMenuItem});
            this.cmExportImport.Name = "cmExportImport";
            this.cmExportImport.ShowImageMargin = false;
            this.cmExportImport.Size = new System.Drawing.Size(156, 98);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exportToolStripMenuItem.Text = "⮝ Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.importToolStripMenuItem.Text = "⮟ Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.clearListToolStripMenuItem.Text = "❌ Clear";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
            // 
            // MainGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.rbList5);
            this.Controls.Add(this.rbList4);
            this.Controls.Add(this.rbList3);
            this.Controls.Add(this.rbList2);
            this.Controls.Add(this.rbList1);
            this.Controls.Add(this.lblCurrentList);
            this.Controls.Add(this.gbConditions);
            this.Controls.Add(this.lblCaptureRegion);
            this.Controls.Add(this.btnReproduce);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnRetain);
            this.Controls.Add(this.captureRegion);
            this.Controls.Add(this.seeThroughRegion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 350);
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
            this.Shown += new System.EventHandler(this.MainGui_Shown);
            this.LocationChanged += new System.EventHandler(this.MainGui_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.MainGui_SizeChanged);
            this.Resize += new System.EventHandler(this.MainGui_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.seeThroughRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.captureRegion)).EndInit();
            this.gbConditions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSubSubAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSubAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgConditions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSubSubSubAmount)).EndInit();
            this.cmExportImport.ResumeLayout(false);
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
        private MetroFramework.Controls.MetroButton btnRetain;
        private MetroFramework.Controls.MetroButton btnNew;
        private MetroFramework.Controls.MetroButton btnReproduce;
        private System.Windows.Forms.Label lblCaptureRegion;
        internal MetroFramework.Controls.MetroLabel lblConditionsNote;
        private System.Windows.Forms.GroupBox gbConditions;
        private System.Windows.Forms.ComboBox cbSubTerms;
        private System.Windows.Forms.NumericUpDown numSubAmount;
        internal MetroFramework.Controls.MetroLabel lblSubAmount;
        private MetroFramework.Components.MetroToolTip toolTip;
        internal MetroFramework.Components.MetroStyleManager metroStyleManager;
        internal System.Windows.Forms.DataGridView dgConditions;
        private System.Windows.Forms.ComboBox cbSubSubTerms;
        private System.Windows.Forms.NumericUpDown numSubSubAmount;
        internal MetroFramework.Controls.MetroLabel lblSubSubAmount;
        internal MetroFramework.Controls.MetroCheckBox chkbxAnyAmount;
        private System.Windows.Forms.ComboBox cbSubSubSubTerms;
        private System.Windows.Forms.NumericUpDown numSubSubSubAmount;
        internal MetroFramework.Controls.MetroLabel lblSubSubSubAmount;
        private MetroFramework.Controls.MetroRadioButton rbList1;
        private MetroFramework.Controls.MetroRadioButton rbList2;
        private MetroFramework.Controls.MetroRadioButton rbList3;
        private MetroFramework.Controls.MetroRadioButton rbList4;
        private MetroFramework.Controls.MetroRadioButton rbList5;
        private System.Windows.Forms.Label lblCurrentList;
        private MetroFramework.Controls.MetroContextMenu cmExportImport;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}

