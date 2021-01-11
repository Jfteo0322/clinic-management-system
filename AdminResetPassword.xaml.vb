Imports System.Data
Imports System.Data.OleDb
Imports MaterialDesignThemes.Wpf

Public Class AdminResetPassword

    'Database connection variable declarations
    Dim con As New OleDbConnection
    Dim prov As String = "Provider = Microsoft.Jet.OLEDB.4.0;"
    Dim src As String = "Data Source=D:\VB Sources\Oceana Clinic Management\bin\Debug\Oceana.mdb"
    Dim ConnectionString As String = prov & src

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        loadUsers()

    End Sub

    Public Sub loadUsers()

        'Querying database for Users
        con.ConnectionString = ConnectionString
        con.Open()
        Dim sql As String
        sql = "SELECT Username FROM [User]"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboSelectUsername.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()


    End Sub

    Private Sub btnResetPassword_Click(sender As Object, e As RoutedEventArgs)

        Using con As New OleDbConnection(ConnectionString)

            'SQL query to update database with new credentials
            Dim sql As String = "UPDATE [User] SET [Password] = @Password WHERE Username = @Username;"

            'Getting username and password value from form 
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@Password", txtPassword.Password.ToString)
            cmd.Parameters.AddWithValue("@Username", cboSelectUsername.Text)
            con.Open()

            'Checking for empty password field
            If txtPassword.Password.ToString = "" Then
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(2))
                msgqueue.Enqueue("Password cannot be empty")
                msgSnackbar.MessageQueue = msgqueue

            Else
                cmd.ExecuteNonQuery()
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(2))
                msgqueue.Enqueue("Password updated successfully")
                msgSnackbar.MessageQueue = msgqueue

                'Resetting form values
                cboSelectUsername.Text = String.Empty
                txtPassword.Clear()

            End If

        End Using

    End Sub
End Class
