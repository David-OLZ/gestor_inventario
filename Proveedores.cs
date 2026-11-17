using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public partial class Proveedores : Form
    {
        private readonly DataGridView dgvProveedores;

        private const string ConnectionString =
            "Data Source=LAPTOP-HP-DAVID\\SQLEXPRESS;" +
            "Initial Catalog=prueba;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";

        public Proveedores()
        {
            InitializeComponent();

            // Cambiar el botón Actualizar por Buscar
            button2.Text = "&Buscar";
            button2.Click += BuscarClick;

            // Crear la lista de proveedores
            dgvProveedores = new DataGridView
            {
                Name = "dgvProveedores",
                Location = new Point(21, 245),
                Size = new Size(700, 300),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            Controls.Add(dgvProveedores);

            // Ajustar el tamaño inicial del formulario
            ClientSize = new Size(760, 580);
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void BuscarClick(object sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            const string sql = """
                SELECT
                    id AS ID,
                    proveedor AS Proveedor,
                    direccion AS Dirección,
                    fecha_registro AS [Fecha de registro]
                FROM proveedores
                WHERE @id = ''
                   OR CONVERT(varchar(100), id) = @id
                ORDER BY id;
                """;

            try
            {
                using SqlConnection cnx = new(ConnectionString);
                using SqlCommand cmd = new(sql, cnx);

                cmd.Parameters.Add("@id", SqlDbType.VarChar, 100)
                    .Value = txt_id.Text.Trim();

                using SqlDataAdapter adapter = new(cmd);
                DataTable tabla = new();
                adapter.Fill(tabla);

                dgvProveedores.DataSource = tabla;

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron proveedores.",
                        "Buscar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al consultar los proveedores:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void insertClick(object sender, EventArgs e)
        {
            if (!int.TryParse(txt_id.Text.Trim(), out int id))
            {
                MessageBox.Show(
                    "El ID debe ser un número entero.",
                    "Dato inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txt_proveedores.Text) ||
                string.IsNullOrWhiteSpace(txt_direccion.Text))
            {
                MessageBox.Show(
                    "Escribe el proveedor y la dirección.",
                    "Datos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using SqlConnection cnx = new(ConnectionString);
                cnx.Open();

                const string consultaExistencia = """
                    SELECT proveedor, direccion, fecha_registro
                    FROM proveedores
                    WHERE id = @id;
                    """;

                using SqlCommand cmdExistencia = new(consultaExistencia, cnx);
                cmdExistencia.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using SqlDataReader reader = cmdExistencia.ExecuteReader();

                if (reader.Read())
                {
                    string proveedorActual = reader["proveedor"].ToString() ?? string.Empty;
                    string direccionActual = reader["direccion"].ToString() ?? string.Empty;
                    DateTime fechaActual = Convert.ToDateTime(reader["fecha_registro"]);

                    string proveedorNuevo = txt_proveedores.Text.Trim();
                    string direccionNueva = txt_direccion.Text.Trim();
                    DateTime fechaNueva = fecha_registro.Value;

                    bool datosDiferentes =
                        !string.Equals(
                            proveedorActual,
                            proveedorNuevo,
                            StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(
                            direccionActual,
                            direccionNueva,
                            StringComparison.OrdinalIgnoreCase) ||
                        fechaActual != fechaNueva;

                    reader.Close();

                    if (!datosDiferentes)
                    {
                        MessageBox.Show(
                            "El ID ya existe y los datos son iguales. " +
                            "No hay cambios para realizar.",
                            "Proveedor existente",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }

                    DialogResult respuesta = MessageBox.Show(
                        "El ID ya existe con otros datos.\n\n" +
                        "¿Deseas modificar la información del proveedor?",
                        "Modificar proveedor",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (respuesta != DialogResult.OK)
                    {
                        MessageBox.Show(
                            "La modificación fue cancelada.",
                            "Operación cancelada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }

                    const string sqlActualizar = """
                        UPDATE proveedores
                        SET proveedor = @proveedor,
                            direccion = @direccion,
                            fecha_registro = @fecha_registro
                        WHERE id = @id;
                        """;

                    using SqlCommand cmdActualizar = new(sqlActualizar, cnx);

                    cmdActualizar.Parameters.Add("@id", SqlDbType.Int)
                        .Value = id;

                    cmdActualizar.Parameters.Add("@proveedor", SqlDbType.VarChar, 100)
                        .Value = proveedorNuevo;

                    cmdActualizar.Parameters.Add("@direccion", SqlDbType.VarChar, 200)
                        .Value = direccionNueva;

                    cmdActualizar.Parameters.Add("@fecha_registro", SqlDbType.DateTime)
                        .Value = fechaNueva;

                    cmdActualizar.ExecuteNonQuery();

                    MessageBox.Show(
                        "El proveedor fue modificado correctamente.",
                        "Modificación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    CargarProveedores();
                    return;
                }

                reader.Close();

                const string sqlInsertar = """
                    INSERT INTO proveedores
                        (id, proveedor, direccion, fecha_registro)
                    VALUES
                        (@id, @proveedor, @direccion, @fecha_registro);
                    """;

                using SqlCommand cmdInsertar = new(sqlInsertar, cnx);

                cmdInsertar.Parameters.Add("@id", SqlDbType.Int)
                    .Value = id;

                cmdInsertar.Parameters.Add("@proveedor", SqlDbType.VarChar, 100)
                    .Value = txt_proveedores.Text.Trim();

                cmdInsertar.Parameters.Add("@direccion", SqlDbType.VarChar, 200)
                    .Value = txt_direccion.Text.Trim();

                cmdInsertar.Parameters.Add("@fecha_registro", SqlDbType.DateTime)
                    .Value = fecha_registro.Value;

                cmdInsertar.ExecuteNonQuery();

                MessageBox.Show(
                    "Proveedor insertado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarProveedores();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al guardar el proveedor:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void deleteClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_id.Text))
            {
                MessageBox.Show(
                    "Escribe el ID del proveedor que deseas eliminar.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar el proveedor?",
                "Advertencia",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            const string sql = """
                DELETE FROM proveedores
                WHERE id = @id;
                """;

            try
            {
                using SqlConnection cnx = new(ConnectionString);
                using SqlCommand cmd = new(sql, cnx);

                cmd.Parameters.Add("@id", SqlDbType.Int)
                    .Value = int.Parse(txt_id.Text);

                cnx.Open();

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show(
                        "Proveedor eliminado correctamente.",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LimpiarCampos();
                    CargarProveedores();
                }
                else
                {
                    MessageBox.Show(
                        "No se encontró el proveedor.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "El ID debe ser un número entero.",
                    "Dato inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al eliminar el proveedor:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txt_id.Clear();
            txt_proveedores.Clear();
            txt_direccion.Clear();
            fecha_registro.Value = DateTime.Now;
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void fecha_registro_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
