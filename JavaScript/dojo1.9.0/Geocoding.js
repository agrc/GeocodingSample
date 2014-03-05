define('Geocode', ["dojo/request", 'dojo/_base/lang', 'dojo/Deferred'],

function(request, lang, Deferred) {
  var urlTemplate = "http://api.mapserv.utah.gov/api/v1/geocode/{street}/{zone}";

  return locate = function(street, zone, apiKey, params) {
    var def = new Deferred();

    var url = lang.replace(urlTemplate, {
      street: street,
      zone: zone
    });

    params = !params ? {} : params;

    params.apiKey = apiKey;

    request.get(url, {
      headers: {
        "Content-Type": "application/json"
      },
      query: params,
      handleAs: "json"
    }).then(function(response) {
      if (response.status !== 200) {
        var message = lang.replace("{street} {zone} was not found. {message}", {
          street: street,
          zone: zone,
          message: response.message
        });

        console.log(message);
        def.reject(message);
      }

      var result = response.result;

      console.log(lang.replace("match: {matchAddress} score [{score}]", result))

      def.resolve(response.result.location);
    },

    function(err) {
      def.reject(lang.replace("{street} {zone} was not found. {message}", {
        street: street,
        zone: zone,
        message: err.response.data.message
      }));
    });

    return def;
  };
});

require(['Geocode'], function(Geocode) {
  // Create your api key at
  // https://developer.mapserv.utah.gov/secure/KeyManagement
  var result = Geocode('123 South Main St', 'SLC', 'insert your api key here', {
    acceptScore: 90,
    spatialReference: 4326
  });

  result.then(function(result) {
    console.log(result);
  }, function(err) {
    console.warn(err);
  });
});
