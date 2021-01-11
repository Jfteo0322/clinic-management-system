Public Class Doctor
    Public Sub New(displayname As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtDisplayname.Text = displayname

    End Sub

    Private Sub Doctor_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        'Allowing drag to move 
        Me.DragMove()
    End Sub

    Private Sub btnAddRecord_Click(sender As Object, e As RoutedEventArgs) Handles btnAddRecord.Click
        'Navigating to AddRecord page
        MainPage.Children.Clear()
        Dim AddRecord As New UserControl
        AddRecord = New DoctorAddRecord
        MainPage.Children.Add(AddRecord)
    End Sub

    Private Sub btnHome_Click(sender As Object, e As RoutedEventArgs)
        'Navigating to Home page
        MainPage.Children.Clear()
        Dim DoctorHome As New UserControl
        DoctorHome = New DoctorHome
        MainPage.Children.Add(DoctorHome)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs)
        'Logout back to login page
        Dim logout As New MainWindow
        logout.Show()
        Me.Close()
    End Sub
End Class
