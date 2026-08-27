Imports System.Net.Sockets

'---class to contain information of each client---
Public Class WebCamClient
    '--constant for LineFeed character---
    Private Const LF As Integer = 10

    '---contains a list of all the clients---
    Public Shared AllClients As New Hashtable

    '---information about the client---
    Private _client As TcpClient
    Private _clientIP As String

    '---used for sending/receiving data---
    Private data() As Byte

    '---used to store partially received data---
    Private partialStr As String

    '---when a client is connected---
    Public Sub New(ByVal client As TcpClient)
        _client = client

        '---get the client IP address---
        _clientIP = client.Client.RemoteEndPoint.ToString

        '---add the current client to the hash table---
        AllClients.Add(_clientIP, Me)

        '---start reading data from the client in a separate thread---
        ReDim data(_client.ReceiveBufferSize - 1)
        _client.GetStream.BeginRead(data, 0, _
           CInt(_client.ReceiveBufferSize), _
           AddressOf ReceiveMessage, Nothing)
    End Sub

    '---send the data to the client---
    Public Sub SendData(ByVal data As Byte())
        Try
            Dim ns As System.Net.Sockets.NetworkStream
            SyncLock _client.GetStream
                ns = _client.GetStream
                ns.Write(data, 0, data.Length)
            End SyncLock
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    '---receiving a message from the client---
    Public Sub ReceiveMessage(ByVal ar As IAsyncResult)
        '---read from client---
        Dim bytesRead As Integer
        Try
            SyncLock _client.GetStream
                bytesRead = _client.GetStream.EndRead(ar)
            End SyncLock
            '---client has disconnected---
            If bytesRead < 1 Then
                AllClients.Remove(_clientIP)
                Exit Sub
            Else
                Dim messageReceived As String
                Dim i As Integer = 0
                Dim start As Integer = 0
                '---loop until no more chars---
                While data(i) <> 0
                    '---do not scan more than what is read---
                    If i + 1 > bytesRead Then Exit While

                    '---if LF is detected---
                    If data(i) = LF Then
                        messageReceived = partialStr & _
                           System.Text.Encoding.ASCII.GetString( _
                           data, start, i - start)
                        Console.WriteLine("Received: " & messageReceived)

                        If messageReceived.StartsWith("Send") Then
                            SendData(Image)
                        End If
                        start = i + 1
                    End If
                    i += 1
                End While
                '---partial string---
                If start <> i Then
                    partialStr = _
                       System.Text.Encoding.ASCII.GetString( _
                       data, start, i - start)
                End If
            End If

            '---continue reading from client---
            SyncLock _client.GetStream
                _client.GetStream.BeginRead(data, 0, _
                CInt(_client.ReceiveBufferSize), _
                AddressOf ReceiveMessage, Nothing)
            End SyncLock
        Catch ex As Exception
            '---remove the client from the HashTable---
            AllClients.Remove(_clientIP)
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

End Class
