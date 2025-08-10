using System.Drawing;
using System.Windows.Forms;

namespace LocalCachePlugin
{
    partial class CachePage
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Location = new Point(3, 7);
            listView1.Name = "listView1";
            listView1.Size = new Size(440, 440);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.Columns.Clear();
            listView1.Columns.Add("Key", 250);
            listView1.Columns.Add("Value", 150);
            // 
            // CachePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(listView1);
            Name = "CachePage";
            Size = new Size(448, 450);
            ResumeLayout(false);
        }

        #endregion
        private ListView listView1;
    }
}
