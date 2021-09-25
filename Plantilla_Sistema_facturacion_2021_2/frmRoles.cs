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
    public partial class frmRoles : Form
    {
        clsAcceso_datos Acceso = new clsAcceso_datos();
        DataTable dt = new DataTable();

        public frmRoles()
        {
            InitializeComponent();
        }

        public void LLENAR_GRID()
        {
            dt = Acceso.CargarTabla("TBLROLES", "");
            dgRol.DataSource = dt;

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult Rta;
            Rta = MessageBox.Show("¿Desea salir de la edición?", "MENSAJE DE ADVERTENCIA",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (Rta == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void frmRoles_Load(object sender, EventArgs e)
        {
            LLENAR_GRID();
        }
    }
}
