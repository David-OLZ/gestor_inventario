using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practica{
    public partial class login : Form
    {
        //string connectionString = "Data Source=A-228-00\\SQLEXPRESS;Initial Catalog=Prueba;Integrated Security=True;TrustServerCertificate=True";
        //string connectionString = "Server=A-228-00\\SQLEXPRESS;Database=Master;Trusted_Connection=True;TrustServerCertificate=True";

        string connectionString;


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

            connectionString = "Server=" + this.txtServer.Text + ";Database=" + this.txtDatabase.Text + ";Trusted_Connection=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MessageBox.Show("Conexión exitosa");
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

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
