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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            string Respuesta = "";// Creamos una variable para controlar si encontró el usuario en la base de datos

            if (txtUsuario.Text != "" && txtPassword.Text != string.Empty) // Verifico que el usuario y la clave no sean campos vacios
            {
                clsAcceso_datos Acceso = new clsAcceso_datos(); // Creamos un objeto con la clase Acceso_datos
                Respuesta = Acceso.ValidarUsuario(txtUsuario.Text, txtPassword.Text); // Se le envía el nombre de usuario y la clave al metodo validar usuario de la clase Acceso_datos

                if (Respuesta != string.Empty) // Verificamos que la variable Respuesta no sea un campo vacío 
                {
                    MessageBox.Show("¡Bienvenido " + Respuesta + "!");

                    frmPrincipal principal = new frmPrincipal();
                    this.Hide();
                    principal.Show();


                }
                else
                {
                    //En caso que la variable Respuesta sea vacía  
                    MessageBox.Show("USUARIO NO ENCONTRADO");
                    txtUsuario.Text = "";
                    txtUsuario.Focus();
                    txtPassword.Text = "";

                }
            }
            else
            {
                MessageBox.Show("¡ERROR, Los campos de Usuario y contraseña no deben de estar vacíos!");
                txtUsuario.Focus();
            }
        }
    }
}
