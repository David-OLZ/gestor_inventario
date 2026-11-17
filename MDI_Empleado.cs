using System;
using System.Windows.Forms;

namespace Practica
{
    public partial class MDI_Empleado : Form
    {
        public MDI_Empleado()
        {
            InitializeComponent();
        }

        private void MDI_Empleado_Load(object sender, EventArgs e)
        {
        }

        private void AbrirFormularioHijo<T>() where T : Form, new()
        {
            // Evita abrir varias veces el mismo formulario
            foreach (Form formulario in MdiChildren)
            {
                if (formulario is T)
                {
                    formulario.Activate();
                    return;
                }
            }

            T formularioHijo = new T
            {
                MdiParent = this,
                WindowState = FormWindowState.Maximized,
                StartPosition = FormStartPosition.WindowsDefaultLocation
            };

            formularioHijo.Show();
        }

        private void salirToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        private void Item_Catalogos_Categorias_Click(
            object? sender,
            EventArgs e)
        {
            AbrirFormularioHijo<CategoriasView>();
        }

        private void Item_Catalogos_Productos_Click(
            object? sender,
            EventArgs e)
        {
            AbrirFormularioHijo<ProductosView>();
        }
    }
}
