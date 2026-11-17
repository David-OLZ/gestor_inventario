using System;
using System.Windows.Forms;

namespace Practica
{
    public partial class MDI_Principal : Form
    {
        public MDI_Principal()
        {
            InitializeComponent();

            itemCatalogosMovimientos.Click += ItemCatalogosMovimientos_Click;
            salirToolStripMenuItem.Click += SalirToolStripMenuItem_Click;
        }

        private void MDI_Principal_Load(object sender, EventArgs e)
        {
        }

        private void AbrirFormularioHijo<T>() where T : Form, new()
        {
            // Evita abrir varias veces el mismo catálogo
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
                StartPosition = FormStartPosition.CenterParent
            };

            formularioHijo.Show();
        }

        private void Item_Catalogos_Suppliers_Click_1(
            object sender,
            EventArgs e)
        {
            AbrirFormularioHijo<Proveedores>();
        }

        private void ItemCategorias_Click(
            object sender,
            EventArgs e)
        {
            AbrirFormularioHijo<Categorias>();
        }

        private void ItemProductos_Click(
            object sender,
            EventArgs e)
        {
            AbrirFormularioHijo<Productos>();
        }

        private void ItemCatalogosMovimientos_Click(
            object? sender,
            EventArgs e)
        {
            AbrirFormularioHijo<Movimientos>();
        }

        private void SalirToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }
    }
}
