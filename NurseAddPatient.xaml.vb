Imports System.Data
Imports System.Data.OleDb
Imports MaterialDesignThemes.Wpf

Public Class NurseAddPatient

    'Database connection variable declarations
    Dim con As New OleDbConnection
    Dim prov As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
    Dim src As String = "Data Source=D:\VB Sources\Oceana Clinic Management\bin\Debug\Oceana.mdb"
    Dim ConnectionString As String = prov & src

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        loadBloodType()
        loadGender()

    End Sub

    Public Sub loadGender()

        'Querying database for gender types
        con.ConnectionString = ConnectionString
        con.Open()
        Dim sql As String
        sql = "SELECT Gender FROM Gender"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboGender.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Public Sub loadBloodType()

        'Querying database for gender types
        con.ConnectionString = ConnectionString
        con.Open()
        Dim sql As String
        sql = "SELECT BloodType FROM BloodType"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboBloodType.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As RoutedEventArgs) Handles btnAddUser.Click

        Using con As New OleDbConnection(ConnectionString)

            'SQL query to insert new patient details
            'Ever wondered how far right can you type ?
            Dim sql As String = "INSERT INTO Patient (ICNumber, Name, DateofBirth, Gender, ContactNumber, Email, Weight, Height, BloodType, Allergy) VALUES (@ICNumber, @Name, @DOB, @Gender, @ContactNumber, @Email, @Weight, @Height, @BloodType, @Allergy );"

            'Getting variable values from form 
            Dim cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@ICNumber", txtICNumber.Text)
            cmd.Parameters.AddWithValue("@Name", txtName.Text)
            cmd.Parameters.AddWithValue("@DOB", txtDateofBirth.Text)
            cmd.Parameters.AddWithValue("@Gender", cboGender.Text)
            cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text)
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text)
            cmd.Parameters.AddWithValue("@Height", txtHeight.Text)
            cmd.Parameters.AddWithValue("@BloodType", cboBloodType.Text)
            cmd.Parameters.AddWithValue("@Allergy", txtAllergy.Text)
            con.Open()

            'Checking for empty fields
            If txtICNumber.Text = "" Or txtName.Text = "" Or txtDateofBirth.Text = "" Or cboGender.Text = "" Or txtContact.Text = "" Or txtEmail.Text = "" Or txtWeight.Text = "" Or cboBloodType.Text = "" Then
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(2))
                msgqueue.Enqueue("Please fill in all fields")
                msgSnackbar.MessageQueue = msgqueue

                'Adding new record to database
            Else
                cmd.ExecuteNonQuery()
                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(2))
                msgqueue.Enqueue("New patient added successfully")
                msgSnackbar.MessageQueue = msgqueue

                'Resetting form values
                txtICNumber.Clear()
                txtName.Clear()
                txtDateofBirth.Clear()
                cboBloodType.Text = String.Empty
                txtContact.Clear()
                txtEmail.Clear()
                txtWeight.Clear()
                txtHeight.Clear()
                cboBloodType.Text = String.Empty
                txtAllergy.Clear()

            End If

        End Using
    End Sub
End Class
