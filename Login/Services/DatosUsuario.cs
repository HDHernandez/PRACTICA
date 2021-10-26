using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Incluir librerias SQL
using System.Data;
using System.Data.SqlClient;
using Login.Models;
using System.Data.Common;
using System.Windows;

namespace Login.Services
{
    public class DatosUsuario
    {
        public DatosUsuario() { }

        //METODO PARA LISTAR USUARIOS
        public static List<UsuariosModel> MostrarUsuarios()
        {
            List<UsuariosModel> lstUsuarios = new List<UsuariosModel>();
            try
            {
                using (var conn=new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    //Aperturando base de datos
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARUSUARIO";//Nombre del procedimiento
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorriendo el DataReader
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    UsuariosModel usr = new UsuariosModel();
                                    usr.UsuarioId = int.Parse(dr["USUARIOID"].ToString());
                                    usr.NombreCompleto = dr["NOMBRECOMPLETO"].ToString();
                                    usr.Correo = dr["CORREO"].ToString();
                                    usr.Clave = dr["CLAVE"].ToString();

                                    //AGREGAR REGISTRO A LA LISTA
                                    lstUsuarios.Add(usr);
                                }//Fin de While
                            }//Fin de IF hasRows
                        }//FIn de using reader
                    }//Fin de using de sentencias sql
                }//Fin de using de conexion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: "+ex.Message,"Error",
                    MessageBoxButton.OK,MessageBoxImage.Error);
            }

            return lstUsuarios;
        }//Fin de MostrarUsuarios
    
        //METODO PARA INSERCION DE REGISTROS
        public static int InsertarUsuarios(UsuariosModel pusuarios)
        {
            int res = 0;
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPINSERTARUSUARIO";
                        command.Parameters.AddWithValue("@NOMBRE",pusuarios.NombreCompleto);
                        command.Parameters.AddWithValue("@CORREO", pusuarios.Correo);
                        command.Parameters.AddWithValue("@CLAVE", pusuarios.Clave);
                        //Ejecutar StoreProcedure
                        res = command.ExecuteNonQuery();
                    }//Fin de usgin sql
                }//Fin de using de conexion
            }//Fin try
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return res;
        }//Fin de insertarusuarios


        //METODO PARA MODIFICACION DE REGISTROS
        public static int ModificarUsuario(UsuariosModel pusuarios)
        {
            int res = 0;
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMODIFICARUSUARIO";
                        command.Parameters.AddWithValue("@USUARIOID", pusuarios.UsuarioId);
                        command.Parameters.AddWithValue("@NOMBRE", pusuarios.NombreCompleto);
                        command.Parameters.AddWithValue("@CORREO", pusuarios.Correo);
                        command.Parameters.AddWithValue("@CLAVE", pusuarios.Clave);
                        //Ejecutar StoreProcedure
                        res = command.ExecuteNonQuery();
                    }//Fin de usgin sql
                }//Fin de using de conexion
            }//Fin try
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return res;
        }//Fin de Modificarusuarios

        //METODO PARA ELIMINAR REGISTROS
        public static int EliminarUsuario(UsuariosModel pusuarios)
        {
            int res = 0;
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPELIMINARUSUARIO";
                        command.Parameters.AddWithValue("@USUARIOID", pusuarios.UsuarioId);
                        
                        //Ejecutar StoreProcedure
                        res = command.ExecuteNonQuery();
                    }//Fin de usgin sql
                }//Fin de using de conexion
            }//Fin try
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return res;
        }//Fin de Eliminarusuarios

        #region CARGA DE DATOS A COMBO BOX
        public List<SeccionModel> cargarSeccion()
        {
            List<SeccionModel> lstSecciones = new List<SeccionModel>();
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "mostrarSeccion";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    SeccionModel sm = new SeccionModel();
                                    sm.seccionID = int.Parse(dr["ID"].ToString());
                                    lstSecciones.Add(sm);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstSecciones;
        }

        public List<CursoModel> cargarCursos()
        {
            List<CursoModel> lstCursos = new List<CursoModel>();
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "mostrarCursos";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    CursoModel cm = new CursoModel();
                                    cm.cursoID = int.Parse(dr["ID"].ToString());
                                    cm.descripcion = dr["CURSO"].ToString();
                                    lstCursos.Add(cm);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstCursos;
        }
        #endregion
    }//Fin de clase
}
