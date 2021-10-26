using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//Incluir librerias SQL
using System.Data;
using System.Data.SqlClient;

namespace Login
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        #region DECLARACION DE VARIABLES LOCALES
        //Conexion a la base de datos
        SqlConnection conDB = new SqlConnection(Properties.Settings.Default.conexionDB);

        //Variable para consultas SQL
        string consultaSQL = null;
        #endregion

        #region METODOS PERSONALIZADOS
        void EncontrarUsuario()
        {
            int resultado = 0;
            
            //Aperturar la base de datos
            if (conDB.State == ConnectionState.Closed)
                conDB.Open();

            //Consulta SQL para buscar usuario
            consultaSQL = null;
            consultaSQL = "SELECT DBO.FNENCONTRARUSUARIO(@User,@Password)";

            SqlCommand sqlCmd = new SqlCommand(consultaSQL,conDB);
            sqlCmd.CommandType = CommandType.Text;
            //Enviar valores por parametro
            sqlCmd.Parameters.AddWithValue("@User",txtCorreo.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@Password", txtClave.Password.Trim());

            //Ejecutar consulta SQL
            resultado = Convert.ToInt32(sqlCmd.ExecuteScalar());

            //Evaluar el resultado
            if (resultado == 1)
            {
                //Instanciar al formuario de usurios
                frmUsuarios frm = new frmUsuarios();
                frm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario no encontrdao","Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //cerrar la base de datos
            conDB.Close();
        }
        #endregion

        #region EVENTOS DEL FORMULARIO
        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            //Llamada al metodo para validar el usuario
            EncontrarUsuario();
        }
        #endregion

        
    }
}
