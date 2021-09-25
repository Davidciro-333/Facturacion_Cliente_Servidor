using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Plantilla_Sistema_facturacion_2021_2
{
    class clsAcceso_datos
    {
        SqlConnection Connection;  // Se define la variable para la conexion de tipo SqlConnection
        SqlCommand cmd;              //Se define la variable para los cambios en la Base de datos
        SqlDataReader lectorDatos = null;
        SqlDataAdapter dataAdapter;
        //DataTable dataTable;
        DataSet dataSet;

        public void AbrirBD()       //Metodo para abrir la base de datos
        {
            try         //Permite capturar el error en caso de que se presente, evitando que se cierre la aplicación
            {
                // Creamos un objeto tipo SqlConnection a la base de datos y se pasa como parametro la cadena de conexion
                Connection = new SqlConnection("Data Source = localhost\\SQLEXPRESS; Initial Catalog = [DBFACTURAS]; Integrated Security = True");
                Connection.Open();
            }
            catch (Exception ex)   //Si hay un error se presenta el siguiente mensaje
            {
                MessageBox.Show("Error al abrir la conexión" + ex);
            }
        }

        public void CerrarBD()      //Metodo para cerrar la base de datos
        {
            try
            {
                Connection.Close(); // Invocamos metodo para cerrar la base de datos
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexión" + ex);
            }
        }

        public string ValidarUsuario(string StrUsuario, string StrClave)
        {
            try
            {
                string strEmpleado = "";

                string Sentencia = $"Select e.strNombre, e.IdRolEmpleado from TBLSEGURIDAD s JOIN TBLEMPLEADO e ON s.IdEmpleado = e.IdEmpleado where StrUsuario = '{StrUsuario}' and StrClave = '{StrClave}'";
                AbrirBD();
                cmd = new SqlCommand();

                //Utilizamos las propiedades de SqlCommand
                cmd.Connection = Connection;
                cmd.CommandText = Sentencia;
                cmd.CommandType = CommandType.Text; // Otros tipos son: CommanType.StoredProcedure      CommandType.TableDirect
                cmd.CommandTimeout = 10;
                lectorDatos = cmd.ExecuteReader();
                while (lectorDatos.Read())
                {
                    strEmpleado = Convert.ToString(lectorDatos.GetValue(0));
                }

                if (lectorDatos != null)    // Cerramos el lector
                {
                    lectorDatos.Close();
                }

                return strEmpleado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falló la lectura " + ex);
                return "";
            }
        }

        public DataTable CargarTabla(string tabla, string strCondicion)
        {
            try
            {
                AbrirBD();
                string SQL = "Select * from " + tabla + " " + strCondicion;
                dataAdapter = new SqlDataAdapter(SQL, Connection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, tabla);
                DataTable dt = new DataTable();
                dt = dataSet.Tables[tabla];
                CerrarBD();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR EN LA CONSULTA: " + ex.ToString());
                return null;
            }
        }

        public string EjecutarComando(string Sentencia)
        {
            string Salida = "Los datos se actualizaron satisfactoriamente";

            try
            {
                int Retornado;

                AbrirBD();
                cmd = new SqlCommand(Sentencia, Connection);
                Retornado = cmd.ExecuteNonQuery(); // Usado para el UPDATE, INSERT, DELETE
                CerrarBD();

                if (Retornado > 0)
                { 
                    Salida = "Los datos fueron actualizados";
                }
                else
                {
                    Salida = "Los datos no fueron actualizados";
                }
            }
            catch (Exception ex)
            {
                Salida = "Falló inserción: " + ex;
            }

            return Salida;
        }

        // Método que permite consultar con una sentencia (select) o invocar un procedimiento almacenado
        public DataTable EjecutarComandoDatos(string cmd)
        {
            try
            {
                AbrirBD();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd, Connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                CerrarBD();
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falló la opercion " + ex);
                return null;
            }
        }

    }
}
