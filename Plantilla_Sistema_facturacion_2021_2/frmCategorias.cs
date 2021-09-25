using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// TO DO: frmRoles, frmInformes, Boton Eliminar de frmFactura, Video Sustentacion
/// </summary>

namespace Plantilla_Sistema_facturacion_2021_2
{
    public partial class frmCategorias : Form
    {
        public frmCategorias()
        {
            InitializeComponent();
        }

        clsAcceso_datos Acceso = new clsAcceso_datos();
        DataTable dt = new DataTable();

        public void LLENAR_GRID()
        {
            dt = Acceso.CargarTabla("TBLCATEGORIA_PROD", "");
            dgCategorias.DataSource = dt;
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

            if (txtCodigoCategoria.Text == string.Empty)
            {
                MensajeError.SetError(txtCodigoCategoria, "Debe ingresar el código de la categoria");
                txtCodigoCategoria.Focus();
                errorCampos = false;
            }

            if (txtNombreCategoria.Text == string.Empty)
            {
                MensajeError.SetError(txtNombreCategoria, "Debe ingresar el nombre de la categoria");
                txtNombreCategoria.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtNombreCategoria, "");
            }

            if (!esNumerico(txtCodigoCategoria.Text))
            {
                MensajeError.SetError(txtCodigoCategoria, "El Codigo de categoria debe ser númerico");
                txtCodigoCategoria.Focus();
                return false;
            }
            else
            {            
                MensajeError.SetError(txtCodigoCategoria, "");
            }


            return errorCampos;
        }


        public bool Guardar()
        {
            Boolean Actualizado = false;

            if (Validar())
            {
                try
                {
                    string Sentencia = $"Exec actualizar_CategoriaProd '{Convert.ToInt32(txtCodigoCategoria.Text)}', '{txtNombreCategoria.Text}', '{DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")}','Javier'";
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
            string Sentencia = $"Exec Eliminar_CategoriaProducto '{ Convert.ToInt32(txtCodigoCategoria.Text)}'";
            MessageBox.Show(Acceso.EjecutarComando(Sentencia));
            LLENAR_GRID(); // Actualizamos el Grid para mostrar los cambio
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

        private void frmCategorias_Load(object sender, EventArgs e)
        {
            LLENAR_GRID();
        }

        private void dgCategorias_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int posActual = 0;
            posActual = dgCategorias.CurrentRow.Index;
            txtCodigoCategoria.Text = dgCategorias[0, posActual].Value.ToString();
            txtNombreCategoria.Text = dgCategorias[1, posActual].Value.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btnBuscarCategoria_Click(object sender, EventArgs e)
        {
            if (txtBuscarCategoria.Text != "")
            {
                string Sentencia = $"select * from TBLCATEGORIA_PROD where StrDescripcion like '%{txtBuscarCategoria.Text}%'";
                dgCategorias.DataSource = Acceso.EjecutarComandoDatos(Sentencia);
                txtBuscarCategoria.Text = "";
            }
            else
            {
                LLENAR_GRID();
            }

        }
    }
}
