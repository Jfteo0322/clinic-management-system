Public Class NursePaymentConfirmation

    'Bunch of Public properties to accept values from Nurse Payment form 
    Public Property invoiceID As String
    Public Property amountPaid As String
    Public Property change As String
    Public Property total As String

    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        'Allowing drag to move
        Me.DragMove()
    End Sub

    Private Sub NursePaymentConfirmation_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        'Changing label content to variables from Payment form

        lblInvoiceID.Text = invoiceID
        lblTotalAmt.Text = "RM " & total
        lblPaidAmt.Text = "RM " & amountPaid
        lblChangeAmt.Text = "RM " & change

    End Sub

    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
