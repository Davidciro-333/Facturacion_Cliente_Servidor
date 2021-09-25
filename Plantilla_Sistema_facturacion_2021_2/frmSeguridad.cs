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
    public partial class frmSeguridad : Form
    {
        public frmSeguridad()
        {
            InitializeComponent();
        }

        private void LlenarComboEmpleados()
        {
            DataTable dt = new DataTable();
            clsAcceso_datos Acceso = new clsAcceso_datos(); // creamos un objeto con la clase Acceso_datos
            dt = Acceso.CargarTabla("TBLEMPLEADO", "");
            cmbEmpleado.DataSource = dt;
            cmbEmpleado.DisplayMember = "strNombre";
            cmbEmpleado.ValueMember = "IdEmpleado";
            Acceso.CerrarBD();
        }

        private Boolean Validar()
        {
            Boolean errorCampos = true;

            if (txtUsuario.Text == string.Empty)
            {
                MensajeError.SetError(txtUsuario, "debe ingresar un valor de Usuario");
                txtUsuario.Focus();
                errorCampos = false;
            }
            else 
            {
                MensajeError.SetError(txtUsuario, ""); 
            }

            if (txtClave.Text == "")
            {
                MensajeError.SetError(txtClave, "Debe ingresar un valor de cédula");
                txtClave.Focus();
                errorCampos = false;
            }
            else 
            {
                MensajeError.SetError(txtClave, ""); 
            }
            return errorCampos;
        }

        // Metodo para validar si los valores son numerico
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

        // Funcion que permite guardar los datos de ingreso a un usuario
        public bool Guardar()
        {
            Boolean Actualizado = false;
            if (Validar())
            {
                try
                {
                    clsAcceso_datos Acceso = new clsAcceso_datos();
                    string sentencia = $"Exec actualizar_Seguridad '{ Convert.ToInt32(cmbEmpleado.SelectedValue)}','{txtUsuario.Text}','{txtClave.Text}','{DateTime.Now.ToString("yyyy-MM-dd")}','Javier'";
                    MessageBox.Show(Acceso.EjecutarComando(sentencia));
                    Actualizado = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("falló inserción: " + ex);
                    Actualizado = false;
                }
            }

            return Actualizado;
        }

        // Funcion que permite eliminar los datos de ingreso de un usuario
        public void Eliminar()
        {
            clsAcceso_datos Acceso = new clsAcceso_datos();
            string sentencia = $"Exec Eliminar_Seguridad '{Convert.ToInt32(cmbEmpleado.SelectedValue)}'";
            MessageBox.Show(Acceso.EjecutarComando(sentencia));
            txtUsuario.Text = "";
            txtClave.Text = "";
        }

        //Funcion que permite consultar los datos de ingreso de un usuario
        public void Consultar()
        {
            DataTable dt = new DataTable();
            string sentencia = "select StrUsuario,StrClave from TBLSEGURIDAD where IdEmpleado = " + cmbEmpleado.SelectedValue.ToString();
            clsAcceso_datos Acceso = new clsAcceso_datos(); // creamos un objeto con la clase Acceso_datos
            dt = Acceso.EjecutarComandoDatos(sentencia);

            if (dt.Rows.Count > 0)
            {
                txtUsuario.Text = dt.Rows[0]["StrUsuario"].ToString();
                txtClave.Text = dt.Rows[0]["StrClave"].ToString();
            }
            else
            {
                MessageBox.Show("El usuario no dispone de datos de ingreso");
                txtUsuario.Text = "";
                txtClave.Text = "";
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //verificamos si desea cerrar la ventana
            DialogResult Rta;
            Rta = MessageBox.Show("Desea salir de la edición ?", "MENSAJE DE ADVERTENCIA",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (Rta == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void frmSeguridad_Load(object sender, EventArgs e)
        {
            LlenarComboEmpleados();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

    }
}
