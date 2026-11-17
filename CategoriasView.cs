using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class CategoriasView : Form
    {
        private ListBox lst;
        private Button btnCerrar;

        public CategoriasView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lst = new ListBox();
            btnCerrar = new Button();
            SuspendLayout();
            // 
            // lst
            // 
            lst.Items.AddRange(new object[] { "Electrónica", "Alimentos", "Ropa" });
            lst.Location = new Point(0, 0);
            lst.Name = "lst";
            lst.Size = new Size(120, 94);
            lst.TabIndex = 0;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(0, 0);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 23);
            btnCerrar.TabIndex = 1;
            // 
            // CategoriasView
            // 
            ClientSize = new Size(320, 300);
            Controls.Add(lst);
            Controls.Add(btnCerrar);
            Name = "CategoriasView";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ver Categorías";
            ResumeLayout(false);
        }
    }
}
