namespace SmartMatrix
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reservedLexemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.butAnalyzeProgram = new System.Windows.Forms.Button();
            this.listBoxLexems = new System.Windows.Forms.ListBox();
            this.listBoxErrors = new System.Windows.Forms.ListBox();
            this.richTextBoxNumberOfLines = new System.Windows.Forms.RichTextBox();
            this.ButtonClear = new System.Windows.Forms.Button();
            this.richTextBoxTextOfProgram = new System.Windows.Forms.RichTextBox();
            this.correctSyntax = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1072, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reservedLexemsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // reservedLexemsToolStripMenuItem
            // 
            this.reservedLexemsToolStripMenuItem.Name = "reservedLexemsToolStripMenuItem";
            this.reservedLexemsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.reservedLexemsToolStripMenuItem.Text = "Reserved lexems";
            this.reservedLexemsToolStripMenuItem.Click += new System.EventHandler(this.reservedLexemsToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 582);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(742, 420);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Error list";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(162, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 31);
            this.label2.TabIndex = 5;
            this.label2.Text = "Text of program";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(720, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 31);
            this.label3.TabIndex = 7;
            this.label3.Text = "Analysis";
            // 
            // butAnalyzeProgram
            // 
            this.butAnalyzeProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.butAnalyzeProgram.Location = new System.Drawing.Point(149, 447);
            this.butAnalyzeProgram.Name = "butAnalyzeProgram";
            this.butAnalyzeProgram.Size = new System.Drawing.Size(190, 50);
            this.butAnalyzeProgram.TabIndex = 8;
            this.butAnalyzeProgram.Text = "Analyze";
            this.butAnalyzeProgram.UseVisualStyleBackColor = true;
            this.butAnalyzeProgram.Click += new System.EventHandler(this.butAnalyzeProgram_Click);
            // 
            // listBoxLexems
            // 
            this.listBoxLexems.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxLexems.FormattingEnabled = true;
            this.listBoxLexems.HorizontalScrollbar = true;
            this.listBoxLexems.ItemHeight = 16;
            this.listBoxLexems.Location = new System.Drawing.Point(571, 60);
            this.listBoxLexems.Name = "listBoxLexems";
            this.listBoxLexems.Size = new System.Drawing.Size(436, 356);
            this.listBoxLexems.TabIndex = 9;
            // 
            // listBoxErrors
            // 
            this.listBoxErrors.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxErrors.FormattingEnabled = true;
            this.listBoxErrors.HorizontalScrollbar = true;
            this.listBoxErrors.ItemHeight = 16;
            this.listBoxErrors.Location = new System.Drawing.Point(571, 447);
            this.listBoxErrors.Name = "listBoxErrors";
            this.listBoxErrors.Size = new System.Drawing.Size(436, 132);
            this.listBoxErrors.TabIndex = 10;
            // 
            // richTextBoxNumberOfLines
            // 
            this.richTextBoxNumberOfLines.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxNumberOfLines.Location = new System.Drawing.Point(13, 60);
            this.richTextBoxNumberOfLines.Name = "richTextBoxNumberOfLines";
            this.richTextBoxNumberOfLines.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBoxNumberOfLines.Size = new System.Drawing.Size(38, 356);
            this.richTextBoxNumberOfLines.TabIndex = 11;
            this.richTextBoxNumberOfLines.Text = "";
            // 
            // ButtonClear
            // 
            this.ButtonClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonClear.Location = new System.Drawing.Point(149, 528);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(190, 50);
            this.ButtonClear.TabIndex = 12;
            this.ButtonClear.Text = "Clear";
            this.ButtonClear.UseVisualStyleBackColor = true;
            this.ButtonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // richTextBoxTextOfProgram
            // 
            this.richTextBoxTextOfProgram.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxTextOfProgram.Location = new System.Drawing.Point(49, 60);
            this.richTextBoxTextOfProgram.Name = "richTextBoxTextOfProgram";
            this.richTextBoxTextOfProgram.Size = new System.Drawing.Size(450, 356);
            this.richTextBoxTextOfProgram.TabIndex = 13;
            this.richTextBoxTextOfProgram.Text = "";
            this.richTextBoxTextOfProgram.WordWrap = false;
            this.richTextBoxTextOfProgram.VScroll += new System.EventHandler(this.textOfProgram_VScroll);
            this.richTextBoxTextOfProgram.TextChanged += new System.EventHandler(this.textOfProgram_TextChanged);
            // 
            // correctSyntax
            // 
            this.correctSyntax.AutoSize = true;
            this.correctSyntax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.correctSyntax.ForeColor = System.Drawing.SystemColors.Highlight;
            this.correctSyntax.Location = new System.Drawing.Point(199, 419);
            this.correctSyntax.Name = "correctSyntax";
            this.correctSyntax.Size = new System.Drawing.Size(106, 16);
            this.correctSyntax.TabIndex = 14;
            this.correctSyntax.Text = "Correct syntax";
            this.correctSyntax.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 606);
            this.Controls.Add(this.correctSyntax);
            this.Controls.Add(this.richTextBoxTextOfProgram);
            this.Controls.Add(this.ButtonClear);
            this.Controls.Add(this.richTextBoxNumberOfLines);
            this.Controls.Add(this.listBoxErrors);
            this.Controls.Add(this.listBoxLexems);
            this.Controls.Add(this.butAnalyzeProgram);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button butAnalyzeProgram;
        private System.Windows.Forms.ListBox listBoxLexems;
        private System.Windows.Forms.ListBox listBoxErrors;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reservedLexemsToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBoxNumberOfLines;
        private System.Windows.Forms.Button ButtonClear;
        private System.Windows.Forms.RichTextBox richTextBoxTextOfProgram;
        private System.Windows.Forms.Label correctSyntax;
    }
}

