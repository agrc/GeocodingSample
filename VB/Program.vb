Module Program

    Sub Main()
        'Register to receive an API key at developer.mapserv.utah.gov
        Dim g = new Geocoder("your api key here")
        Dim acceptScoreThreshold As Integer = 90
        Dim location = g.Locate("123 South Main Street", "SLC", New Dictionary(Of String, Object) From {
                                   {"acceptScore", acceptScoreThreshold},
                                   {"spatialReference", 4326}
                                   })

        Console.WriteLine(location)
        Console.ReadKey()
    End Sub

End Module