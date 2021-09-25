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
    public partial class frmEditarCliente : Form
    {
        public int IdCliente { get; set; }
        public string strCliente { get; set; }
        public string strDocumento { get; set; }
        public string strTelefono { get; set; }
        public string strDireccion { get; set; }
        public string strEmail { get; set; }

        DataTable dt = new DataTable();
        clsAcceso_datos Acceso = new clsAcceso_datos(); // creamos un objeto con la clase clsAcceso_datos

        public frmEditarCliente()
        {
            InitializeComponent();
        }

        private void LLENAR_CLIENTE()
        {
            if (IdCliente == 0)
            {// REGISTRO NUEVO SOLO MOSTRAMOS TITULO Y CAMPOS VACIOS
                lblEditarCliente.Text = "INGRESO NUEVO CLIENTE";
            }
            else
            {//ACTUALIZAR EL REGISTRO CON EL ID RECIBIDO
                string sentencia = $"select * from TBLCLIENTES where IdCliente = {IdCliente}";
                dt = Acceso.EjecutarComandoDatos(sentencia);
                foreach (DataRow row in dt.Rows)
                {
                    txtIdCliente.Text = row[0].ToString();
                    txtNombreCliente.Text = row[1].ToString();
                    txtDocumento.Text = row[2].ToString();
                    txtDireccion.Text = row[3].ToString();
                    txtTelefono.Text = row[4].ToString();
                    txtEmail.Text = row[5].ToString();
                }
            }
        }

        private void frmEditarCliente_Load(object sender, EventArgs e)
        {
            if (IdCliente == 0)
            {
                //Registro nuevo
                lblEditarCliente.Text = "INGRESO NUEVO CLIENTE";

                int x = (Convert.ToInt32(Size.Width) / 2) - (Convert.ToInt32(lblEditarCliente.Size.Width) / 2);
                int y = lblEditarCliente.Location.Y;
                lblEditarCliente.Location = new Point(x, y);
            }
            else
            {
                //Actualizar registro con el ID pasado

                lblEditarCliente.Text = "MODIFICAR CLIENTE";
                txtIdCliente.Text = IdCliente.ToString();
                txtNombreCliente.Text = strCliente;
                txtDocumento.Text = strDocumento;
                txtDireccion.Text = strDireccion;
                txtTelefono.Text = strTelefono;
                txtEmail.Text = strEmail;

                int x = (Convert.ToInt32(Size.Width) / 2) - (Convert.ToInt32(lblEditarCliente.Size.Width) / 2);
                int y = lblEditarCliente.Location.Y;
                lblEditarCliente.Location = new Point(x, y);
            }

            LLENAR_CLIENTE();

        }

        // ------- funciones que permiten el ingreso , retiro y actualización de la información de empleados en la base de datos
        public bool Guardar()
        {
            Boolean actualizado = false;
            if (Validar())
            {
                try
                {
                    string sentencia = $"Exec [actualizar_Cliente] { txtIdCliente.Text},'{txtNombreCliente.Text}',{txtDocumento.Text},'{txtDireccion.Text}','{txtTelefono.Text}', '{txtEmail.Text}','Javier','{DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000")}'";
                    MessageBox.Show(Acceso.EjecutarComando(sentencia));

                    actualizado = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("falló inserción: " + ex);
                    actualizado = false;
                }
            }
            return actualizado;
        }

        //FUNCIÓN QE PERMITE VALIDAR LOS CAMPOS DEL FORMULARIO
        private Boolean Validar()
        {
            Boolean errorCampos = true;
            if (txtNombreCliente.Text == string.Empty)
            {
                MensajeError.SetError(txtNombreCliente, "Debe ingresar el nombre del Cliente");
                txtNombreCliente.Focus();
                errorCampos = false;

            }
            else
            {
                MensajeError.SetError(txtNombreCliente, "");
            }
            if (txtDocumento.Text == "")
            {
                MensajeError.SetError(txtDocumento, "Debe ingresar el documento");
                txtDocumento.Focus();
                errorCampos = false;
            }
            else 
            { 
                MensajeError.SetError(txtDocumento, ""); 
            }
            if (!esNumerico(txtDocumento.Text))
            {
                MensajeError.SetError(txtDocumento, "El Documento debe ser numerico");
                txtDocumento.Focus();
                return false;
            }
            MensajeError.SetError(txtDocumento, "");
            return errorCampos;
        }

        //función para validar si un valor dado es numerico
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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        { 
            this.Close();
        }
    }
}
