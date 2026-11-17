using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class Categorias : Form
    {
        private RadioButton rbCrear, rbEliminar, rbSeleccionar;
        private Label lblTexto;
        private TextBox txtTexto;
        private ListBox lstCategorias;
        private Button btnAceptar, btnCancelar;

        public string Accion { get; private set; }
        public string Valor { get; private set; }

        public Categorias()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Categorías";
            this.ClientSize = new Size(420, 300);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            rbCrear = new RadioButton() { Text = "Crear categoría", Location = new Point(20, 20), AutoSize = true };
            rbEliminar = new RadioButton() { Text = "Eliminar categoría", Location = new Point(20, 50), AutoSize = true };
            rbSeleccionar = new RadioButton() { Text = "Seleccionar categoría", Location = new Point(20, 80), AutoSize = true };

            rbCrear.CheckedChanged += Rb_CheckedChanged;
            rbEliminar.CheckedChanged += Rb_CheckedChanged;
            rbSeleccionar.CheckedChanged += Rb_CheckedChanged;

            lblTexto = new Label() { Text = "Nombre de categoría:", Location = new Point(20, 120), AutoSize = true };
            txtTexto = new TextBox() { Location = new Point(150, 118), Width = 240 };

            lstCategorias = new ListBox() { Location = new Point(20, 120), Size = new Size(370, 100), Visible = false };
            // Ejemplo de datos; en integración real se cargarían desde la BD
            lstCategorias.Items.AddRange(new object[] { "Electrónica", "Alimentos", "Ropa" });

            btnAceptar = new Button() { Text = "Aceptar", Location = new Point(220, 240), DialogResult = DialogResult.OK };
            btnAceptar.Click += BtnAceptar_Click;
            btnCancelar = new Button() { Text = "Cancelar", Location = new Point(310, 240), DialogResult = DialogResult.Cancel };

            this.Controls.AddRange(new Control[] { rbCrear, rbEliminar, rbSeleccionar, lblTexto, txtTexto, lstCategorias, btnAceptar, btnCancelar });

            rbCrear.Checked = true; // opción por defecto
        }

        private void Rb_CheckedChanged(object? sender, EventArgs e)
        {
            if (rbSeleccionar.Checked)
            {
                txtTexto.Visible = false;
                lblTexto.Visible = false;
                lstCategorias.Visible = true;
            }
            else
            {
                txtTexto.Visible = true;
                lblTexto.Visible = true;
                lstCategorias.Visible = false;
            }
        }

        private void BtnAceptar_Click(object? sender, EventArgs e)
        {
            if (rbCrear.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtTexto.Text))
                {
                    MessageBox.Show("Introduzca el nombre de la categoría.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                Accion = "Crear";
                Valor = txtTexto.Text.Trim();
            }
            else if (rbEliminar.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtTexto.Text))
                {
                    MessageBox.Show("Introduzca el nombre de la categoría a eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                Accion = "Eliminar";
                Valor = txtTexto.Text.Trim();
            }
            else // seleccionar
            {
                if (lstCategorias.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione una categoría de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                Accion = "Seleccionar";
                Valor = lstCategorias.SelectedItem.ToString();
            }
            // Si llega aquí, se cierra con DialogResult.OK
        }
    }
}
