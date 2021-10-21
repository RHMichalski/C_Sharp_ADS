# C# ADS Web Service

<p align="justify">Program stworzony do obsługi wewnętrznych wymuszeń serwisowych sterownika programowalnego. Wykorzystywany jest do tego ADS Web Service. Z poziomu programu nadpisujemy wewnętrzne adresy w pamięci sterownika. Proste narzędzie stworzone aby w łatwy sposób rozwiązać problem.</a>
<br>
<hr>

<img align="left" src="https://user-images.githubusercontent.com/92121311/138095541-77ab114d-abe9-4e2f-b073-9235a32b2ebf.PNG" alt="DI" width="370" height="305" >
<img align="right" src="https://user-images.githubusercontent.com/92121311/138095560-de72eb46-e5b6-4f23-bf92-716b67aab2ec.PNG" alt="DO" width="370" height="305" >
<br display="block" clear="both">
<hr>
<img align="left" src="https://user-images.githubusercontent.com/92121311/138095571-d573b85e-531f-45b4-a3d8-c0727a509e8b.PNG" alt="AI" width="370" height="305" >
<img align="right" src="https://user-images.githubusercontent.com/92121311/138095577-d9e5cd66-5bb5-454b-9df1-c93fab0b981e.PNG" alt="RTD" width="370" height="305" >
<br display="block" clear="both">
<br>
<p align="center"><small><i>Interfejs do obsługi wymuszeń.</i></small></p>
<br>
<br>
<hr>

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

<p align="justify">Metoda wywoływana po naciścnięciu przycisku <b>Połącz</b>. Pobiera string z pola" AMS Net Id" oraz "ADS Port". Następnie, jeśłi w pole tekstowe wstawiony jest odpowiedni ciąg znaków, flaga bAdsOk ustawiana jest na 1, wywoływana jest metoda CheckConnection(). W przypadku gdy flaga bAdsOk zostanie z wartością prawdziwą następuje aktualizacja.</a>
<hr>

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

<p align="justify">Metoda sprawdza połączenie ze sterownikiem. Zostaje wysłany jeden pakiet kontrolny i w przypadku gdy komunikacja się powiedzie flaga bAdsOk pozostaje z wartością 1. W przypadku wyłapania wyjątku, flaga ustawiana jest na false a na ekran wyrzucany jest kod błędu.</a>
<hr>


```C#

    private void timer1_Tick(object sender, EventArgs e)
    {
        picBoxStatusOFF.Visible = !bAdsOk;
        picBoxStatusON.Visible = bAdsOk;
    }
```

<p align="justify">Prosty sposób na asynchroniczną obsługę wskaźnika "status".</a>
<hr>

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

<p align="justify">Metody wywoływane kolejno podczas: zaznaczenia dowolnego checkboxa w panelu DI, zaznaczenia checkboxa w panelu AI oraz wciśnięcia klawisza enter podczas aktywnego pola tekstowego w obszarze AI. Metoda dodatkowo gubi focus oraz poprzez pobranie zawartości z tych samych adresów, weryfikuje poprawność wpisanej wartości.</a>
<hr>

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

<p align="justify">Metoda stworzona by w łatwy sposób "wyczyścić" wymuszenia ze wszystkich dostępnych adresów.</a>
<hr>




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


<p align="justify">Metoda wykorzystywana do wysyłania informacji do WebService.</a>

