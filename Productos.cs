using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class Productos : Form
    {
        private Label lblId, lblNombre, lblDescripcion, lblPrecio;
        private TextBox txtId, txtNombre, txtDescripcion, txtPrecio;
        private Button btnAceptar, btnCancelar;

        public Productos()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Productos";
            this.ClientSize = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            lblId = new Label() { Text = "ID:", Location = new Point(20, 20), AutoSize = true };
            txtId = new TextBox() { Location = new Point(120, 18), Width = 240 };

            lblNombre = new Label() { Text = "Nombre:", Location = new Point(20, 60), AutoSize = true };
            txtNombre = new TextBox() { Location = new Point(120, 58), Width = 240 };

            lblDescripcion = new Label() { Text = "Descripción:", Location = new Point(20, 100), AutoSize = true };
            txtDescripcion = new TextBox() { Location = new Point(120, 98), Width = 240, Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical };

            lblPrecio = new Label() { Text = "Precio:", Location = new Point(20, 180), AutoSize = true };
            txtPrecio = new TextBox() { Location = new Point(120, 178), Width = 240 };

            btnAceptar = new Button() { Text = "Aceptar", Location = new Point(200, 230), DialogResult = DialogResult.OK };
            btnAceptar.Click += BtnAceptar_Click;
            btnCancelar = new Button() { Text = "Cancelar", Location = new Point(290, 230), DialogResult = DialogResult.Cancel };

            this.Controls.AddRange(new Control[] { lblId, txtId, lblNombre, txtNombre, lblDescripcion, txtDescripcion, lblPrecio, txtPrecio, btnAceptar, btnCancelar });
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El ID y el Nombre son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text, out var p))
            {
                MessageBox.Show("El precio debe ser un valor numérico.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
            // Validación pasada; el formulario se cerrará con DialogResult.OK
        }

        public string Id => txtId.Text;
        public string Nombre => txtNombre.Text;
        public string Descripcion => txtDescripcion.Text;
        public decimal Precio => decimal.TryParse(txtPrecio.Text, out var v) ? v : 0m;
    }
}
