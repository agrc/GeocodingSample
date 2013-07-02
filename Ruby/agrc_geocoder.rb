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

    response = JSON.parse(Net::HTTP.get(uri))
    raise AGRCGeocoderException.new(response['message']) if response['status'] != 200
    {
      :score => response['result']['score'],
      :x => response['result']['location']['y'],
      :y => response['result']['location']['x']
    }
  end
end

class AGRCGeocoderException < Exception; end

