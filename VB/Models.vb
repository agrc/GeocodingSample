Public Class ResultContainer
    Public Property Status As Int32
    Public Property Message As String
    Public Property Result As GeocodeResult
End Class

Public Class GeocodeResult
    Public Property Location As Location
    Public Property Score As Double
    Public Property Locator As String
    Public Property MatchAddress As String
    Public Property InputAddress As String
    Public Property StandardizedAddress As String
    Public Property AddressGrid As String
End Class

Public Class Location
    Public Property X As Double
    Public Property Y As Double

    Overrides Function ToString() As String
        Return string.Format("X: {0}, Y: {1}", X, Y)
    End Function
End Class