Imports System.Data
Imports System.Data.OleDb
Imports MaterialDesignThemes.Wpf

Public Class AdminAddUser

    'Database connection variable declarations
    Dim con As New OleDbConnection
    Dim prov As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
    Dim src As String = "Data Source=D:\VB Sources\Oceana Clinic Management\bin\Debug\Oceana.mdb"
    Dim ConnectionString As String = prov & src

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Loading user levels
        loadUserLevels()

    End Sub

    Public Sub loadUserLevels()

        'Querying database for user roles
        con.ConnectionString = ConnectionString
        con.Open()
        Dim sql As String
        sql = "SELECT UserRole FROM [UserRoles]"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboUserLevel.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As RoutedEventArgs) Handles btnAddUser.Click

        Using con As New OleDbConnection(ConnectionString)

            'SQL query to insert new treatment details
            Dim sql As String = "INSERT INTO [USER] (Username, [Password], UserRole, Email, DisplayName) VALUES (@Username, @Password, @UserRole, @Email, @DisplayName);"

            'Getting variable values from form 
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            cmd.Parameters.AddWithValue("@Password", txtPassword.Password.ToString)
            cmd.Parameters.AddWithValue("@UserRole", cboUserLevel.Text)
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            cmd.Parameters.AddWithValue("@DisplayName", txtDisplayName.Text)
            con.Open()

            'Checking for empty fields
            If txtUsername.Text = "" Or txtPassword.Password.ToString = "" Or cboUserLevel.Text = "" Or txtEmail.Text = "" Or txtDisplayName.Text = "" Then
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(2))
                msgqueue.Enqueue("Please fill in all fields")
                msgSnackbar.MessageQueue = msgqueue

                'Adding new record to database
            Else

                'Executing query
                cmd.ExecuteNonQuery()

                'New user added message
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(2))
                msgqueue.Enqueue("New user added successfully")
                msgSnackbar.MessageQueue = msgqueue

                'Resetting form values
                txtUsername.Clear()
                txtPassword.Clear()
                cboUserLevel.Text = String.Empty
                txtEmail.Clear()
                txtDisplayName.Clear()

            End If

        End Using


    End Sub
End Class
