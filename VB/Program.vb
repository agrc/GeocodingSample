Module Program

    Sub Main()
        'Common spatialReference wkid constants - see http://resources.arcgis.com/en/help/main/10.1/018z/pdf/geographic_coordinate_systems.pdf
        Const UtmZone12N As Integer = 26912 'Default
        Const LatLongWgs84 As Integer = 4326
        Const WebMercator As Integer = 3857
        'Register to receive an API key at developer.mapserv.utah.gov
        Dim g = new Geocoder("your api key here")
        Dim acceptScoreThreshold As Integer = 90
        Dim location = g.Locate("123 South Main Street", "SLC", New Dictionary(Of String, Object) From {
                                   {"acceptScore", acceptScoreThreshold},
                                   {"spatialReference", LatLongWgs84}
                                   })

        Console.WriteLine(location)
        Console.ReadKey()
    End Sub

End Module