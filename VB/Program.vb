Module Program

    Sub Main()
        Dim g = new Geocoder("your api key here")
        Dim location = g.Locate("123 South Main Street", "SLC", New Dictionary(Of String, Object) From {
                                   {"score", 90},
                                   {"spatialReference", 4326}
                                   })

        Console.WriteLine(location)
        Console.ReadKey()
    End Sub

End Module