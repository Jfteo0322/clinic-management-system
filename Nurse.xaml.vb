Imports System.Data
Imports System.Data.OleDb
Public Class Nurse

    'Database connection parameters
    Dim con As New OleDb.OleDbConnection
    Dim prov As String = "Provider=Microsoft.Jet.OLEDB.4.0;"
    Dim src As String = "Data Source=D:\VB Sources\Oceana Clinic Management\bin\Debug\Oceana.mdb"

    Public Sub New(displayname As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtDisplayname.Text = displayname

    End Sub

    Private Sub Nurse_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        'Allowing drag to move
        Me.DragMove()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        'Navigating to home page 
        MainPage.Children.Clear()
        Dim NurseHomePage As New UserControl
        NurseHomePage = New NurseHome
        MainPage.Children.Add(NurseHomePage)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        'Navigating to billing page 
        MainPage.Children.Clear()
        Dim NurseBillingPage As New UserControl
        NurseBillingPage = New NurseBilling
        MainPage.Children.Add(NurseBillingPage)
    End Sub

    Public Sub btnPayment_Click(sender As Object, e As RoutedEventArgs) Handles btnPayment.Click
        'Navigating to payment page
        MainPage.Children.Clear()
        Dim NursePaymentPage As New UserControl
        NursePaymentPage = New NursePayment
        MainPage.Children.Add(NursePaymentPage)
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        'Logging out back to login page
        Dim logout As New MainWindow
        logout.Show()
        Me.Close()
    End Sub

    Private Sub btnAddPatient_Click(sender As Object, e As RoutedEventArgs) Handles btnAddPatient.Click
        'Navigating to payment page
        MainPage.Children.Clear()
        Dim NurseAddPatient As New UserControl
        NurseAddPatient = New NurseAddPatient
        MainPage.Children.Add(NurseAddPatient)
    End Sub
End Class
