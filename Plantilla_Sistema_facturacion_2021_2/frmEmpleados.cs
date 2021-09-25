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
/// 
/// Descripción: Aplicación cliente servidor con acceso a datos
/// 
/// Autor: Dev. David Alejandro Ciro Ortiz
/// 
/// Fecha 16 de septiembre de 2021
/// 
/// </summary>

namespace Plantilla_Sistema_facturacion_2021_2
{
    public partial class frmEmpleados : Form
    {
        DataTable dt = new DataTable();

        public frmEmpleados()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //verificamos si desea cerrar la ventana
            DialogResult Rta;
            Rta = MessageBox.Show("¿Desea salir de la edición?", "MENSAJE DE ADVERTENCIA",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (Rta == DialogResult.OK)
            {
                this.Close();
            }            
        }

        public void LLENAR_GRID()
        {
            clsAcceso_datos Acceso = new clsAcceso_datos();
            dt = Acceso.CargarTabla("TBLEMPLEADO", "");
            dgBuscarEmpleado.DataSource = dt;

            dt = Acceso.CargarTabla("TBLROLES", "");
            cmbRolEmpleado.DataSource = dt;
            cmbRolEmpleado.DisplayMember = "StrDescripcion";
            cmbRolEmpleado.ValueMember = "IdRolEmpleado";

        }

        private void frmEmpleados_Load(object sender, EventArgs e)
        {

            LLENAR_GRID();
        }

        private void dgBuscarEmpleado_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /// <summary>
            /// Cargamos el contenido de la fila del grid en los campos del formulario, la información se carga
            /// en los campos cada vez que damos clic en una fila del grid 
            /// </summary>

            int posActual = 0;
            posActual = dgBuscarEmpleado.CurrentRow.Index;
            txtIdEmpleado.Text = dgBuscarEmpleado[0, posActual].Value.ToString();
            txtNombreEmpleado.Text = dgBuscarEmpleado[1, posActual].Value.ToString();
            txtDocumentoEmpleado.Text = dgBuscarEmpleado[2, posActual].Value.ToString();
            txtDireccionEmpleado.Text = dgBuscarEmpleado[3, posActual].Value.ToString();
            txtTelefonoEmpleado.Text = dgBuscarEmpleado[4, posActual].Value.ToString();
            txtEmailEmpleado.Text = dgBuscarEmpleado[5, posActual].Value.ToString();
            cmbRolEmpleado.SelectedValue = Convert.ToInt16(dgBuscarEmpleado[6, posActual].Value.ToString());
            dtpFechaIngreso.Value = Convert.ToDateTime(dgBuscarEmpleado[7, posActual].Value.ToString());

            if (dgBuscarEmpleado[8, posActual].Value.ToString() != "")
            {
                dtpFechaRetiro.Value = Convert.ToDateTime(dgBuscarEmpleado[8, posActual].Value.ToString());
            }
            else
            {
                dtpFechaRetiro.Value = Convert.ToDateTime("01/01/1900");
            }
            txtDatosAdicionales.Text = dgBuscarEmpleado[9, posActual].Value.ToString();
        }

        private Boolean Validar()
        {
            Boolean errorCampos = true;

            if (txtNombreEmpleado.Text == string.Empty)
            {
                MensajeError.SetError(txtNombreEmpleado, "Debe ingresar el nombre del empleado");
                txtNombreEmpleado.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtNombreEmpleado, "");
            }
            if (txtDocumentoEmpleado.Text == string.Empty)
            {
                MensajeError.SetError(txtDocumentoEmpleado, "Debe ingresar documento del empleado");
                txtDocumentoEmpleado.Focus();
                errorCampos = false;
            }
            else
            {
                MensajeError.SetError(txtDocumentoEmpleado, "");
            }

            if (!esNumerico(txtDocumentoEmpleado.Text))
            {
                MensajeError.SetError(txtDocumentoEmpleado, "El documento debe ser numérico");
                txtDocumentoEmpleado.Focus();
                return false;
            }
            else
            {
                MensajeError.SetError(txtDocumentoEmpleado, "");
            }

            if (!esNumerico(txtTelefonoEmpleado.Text))
            {
                MensajeError.SetError(txtTelefonoEmpleado, "El telefono debe ser numérico");
                txtTelefonoEmpleado.Focus();
                return false;
            }
            else
            {
                MensajeError.SetError(txtTelefonoEmpleado, "");
            }


            return errorCampos;
        }

        // Funcion para validar si un valor dado es númerico
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

        // Funciones que permiten el ingreso , retiro y actualización de la información de empleados en la de datos
        public bool Guardar()
        {
            Boolean Actualizado = false;

            int Id = 0;
            if (txtIdEmpleado.Text == string.Empty)
            {
                Id = 0;
            }
            else
            {
                Id = Convert.ToInt32(txtIdEmpleado.Text);
            }

            if (Validar())
            {
                try
                {
                    clsAcceso_datos Acceso = new clsAcceso_datos();
                    string Sentencia = $"Exec actualizar_Empleado '{Id}', '{txtNombreEmpleado.Text}', {Convert.ToInt32(txtDocumentoEmpleado.Text)}, '{txtDireccionEmpleado.Text}','{txtTelefonoEmpleado.Text}','{txtEmailEmpleado.Text}', {cmbRolEmpleado.SelectedValue}, '{dtpFechaIngreso.Value.ToString("yyyy-MM-dd 00:00:00.000")}', '{dtpFechaRetiro.Value.ToString("yyyy-MM-dd 00:00:00.000")}', '{txtDatosAdicionales.Text}', '{DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")}', 'Javier'";
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

        // Elimina un registro de un empleado de la base de datos
        public void Eliminar()
        {
            clsAcceso_datos Acceso = new clsAcceso_datos();
            string Sentencia = $"Exec Eliminar_empleado '{ Convert.ToInt32(txtIdEmpleado.Text)}'";
            MessageBox.Show(Acceso.EjecutarComando(Sentencia));
            LLENAR_GRID(); // Actualizamos el Grid para mostrar los cambio
        }

        // Limpiamos los campos del formulario para ingresarle nuveos datos
        public void Nuevo()
        {
            txtIdEmpleado.Text = "";
            txtNombreEmpleado.Text = "";
            txtDocumentoEmpleado.Text = "";
            txtDireccionEmpleado.Text = "";
            txtTelefonoEmpleado.Text = "";
            txtEmailEmpleado.Text = "";
            cmbRolEmpleado.SelectedIndex = 0;
            dtpFechaIngreso.Value = DateTime.Now;
            dtpFechaRetiro.Value = Convert.ToDateTime("01/01/1900");
            txtDatosAdicionales.Text = "";
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

        private void btnBuscarEmpleado_Click(object sender, EventArgs e)
        {
            clsAcceso_datos Acceso = new clsAcceso_datos();
            if (txtBuscarEmpleado.Text != "")
            {
                string Sentencia = $"select * from TBLEMPLEADO where strNombre like '%{txtBuscarEmpleado.Text}%'";
                dgBuscarEmpleado.DataSource = Acceso.EjecutarComandoDatos(Sentencia);
                txtBuscarEmpleado.Text = "";
            }
            else
            {
                LLENAR_GRID();
            }
        }
    }
}
