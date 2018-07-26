Imports System.Net
Imports System.Net.Http
Imports System.Web.Script.Serialization

Public Class Geocoder
    Private Const UrlTemplate As String = "http://api.mapserv.utah.gov/api/v1/geocode/{{street}}/{{zone}}"
    Private ReadOnly _apiKey As String

    Public Sub New(apiKey As String)
        _apiKey = apiKey
    End Sub

    Public Function Locate(street As String, zone As String, Optional options As Dictionary(Of String, Object) = Nothing) As Location

        Dim url = UrlTemplate.Replace("{{street}}", street).Replace("{{zone}}", zone)
        If options Is Nothing
            options = New Dictionary(Of String, Object)
        End If

        options.Add("apikey", _apiKey)

        url += "?" 

        For Each pair As KeyValuePair(Of String,Object) In options
            url += "&" + String.Concat(Uri.EscapeDataString(pair.Key), "=", Uri.EscapeDataString(pair.Value.ToString()))
        Next

        Dim client = New HttpClient()
        Dim response = client.GetAsync(url).Result

        Dim responseString = response.Content.ReadAsStringAsync().Result

        Dim serializer = New JavaScriptSerializer()

        Dim resultContainer = serializer.Deserialize (Of ResultContainer)(responseString)

        If response.StatusCode <> HttpStatusCode.OK Or resultContainer.Status <> HttpStatusCode.OK
            Console.WriteLine("{0} {1} was not found. {2}", street, zone, resultContainer.Message)

            Return Nothing
        End If

        Dim result = resultContainer.Result

        Console.WriteLine("match: {0} score [{1}]", result.Score, result.MatchAddress)

        Return result.Location
    End Function

End Class