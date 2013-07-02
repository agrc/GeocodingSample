require 'net/http'
require 'json'

class AGRCGeocoder
  # get your API key at https://developer.mapserv.utah.gov/secure/KeyManagement

  attr_accessor :api_key

  def initialize(api_key)
    @api_key = api_key
    @api_url = 'http://api.mapserv.utah.gov/api/v1/geocode/%s/%s' # street, zone
  end

  def locate(address, zone, params = {})
    params['apiKey'] = @api_key
    uri = URI(URI.encode(sprintf(@api_url, address, zone)))
    uri.query = URI.encode_www_form(params)

    res = Net::HTTP.get_response(uri)
    raise AGRCGeocoderException.new("Received HTTP status #{res.code}") if res.code.to_i != 200

    obj = JSON.parse(res.body)
    raise AGRCGeocoderException.new(obj['message']) if obj['status'] != 200
    {
      :score => obj['result']['score'],
      :x => obj['result']['location']['x'],
      :y => obj['result']['location']['y']
    }
  end
end

class AGRCGeocoderException < Exception; end

