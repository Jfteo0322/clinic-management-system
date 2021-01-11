Public Class Admin
    Public Sub New(displayname As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtDisplayname.Text = displayname

    End Sub

    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        'Allowing drag to move
        Me.DragMove()
    End Sub

    Private Sub btnResetPassword_Click(sender As Object, e As RoutedEventArgs)
        'Navigating to ResetPassword page
        MainPage.Children.Clear()
        Dim ResetPasswordPage As New UserControl
        ResetPasswordPage = New AdminResetPassword
        MainPage.Children.Add(ResetPasswordPage)
    End Sub

    Private Sub btnHome_Click(sender As Object, e As RoutedEventArgs)
        'Navigating to home page
        MainPage.Children.Clear()
        Dim HomePage As New UserControl
        HomePage = New AdminHome
        MainPage.Children.Add(HomePage)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        'Logout back to login page
        Dim logout As New MainWindow
        logout.Show()
        Me.Close()
    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As RoutedEventArgs) Handles btnAddUser.Click
        'Navigating to add user page
        MainPage.Children.Clear()
        Dim AddUser As New UserControl
        AddUser = New AdminAddUser
        MainPage.Children.Add(AddUser)
    End Sub

End Class

