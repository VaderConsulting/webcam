Imports System.Runtime.InteropServices

Public Class Form1
    '---port no for listening and sending data---
    Const IP_Address As String = "127.0.0.1"
    Const portNo As Integer = 500

    '---use to spin off a thread to listen for incoming connections---
    Dim t As System.Threading.Thread

    '*****
    '---constants for capturing the video from webcam---
    Const WM_CAP_START = &H400S
    Const WS_CHILD = &H40000000
    Const WS_VISIBLE = &H10000000

    Const WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10
    Const WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11
    Const WM_CAP_EDIT_COPY = WM_CAP_START + 30
    Const WM_CAP_SEQUENCE = WM_CAP_START + 62
    Const WM_CAP_FILE_SAVEAS = WM_CAP_START + 23

    Const WM_CAP_SET_SCALE = WM_CAP_START + 53
    Const WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52
    Const WM_CAP_SET_PREVIEW = WM_CAP_START + 50

    Const SWP_NOMOVE = &H2S
    Const SWP_NOSIZE = 1
    Const SWP_NOZORDER = &H4S
    Const HWND_BOTTOM = 1

    '--The capGetDriverDescription function retrieves the version 
    ' description of the capture driver--
    Declare Function capGetDriverDescriptionA Lib "avicap32.dll" _
       (ByVal wDriverIndex As Short, _
        ByVal lpszName As String, ByVal cbName As Integer, _
        ByVal lpszVer As String, _
        ByVal cbVer As Integer) As Boolean

    '--The capCreateCaptureWindow function creates a capture window--
    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
       (ByVal lpszWindowName As String, ByVal dwStyle As Integer, _
        ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, _
        ByVal nHeight As Short, ByVal hWnd As Integer, _
        ByVal nID As Integer) As Integer

    '--This function sends the specified message to a window or windows--
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
       (ByVal hwnd As Integer, ByVal Msg As Integer, _
        ByVal wParam As Integer, _
       <MarshalAs(UnmanagedType.AsAny)> ByVal lParam As Object) As Integer

    '--Sets the position of the window relative to the screen buffer--
    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" _
       (ByVal hwnd As Integer, _
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, _
        ByVal y As Integer, _
        ByVal cx As Integer, ByVal cy As Integer, _
        ByVal wFlags As Integer) As Integer

    '--This function destroys the specified window--
    Declare Function DestroyWindow Lib "user32" _
       (ByVal hndw As Integer) As Boolean

    '---used as a window handle---
    Dim hWnd As Integer

    '---preview the selected video source---
    Private Sub PreviewVideo(ByVal pbCtrl As PictureBox)
        hWnd = capCreateCaptureWindowA(0, _
            WS_VISIBLE Or WS_CHILD, 0, 0, 0, _
            0, pbCtrl.Handle.ToInt32, 0)
        If SendMessage( _
           hWnd, WM_CAP_DRIVER_CONNECT, _
           0, 0) Then
            '---set the preview scale---
            SendMessage(hWnd, WM_CAP_SET_SCALE, True, 0)
            '---set the preview rate (ms)---
            SendMessage(hWnd, WM_CAP_SET_PREVIEWRATE, 30, 0)
            '---start previewing the image---
            SendMessage(hWnd, WM_CAP_SET_PREVIEW, True, 0)
            '---resize window to fit in PictureBox control---
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, _
               pbCtrl.Width, pbCtrl.Height, _
               SWP_NOMOVE Or SWP_NOZORDER)
        Else
            '--error connecting to video source---
            DestroyWindow(hWnd)
        End If
    End Sub

    '*****
    '---save the video data into the Image global variable---
    Public Sub CaptureImage()
        Dim data As IDataObject
        Dim bmap As Image
        Dim ms As New IO.MemoryStream()

        '---copy the image to the clipboard---
        SendMessage(hWnd, WM_CAP_EDIT_COPY, 0, 0)

        '---retrieve the image from clipboard and convert it 
        ' to the bitmap format
        data = Clipboard.GetDataObject()
        If data Is Nothing Then Exit Sub
        If data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
            '---convert the data into a Bitmap---
            bmap = CType(data.GetData(GetType(System.Drawing.Bitmap)), Image)
            '---save the Bitmap into a memory stream---
            bmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)
            '---write the Bitmap from stream into a byte array---
            Image = ms.GetBuffer
        End If
    End Sub

    '*****
    '---save the video image at regular intervals---
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        CaptureImage()
    End Sub

    Private Sub Form1_FormClosing( _
       ByVal sender As Object, _
       ByVal e As System.Windows.Forms.FormClosingEventArgs) _
       Handles Me.FormClosing
        t.Abort()
        End
    End Sub

    Private Sub Form1_Load( _
       ByVal sender As System.Object, _
       ByVal e As System.EventArgs) _
       Handles MyBase.Load

        '---preview the selected video source
        PreviewVideo(PictureBox1)

        '---listen for incoming connections from clients---
        t = New System.Threading.Thread(AddressOf Listen)
        t.Start()
    End Sub

    '---listen for incoming connections---
    Private Sub Listen()
        Dim localAdd As System.Net.IPAddress = _
           System.Net.IPAddress.Parse(IP_Address)
        Dim listener As New System.Net.Sockets.TcpListener(localAdd, portNo)
        listener.Start()
        While True
            Dim user As New WebCamClient(listener.AcceptTcpClient)
        End While
    End Sub
End Class
