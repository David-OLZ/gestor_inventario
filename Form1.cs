using Microsoft.Data.SqlClient;
using System.IO;

namespace Practica{
    public partial class login : Form
    {
        //string connectionString = "Data Source=A-228-00\\SQLEXPRESS;Initial Catalog=Prueba;Integrated Security=True;TrustServerCertificate=True";
        //string connectionString = "Server=A-228-00\\SQLEXPRESS;Database=Master;Trusted_Connection=True;TrustServerCertificate=True";

        private string? mdfPath;


        public login()
        {
            InitializeComponent();
        }

        private void cmdSalir_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Click en el botón salir!!!", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void cmdAceptar_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text.Trim();
            string database = txtDatabase.Text.Trim();

            if (string.IsNullOrWhiteSpace(server))
            {
                MessageBox.Show("Escribe el nombre del servidor o instancia de SQL Server.",
                    "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServer.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(database))
            {
                MessageBox.Show("Escribe el nombre de la base de datos o selecciona un archivo MDF.",
                    "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDatabase.Focus();
                return;
            }

            try
            {
                DatabaseConfig.Configure(server, database, mdfPath);

                using SqlConnection connection = new(DatabaseConfig.ConnectionString);
                connection.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudo conectar con SQL Server:\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verificar que se haya seleccionado un puesto
            string puesto = comboBox1.SelectedItem?.ToString() ?? comboBox1.Text;
            if (string.IsNullOrWhiteSpace(puesto))
            {
                MessageBox.Show("Seleccione un puesto antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (puesto == "Empleado")
            {
                using (MDI_Empleado mdiEmp = new MDI_Empleado())
                {
                    mdiEmp.ShowDialog();
                }
            }
            else if (puesto == "Administrador")
            {
                using (MDI_Principal mdi = new MDI_Principal())
                {
                    mdi.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No tiene permiso para abrir la aplicación con el puesto seleccionado.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnBuscarMdf_Click(object? sender, EventArgs e)
        {
            DialogResult tipo = MessageBox.Show(
                "¿Deseas seleccionar un archivo MDF?\n\nPulsa No para seleccionar una carpeta que contenga un MDF.",
                "Ubicación de la base de datos",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (tipo == DialogResult.Cancel)
                return;

            if (tipo == DialogResult.Yes)
            {
                using OpenFileDialog archivo = new()
                {
                    Title = "Seleccionar archivo MDF",
                    Filter = "Base de datos SQL Server (*.mdf)|*.mdf",
                    CheckFileExists = true,
                    Multiselect = false
                };

                if (archivo.ShowDialog(this) != DialogResult.OK)
                    return;

                mdfPath = archivo.FileName;
            }
            else
            {
                using FolderBrowserDialog carpeta = new()
                {
                    Description = "Selecciona la carpeta que contiene el archivo MDF",
                    UseDescriptionForTitle = true,
                    ShowNewFolderButton = false
                };

                if (carpeta.ShowDialog(this) != DialogResult.OK)
                    return;

                string[] archivosMdf = Directory.GetFiles(
                    carpeta.SelectedPath, "*.mdf", SearchOption.TopDirectoryOnly);

                if (archivosMdf.Length == 0)
                {
                    MessageBox.Show("La carpeta seleccionada no contiene ningún archivo MDF.",
                        "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (archivosMdf.Length > 1)
                {
                    MessageBox.Show("La carpeta contiene varios archivos MDF. Selecciona un archivo MDF directamente.",
                        "Varios archivos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                mdfPath = archivosMdf[0];
            }

            txtDatabase.Text = Path.GetFileNameWithoutExtension(mdfPath);
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtServer_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
