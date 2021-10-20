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

        //Background worker
        //BackgroundWorker workerThread = null;
        #endregion

        #region Variables
        private string szWebServiceUrl = "http://localhost/TcAdsWebService/TcAdsWebService.dll";
        public string szAmsNetId = "10.1.0.50.1.1";
        public int iPort = 801;
        public UInt32 iIndexGroup = 0x4020;
        bool _keepRunning = false;
        bool bAdsOk = false;
        private byte[] abDataBufferDI = new byte[240];
        private byte[] abDataBufferDIs = new byte[240];
        private byte[] abDataBufferDO = new byte[240];
        private byte[] abDataBufferDOs = new byte[240];
        private byte[] abDataBufferAI = new byte[160];
        private byte[] abDataBufferAIs = new byte[160];
        private byte[] abDataBufferAO = new byte[160];
        private byte[] abDataBufferAOs = new byte[160];
        private byte[] abDataBufferRTD = new byte[160];
        private byte[] abDataBufferRTDs = new byte[160];

        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            // InstantiateWorkerThread();

            txtAmsNetId.Text = szAmsNetId;
            txtAdsPort.Text = iPort.ToString();
            textBox_Password.Text = "********";

            // link the WebService with its library
            TcWebService.Url = szWebServiceUrl;
        }
        #endregion

        #region Events
        private void Form1_Load_1(object sender, EventArgs e)
        {
            listBoxDI.SelectedIndex = 0;
            listBoxDO.SelectedIndex = 0;
            listBoxAI.SelectedIndex = 0;
            listBoxAO.SelectedIndex = 0;
            listBoxRTD.SelectedIndex = 0;

            tabKBusCart.SelectedIndex = 0;
        }

        private void btnSetSettings_Click(object sender, EventArgs e)
        {
            szAmsNetId = txtAmsNetId.Text;
            iPort = Convert.ToInt32(txtAdsPort.Text);
            if (textBox_Password.Text == "PaulinaBardowska") bAdsOk = true;
            CheckConnection();
            if (bAdsOk)
            {
                ReadDI();
                ReadDO();
                ReadAI();
                ReadAO();
                ReadRTD();
            } 
        }

        // Check Connection
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            bAdsOk = false;
            szAmsNetId = "";
        }

        // Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            picBoxStatusOFF.Visible = !bAdsOk;
            picBoxStatusON.Visible = bAdsOk;
        }

        // Write
        private void chkBoxDI_click(object sender, MouseEventArgs e)
        {
            if (bAdsOk) WriteDI();
        }
        private void chkBoxDO_click(object sender, MouseEventArgs e)
        {
            if (bAdsOk) WriteDO();
        }
        private void chkBoxAI_click(object sender, MouseEventArgs e)
        {
            if (bAdsOk) WriteAI();
        }
        private void textBoxAI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & bAdsOk)
            {
                WriteAI();
                this.ActiveControl = null;
                ReadAI();
            }
        }

        private void chkBoxAO_click(object sender, MouseEventArgs e)
        {
            if (bAdsOk) WriteAO();
        }
        private void textBoxAO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & bAdsOk)
            {
                WriteAO();
                this.ActiveControl = null;
                ReadAO();
            }
        }

        private void chkBoxRTD_click(object sender, MouseEventArgs e)
        {
            if (bAdsOk) WriteRTD();
        }
        private void textBoxRTD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & bAdsOk)
            {
                WriteRTD();
                this.ActiveControl = null;
                ReadRTD();
            }
        }

        private void buttonUnforceALL_Click(object sender, EventArgs e)
        {
            try
            {
                for (uint i = 1; i < 2401; i++)
                {
                    if (bAdsOk) TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + i, BitConverter.GetBytes(0));
                    progressBarUnforceALL.Value = ((int)i / 24);
                }
                progressBarUnforceALL.Value = 0;

                if (bAdsOk)
                {
                    ReadDI();
                    ReadDO();
                    ReadAI();
                    ReadAO();
                    ReadRTD();
                }
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }


        // Read
        private void listBoxDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxDI.Text = listBoxDI.SelectedItem.ToString();
            if (bAdsOk) ReadDI();            
        }
        private void listBoxDO_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxDO.Text = listBoxDO.SelectedItem.ToString();
            if (bAdsOk) ReadDO();
        }
        private void listBoxAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxAI.Text = listBoxAI.SelectedItem.ToString();
            if (bAdsOk) ReadAI(); 
        }
        private void listBoxAO_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxAO.Text = listBoxAO.SelectedItem.ToString();
            if (bAdsOk) ReadAO();
        }
        private void listBoxRTD_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxRTD.Text = listBoxRTD.SelectedItem.ToString();
            if (bAdsOk) ReadRTD();
        }

        private void tabKBusCart_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabKBusCart.SelectedIndex)
            {
                case 0: if (bAdsOk) ReadDI(); return;
                case 1: if (bAdsOk) ReadDO(); return;
                case 2: if (bAdsOk) ReadAI(); return;
                case 3: if (bAdsOk) ReadAO(); return;
                case 4: if (bAdsOk) ReadRTD(); return;
            }
        }

        #endregion


        #region Methods

        private void CheckConnection()
        {
            try
            {
                byte[] check;
                if (bAdsOk) TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 5000, 1, out check);
            }
            catch (SoapException ex)
            {
                bAdsOk = false;
                MessageBox.Show(ex.Message);
            }

            catch (Exception ex)
            {
                bAdsOk = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void ReadDI()
        {
            // DataBuffer for the incoming data: 
            // 1 byte for "PlcVarBool", 
            // 2 bytes for "PlcVarInt" 

            try
            {
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 5000, 240, out abDataBufferDI);
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 5300, 240, out abDataBufferDIs);

                // converts the first byte of the buffer to bool
                // bool bVarBool = BitConverter.ToBoolean(abDataBufferDI, 0);
                chkboxDI_0.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8));
                chkboxDI_1.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 1);
                chkboxDI_2.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 2);
                chkboxDI_3.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 3);

                chkboxDI_4.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 4);
                chkboxDI_5.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 5);
                chkboxDI_6.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 6);
                chkboxDI_7.Checked = BitConverter.ToBoolean(abDataBufferDI, (listBoxDI.SelectedIndex * 8) + 7);

                chkboxDI_0s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8));
                chkboxDI_1s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 1);
                chkboxDI_2s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 2);
                chkboxDI_3s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 3);

                chkboxDI_4s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 4);
                chkboxDI_5s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 5);
                chkboxDI_6s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 6);
                chkboxDI_7s.Checked = BitConverter.ToBoolean(abDataBufferDIs, (listBoxDI.SelectedIndex * 8) + 7);

                // converts the second and third byte of the buffer to int
                //int iVarInt = BitConverter.ToInt16(abDataBufferDI, 1);

                // writes values into the text-boxes
                //txtBool.Text = bVarBool.ToString();
                //txtInt.Text = iVarInt.ToString();
            }

            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void ReadDO()
        {
            // DataBuffer for the incoming data: 
            // 1 byte for "PlcVarBool", 
            // 2 bytes for "PlcVarInt" 

            try
            {
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 5600, 240, out abDataBufferDO);
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 5900, 240, out abDataBufferDOs);

                // converts the first byte of the buffer to bool
                // bool bVarBool = BitConverter.ToBoolean(abDataBufferDI, 0);
                chkboxDO_0.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8));
                chkboxDO_1.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 1);
                chkboxDO_2.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 2);
                chkboxDO_3.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 3);

                chkboxDO_4.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 4);
                chkboxDO_5.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 5);
                chkboxDO_6.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 6);
                chkboxDO_7.Checked = BitConverter.ToBoolean(abDataBufferDO, (listBoxDO.SelectedIndex * 8) + 7);

                chkboxDO_0s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8));
                chkboxDO_1s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 1);
                chkboxDO_2s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 2);
                chkboxDO_3s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 3);

                chkboxDO_4s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 4);
                chkboxDO_5s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 5);
                chkboxDO_6s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 6);
                chkboxDO_7s.Checked = BitConverter.ToBoolean(abDataBufferDOs, (listBoxDO.SelectedIndex * 8) + 7);

                // converts the second and third byte of the buffer to int
                //int iVarInt = BitConverter.ToInt16(abDataBufferDI, 1);

                // writes values into the text-boxes
                //txtBool.Text = bVarBool.ToString();
                //txtInt.Text = iVarInt.ToString();
            }

            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void ReadAI()
        {
            // DataBuffer for the incoming data: 
            // 1 byte for "PlcVarBool", 
            // 2 bytes for "PlcVarInt" 

            try
            {
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 6200, 160, out abDataBufferAI);
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 6400, 160, out abDataBufferAIs);

                // converts the first byte of the buffer to bool
                // bool bVarBool = BitConverter.ToBoolean(abDataBufferDI, 0);
                chkBoxAI_0.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16));
                chkBoxAI_1.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 2);
                chkBoxAI_2.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 4);
                chkBoxAI_3.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 6);

                chkBoxAI_4.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 8);
                chkBoxAI_5.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 10);
                chkBoxAI_6.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 12);
                chkBoxAI_7.Checked = BitConverter.ToBoolean(abDataBufferAI, (listBoxAI.SelectedIndex * 16) + 14);

                textBoxAI_0s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16))).ToString();
                textBoxAI_1s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 2)).ToString();
                textBoxAI_2s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 4)).ToString();
                textBoxAI_3s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 6)).ToString();

                textBoxAI_4s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 8)).ToString();
                textBoxAI_5s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 10)).ToString();
                textBoxAI_6s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 12)).ToString();
                textBoxAI_7s.Text = (BitConverter.ToInt16(abDataBufferAIs, (listBoxAI.SelectedIndex * 16) + 14)).ToString();

                // converts the second and third byte of the buffer to int
                //int iVarInt = BitConverter.ToInt16(abDataBufferDI, 1);

                // writes values into the text-boxes
                //txtBool.Text = bVarBool.ToString();
                //txtInt.Text = iVarInt.ToString();
            }

            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void ReadAO()
        {
            // DataBuffer for the incoming data: 
            // 1 byte for "PlcVarBool", 
            // 2 bytes for "PlcVarInt" 

            try
            {
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 6600, 160, out abDataBufferAO);
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 6800, 160, out abDataBufferAOs);

                // converts the first byte of the buffer to bool
                // bool bVarBool = BitConverter.ToBoolean(abDataBufferDI, 0);
                chkBoxAO_0.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16));
                chkBoxAO_1.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 2);
                chkBoxAO_2.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 4);
                chkBoxAO_3.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 6);

                chkBoxAO_4.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 8);
                chkBoxAO_5.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 10);
                chkBoxAO_6.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 12);
                chkBoxAO_7.Checked = BitConverter.ToBoolean(abDataBufferAO, (listBoxAO.SelectedIndex * 16) + 14);

                textBoxAO_0s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16))).ToString();
                textBoxAO_1s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 2)).ToString();
                textBoxAO_2s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 4)).ToString();
                textBoxAO_3s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 6)).ToString();

                textBoxAO_4s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 8)).ToString();
                textBoxAO_5s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 10)).ToString();
                textBoxAO_6s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 12)).ToString();
                textBoxAO_7s.Text = (BitConverter.ToInt16(abDataBufferAOs, (listBoxAO.SelectedIndex * 16) + 14)).ToString();

                // converts the second and third byte of the buffer to int
                //int iVarInt = BitConverter.ToInt16(abDataBufferDI, 1);

                // writes values into the text-boxes
                //txtBool.Text = bVarBool.ToString();
                //txtInt.Text = iVarInt.ToString();
            }

            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void ReadRTD()
        {
            // DataBuffer for the incoming data: 
            // 1 byte for "PlcVarBool", 
            // 2 bytes for "PlcVarInt" 

            try
            {
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 7000, 160, out abDataBufferRTD);
                TcWebService.Read(szAmsNetId, iPort, iIndexGroup, 7200, 160, out abDataBufferRTDs);

                // converts the first byte of the buffer to bool
                // bool bVarBool = BitConverter.ToBoolean(abDataBufferDI, 0);
                chkBoxRTD_0.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16));
                chkBoxRTD_1.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 2);
                chkBoxRTD_2.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 4);
                chkBoxRTD_3.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 6);

                chkBoxRTD_4.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 8);
                chkBoxRTD_5.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 10);
                chkBoxRTD_6.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 12);
                chkBoxRTD_7.Checked = BitConverter.ToBoolean(abDataBufferRTD, (listBoxRTD.SelectedIndex * 16) + 14);

                textBoxRTD_0s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16))).ToString();
                textBoxRTD_1s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 2)).ToString();
                textBoxRTD_2s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 4)).ToString();
                textBoxRTD_3s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 6)).ToString();

                textBoxRTD_4s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 8)).ToString();
                textBoxRTD_5s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 10)).ToString();
                textBoxRTD_6s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 12)).ToString();
                textBoxRTD_7s.Text = (BitConverter.ToInt16(abDataBufferRTDs, (listBoxRTD.SelectedIndex * 16) + 14)).ToString();

                // converts the second and third byte of the buffer to int
                //int iVarInt = BitConverter.ToInt16(abDataBufferDI, 1);

                // writes values into the text-boxes
                //txtBool.Text = bVarBool.ToString();
                //txtInt.Text = iVarInt.ToString();
            }

            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }


        private void WriteDI()
        {

            try
            {
                // byte-arrays for the variables
                // byte[] abDataBool = new byte[1];
                //byte[] abDataInt = new byte[2];

                // gets values from the textboxes
                // bool bValueBool = Convert.ToBoolean(txtBool.Text);
                // int iValueInt = Convert.ToInt16(txtInt.Text);
                // string szValueString = txtString.Text;

                // converts variables to byte-arrays
                //abDataBool = BitConverter.GetBytes(bValueBool);
                //abDataInt = BitConverter.GetBytes((Int16)iValueInt);
                //chkboxDI_1_0
                // writes values to PLC
                
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 0, BitConverter.GetBytes(chkboxDI_0.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 1, BitConverter.GetBytes(chkboxDI_1.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 2, BitConverter.GetBytes(chkboxDI_2.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 3, BitConverter.GetBytes(chkboxDI_3.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 4, BitConverter.GetBytes(chkboxDI_4.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 5, BitConverter.GetBytes(chkboxDI_5.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 6, BitConverter.GetBytes(chkboxDI_6.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + ((uint)listBoxDI.SelectedIndex * 8) + 7, BitConverter.GetBytes(chkboxDI_7.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 0, BitConverter.GetBytes(chkboxDI_0s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 1, BitConverter.GetBytes(chkboxDI_1s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 2, BitConverter.GetBytes(chkboxDI_2s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 3, BitConverter.GetBytes(chkboxDI_3s.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 4, BitConverter.GetBytes(chkboxDI_4s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 5, BitConverter.GetBytes(chkboxDI_5s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 6, BitConverter.GetBytes(chkboxDI_6s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5300 + ((uint)listBoxDI.SelectedIndex * 8) + 7, BitConverter.GetBytes(chkboxDI_7s.Checked));
                /*
                for (uint i = 0; i < 8; i++)
                {
                   // string CheckBox _boxname = chkboxDI_1_0.Name;6
                    TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + i, BitConverter.GetBytes(chkboxDI_1_0.Checked));
                }
                */
                //TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 1, abDataInt);chkboxD_1_0s
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void WriteDO()
        {

            try
            {
                // byte-arrays for the variables
                // byte[] abDataBool = new byte[1];
                //byte[] abDataInt = new byte[2];

                // gets values from the textboxes
                // bool bValueBool = Convert.ToBoolean(txtBool.Text);
                // int iValueInt = Convert.ToInt16(txtInt.Text);
                // string szValueString = txtString.Text;

                // converts variables to byte-arrays
                //abDataBool = BitConverter.GetBytes(bValueBool);
                //abDataInt = BitConverter.GetBytes((Int16)iValueInt);
                //chkboxDI_1_0
                // writes values to PLC

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 0, BitConverter.GetBytes(chkboxDO_0.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 1, BitConverter.GetBytes(chkboxDO_1.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 2, BitConverter.GetBytes(chkboxDO_2.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 3, BitConverter.GetBytes(chkboxDO_3.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 4, BitConverter.GetBytes(chkboxDO_4.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 5, BitConverter.GetBytes(chkboxDO_5.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 6, BitConverter.GetBytes(chkboxDO_6.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5600 + ((uint)listBoxDO.SelectedIndex * 8) + 7, BitConverter.GetBytes(chkboxDO_7.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 0, BitConverter.GetBytes(chkboxDO_0s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 1, BitConverter.GetBytes(chkboxDO_1s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 2, BitConverter.GetBytes(chkboxDO_2s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 3, BitConverter.GetBytes(chkboxDO_3s.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 4, BitConverter.GetBytes(chkboxDO_4s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 5, BitConverter.GetBytes(chkboxDO_5s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 6, BitConverter.GetBytes(chkboxDO_6s.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5900 + ((uint)listBoxDO.SelectedIndex * 8) + 7, BitConverter.GetBytes(chkboxDO_7s.Checked));
                /*
                for (uint i = 0; i < 8; i++)
                {
                   // string CheckBox _boxname = chkboxDI_1_0.Name;6
                    TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + i, BitConverter.GetBytes(chkboxDI_1_0.Checked));
                }
                */
                //TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 1, abDataInt);chkboxD_1_0s
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void WriteAI()
        {

            try
            {
                // byte-arrays for the variables
                // byte[] abDataBool = new byte[1];
                //byte[] abDataInt = new byte[2];

                // gets values from the textboxes
                // bool bValueBool = Convert.ToBoolean(txtBool.Text);
                // int iValueInt = Convert.ToInt16(txtInt.Text);
                // string szValueString = txtString.Text;

                // converts variables to byte-arrays
                //abDataBool = BitConverter.GetBytes(bValueBool);
                //abDataInt = BitConverter.GetBytes((Int16)iValueInt);
                //chkboxDI_1_0
                // writes values to PLC

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 0, BitConverter.GetBytes(chkBoxAI_0.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 2, BitConverter.GetBytes(chkBoxAI_1.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 4, BitConverter.GetBytes(chkBoxAI_2.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 6, BitConverter.GetBytes(chkBoxAI_3.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 8, BitConverter.GetBytes(chkBoxAI_4.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 10, BitConverter.GetBytes(chkBoxAI_5.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 12, BitConverter.GetBytes(chkBoxAI_6.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6200 + ((uint)listBoxAI.SelectedIndex * 16) + 14, BitConverter.GetBytes(chkBoxAI_7.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 0, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_0s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 2, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_1s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 4, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_2s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 6, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_3s.Text)));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 8, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_4s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 10, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_5s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 12, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_6s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6400 + ((uint)listBoxAI.SelectedIndex * 16) + 14, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAI_7s.Text)));
                /*
                for (uint i = 0; i < 8; i++)
                {
                   // string CheckBox _boxname = chkboxDI_1_0.Name;6
                    TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + i, BitConverter.GetBytes(chkboxDI_1_0.Checked));
                }
                */
                //TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 1, abDataInt);chkboxD_1_0s
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void WriteAO()
        {

            try
            {
                // byte-arrays for the variables
                // byte[] abDataBool = new byte[1];
                //byte[] abDataInt = new byte[2];

                // gets values from the textboxes
                // bool bValueBool = Convert.ToBoolean(txtBool.Text);
                // int iValueInt = Convert.ToInt16(txtInt.Text);
                // string szValueString = txtString.Text;

                // converts variables to byte-arrays
                //abDataBool = BitConverter.GetBytes(bValueBool);
                //abDataInt = BitConverter.GetBytes((Int16)iValueInt);
                //chkboxDI_1_0
                // writes values to PLC

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 0, BitConverter.GetBytes(chkBoxAO_0.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 2, BitConverter.GetBytes(chkBoxAO_1.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 4, BitConverter.GetBytes(chkBoxAO_2.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 6, BitConverter.GetBytes(chkBoxAO_3.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 8, BitConverter.GetBytes(chkBoxAO_4.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 10, BitConverter.GetBytes(chkBoxAO_5.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 12, BitConverter.GetBytes(chkBoxAO_6.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6600 + ((uint)listBoxAO.SelectedIndex * 16) + 14, BitConverter.GetBytes(chkBoxAO_7.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 0, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_0s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 2, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_1s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 4, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_2s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 6, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_3s.Text)));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 8, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_4s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 10, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_5s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 12, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_6s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 6800 + ((uint)listBoxAO.SelectedIndex * 16) + 14, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxAO_7s.Text)));
                /*
                for (uint i = 0; i < 8; i++)
                {
                   // string CheckBox _boxname = chkboxDI_1_0.Name;6
                    TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + i, BitConverter.GetBytes(chkboxDI_1_0.Checked));
                }
                */
                //TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 1, abDataInt);chkboxD_1_0s
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }

        private void WriteRTD()
        {

            try
            {
                // byte-arrays for the variables
                // byte[] abDataBool = new byte[1];
                //byte[] abDataInt = new byte[2];

                // gets values from the textboxes
                // bool bValueBool = Convert.ToBoolean(txtBool.Text);
                // int iValueInt = Convert.ToInt16(txtInt.Text);
                // string szValueString = txtString.Text;

                // converts variables to byte-arrays
                //abDataBool = BitConverter.GetBytes(bValueBool);
                //abDataInt = BitConverter.GetBytes((Int16)iValueInt);
                //chkboxDI_1_0
                // writes values to PLC

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 0, BitConverter.GetBytes(chkBoxRTD_0.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 2, BitConverter.GetBytes(chkBoxRTD_1.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 4, BitConverter.GetBytes(chkBoxRTD_2.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 6, BitConverter.GetBytes(chkBoxRTD_3.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 8, BitConverter.GetBytes(chkBoxRTD_4.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 10, BitConverter.GetBytes(chkBoxRTD_5.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 12, BitConverter.GetBytes(chkBoxRTD_6.Checked));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7000 + ((uint)listBoxRTD.SelectedIndex * 16) + 14, BitConverter.GetBytes(chkBoxRTD_7.Checked));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 0, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_0s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 2, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_1s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 4, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_2s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 6, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_3s.Text)));

                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 8, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_4s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 10, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_5s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 12, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_6s.Text)));
                TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 7200 + ((uint)listBoxRTD.SelectedIndex * 16) + 14, BitConverter.GetBytes((Int16)Convert.ToInt16(textBoxRTD_7s.Text)));
                /*
                for (uint i = 0; i < 8; i++)
                {
                   // string CheckBox _boxname = chkboxDI_1_0.Name;6
                    TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 5000 + i, BitConverter.GetBytes(chkboxDI_1_0.Checked));
                }
                */
                //TcWebService.Write(szAmsNetId, iPort, iIndexGroup, 1, abDataInt);chkboxD_1_0s
            }
            catch (SoapException ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bAdsOk = false;
            }
        }



        #endregion

        private void textBoxAO_0s_Leave(object sender, EventArgs e)
        {
            this.Text = "0";
        }
    }
}