Imports System.Data.OleDb
Imports MaterialDesignThemes.Wpf
Class MainWindow
    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        'Enable drag to move 
        Me.DragMove()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs)

        'Database connection parameters
        Dim prov As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
        Dim src As String = "Data Source=Oceana.mdb"
        Dim ConnectionString As String = prov & src

        Using conn As New OleDbConnection(ConnectionString)

            'SQL Login query using username and password from textbox
            Dim loginQuery As String = "SELECT * FROM [User] WHERE Username = @Username AND Password = @Password"
            Dim cmd As New OleDbCommand(loginQuery, conn)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            cmd.Parameters.AddWithValue("@Password", txtPassword.Password)
            conn.Open()
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            'Checking for username and password in database
            If reader.HasRows Then

                'Displaying the right window for each user
                While reader.Read()
                    Select Case reader("UserRole")

                        Case "AD"
                            Dim i As New Admin(reader("Displayname"))
                            i.Show()
                            Me.Hide()

                        Case "DR"
                            Dim i As New Doctor(reader("DisplayName"))
                            i.Show()
                            Me.Hide()

                        Case "NS"
                            Dim i As New Nurse(reader("DisplayName"))
                            i.Show()
                            Me.Hide()

                    End Select
                End While

            Else
                'Error message if login credentials are not found 
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
                msgqueue.Enqueue("Invalid login credentials")
                msgSnackbar.MessageQueue = msgqueue
            End If

        End Using
    End Sub

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs) Handles btnExit.Click
        'Exit Application
        Close()
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        'Allowing user to press the Enter key to login
        If e.Key = Key.Enter Then
            Call btnLogin_Click(sender, e)
        End If

    End Sub
End Class
