Imports System.Net.Sockets
Imports System.IO

Public Class Form1
    '---get own IP address---
    Private ips As Net.IPHostEntry = _
       Net.Dns.GetHostEntry(Net.Dns.GetHostName())

    '---port nos and server IP address---
    Const PORTNO As Integer = 500
    Private server_IP As String = "127.0.0.1"

    '---size of the video image---
    Const SIZEOFIMAGE As Integer = 341504

    '---use for connecting to the server---
    Private client As TcpClient

    '--used for sending and receiving data---
    Private data() As Byte

    '---used for receiving images from the server---
    Private t As System.Threading.Thread

    Private Sub ReceiveImageLoop()
        '---keep on receiving image until an error occurs---
        While ReceiveImage()
        End While
        '---display error message---
        MsgBox("Server has stopped responding. Please try restarting the video.")
    End Sub

    '---Sends a message to the server---
    Private Sub SendMessage(ByVal message As String)
        '---adds a carriage return char---
        message += vbLf
        Try
            '---send the text
            Dim ns As System.Net.Sockets.NetworkStream
            SyncLock client.GetStream
                ns = client.GetStream
                Dim bytesToSend As Byte() = _
                   System.Text.Encoding.ASCII.GetBytes(message)
                '---sends the text---
                ns.Write(bytesToSend, 0, bytesToSend.Length)
            End SyncLock
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    '---receive video image from server---
    Public Function ReceiveImage() As Boolean
        Dim s As New MemoryStream
        Try
            Dim nws As NetworkStream = client.GetStream
            Dim counter As Integer = 0
            Dim totalBytes As Integer = 0

            Do
                '---read the incoming data---
                Dim bytesRead As Integer = _
                   nws.Read(data, 0, client.ReceiveBufferSize)
                totalBytes += bytesRead
                '---write the byte() array into the memory stream---
                s.Write(data, 0, bytesRead)
                counter += 1
                'Loop Until totalBytes >= SIZEOFIMAGE
            Loop Until totalBytes >= SIZEOFIMAGE

            '---display the image in the PictureBox control---
            PictureBox1.Image = Image.FromStream(s)
        Catch ex As InvalidOperationException
            '---ignore this error---
            Console.WriteLine(ex.ToString)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return False
        End Try

        '---ask the server to send the next image---
        SendMessage("Send")
        Return True
    End Function

    Private Sub btnStartStop_Click( _
       ByVal sender As System.Object, _
       ByVal e As System.EventArgs) _
       Handles btnStartStop.Click

        If CType(sender, Button).Text = "Start" Then
            Try
                '---set the server IP address---
                server_IP = txtServerIP.Text

                '---connect to the server---
                client = New TcpClient
                client.Connect(server_IP, PORTNO)
                ReDim data(client.ReceiveBufferSize - 1)

                '---send message---
                SendMessage("Send")

                '---begin reading data asynchronously from the server---
                t = New System.Threading.Thread(AddressOf ReceiveImageLoop)
                t.Start()

                '---change the text on the Button---
                CType(sender, Button).Text = "Stop"
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        Else
            '---send message---
            SendMessage("Stop")
            t.Abort()

            '---change the text on the Button---
            CType(sender, Button).Text = "Start"
        End If
    End Sub

    Private Sub Form1_FormClosing( _
       ByVal sender As Object, _
       ByVal e As System.Windows.Forms.FormClosingEventArgs) _
       Handles Me.FormClosing
        t.Abort()
    End Sub

End Class
