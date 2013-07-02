require 'webmock/rspec'

require_relative '../agrc_geocoder'

describe 'AGRCGeocoder' do
  describe '#initialize' do
    it 'stores the provided api key' do
      geocoder = AGRCGeocoder.new('abc123')
      geocoder.api_key.should == 'abc123'
    end
  end

  describe '#locate' do
    before :all do
      @geocoder = AGRCGeocoder.new('abc123')
    end

    it 'returns a result location hash with x, y, and score if successful' do
      stubbed_response = '{"result":{"location":{"x":-111.89091588102185,"y":40.7666258019694},"score":100.0,"locator":"Centerlines.StatewideRoads","matchAddress":"123 S Main St, Salt Lake City","inputAddress":"123 South Main St, SLC"},"status":200}'
      stub_request(:get, "http://api.mapserv.utah.gov/api/v1/geocode/123%20South%20Main%20St/SLC?apiKey=abc123&spatialReference=4326").
        to_return(:status => 200, :body => stubbed_response, :headers => {})
      result = @geocoder.locate('123 South Main St', 'SLC', { spatialReference: 4326 })

      result.should be_an_instance_of(Hash)
      result[:score].should == 100
      result[:x].should == -111.89091588102185
      result[:y].should == 40.7666258019694
    end

    it 'throws an AGRCGeocoderException with message if there was a problem' do
      stub_request(:get, "http://api.mapserv.utah.gov/api/v1/geocode/123%20South%20Main%20St/SLC?apiKey=abc123").
        to_return(:status => 200, :body => '{"status":400,"message":"Invalid API key."}', :headers => {})
      expect { result = @geocoder.locate('123 South Main St', 'SLC') }.to raise_error(AGRCGeocoderException, 'Invalid API key.')
    end

    it 'throws an AGRCGeocoderException if receiving bad status code' do
      valid_stubbed_response = '{"result":{"location":{"x":-111.89091588102185,"y":40.7666258019694},"score":100.0,"locator":"Centerlines.StatewideRoads","matchAddress":"123 S Main St, Salt Lake City","inputAddress":"123 South Main St, SLC"},"status":200}'
      stub_request(:get, "http://api.mapserv.utah.gov/api/v1/geocode/123%20South%20Main%20St/SLC?apiKey=abc123").
        to_return(:status => 400, :body => valid_stubbed_response, :headers => {})
      expect { result = @geocoder.locate('123 South Main St', 'SLC') }.to raise_error(AGRCGeocoderException, 'Received HTTP status 400')
    end
  end
end

