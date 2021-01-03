namespace DeskTop_Mascot
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.programEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMotionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.topMostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicsCalcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingWindowPosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Setting";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programEndToolStripMenuItem,
            this.cameraControlToolStripMenuItem,
            this.selectModelsToolStripMenuItem,
            this.selectMotionToolStripMenuItem,
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 176);
            // 
            // programEndToolStripMenuItem
            // 
            this.programEndToolStripMenuItem.Name = "programEndToolStripMenuItem";
            this.programEndToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.programEndToolStripMenuItem.Text = "Exit";
            this.programEndToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // cameraControlToolStripMenuItem
            // 
            this.cameraControlToolStripMenuItem.Name = "cameraControlToolStripMenuItem";
            this.cameraControlToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.cameraControlToolStripMenuItem.Text = "Camera_Control";
            this.cameraControlToolStripMenuItem.Click += new System.EventHandler(this.cameraToolStripMenuItem_Click);
            // 
            // selectModelsToolStripMenuItem
            // 
            this.selectModelsToolStripMenuItem.Name = "selectModelsToolStripMenuItem";
            this.selectModelsToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.selectModelsToolStripMenuItem.Text = "Select_Models";
            this.selectModelsToolStripMenuItem.Click += new System.EventHandler(this.modelsToolStripMenuItem_Click);
            // 
            // selectMotionToolStripMenuItem
            // 
            this.selectMotionToolStripMenuItem.Name = "selectMotionToolStripMenuItem";
            this.selectMotionToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.selectMotionToolStripMenuItem.Text = "Select_Motions";
            this.selectMotionToolStripMenuItem.Click += new System.EventHandler(this.motionsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topMostToolStripMenuItem,
            this.physicsCalcToolStripMenuItem,
            this.scheduleToolStripMenuItem,
            this.settingWindowPosToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(210, 30);
            this.toolStripMenuItem1.Text = "Options";
            // 
            // topMostToolStripMenuItem
            // 
            this.topMostToolStripMenuItem.Name = "topMostToolStripMenuItem";
            this.topMostToolStripMenuItem.Size = new System.Drawing.Size(231, 30);
            this.topMostToolStripMenuItem.Text = "TopMost";
            this.topMostToolStripMenuItem.Click += new System.EventHandler(this.topMostToolStripMenuItem_Click);
            // 
            // physicsCalcToolStripMenuItem
            // 
            this.physicsCalcToolStripMenuItem.Name = "physicsCalcToolStripMenuItem";
            this.physicsCalcToolStripMenuItem.Size = new System.Drawing.Size(231, 30);
            this.physicsCalcToolStripMenuItem.Text = "Physics_Calc";
            this.physicsCalcToolStripMenuItem.Click += new System.EventHandler(this.physicsCalcToolStripMenuItem_Click);
            // 
            // scheduleToolStripMenuItem
            // 
            this.scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
            this.scheduleToolStripMenuItem.Size = new System.Drawing.Size(231, 30);
            this.scheduleToolStripMenuItem.Text = "Schedule";
            this.scheduleToolStripMenuItem.Click += new System.EventHandler(this.scheduleToolStripMenuItem_Click);
            // 
            // settingWindowPosToolStripMenuItem
            // 
            this.settingWindowPosToolStripMenuItem.Name = "settingWindowPosToolStripMenuItem";
            this.settingWindowPosToolStripMenuItem.Size = new System.Drawing.Size(231, 30);
            this.settingWindowPosToolStripMenuItem.Text = "Position_of_Dialog";
            this.settingWindowPosToolStripMenuItem.Click += new System.EventHandler(this.settingWindowPosToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 300);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem programEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cameraControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectMotionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectModelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem topMostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem physicsCalcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingWindowPosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scheduleToolStripMenuItem;
    }
}

