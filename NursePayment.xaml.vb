Imports System.Data
Imports System.Data.OleDb
Imports MaterialDesignThemes.Wpf

Public Class NursePayment

    'Declaring database variables
    Dim con As New OleDbConnection
    Dim prov As String = "Provider = Microsoft.Jet.OLEDB.4.0;"
    Dim src As String = "Data Source=D:\VB Sources\Oceana Clinic Management\bin\Debug\Oceana.mdb"
    Dim connectionstring As String = prov & src

    Public Sub New()

        ' This call is required by the designer somehow , this was auto-generated
        InitializeComponent()

        ' Calling loadPatients() subroutine
        loadPatients()
    End Sub

    Public Sub loadPatients()

        'Selecting IC Number from Patient table
        con.ConnectionString = prov & src
        con.Open()
        Dim sql As String
        sql = "SELECT TreatmentID FROM Billing WHERE PaymentStatus = 'Unpaid' "
        Dim cmd As New OleDbCommand(sql, con)
        Dim reader As OleDbDataReader = cmd.ExecuteReader

        'Importing data from database to Combobox
        While reader.Read
            cboSelectTreatmentID.Items.Add(reader(0).ToString)
        End While

        reader.Close()
        con.Close()

    End Sub

    Private Sub cboSelectTreatmentID_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSelectTreatmentID.SelectionChanged

        'Loading InvoiceID , Total and Medical Treatment details

        Using con As New OleDbConnection(connectionstring)

            'SQL query to get InvoiceID from database
            Dim sqlInvoice As String = "SELECT InvoiceID FROM Billing WHERE TreatmentID = @TreatmentID"

            Dim cmdInvoice As New OleDbCommand(sqlInvoice, con)
            Dim InvoiceID As Integer
            Dim reader As OleDbDataReader
            con.Open()

            cmdInvoice.Parameters.AddWithValue("@TreatmentID", cboSelectTreatmentID.SelectedItem)
            reader = cmdInvoice.ExecuteReader

            While reader.Read
                InvoiceID = reader.Item("InvoiceID")
            End While

            'Displaying InvoiceID with prefix 
            lblInvoiceID.Text = "OC" & InvoiceID.ToString("000")



            'SQL query to get total amount from database
            Dim sqlTotal As String = "SELECT Total FROM Billing WHERE TreatmentID = @TreatmentID"

            Dim cmdTotal As New OleDbCommand(sqlTotal, con)
            Dim total As Integer

            cmdTotal.Parameters.AddWithValue("@TreatmentID", cboSelectTreatmentID.SelectedItem)
            reader = cmdTotal.ExecuteReader

            While reader.Read
                total = reader.Item("Total")
            End While

            'Prefix manually added in form because label text will be need to be converted to integer during Change calculation
            lblRM.Text = "RM"
            lblTotal.Text = total



            'Converting selected TreatmentID to integer due to Treatment table accepting data in the form of integer
            'Basically trimming off the prefix and converting the string into integer
            'Example : T001 would become 001 , being read as 1 by Access database query 
            Dim prefixTreatmentID As String = cboSelectTreatmentID.SelectedItem
            Dim removeTreatmentID As String = prefixTreatmentID.Remove(0, 1)
            Dim TreatmentID As Integer = Convert.ToInt32(removeTreatmentID)

            'SQL query to get Service Details and Service Quantity from database
            Dim sqlService As String = "SELECT Service FROM Treatment WHERE TreatmentID = @TreatmentID"
            Dim sqlServiceQty As String = "SELECT ServiceQuantity FROM Treatment WHERE TreatmentID = @TreatmentID"

            Dim cmdService As New OleDbCommand(sqlService, con)
            Dim cmdServiceQty As New OleDbCommand(sqlServiceQty, con)
            Dim service As String
            Dim serviceqty As String

            cmdService.Parameters.AddWithValue("@TreatmentID", TreatmentID)
            reader = cmdService.ExecuteReader

            While reader.Read
                service = reader.Item("Service")
            End While

            cmdServiceQty.Parameters.AddWithValue("@TreatmentID", TreatmentID)
            reader = cmdServiceQty.ExecuteReader

            While reader.Read
                serviceqty = reader.Item("ServiceQuantity")
            End While

            'Displaying Service and Service Quantity on a label
            lblService.Text = serviceqty & " x " & service



            'SQL query to get Medicine and Medicine Quantity from database
            Dim sqlMedicine As String = "SELECT Medicine FROM Treatment WHERE TreatmentID = @TreatmentID"
            Dim sqlMedicineQty As String = "SELECT MedicineQuantity FROM Treatment WHERE TreatmentID = @TreatmentID"

            Dim cmdMedicine As New OleDbCommand(sqlMedicine, con)
            Dim cmdMedicineQty As New OleDbCommand(sqlMedicineQty, con)
            Dim medicine As String
            Dim medicineqty As String

            cmdMedicine.Parameters.AddWithValue("@TreatmentID", TreatmentID)
            reader = cmdMedicine.ExecuteReader

            While reader.Read
                medicine = reader.Item("Medicine")
            End While

            cmdMedicineQty.Parameters.AddWithValue("@TreatmentID", TreatmentID)
            reader = cmdMedicineQty.ExecuteReader

            While reader.Read
                medicineqty = reader.Item("MedicineQuantity")
            End While

            'Displaying Medicine and Medicine Quantity on a label
            lblMedicine.Text = medicineqty & " x " & medicine



            'Displaying subtotal for Service and Medicine
            Dim sqlServicePrice As String = "SELECT Price FROM Services WHERE Name = @Service"

            Dim cmdServicePrice As New OleDbCommand(sqlServicePrice, con)
            Dim serviceprice As Integer

            cmdServicePrice.Parameters.AddWithValue("@Service", service)
            reader = cmdServicePrice.ExecuteReader

            While reader.Read
                serviceprice = reader.Item("Price")
            End While

            'Displaying price labels
            lblServicePrice.Text = "RM " & (serviceprice * serviceqty)
            lblMedicinePrice.Text = "RM " & (10 * medicineqty)

            'Displaying doctor consultation labels
            lblDoctorConsultation.Text = "1 x Doctor Consultation"
            lblConsultationCost.Text = "RM 30"

        End Using

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        If txtAmountPaid.Text = "" Then

            Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
            msgqueue.Enqueue("Please enter a payment amount")
            msgSnackbar.MessageQueue = msgqueue

        Else

            'Checking if enough payment has been made
            Dim total As Integer = Convert.ToInt32(lblTotal.Text)
            Dim paid As Integer = Convert.ToInt32(txtAmountPaid.Text)

            If paid < total Then

                Dim msgqueue = New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
                msgqueue.Enqueue("Amount paid less than total amount")
                msgSnackbar.MessageQueue = msgqueue

            Else

                'Calculating change amount
                Dim change As String
                Dim amtpaid As Double = Convert.ToDouble(txtAmountPaid.Text)
                Dim totalamt As String = Convert.ToInt32(lblTotal.Text)
                change = amtpaid - totalamt

                'Passing variables to Payment Confirmation form 
                Dim NursePaymentConfirmation As New NursePaymentConfirmation
                NursePaymentConfirmation.invoiceID = lblInvoiceID.Text
                NursePaymentConfirmation.total = lblTotal.Text
                NursePaymentConfirmation.amountPaid = txtAmountPaid.Text
                NursePaymentConfirmation.change = change
                NursePaymentConfirmation.ShowDialog()

                Using con As New OleDbConnection(connectionstring)

                    'SQL query to update database with paid status
                    Dim sql As String = "UPDATE Billing SET [PaymentStatus] = 'Paid' WHERE TreatmentID = @TreatmentID ;"

                    'Getting TreatmentID value from combobox 
                    Dim cmd As New OleDbCommand(sql, con)
                    Dim TreatmentID As String = cboSelectTreatmentID.SelectedItem
                    cmd.Parameters.AddWithValue("@TreatmentID", TreatmentID)
                    con.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End If
        End If

    End Sub

End Class
