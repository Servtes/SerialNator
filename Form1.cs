using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SerialNator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MouseMove += grafoNatorMouseMove;
            Control.CheckForIllegalCrossThreadCalls = false;

            foreach (var puertoCom in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(puertoCom);
            }
            
        }

        #region Funciones necesarias para mover la ventana sin barra de titulo [Jose Sosa]

        //Declaraciones del API de Windows (y constantes usadas para mover el form)
        private const int WM_SYSCOMMAND = 0x112;
        private const int MOUSE_MOVE = 0xF012;
        //
        // Declaraciones del API
        //
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();


        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        //
        // función privada usada para mover el formulario actual
        //
        private void MoverForm()
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, MOUSE_MOVE, 0);
        }


        private void grafoNatorMouseMove(object sender, MouseEventArgs e)
        {
            MoverForm();
        }

  
        private void AcercaDe_Click(object sender, EventArgs e)
        {

        }

        private void btnMinimizar_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Close();
        }

        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "200";

            #region balon informativo sobre los controles [Jose Sosa]
            var balonInformativo = new ToolTip
                                       {
                                           UseFading = true,
                                           UseAnimation = true,
                                           IsBalloon = true,
                                           AutoPopDelay = 15000,
                                           AutomaticDelay = 800,
                                           ReshowDelay = 500,
                                           ShowAlways = true
                                       };
            balonInformativo.SetToolTip(trackBar1,"Establece el tiempo en milisegundos de duracion de cada pulso del reloj");
            balonInformativo.SetToolTip(comboBox1,"Muestra los puertos COM disponibles");
            balonInformativo.SetToolTip(btnInicio,"Inicia el tren de pulsos");
            balonInformativo.SetToolTip(btnParo,"Detiene el tren de pulsos");
            #endregion
            timer1.Stop();
           
        }

        private  void setMilisegundos()
        {
            var valorMilisegundos = trackBar1.Value.ToString();
            label2.Text = valorMilisegundos;
            var milisegundos = Convert.ToInt32(label2.Text);
            timer1.Interval = milisegundos;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
           setMilisegundos();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.Text;
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
           setMilisegundos();
           timer1.Enabled = true;
        }

        //Simulacion de tren de pulsos utilizando un Timer
        private bool cambia = true;
        private void timer1_Tick(object sender, EventArgs e)
        {

          /* if (cambia)
            {
                try
                {
                    serialPort1.Open();
                    cambia = false;
                }
                catch (Exception mensaje)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                    MessageBox.Show(mensaje.ToString(), "Error de Comunicacion");
                }
                cambia = false;
            }
            else
            {
                try
                {
                    serialPort1.Close();
                    cambia = true;
                }
                catch (Exception mensaje)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                    MessageBox.Show(mensaje.ToString(), "Error de Comunicacion");
                }
                cambia = true;
            }*/


           if(cambia)
           {
               button1.BackColor = Color.LightCoral;
               button1.Text = "0";
               cambia = false;
           }
           else
           {
               button1.BackColor = Color.LightSeaGreen;
               button1.Text = "1";
               cambia = true;
           }
            
        }

        private void btnParo_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
            serialPort1.Close();
        }
       
    }
}
