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
    public partial class frmProductos : Form
    {

        clsAcceso_datos Acceso = new clsAcceso_datos();
        DataTable dt = new DataTable();

        public frmProductos()
        {
            InitializeComponent();
        }

        public void LLENAR_GRID()
        {
            dt = Acceso.CargarTabla("TBLPRODUCTO", "");
            dgvProductos.DataSource = dt;

            dt = Acceso.CargarTabla("TBLCATEGORIA_PROD", "");
            cmbCategoria.DataSource = dt;
            cmbCategoria.DisplayMember = "StrDescripcion";
            cmbCategoria.ValueMember = "IdCategoria";

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

            if (txtIdProducto.Text == string.Empty)
            {
                MensajeError.SetError(txtNombreProducto, "Debe ingresar el Id del producto");
                txtNombreProducto.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtIdProducto, "");
            }

            if (txtNombreProducto.Text == string.Empty)
            {                
                MensajeError.SetError(txtNombreProducto, "Debe ingresar el nombre del producto");
                txtNombreProducto.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtNombreProducto, "");
            }
            if (txtCodReferencia.Text == string.Empty)
            {
                MensajeError.SetError(txtCodReferencia, "Debe ingresar el código de referencia");
            }
            else
            {
                MensajeError.SetError(txtCodReferencia, "");
            }
            if (txtPrecioCompra.Text == string.Empty)
            {
                MensajeError.SetError(txtPrecioCompra, "Debe ingresar el precio de la compra");
            }
            else
            {
                MensajeError.SetError(txtPrecioCompra, "");
            }

            if (txtPrecioVenta.Text == string.Empty)
            {
                MensajeError.SetError(txtPrecioVenta, "Debe ingresar el precio de la venta");
            }
            else
            {
                MensajeError.SetError(txtPrecioVenta, "");
            }

            if (txtCantidadStock.Text == string.Empty)
            {
                MensajeError.SetError(txtCantidadStock, "Debe ingresar la cantidad en Stock");
            }
            else
            {
                MensajeError.SetError(txtCantidadStock, "");
            }

            if (!esNumerico(txtIdProducto.Text))
            {
                MensajeError.SetError(txtIdProducto, "El Id del producto debe ser númerico");
                txtIdProducto.Focus();
                return false;
            }

            if (!esNumerico(txtCodReferencia.Text))
            {
                MensajeError.SetError(txtCodReferencia, "El Codigo de referencia debe ser numérico");
                txtCodReferencia.Focus();
                return false;
            }

            if (!esNumerico(txtPrecioCompra.Text))
            {
                MensajeError.SetError(txtPrecioCompra, "El precio de compra debe ser numérico");
                txtPrecioCompra.Focus();
                return false;
            }

            if (!esNumerico(txtPrecioVenta.Text))
            {
                MensajeError.SetError(txtPrecioVenta, "El precio de venta debe ser numérico");
                txtPrecioVenta.Focus();
                return false;
            }

            if (!esNumerico(txtCantidadStock.Text))
            {
                MensajeError.SetError(txtCantidadStock, "La cantidad en stock debe ser numérico");
                txtCantidadStock.Focus();
                return false;
            }


            MensajeError.SetError(txtIdProducto, "");
            MensajeError.SetError(txtCodReferencia, "");
            MensajeError.SetError(txtPrecioCompra, "");
            MensajeError.SetError(txtPrecioVenta, "");
            MensajeError.SetError(txtCantidadStock, "");

            return errorCampos;
        }

        public void Nuevo()
        {
            txtIdProducto.Text = "";
            txtNombreProducto.Text = "";
            txtCodReferencia.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtCantidadStock.Text = "";
            cmbCategoria.SelectedIndex = 0;
            txtRutaImagen.Text = "";
            txtDetalleProducto.Text = "";
        }

        public bool Guardar()
        {
            Boolean Actualizado = false;

            if (Validar())
            {
                try
                {
                    string Sentencia = $"Exec actualizar_Producto '{Convert.ToInt32(txtIdProducto.Text)}', '{txtNombreProducto.Text}', {Convert.ToInt32(txtCodReferencia.Text)}, '{Convert.ToInt32(txtPrecioCompra.Text)}','{Convert.ToInt32(txtPrecioVenta.Text)}','{Convert.ToInt32(cmbCategoria.SelectedValue)}', {txtDetalleProducto.Text}, '{txtRutaImagen.Text}', '{Convert.ToInt32(txtCantidadStock.Text)}', '{DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")}','Javier'";
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
            clsAcceso_datos Acceso = new clsAcceso_datos();
            string Sentencia = $"Exec Eliminar_Producto '{ Convert.ToInt32(txtIdProducto.Text)}'";
            MessageBox.Show(Acceso.EjecutarComando(Sentencia));
            LLENAR_GRID(); // Actualizamos el Grid para mostrar los cambio
        }


        private void frmProductos_Load(object sender, EventArgs e)
        {
            LLENAR_GRID();
        }

        private void dgvProductos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int posActual = 0;
            posActual = dgvProductos.CurrentRow.Index;
            txtIdProducto.Text = dgvProductos[0, posActual].Value.ToString();
            txtNombreProducto.Text = dgvProductos[1, posActual].Value.ToString();
            txtCodReferencia.Text = dgvProductos[2, posActual].Value.ToString();
            txtPrecioCompra.Text = dgvProductos[3, posActual].Value.ToString();
            txtPrecioVenta.Text = dgvProductos[4, posActual].Value.ToString();
            cmbCategoria.SelectedValue = Convert.ToInt16(dgvProductos[5, posActual].Value.ToString());            
            txtDetalleProducto.Text = dgvProductos[6, posActual].Value.ToString();
            txtRutaImagen.Text = dgvProductos[7, posActual].Value.ToString();
            txtCantidadStock.Text = dgvProductos[8, posActual].Value.ToString();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar();
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

        private void btnBuscarProductos_Click(object sender, EventArgs e)
        {
            if (txtBuscarProducto.Text != "")
            {
                string Sentencia = $"select * from TBLPRODUCTO where StrNombre like '%{txtBuscarProducto.Text}%'";
                dgvProductos.DataSource = Acceso.EjecutarComandoDatos(Sentencia);
                txtBuscarProducto.Text = "";
            }
            else
            {
                LLENAR_GRID();
            }

        }
    }
}
