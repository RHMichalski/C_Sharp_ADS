#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.Services.Protocols;
#endregion

namespace TcAdsWebService01
{
    public partial class Form1 : Form
    {
        #region Instances
        // encode byte-array to strings and backwards
        private System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();

        // Instance of the TcAdsWebService
        TcAdsWebService.TcAdsWebService TcWebService = new TcAdsWebService.TcAdsWebService();
        #endregion

        #region Variables
        private string szWebServiceUrl = "http://localhost/TcAdsWebService/TcAdsWebService.dll";
        public string szAmsNetId = "192.168.0.2.1.1";
        public int iPort = 801;
        public UInt32 iIndexGroup = 0x4020;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            
            txtWebServiceUrl.Text = szWebServiceUrl;
            txtAmsNetId.Text = szAmsNetId;
            txtAdsPort.Text = iPort.ToString();

            // link the WebService with its library
            TcWebService.Url = szWebServiceUrl;
        }
        #endregion

        #region Events
        private void btnRead_Click(object sender, System.EventArgs e)
        {
            szWebServiceUrl = txtWebServiceUrl.Text;
            szAmsNetId = txtAmsNetId.Text;
            iPort = Convert.ToInt32(txtAdsPort.Text);
            Read();
        }

        private void btnWrite_Click(object sender, System.EventArgs e)
        {
            szWebServiceUrl = txtWebServiceUrl.Text;
            szAmsNetId = txtAmsNetId.Text;
            iPort = Convert.ToInt32(txtAdsPort.Text);
            Write();
        }
        #endregion
        
        #region Methods
        private void Read()
        {
            // DataBuffer for the incoming data: 
            // 1 byte for "PlcVarBool", 
            // 2 bytes for "PlcVarInt" 
            // and 81 bytes for "PlcVarString" = 84 bytes
            byte[] abDataBuffer = new byte[84]; 

            try
            {
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 0, 84, out abDataBuffer);

                // converts the first byte of the buffer to bool
                bool bVarBool = BitConverter.ToBoolean(abDataBuffer, 0);

                // converts the second and third byte of the buffer to int
                int iVarInt = BitConverter.ToInt16(abDataBuffer, 1);

                // converts the other bytes of the buffer to string
                string szVarString = encoder.GetString(abDataBuffer, 3, 81);

                // writes values into the text-boxes
                txtBool.Text = bVarBool.ToString();
                txtInt.Text = iVarInt.ToString();
                txtString.Text = szVarString;
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void Write()
        {
            try
            {
                // byte-arrays for the variables
                byte[] abDataBool = new byte[1];
                byte[] abDataInt = new byte[2];
                byte[] abDataString = new byte[81];

                // gets values from the textboxes
                bool bValueBool = Convert.ToBoolean(txtBool.Text);
                int iValueInt = Convert.ToInt16(txtInt.Text);
                string szValueString = txtString.Text;

                // converts variables to byte-arrays
                abDataBool = BitConverter.GetBytes(bValueBool);
                abDataInt = BitConverter.GetBytes((Int16)iValueInt);

                encoder.GetBytes(szValueString,							    // Source
                                    0,										// Position of the first character to convert
                                    encoder.GetByteCount(szValueString),	// Gets the length of the source
                                    abDataString,							// Byte-array to save the conversion to
                                    0);										// First byte in the array to save the conversion to

                // writes values to PLC
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 0, abDataBool);
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 1, abDataInt);
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 3, abDataString);
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}