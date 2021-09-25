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
    public partial class frmFacturas : Form
    {
        public frmFacturas()
        {
            InitializeComponent();
        }

        clsAcceso_datos Acceso = new clsAcceso_datos();
        DataTable dt = new DataTable();

        public void LLENAR_GRID()
        {
            dt = Acceso.CargarTabla("TBLFACTURA", "");
            dgvFactura.DataSource = dt;

            dt = Acceso.CargarTabla("TBLCLIENTES", "");
            cmbCliente.DataSource = dt;
            cmbCliente.DisplayMember = "StrNombre";
            cmbCliente.ValueMember = "IdCliente";

            dt = Acceso.CargarTabla("TBLEMPLEADO", "");
            cmbEmpleado.DataSource = dt;
            cmbEmpleado.DisplayMember = "StrNombre";
            cmbEmpleado.ValueMember = "IdEmpleado";

            dt = Acceso.CargarTabla("TBLESTADO_FACTURA", "");
            cmbEstadoFactura.DataSource = dt;
            cmbEstadoFactura.DisplayMember = "StrDescripcion";
            cmbEstadoFactura.ValueMember = "IdEstadoFactura";
        }

        private bool esNumerico(string num)
        {
            try
            {
                double x = Convert.ToDouble(num);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private Boolean Validar()
        {
            Boolean errorCampos = true;

            if (txtNumeroFactura.Text == string.Empty)
            {
                MensajeError.SetError(txtNumeroFactura, "Debe ingresar el numero de la factura");
                txtNumeroFactura.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtNumeroFactura, "");
            }

            if (txtDescuento.Text == string.Empty)
            {
                MensajeError.SetError(txtDescuento, "Debe ingresar el descuento");
                txtDescuento.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtDescuento, "");
            }

            if (txtTotalIVA.Text == string.Empty)
            {
                MensajeError.SetError(txtTotalIVA, "Debe ingresar el IVA");
                txtTotalIVA.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtTotalIVA, "");
            }

            if (!esNumerico(txtNumeroFactura.Text))
            {
                MensajeError.SetError(txtNumeroFactura, "El numero de factura debe ser numérico");
                txtNumeroFactura.Focus();
                return false;
            }

            if (!esNumerico(txtDescuento.Text))
            {
                MensajeError.SetError(txtDescuento, "El descuento debe ser numérico");
                txtDescuento.Focus();
                return false;
            }

            if (!esNumerico(txtTotalIVA.Text))
            {
                MensajeError.SetError(txtTotalIVA, "El IVA debe ser numérico");
                txtTotalIVA.Focus();
                return false;
            }


            MensajeError.SetError(txtNumeroFactura, "");            
            MensajeError.SetError(txtDescuento, "");
            MensajeError.SetError(txtTotalIVA, "");
            MensajeError.SetError(txtTotalFactura, "");

            return errorCampos;
        }


        public bool Guardar()
        {
            Boolean Actualizado = false;

            if (Validar())
            {
                try
                {
                    string Sentencia = $"Exec actualizar_Factura '{Convert.ToInt32(txtNumeroFactura.Text)}', '{dtpFechaRegistro.Value.ToString("yyyy-MM-dd 00:00:00.000")}', {cmbCliente.SelectedValue}, '{cmbEmpleado.SelectedValue}','{Convert.ToDouble(txtDescuento.Text)}','{Convert.ToDouble(txtTotalIVA.Text)}', {Convert.ToDouble(txtTotalFactura.Text)}, '{cmbEstadoFactura.SelectedValue}', '{DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")}','Javier'";
                    MessageBox.Show(Acceso.EjecutarComando(Sentencia));
                    LLENAR_GRID();
                    Actualizado = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Falló la inserción " + ex);
                    Actualizado = false;
                }
            }

            return Actualizado;

        }

        public void Eliminar()
        {
            //clsAcceso_datos Acceso = new clsAcceso_datos();
            //string Sentencia = $"Exec Eliminar_Factura '{ Convert.ToInt32(txtIdEmpleado.Text)}'";
            //MessageBox.Show(Acceso.EjecutarComando(Sentencia));
            //LLENAR_GRID(); // Actualizamos el Grid para mostrar los cambio
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

        private void frmFacturas_Load(object sender, EventArgs e)
        {
            LLENAR_GRID();
        }

        private void dgvFactura_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int posActual = 0;
            posActual = dgvFactura.CurrentRow.Index;
            txtNumeroFactura.Text = dgvFactura[0, posActual].Value.ToString();
            dtpFechaRegistro.Value = Convert.ToDateTime(dgvFactura[1, posActual].Value.ToString());
            cmbCliente.SelectedValue = Convert.ToInt16(dgvFactura[2, posActual].Value.ToString());
            cmbEmpleado.SelectedValue = Convert.ToInt16(dgvFactura[3, posActual].Value.ToString());
            txtDescuento.Text = dgvFactura[4, posActual].Value.ToString();
            txtTotalIVA.Text = dgvFactura[5, posActual].Value.ToString();
            txtTotalFactura.Text = dgvFactura[6, posActual].Value.ToString();
            cmbEstadoFactura.SelectedValue = Convert.ToInt16(dgvFactura[7, posActual].Value.ToString());
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }
    }
}
