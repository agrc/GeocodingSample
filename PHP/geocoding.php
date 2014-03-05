/**
 * Access API from http://api.mapserv.utah.gov
 * uses curl library
 */
class geocoder {
	
	private $apiKey = '';
	
	private $apiUrl = 'http://api.mapserv.utah.gov/api/v1/geocode/%s/%s';
	
	private $options = array(
			CURLOPT_RETURNTRANSFER => true, //return the transfer as string instead of writing directly
			CURLOPT_HTTPHEADER => array('Content-type: application/json'), //specify content type (json)
			CURLOPT_REFERER => 'http://www.example.com/'	//status 400 "Referrer http header is missing" without this line, but it needs to be the originating website 
		);

	/**
	 *	Class constructor / 
	 */
	public function __construct($userApiKey) {
	
		$this->apiKey = $userApiKey;	
	}
	
	/**
	 * curl processes the request as a function so that it can be reused with multiple mapserv api requests
	 * @param object $options
	 * @return 
	 */
	private function curl ($options){
	    
	    $cUrl = curl_init();
	    curl_setopt_array( $cUrl, $options );
	    $response = curl_exec( $cUrl );
	    curl_close( $cUrl );	
		
		return $response;	
	}
		
	/**
	 * locate returns x/y coordinate based on address and zone (and possibly other parameters)
	 * @param object $address
	 * @param object $zone
	 * @param object $parameters [optional]
	 * @return 
	 */
	public function locate ($address, $zone, $parameters = array()){
						
		$parameters['apiKey'] = $this->apiKey;
		
		$paramArray = array();
		foreach($parameters as $k => $v){
			$paramArray[] = $k.'='.$v;
		}
		$parameterStr = '?'.implode('&', $paramArray);
	
		$url = sprintf($this->apiUrl, rawurlencode($address), rawurlencode($zone) ).$parameterStr;
		
		$this->options[CURLOPT_URL] = $url;
	
		$response = $this->curl($this->options);
	
	    	$decoded = json_decode( $response );
		
		if($decoded->status != 200){
			$decoded->result = 'Error status = '.$decoded->status;
		}
		
		return $decoded->result; 
	}
	
}

//Usage

//generate private apiKey at https://developer.mapserv.utah.gov/secure/KeyManagement
$geocoder = new geocoder(‘insert your api key here');  
$result = $geocoder->locate('123 South Main Street', 'SLC', array('acceptScore' => 90, 'spatialReference' => 4326));

echo '<pre>'.print_r($result,1).'</pre>’;
