using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plantilla_Sistema_facturacion_2021_2
{
    public partial class frmClientesV2 : Form
    {
        public frmClientesV2()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        clsAcceso_datos Acceso = new clsAcceso_datos(); // creamos un objeto con la clase Acceso_datos

        private void LLENAR_GRID()
        {
            dgvClientesCRUD2.Rows.Clear();
            string sentencia = $"SELECT IdCliente, StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail FROM TBLCLIENTES";
            dt = Acceso.EjecutarComandoDatos(sentencia);
            foreach (DataRow row in dt.Rows) { dgvClientesCRUD2.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]); }
        }

        

        private void frmClientesV2_Load(object sender, EventArgs e)
        {
            LLENAR_GRID();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            frmEditarCliente editarCliente = new frmEditarCliente();
            editarCliente.IdCliente = 0;
            editarCliente.ShowDialog();
            LLENAR_GRID();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvClientesCRUD2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvClientesCRUD2.Columns[e.ColumnIndex].Name == "btnBorrar")
            {
                int posActual = dgvClientesCRUD2.CurrentRow.Index;
                if (MessageBox.Show($"Seguro de borrar al cliente {dgvClientesCRUD2[1, posActual].Value.ToString()}", "CONFIRMACION",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sentencia = $"Exec Eliminar_cliente '{ Convert.ToInt32(dgvClientesCRUD2[0, posActual].Value.ToString())}'";
                    MessageBox.Show(Acceso.EjecutarComando(sentencia));
                    LLENAR_GRID(); // actualizamos de nuevo el grid para que refleje el cambio
                }
            }
            if (dgvClientesCRUD2.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                int posActualFila = dgvClientesCRUD2.CurrentRow.Index;
                frmEditarCliente editarCliente = new frmEditarCliente();
                editarCliente.IdCliente = int.Parse(dgvClientesCRUD2[0, posActualFila].Value.ToString());
                editarCliente.strCliente = dgvClientesCRUD2[1, posActualFila].Value.ToString();
                editarCliente.strDocumento = dgvClientesCRUD2[2, posActualFila].Value.ToString();
                editarCliente.strDireccion = dgvClientesCRUD2[3, posActualFila].Value.ToString();
                editarCliente.strTelefono = dgvClientesCRUD2[4, posActualFila].Value.ToString();
                editarCliente.strEmail = dgvClientesCRUD2[5, posActualFila].Value.ToString();
                editarCliente.ShowDialog();
                LLENAR_GRID();
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (txtBuscarCliente.Text != "")
            {
                dgvClientesCRUD2.Rows.Clear();
                string sentencia = $"select * from TBLCLIENTES where strNombre like '%{txtBuscarCliente.Text}%'";
                dt = Acceso.EjecutarComandoDatos(sentencia);
                foreach (DataRow row in dt.Rows) { dgvClientesCRUD2.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]); }
                txtBuscarCliente.Text = "";
            }
            else
            {
                LLENAR_GRID();
            }
        }
    }
}
