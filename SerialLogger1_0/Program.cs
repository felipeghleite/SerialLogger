using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace SerialLogging
    {
    public class SerialClass
        {
        public static char[] readBuffer = new char[Program.serial.ReadBufferSize];
        string charStr = new string(readBuffer, 0 , Program.serial.ReadBufferSize);
        Form1 frm;
        public SerialClass( Form1 form )
            {
            frm = form;
            }
        public void read( object sender , EventArgs e )
            {
            if( Program.serial.IsOpen == true )
                {
                //charStr = Program.serial.ReadExisting();
                charStr = Program.serial.ReadLine();
                frm.textBox2_UpdateText( charStr );
                }
            else
                {

                }
            }

        }


    static class Program
        {
        public static string[] portString = SerialPort.GetPortNames();
        public static int x = 0;
        public static int y = 0;
        /* Creating a Serial Object*/
        public static SerialPort serial = new SerialPort();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new Form1() );
            }

        public static void SerialOpen( string Port , int BaudRate , int DataBits , Parity parity )
            {
            if( !serial.IsOpen )
                {
                serial.PortName = Port;
                serial.BaudRate = BaudRate;
                serial.DataBits = DataBits;
                serial.Parity = parity;
                serial.Open();

                serial.DataReceived += new SerialDataReceivedEventHandler( Form1.oSerial.read );
                }
            }
        public static void SendText( string Text )
            {
            if( serial.IsOpen )
                {
                serial.Write( Text );
                }

            }

        public static void closeCom()
            {
            Thread.Sleep( 100 );
            serial.Close();
            Thread.Sleep( 100 );

            }

        }


    }
