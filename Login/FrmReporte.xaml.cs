using Login.Reportes;
using Login.Services;
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
using System.Windows.Shapes;

namespace Login
{
    /// <summary>
    /// Lógica de interacción para FrmReporte.xaml
    /// </summary>
    public partial class FrmReporte : Window
    {
        public FrmReporte()
        {
            InitializeComponent();
            cargar();
        }
        DatosUsuario du = new DatosUsuario();
        #region METODOS PERSONALIZADOS
        private void cargar()
        {
            cmbSeccion.ItemsSource = du.cargarSeccion();
            cmbSeccion.DisplayMemberPath = "seccionID";
            cmbSeccion.SelectedValuePath = "seccionID";
            cmbCurso.ItemsSource = du.cargarCursos();
            cmbCurso.DisplayMemberPath = "descripcion";
            cmbCurso.SelectedValuePath = "cursoID";
        }

        private void envioParametros()
        {
            int seccion = 0, curso = 0;
            rptDetalleAlumnos rda = new rptDetalleAlumnos();
            if (cmbSeccion.SelectedIndex.Equals(-1) && cmbCurso.SelectedIndex.Equals(-1))
            {
                seccion = 0;
                curso = 0;
            }
            else if (cmbSeccion.SelectedIndex > -1 && cmbCurso.SelectedIndex.Equals(-1))
            {
                seccion = int.Parse(cmbSeccion.SelectedValue.ToString());
                curso = 0;
            }
            else if(cmbSeccion.SelectedIndex.Equals(-1) && cmbCurso.SelectedIndex > -1)
            {
                seccion = 0;
                curso = int.Parse(cmbCurso.SelectedValue.ToString());
            }
            else
            {
                seccion = int.Parse(cmbSeccion.SelectedValue.ToString());
                curso = int.Parse(cmbCurso.SelectedValue.ToString());
            }
            rda.SetParameterValue("@SECCIONID", seccion);
            rda.SetParameterValue("@CURSOID", curso);
            crvReporte.ViewerCore.ReportSource = rda;
        }
        #endregion

        private void btnReporte_Click(object sender, RoutedEventArgs e)
        {
            envioParametros();
        }
    }
}
