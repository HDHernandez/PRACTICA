using Login.Models;
using Login.Reportes;
using Login.Services;
//Incluir librerias SQL
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Login
{
    /// <summary>
    /// Lógica de interacción para frmUsuarios.xaml
    /// </summary>
    public partial class frmUsuarios : Window
    {
        public frmUsuarios()
        {
            InitializeComponent();
            
        }

        #region DECLARACION DE VARIABLES LOCALES
        //Conexion a la base de datos
        SqlConnection conDB = new SqlConnection(Properties.Settings.Default.conexionDB);

        //Variable para consultas SQL
        string consultaSQL = null;
        int usuarioid = 0; //Usada para almacenar el ID del usuario seleccionado

        //Banderas para el control del estado de EDITAR o AGREGAR
        bool Agregando = false, Editando = false;
        #endregion

        #region METODOS PERSONALIZADOS
        void MostrarUsuarios()
        {
            dgUsuarios.ItemsSource = DatosUsuario.MostrarUsuarios();
            /*
            //Consulta SQl de registros
            consultaSQL = null;
            consultaSQL = "SELECT USUARIOID,NOMBRECOMPLETO, CORREO FROM USUARIOS";

            //Creando elemento SQLDataAdapter
            SqlDataAdapter da = new SqlDataAdapter(consultaSQL,conDB);
            //Crear DataTable
            DataTable dt = new DataTable();
            //Proceder con llenado del DataTable
            da.Fill(dt);
            //Proceder con el llenado del DataGrid
            dgUsuarios.ItemsSource = dt.DefaultView;
            //Cerrar conexion
            conDB.Close();
            */
        }
        
        //Metodo para bloquear y desbloquear objetos del formulario
        void HabilitarObjetos(bool estado)
        {
            //Habilitar o deshabilitar
            txtNombreCompleto.IsEnabled = estado;
            txtCorreo.IsEnabled = estado;
            txtClave.IsEnabled = estado;
            txtConfirmacion.IsEnabled = estado;
        }//Fin de HabilitarObjetos

        //Limpiar objetos del formulario
        void LimpiarObjetos()
        {
            txtNombreCompleto.Clear();
            txtCorreo.Clear();
            txtClave.Clear();
            txtConfirmacion.Clear();
        }//Fin de LimpiarObjetos

        //Metodo para control de toolbar
        void ControlToolBar()
        {
            //Si el DataGrid esta vacio
            if (dgUsuarios.Items.Count == 0)
            {
                btnNuevo.IsEnabled = true;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;
                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
            }
            else
            {
                //Si el Datagrid tiene por lo menos un registro
                btnNuevo.IsEnabled = true;
                btnEditar.IsEnabled = true;
                btnEliminar.IsEnabled = true;
                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
            }

            //Si estoy AGREGANDO o EDITANDO
            if (Agregando || Editando)
            {
                btnNuevo.IsEnabled = false;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;
                btnGuardar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
        }

        //METODO PARA VALIDAR FORMULARIO
        bool ValidarFormulario()
        {
            bool estado = true;//Asumir que todo esta OK
            string mensaje = null;

            //Validando objetos
            if (string.IsNullOrEmpty(txtNombreCompleto.Text))
            {
                estado = false;
                mensaje += "- Nombre de usuario\n";
            }

            if (string.IsNullOrEmpty(txtCorreo.Text))
            {
                estado = false;
                mensaje += "- Correo\n";
            }

            if (string.IsNullOrEmpty(txtClave.Password))
            {
                estado = false;
                mensaje += "- Clave\n";
            }
            if (string.IsNullOrEmpty(txtConfirmacion.Password))
            {
                estado = false;
                mensaje += "- Confirmación\n";
            }

            if (estado)
            {
                //Indica que todo esta completo en el formulario
                if (txtClave.Password != txtConfirmacion.Password)
                {
                    estado = false;
                    mensaje += "Las clave no son iguales";
                }
            }

            if (!estado)
            {
                MessageBox.Show("Favor de completar o corregir los siguientes campos:\n\n"+mensaje,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }                

            return estado;
        }//Fin de ValidarFormulario
        
        #endregion

        #region EVENTOS DEL FORMULARIO
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Cargando usuario en pantalla
            MostrarUsuarios();

            //Bloqueando objetos del formulario
            HabilitarObjetos(false);

            //Limpiar objetos
            LimpiarObjetos();

            //Configurar toolbar
            ControlToolBar();
        }//Fin de Loaded

        //Evento de editar usuario
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            //Habilitar objetos
            HabilitarObjetos(true);

            //Activando estado de Editar
            Agregando = false;
            Editando = true;

            //Configurar toolbar
            ControlToolBar();

            //Enfoque al Nombre de usuario
            txtNombreCompleto.Focus();
        }

        //Evento para cancelar un proceso
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //Preguntar si se realizara el proceso de cancelacion
            if (MessageBox.Show("Desea cancelar la operación?","Confirmación",
                MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                //Limpiar objetos
                LimpiarObjetos();

                //Bloquear objetos
                HabilitarObjetos(false);

                //Reiniciando valores por defecto de banderas
                Agregando = false;
                Editando = false;

                //Configurar toolbar
                ControlToolBar();
            }
            
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            //Variable de mensajes para confirmar procesos
            string mensaje = null;

            if (ValidarFormulario())
            {
                UsuariosModel usuario = new UsuariosModel();
                usuario.NombreCompleto = txtNombreCompleto.Text;
                usuario.Correo = txtCorreo.Text;
                usuario.Clave = txtClave.Password;

                if (Agregando)
                {
                    //Proceso de insercion de nuevo usuario
                    usuarioid = DatosUsuario.InsertarUsuarios(usuario);
                    mensaje = "Registro insertado correctamente";
                }
                else //Si estoy editando
                {
                    //Estableciendo el ID del registro a modificar
                    usuario.UsuarioId = usuarioid;
                    //Proceso de actualizacion de usuario
                    usuarioid = DatosUsuario.ModificarUsuario(usuario);
                    mensaje = "Registro modificado correctamente";
                }
                
                //Validar si hay un ID correcto
                if (usuarioid > 0)
                {
                    MessageBox.Show(mensaje,
                    "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Limpiar formulario
                    LimpiarObjetos();

                    //Bloquerar objetos de formulario
                    HabilitarObjetos(false);

                    //Reiniciar variables de estado
                    Agregando = false;
                    Editando = false;

                    //Recargar el DataGrid con el nuevo cambio
                    MostrarUsuarios();

                    //Configurar la barra de botones
                    ControlToolBar();
                }//Fin usuarioid
            }//Fin validarformulario
        }//Fin btnGuardar

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsuariosModel usr =(UsuariosModel) dgUsuarios.SelectedItem;
            //Preguntar si el datagrid tiene filas
            if (usr == null)
            {
                return;
            }

            txtNombreCompleto.Text = usr.NombreCompleto;
            txtCorreo.Text = usr.Correo;
            txtClave.Password = usr.Clave;
            txtConfirmacion.Password = usr.Clave;
            usuarioid = usr.UsuarioId;
        }

        //EVENTO DE BOTON ELIMINAR
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //Validar si hay registros para eliminar
            if (dgUsuarios.Items.Count>0)
            {
                if (MessageBox.Show("Desea eliminar el registro #"+usuarioid+"?", "Confirmación",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    UsuariosModel usuario = new UsuariosModel();
                    //Estableciendo el ID a eliminar
                    usuario.UsuarioId = usuarioid;

                    //Proceder con la eliminacion
                    if (DatosUsuario.EliminarUsuario(usuario)>0)
                    {
                        //Mostrar mensaje
                        MessageBox.Show("Registro eliminado correctamente", 
                            "Confirmación", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);

                        //Limpiar formulario
                        LimpiarObjetos();

                        //Recargar DataGrid
                        MostrarUsuarios();

                        //Reiniciar las variables de estado
                        Agregando = false;
                        Editando = false;

                        //Control de botones
                        ControlToolBar();
                    }
                }//Fin de confirmacion
            }//Fin de validacion de DataGrid
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            //Instanciar al reporte
            rptUsuarios rpt = new rptUsuarios();

            //Instanciamiento para el visor
            vsReporte visor = new vsReporte();

            //Configuracion de la carga del reporte
            rpt.Load(@"rptUsuarios.rep");

            //Establecimiento de parametros para el reporte
            rpt.SetParameterValue("pmUsuarioId", usuarioid);

            //Asignar el origen del reporte al visor
            visor.crvReporte.ViewerCore.ReportSource = rpt;

            //Mostrar reporte de visor en pantalla
            visor.Show();
        }

        //Evento de Nuevo usuario
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            //Habilitar objetos
            HabilitarObjetos(true);

            //Limpiar objetos
            LimpiarObjetos();

            //Activando estado de Agregar
            Agregando = true;
            Editando = false;

            //Configurar toolbar
            ControlToolBar();

            //Enfoque al Nombre de usuario
            txtNombreCompleto.Focus();
        }
        #endregion
    }
}
