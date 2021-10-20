# C# ADS Web Service

<p align="justify">Program stworzony do obsługi wewnętrznych wymuszeń serwisowych sterownika programowalnego. Wykorzystywany jest do tego ADS Web Service. Z poziomu programu nadpisujemy wewnętrzne adresy w pamięci sterownika.</a>
<br display="block" clear="both">
<br>

<img align="left" src="https://user-images.githubusercontent.com/92121311/138095541-77ab114d-abe9-4e2f-b073-9235a32b2ebf.PNG" alt="DI">
<img align="right" src="https://user-images.githubusercontent.com/92121311/138095560-de72eb46-e5b6-4f23-bf92-716b67aab2ec.PNG" alt="DO">
<br display="block" clear="both">
<hr>
<img align="left" src="https://user-images.githubusercontent.com/92121311/138095571-d573b85e-531f-45b4-a3d8-c0727a509e8b.PNG" alt="AI">
<img align="right" src="https://user-images.githubusercontent.com/92121311/138095577-d9e5cd66-5bb5-454b-9df1-c93fab0b981e.PNG" alt="RTD">
<br display="block" clear="both">
<br>
<p align="center"><small><i>Wygląd strony głównej</i></small></p>
<br>

```C#

  private void btnSetSettings_Click(object sender, EventArgs e)
  {
      szAmsNetId = txtAmsNetId.Text;
      iPort = Convert.ToInt32(txtAdsPort.Text);
      if (textBox_Password.Text == "********") bAdsOk = true;
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
```

```C#

    private void timer1_Tick(object sender, EventArgs e)
    {
        picBoxStatusOFF.Visible = !bAdsOk;
        picBoxStatusON.Visible = bAdsOk;
    }
```

```C#

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
```

```C#

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
```


```C#

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
```

```C#

    private void WriteDI()
    {

        try
        {
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
```

