namespace English_Test_Generator
{
    partial class MSWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSWord));
            this.radRichTextEditor1 = new Telerik.WinControls.UI.RadRichTextEditor();
            this.radRichTextEditorRuler1 = new Telerik.WinControls.UI.RadRichTextEditorRuler();
            this.richTextEditorRibbonBar1 = new Telerik.WinControls.UI.RichTextEditorRibbonBar();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditorRuler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richTextEditorRibbonBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // radRichTextEditor1
            // 
            this.radRichTextEditor1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRichTextEditor1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.radRichTextEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radRichTextEditor1.LayoutMode = Telerik.WinForms.Documents.Model.DocumentLayoutMode.Paged;
            this.radRichTextEditor1.Location = new System.Drawing.Point(29, 29);
            this.radRichTextEditor1.Name = "radRichTextEditor1";
            // 
            // 
            // 
            this.radRichTextEditor1.RootElement.ControlBounds = new System.Drawing.Rectangle(29, 29, 200, 200);
            this.radRichTextEditor1.SelectionFill = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(214)))), ((int)(((byte)(220)))), ((int)(((byte)(190)))));
            this.radRichTextEditor1.SelectionStroke = System.Drawing.Color.LightGray;
            this.radRichTextEditor1.Size = new System.Drawing.Size(1250, 516);
            this.radRichTextEditor1.TabIndex = 0;
            this.radRichTextEditor1.ThemeName = "TelerikMetro";
            // 
            // radRichTextEditorRuler1
            // 
            this.radRichTextEditorRuler1.AssociatedRichTextBox = this.radRichTextEditor1;
            this.radRichTextEditorRuler1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radRichTextEditorRuler1.Controls.Add(this.radRichTextEditor1);
            this.radRichTextEditorRuler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radRichTextEditorRuler1.Location = new System.Drawing.Point(0, 174);
            this.radRichTextEditorRuler1.Name = "radRichTextEditorRuler1";
            // 
            // 
            // 
            this.radRichTextEditorRuler1.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 174, 200, 200);
            this.radRichTextEditorRuler1.Size = new System.Drawing.Size(1280, 546);
            this.radRichTextEditorRuler1.TabIndex = 4;
            this.radRichTextEditorRuler1.ThemeName = "TelerikMetro";
            // 
            // richTextEditorRibbonBar1
            // 
            this.richTextEditorRibbonBar1.ApplicationMenuStyle = Telerik.WinControls.UI.ApplicationMenuStyle.BackstageView;
            this.richTextEditorRibbonBar1.AssociatedRichTextEditor = this.radRichTextEditor1;
            this.richTextEditorRibbonBar1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextEditorRibbonBar1.BuiltInStylesVersion = Telerik.WinForms.Documents.Model.Styles.BuiltInStylesVersion.Office2010;
            this.richTextEditorRibbonBar1.EnableKeyMap = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.richTextEditorRibbonBar1.ExitButton.ButtonElement.UseCompatibleTextRendering = false;
            this.richTextEditorRibbonBar1.ExitButton.Text = "Exit";
            this.richTextEditorRibbonBar1.ExitButton.UseCompatibleTextRendering = false;
            this.richTextEditorRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.richTextEditorRibbonBar1.Name = "richTextEditorRibbonBar1";
            // 
            // 
            // 
            // 
            // 
            // 
            this.richTextEditorRibbonBar1.OptionsButton.ButtonElement.UseCompatibleTextRendering = false;
            this.richTextEditorRibbonBar1.OptionsButton.Text = "Options";
            this.richTextEditorRibbonBar1.OptionsButton.UseCompatibleTextRendering = false;
            // 
            // 
            // 
            this.richTextEditorRibbonBar1.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 400, 108);
            this.richTextEditorRibbonBar1.RootElement.StretchVertically = true;
            this.richTextEditorRibbonBar1.Size = new System.Drawing.Size(1280, 174);
            this.richTextEditorRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("richTextEditorRibbonBar1.StartButtonImage")));
            this.richTextEditorRibbonBar1.TabIndex = 3;
            this.richTextEditorRibbonBar1.TabStop = false;
            this.richTextEditorRibbonBar1.Text = "MSWord";
            this.richTextEditorRibbonBar1.ThemeName = "EvalFormTheme";
            // 
            // MSWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.radRichTextEditorRuler1);
            this.Controls.Add(this.richTextEditorRibbonBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MSWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MSWord";
            this.Load += new System.EventHandler(this.MSWord_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditorRuler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richTextEditorRibbonBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRichTextEditor radRichTextEditor1;
        private Telerik.WinControls.UI.RadRichTextEditorRuler radRichTextEditorRuler1;
        private Telerik.WinControls.UI.RichTextEditorRibbonBar richTextEditorRibbonBar1;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
    }
}