Public Class DateTimePicker

    Private Const FormatLengthOfLast As Integer = 2
    Private Enum Direction As Integer
        Previous = -1
        [Next] = 1
    End Enum

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


#Region "Properties"

    Public Property SelectedDate() As Nullable(Of Date)
        Get
            Return CDate(GetValue(SelectedDateProperty))
        End Get

        Set(ByVal value As Nullable(Of Date))
            SetValue(SelectedDateProperty, value)
        End Set
    End Property


    Public Property DateFormat() As String
        Get
            Return CStr(GetValue(DateFormatProperty))
        End Get

        Set(ByVal value As String)
            SetValue(DateFormatProperty, value)
        End Set
    End Property

    Public Property MinimumDate() As Date
        Get
            Return CDate(GetValue(MinimumDateProperty))
        End Get
        Set(ByVal value As Date)
            SetValue(MinimumDateProperty, value)
        End Set
    End Property

    Public Property MaximumDate() As Date
        Get
            Return CDate(GetValue(MaximumDateProperty))
        End Get
        Set(ByVal value As Date)
            SetValue(MaximumDateProperty, value)

        End Set
    End Property



#End Region



#Region "Events"

    Public Custom Event DateChanged As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            [AddHandler](DateChangedEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            [RemoveHandler](DateChangedEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
        End RaiseEvent

    End Event


    Public Shared ReadOnly DateChangedEvent As RoutedEvent =
    EventManager.RegisterRoutedEvent(
    "DateChanged",
    RoutingStrategy.Bubble,
    GetType(RoutedEventHandler),
    GetType(DateTimePicker))


    Public Custom Event DateFormatChanged As RoutedEventHandler

        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(DateFormatChangedEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(DateFormatChangedEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event


    Public Shared ReadOnly DateFormatChangedEvent As RoutedEvent =
        EventManager.RegisterRoutedEvent(
        "DateFormatChanged",
        RoutingStrategy.Bubble,
        GetType(RoutedEventHandler),
        GetType(DateTimePicker))


#End Region


#Region "DependencyProperties"
    Public Shared ReadOnly DateFormatProperty As DependencyProperty = _
                         DependencyProperty.Register("DateFormat", _
                         GetType(String), GetType(DateTimePicker), _
                         New FrameworkPropertyMetadata("yyyy-MM-dd HH:mm",
                          AddressOf onDateFormatChanged),
                        New ValidateValueCallback(AddressOf ValidateDateFormat))


    Public Shared MaximumDateProperty As DependencyProperty = _
      DependencyProperty.Register("MaximumDate", _
                                  GetType(Date), GetType(DateTimePicker), _
                                  New FrameworkPropertyMetadata(CDate("2045-01-01 00:00"),
                                Nothing,
                                  New CoerceValueCallback(AddressOf CoerceMaxDate)))


    Public Shared MinimumDateProperty As DependencyProperty = _
      DependencyProperty.Register("MinimumDate", _
                                  GetType(Date), GetType(DateTimePicker), _
                                  New FrameworkPropertyMetadata(CDate("1900-01-01 00:00"),
                                  Nothing,
                                  New CoerceValueCallback(AddressOf CoerceMinDate)))






    Public Shared ReadOnly SelectedDateProperty As DependencyProperty = _
                           DependencyProperty.Register("SelectedDate", _
                           GetType(Nullable(Of Date)),
                           GetType(DateTimePicker), _
                           New FrameworkPropertyMetadata(Date.Now,
                           New PropertyChangedCallback(AddressOf OnSelectedDateChanged),
                           New CoerceValueCallback(AddressOf CoerceDate)))




#End Region




#Region "EventHandlers"


    Private Sub CalDisplay_SelectedDatesChanged(ByVal sender As Object,
                                                ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles CalDisplay.SelectedDatesChanged

        PopUpCalendarButton.IsChecked = False
        SelectedDate = New Date(Year(CDate(CalDisplay.SelectedDate)),
                              Month(CDate(CalDisplay.SelectedDate)),
                              Day(CDate(CalDisplay.SelectedDate)),
                              Hour(SelectedDate),
                              Minute(SelectedDate), 0)

    End Sub


    Private Sub DateDisplay_PreviewMouseUp(ByVal sender As Object,
                                           ByVal e As System.Windows.Input.MouseButtonEventArgs) _
                                          Handles DateDisplay.PreviewMouseUp

        Dim selstart = DateDisplay.SelectionStart
        FocusOnDatePart(selstart)

    End Sub



    Private Sub DateDisplay_LostFocus(ByVal sender As Object, _
                                      ByVal e As System.Windows.RoutedEventArgs) _
                                      Handles DateDisplay.LostFocus
        While (Not IsDate(DateDisplay.Text) OrElse
               (DateDisplay.Text < MinimumDate) OrElse
               (DateDisplay.Text > MaximumDate)) AndAlso DateDisplay.CanUndo
            DateDisplay.Undo()
        End While

        If IsDate(DateDisplay.Text) AndAlso
            SelectedDate <> CDate(DateDisplay.Text) Then
            SelectedDate = CDate(DateDisplay.Text)
        End If

    End Sub



    Private Sub DateTimePicker_PreviewKeyDown(ByVal sender As Object, _
                                              ByVal e As System.Windows.Input.KeyEventArgs) _
                                              Handles DateDisplay.PreviewKeyDown


        Dim selstart = DateDisplay.SelectionStart
        While Not IsDate(DateDisplay.Text)
            DateDisplay.Undo()
        End While

        e.Handled = True
        Select Case e.Key


            Case Key.Up
                SelectedDate = Increase(selstart, 1)
                FocusOnDatePart(selstart)
            Case Key.Down
                SelectedDate = Increase(selstart, -1)
                FocusOnDatePart(selstart)

            Case Key.Left
                selstart = SelectPreviousPosition(selstart)
                If selstart > -1 Then
                    FocusOnDatePart(selstart)
                Else
                    Me.MoveFocus(New TraversalRequest(FocusNavigationDirection.Previous))
                End If


            Case Key.Right, Key.Tab
                selstart = SelectNextPosition(selstart)
                If selstart > -1 Then
                    FocusOnDatePart(selstart)
                Else
                    PopUpCalendarButton.Focus()

                End If

            Case Else
                If Not Char.IsDigit(CChar(e.KeyboardDevice.ToString)) Then

                    If e.Key = Key.D0 OrElse _
                    e.Key = Key.D1 OrElse _
                    e.Key = Key.D2 OrElse _
                    e.Key = Key.D3 OrElse _
                    e.Key = Key.D4 OrElse _
                    e.Key = Key.D5 OrElse _
                    e.Key = Key.D6 OrElse _
                    e.Key = Key.D7 OrElse _
                    e.Key = Key.D8 OrElse _
                    e.Key = Key.D9 Then
                        e.Handled = False
                    End If
                End If
        End Select

    End Sub

    Private Shared Function CoerceDate(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim dtpicker As DateTimePicker = CType(d, DateTimePicker)
        Dim current As Date = CDate(value)
        If current < dtpicker.MinimumDate Then
            current = dtpicker.MinimumDate
        End If
        If current > dtpicker.MaximumDate Then
            current = dtpicker.MaximumDate
        End If
        Return current
    End Function


    Private Shared Function CoerceMinDate(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim dtpicker As DateTimePicker = CType(d, DateTimePicker)
        Dim current As Date = CDate(value)
        If current >= dtpicker.MaximumDate Then
            Throw New ArgumentException("MinimumDate can not be equal to, or more than maximum date")
        End If
        If current > dtpicker.SelectedDate Then
            dtpicker.SelectedDate = current
        End If
        Return current
    End Function


    Private Shared Function CoerceMaxDate(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim dtpicker As DateTimePicker = CType(d, DateTimePicker)
        Dim current As Date = CDate(value)
        If current <= dtpicker.MinimumDate Then
            Throw New ArgumentException("MaximimumDate can not be equal to, or less than MinimumDate")
        End If
        If current < dtpicker.SelectedDate Then
            dtpicker.SelectedDate = current
        End If
        Return current
    End Function


    Public Shared Sub OnDateFormatChanged(ByVal obj As DependencyObject,
                                          ByVal args As DependencyPropertyChangedEventArgs)

        Dim dtp = DirectCast(obj, DateTimePicker)
        dtp.DateDisplay.Text = Format(dtp.SelectedDate, dtp.DateFormat)
    End Sub

    Public Shared Sub OnSelectedDateChanged(ByVal obj As DependencyObject,
                                            ByVal args As DependencyPropertyChangedEventArgs)

        Dim dtp = DirectCast(obj, DateTimePicker)

        If IsNothing(args.NewValue) Then
            dtp.SelectedDate = args.NewValue

            dtp.DateDisplay.Text = "datum ej satt..."
        Else

            dtp.DateDisplay.Text = Format(args.NewValue, dtp.DateFormat)
            dtp.CalDisplay.SelectedDate = args.NewValue
            dtp.CalDisplay.DisplayDate = args.NewValue

        End If



    End Sub


#End Region



    Public Shared Function ValidateDateFormat(ByVal par As Object) As Boolean
        Dim s As String = CStr(par)
        Return IsDate(Format(Date.Now, s))


    End Function


    Private Function SelectNextPosition(ByVal selstart As Integer) As Integer

        Return SelectPosition(selstart, Direction.Next)

    End Function


    Private Function SelectPreviousPosition(ByVal selstart As Integer) _
                                   As Integer


        Return SelectPosition(selstart, Direction.Previous)

    End Function

    'Selects next or previous date value, depending on the incrementor value  
    'Alternatively moves focus to previous control or the calender button
    Private Function SelectPosition(ByVal selstart As Integer, _
                         ByVal direction As Direction) As Integer

        Dim retval As Integer = 0

        If (selstart > 0 OrElse direction = DateTimePicker.Direction.Next) AndAlso
            (selstart < DateFormat.Length - FormatLengthOfLast OrElse
             direction = DateTimePicker.Direction.Previous) Then
            Dim firstchar As Char = CChar(DateFormat.Substring(selstart, 1))
            Dim nextchar As Char = CChar(DateFormat.Substring(selstart + direction, 1))
            Dim found As Boolean

            While ((nextchar = firstchar OrElse _
                    Not Char.IsLetter(nextchar)) _
                    AndAlso (selstart > 1 OrElse _
                             direction > 0) _
                   AndAlso (selstart < DateFormat.Length - 2 _
                   OrElse direction = DateTimePicker.Direction.Previous))
                selstart += direction
                nextchar = CChar(DateFormat.Substring(selstart + direction, 1))
            End While

            If selstart > 1 Then found = True
            selstart = DateFormat.IndexOf(nextchar)

            If found Then
                retval = selstart
            End If
        Else
            retval = -1
        End If

        Return retval



    End Function



    Private Sub FocusOnDatePart(ByVal selstart As Integer)

        If selstart > DateFormat.Length - 1 Then selstart = DateFormat.Length - 1
        Dim firstchar As Char = CChar(DateFormat.Substring(selstart, 1))

        selstart = DateFormat.IndexOf(firstchar)
        Dim sellength As Integer = Math.Abs((selstart - (DateFormat.LastIndexOf(firstchar) + 1)))
        DateDisplay.Focus()
        DateDisplay.Select(selstart, sellength)

    End Sub

    Private Function Increase(ByVal selstart As Integer,
                              ByVal value As Integer) As Date


        Dim retval As Date = CDate(DateDisplay.Text)

        Try
            Select Case DateFormat.Substring(selstart, 1)
                Case "h", "H"
                    retval = retval.AddHours(value)
                Case "y"
                    retval = retval.AddYears(value)
                Case "M"
                    retval = retval.AddMonths(value)
                Case "m"
                    retval = retval.AddMinutes(value)
                Case "d"
                    retval = retval.AddDays(value)
                Case "s"
                    retval = retval.AddSeconds(value)

            End Select
        Catch ex As ArgumentException
            'Catch dates with year over 9999 etc, dont throw
        End Try

        Return retval
    End Function

End Class
