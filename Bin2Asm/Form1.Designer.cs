namespace Bin2Asm {
	partial class Form1 {
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur Windows Form

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent() {
			this.bpImport = new System.Windows.Forms.Button();
			this.chkAmsdos = new System.Windows.Forms.CheckBox();
			this.chkPack = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// bpImport
			// 
			this.bpImport.Location = new System.Drawing.Point(105, 62);
			this.bpImport.Name = "bpImport";
			this.bpImport.Size = new System.Drawing.Size(124, 23);
			this.bpImport.TabIndex = 0;
			this.bpImport.Text = "Importer fichier";
			this.bpImport.UseVisualStyleBackColor = true;
			this.bpImport.Click += new System.EventHandler(this.bpImport_Click);
			// 
			// chkAmsdos
			// 
			this.chkAmsdos.AutoSize = true;
			this.chkAmsdos.Location = new System.Drawing.Point(12, 12);
			this.chkAmsdos.Name = "chkAmsdos";
			this.chkAmsdos.Size = new System.Drawing.Size(149, 17);
			this.chkAmsdos.TabIndex = 1;
			this.chkAmsdos.Text = "Supprimer en-tête Amsdos";
			this.chkAmsdos.UseVisualStyleBackColor = true;
			// 
			// chkPack
			// 
			this.chkPack.AutoSize = true;
			this.chkPack.Location = new System.Drawing.Point(12, 35);
			this.chkPack.Name = "chkPack";
			this.chkPack.Size = new System.Drawing.Size(191, 17);
			this.chkPack.TabIndex = 2;
			this.chkPack.Text = "Compacter fichier avant génération";
			this.chkPack.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(337, 97);
			this.Controls.Add(this.chkPack);
			this.Controls.Add(this.chkAmsdos);
			this.Controls.Add(this.bpImport);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "Conversion Binaire CPC => Source à assembler (DB)";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button bpImport;
		private System.Windows.Forms.CheckBox chkAmsdos;
		private System.Windows.Forms.CheckBox chkPack;
	}
}

