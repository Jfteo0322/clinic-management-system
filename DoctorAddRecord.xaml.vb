Imports System.Data.OleDb
Imports MaterialDesignThemes.Wpf
Public Class DoctorAddRecord

    'Initialising database connection strings
    Dim con As New OleDbConnection
    Dim prov As String = "Provider = Microsoft.Jet.OLEDB.4.0;"
    Dim src As String = "Data Source=D:\VB Sources\Oceana Clinic Management\bin\Debug\Oceana.mdb"
    Dim ConnectionString As String = prov & src

    Public Sub New()

        ' Some magic happens 
        InitializeComponent()

        ' Calling Combobox subroutines
        loadPatients()
        loadMedicine()
        loadServices()

    End Sub

    Public Sub loadPatients()

        'Selecting IC Number from Patient table
        con.ConnectionString = prov & src
        con.Open()
        Dim sql As String
        sql = "SELECT ICNumber FROM Patient;"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboSelectPatient.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Public Sub loadServices()

        'Selecting services from Services table
        con.ConnectionString = prov & src
        con.Open()
        Dim sql As String
        sql = "SELECT Name FROM Services;"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboSelectServices.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Public Sub loadMedicine()

        'Selecting medicine from Medicine table
        con.ConnectionString = prov & src
        con.Open()
        Dim sql As String
        sql = "SELECT Name FROM Medicine;"
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboSelectMedicine.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Private Sub btnAddTreatment_Click(sender As Object, e As RoutedEventArgs)

        'This is complicated as shit so the basic process is data validation , getting variables from form and finally inserting records to both Treatment and Billing tables

        'Checking for empty patient field
        If cboSelectPatient.Text = "" Then

            Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
            msgqueue.Enqueue("Please select a patient")
            msgSnackbar.MessageQueue = msgqueue

            'Checking for empty fields
        ElseIf cboSelectServices.Text = "" Or cboSelectMedicine.Text = "" Or txtServiceQuantity.Text = "" Or txtMedicineQuantity.Text = "" Or txtDiagnosis.Text = "" Then

            Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
            msgqueue.Enqueue("Please fill in all fields")
            msgSnackbar.MessageQueue = msgqueue

            'Adding new record to database
        Else

            Using con As New OleDbConnection(ConnectionString)


                'Part One , adding record to Treatment table


                'SQL query to insert new treatment details
                Dim sqltreatment As String = "INSERT INTO Treatment (ICNumber, [Date], Service, ServiceQuantity, Medicine, MedicineQuantity, Diagnosis) VALUES (@ICNumber, @Date, @Service, @ServiceQuantity, @Medicine, @MedicineQuantity, @Diagnosis);"

                'Getting variable values from form 
                Dim cmdtreatment As New OleDbCommand(sqltreatment, con)
                cmdtreatment.Parameters.AddWithValue("@ICNumber", cboSelectPatient.Text)
                cmdtreatment.Parameters.AddWithValue("@Date", System.DateTime.Now.ToShortDateString)
                cmdtreatment.Parameters.AddWithValue("@Service", cboSelectServices.Text)
                cmdtreatment.Parameters.AddWithValue("@ServiceQuantity", txtServiceQuantity.Text)
                cmdtreatment.Parameters.AddWithValue("@Medicine", cboSelectMedicine.Text)
                cmdtreatment.Parameters.AddWithValue("@MedicineQuantity", txtMedicineQuantity.Text)
                cmdtreatment.Parameters.AddWithValue("@Diagnosis", txtDiagnosis.Text)
                con.Open()

                'Executing Treatment query
                cmdtreatment.ExecuteNonQuery()


                'Part Two , adding record to Billing table 


                'SQL query to insert new treatment details
                Dim sqlbilling As String = "INSERT INTO Billing (TreatmentID, PaymentStatus, Total, ICNumber) VALUES (@TreatmentID, 'Unpaid', @Total, @ICNumber)"

                'Getting last added TreatmentID record in database
                Dim sqlvalTreatment As String = "SELECT TOP 1 TreatmentID FROM Treatment ORDER BY TreatmentID DESC"
                Dim cmdvalTreatment As New OleDbCommand(sqlvalTreatment, con)
                Dim reader As OleDbDataReader
                Dim valPrefixTreatment As Integer
                Dim valTreatment As String
                reader = cmdvalTreatment.ExecuteReader
                While reader.Read
                    valPrefixTreatment = reader.Item("TreatmentID")
                    valTreatment = "T" & valPrefixTreatment.ToString("000")
                End While

                'Calculating total Service and Medicine prices
                Dim sqlvalService As String = "SELECT Price FROM Services WHERE Name = (SELECT TOP 1 Service FROM Treatment ORDER BY TreatmentID DESC)"
                Dim sqlvalServiceQty As String = "SELECT TOP 1 ServiceQuantity FROM Treatment ORDER BY TreatmentID DESC"
                Dim sqlvalMedicineQty As String = "SELECT TOP 1 MedicineQuantity FROM Treatment ORDER BY TreatmentID DESC"
                Dim cmdvalService As New OleDbCommand(sqlvalService, con)
                Dim cmdvalServiceQty As New OleDbCommand(sqlvalServiceQty, con)
                Dim cmdvalMedicineQty As New OleDbCommand(sqlvalMedicineQty, con)
                Dim valService As String
                Dim valServiceQty As String
                Dim valMedicineQty As String

                reader = cmdvalService.ExecuteReader
                While reader.Read
                    valService = reader.Item("Price")
                End While

                reader = cmdvalServiceQty.ExecuteReader
                While reader.Read
                    valServiceQty = reader.Item("ServiceQuantity")
                End While

                reader = cmdvalMedicineQty.ExecuteReader
                While reader.Read
                    valMedicineQty = reader.Item("MedicineQuantity")
                End While

                'Service price + Medicine price + Doctor consultation  = total
                Dim valtotal As Integer = (valService * valServiceQty) + (valMedicineQty * 10) + 30

                'Getting last added ICNumber in Treatment table
                Dim sqlvalICNumber As String = "SELECT TOP 1 ICNumber FROM Treatment ORDER BY TreatmentID DESC"
                Dim cmdvalICNumber As New OleDbCommand(sqlvalICNumber, con)
                Dim valICNumber As String
                reader = cmdvalICNumber.ExecuteReader
                While reader.Read
                    valICNumber = reader.Item("ICNumber")
                End While

                'Adding billing record to database based on calculated values earlier
                Dim cmdbilling As New OleDbCommand(sqlbilling, con)
                cmdbilling.Parameters.AddWithValue("@TreatmentID", valTreatment.ToString)
                cmdbilling.Parameters.AddWithValue("@Total", valtotal.ToString)
                cmdbilling.Parameters.AddWithValue("@ICNumber", valICNumber.ToString)

                'Executing Billing query
                cmdbilling.ExecuteNonQuery()

            End Using

            'Record succesfully added message
            Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
            msgqueue.Enqueue("Treatment record added successfully")
            msgSnackbar.MessageQueue = msgqueue

            'Resetting form values 
            cboSelectPatient.Text = String.Empty
            cboSelectServices.Text = String.Empty
            cboSelectMedicine.Text = String.Empty
            txtMedicineQuantity.Clear()
            txtServiceQuantity.Clear()
            txtDiagnosis.Clear()

        End If


    End Sub

End Class
