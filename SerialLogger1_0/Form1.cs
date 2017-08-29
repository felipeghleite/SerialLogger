using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace SerialLogging
    {
    public partial class Form1 : Form
        {
        ASCIIEncoding ascii;

        //public byte[] buff = new byte[256];
        public static SerialClass oSerial;

        public static FileStream file;

        public static string filename = new string('a', 1);
        public Form1()
            {
            int index;
            InitializeComponent();
            oSerial = new SerialClass( this );

            /* Writing the COM ports in the comboBox1 (Serial Ports COM) */
            for( index = 0 ; index < (Program.portString.Length) ; index++ )
                {
                this.COM.Items.Add( Program.portString[index] );
                }

            }

        private void Form1_Load( object sender , EventArgs e )
            {
            }

        private void button2_Click( object sender , EventArgs e )
            {

            }

        private void button4_Click( object sender , EventArgs e )
            {

            }

        private void button3_Click( object sender , EventArgs e )
            {

            }

        private void button5_Click( object sender , EventArgs e )
            {
            Program.closeCom();
            }

        private void button1_Click_1( object sender , EventArgs e )
            {
            Program.SerialOpen( (string)this.COM.SelectedItem , Int32.Parse( (string)this.baudrate.SelectedItem ) , Int32.Parse( (string)this.datasize.SelectedItem ) , ((((string)this.parity.SelectedItem) == "No") ? Parity.None : Parity.Odd) );

            }

        private void textBox1_TextChanged( object sender , EventArgs e )
            {
            Program.SendText( textBox1.Text );
            this.textBox1.Clear();
            }

        delegate void SetTextCallback( string text );
        public void textBox2_UpdateText( string Text )
            {
            String[] Data = new string[3];
            Double XValue = 0;
            Double Y1Value = 0;
            Double Y2Value = 0;

            if ( this.textBox2.InvokeRequired )
                {
                SetTextCallback d = new SetTextCallback(textBox2_UpdateText);
                this.Invoke( d , new object[] { Text } );
                }
            else
                {
                if( Text.Length < 30 )
                    {

                    byte[] byteStr = new byte[Text.Length + 3];

                    if( filename != "a" )
                        {
                        file = new FileStream( Form1.filename , FileMode.Append , FileAccess.Write , FileShare.ReadWrite );
                        }

                    byteStr = Encoding.ASCII.GetBytes( DateTime.Now.ToString() + " ; " );
                    try
                        {
                        if( file != null )
                            {
                            file.Write( byteStr , 0 , byteStr.Length);
                            if( !Text.EndsWith( "\n" ) )
                                {
                                Text = Text.Insert( Text.Length , "\n" );
                                }
                            byteStr = Encoding.ASCII.GetBytes( Text );
                            file.Write( byteStr , 0 , byteStr.Length );
                            file.Close();
                            }
                        }
                    catch( NullReferenceException )
                        {
                        Console.WriteLine( "fudeu" );
                        }

                    this.textBox2.AppendText( Text.Replace( "\r\n" , "\n" ).Replace( "\n" , Environment.NewLine ) );
                    if( Text.Contains( ";" ) )
                        {
                        Text = Text.Replace( '.' , ',' );
                        Data = Text.Split( ';' );

                        try
                            {
                            if( Data[0] != null )
                                {
                                XValue = Single.Parse( Data[0] );
                                }
                            else
                                {
                                Console.WriteLine( "fudeu" );
                                }
                            if( Data[1] != null )
                                {
                                Y1Value = Single.Parse( Data[1] );
                                }
                            else
                                {
                                Console.WriteLine( "fudeu" );
                                }
                            if (Data[2] != null)
                            {
                                Y2Value = Single.Parse(Data[2]);
                            }
                            else
                            {
                                Console.WriteLine("fudeu");
                            }
                        }
                        catch( FormatException )
                            {
                            Console.WriteLine( "is not in a valid format." );
                            }
                        catch( OverflowException )
                            {
                            Console.WriteLine( "is outside the range of a Single." );
                            }
                        catch( IndexOutOfRangeException )
                            {
                            Console.WriteLine( "caraio" );
                            }

                        if( (XValue != 0) & (Y1Value != 0) )
                            {
                            chart1.Series["Series1"].Points.AddXY( XValue /*+ Program.x*/ , Y1Value /*+ Program.y*/ );
                            Program.x += 1;
                            Program.y += 1;
                            }
                        else
                            {
                            Console.WriteLine( "fudeu" );
                            }
                        if ((XValue != 0) & (Y2Value != 0))
                        {
                            chart1.Series["Series2"].Points.AddXY(XValue /*+ Program.x*/ , Y2Value /*+ Program.y*/ );
                            Program.x += 1;
                            Program.y += 1;
                        }
                        else
                        {
                            Console.WriteLine("fudeu");
                        }

                    }
                    else
                        {
                        Console.WriteLine( "fudeu" );
                        }
                    }
                }

            }

        private void button1_Click( object sender , EventArgs e )
            {
            /* Button1 is used for folder browsing dialog box. */
            if( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
                {
                textBox3.Text = folderBrowserDialog1.SelectedPath;
                }

            filename = textBox3.Text;
            }

        private void textBox3_TextChanged( object sender , EventArgs e )
            {
            filename = textBox3.Text;
            }

        private void button2_Click_1( object sender , EventArgs e )
            {
            /* Button2 is used only for testing purposes. It add a point to the XY Series and plot into the chart. */ 
            chart1.Series["Series1"].Points.AddXY( Program.x , Program.y);
            Program.x += 1;
            Program.y += 1;
            }
        }

    }
