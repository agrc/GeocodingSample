<?php

/**
 * Access API from http://api.mapserv.utah.gov
 * 
 * @require cURL
 * @internal Create your api key at https://developer.mapserv.utah.gov/secure/KeyManagement
 */
class Geocoder {
    private $api_key;
    private $api_url;
    private $options;

    public function __construct($user_api_key) {
        if(empty($user_api_key)){
            throw new InvalidArgumentException("Expecting API key, none received in Geocoder::__construct");
        }

        $this->api_key = $user_api_key;    
        $this->api_url = 'http://api.mapserv.utah.gov/api/v1/geocode/%s/%s';
        $this->options = array(
            # Return the transfer as string
            CURLOPT_RETURNTRANSFER => true,

            # Specify content type (json)
            CURLOPT_HTTPHEADER => array('Content-type: application/json')
        );
    }

    /**
     * Wrap our cURL call in a function.
     *
     * @param array $options
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
     * Locate returns {X, Y} coordinates based on address, zone and any other parameters
     *
     * @param string $address
     * @param string $zone
     * @param array $parameters [optional]
     * @return 
     */
    public function locate ($address, $zone, $parameters = array()){
        $paramArray = array();
        $parameters['apiKey'] = $this->api_key;

        foreach($parameters as $k => $v){
            $paramArray[] = $k . '=' . $v;
        }

        $this->options[CURLOPT_URL] = sprintf($this->api_url, rawurlencode($address), rawurlencode($zone) ) . 
                                      '?' . implode('&', $paramArray);

        $response = $this->curl($this->options);
        $decoded = json_decode( $response );

        if($decoded->status != 200){
            throw new RuntimeException("Error status: " . $response, $decoded->status);
        }

        return $decoded->result; 
    }
}

try{
    $Geocoder = new Geocoder('enter your desktop key here');
    $result = $Geocoder->locate('123 South Main Street', 'SLC', array('acceptScore' => 90, 'spatialReference' => 4326));
    
    echo '<pre>' . print_r($result, 1) . '</pre>';
} catch(Exception $e){
    if ($e instanceof InvalidArgumentException OR $e instanceof RuntimeException){
        die($e->getMessage());
    } else {
        die("Caught unrecognized exception: " . $e->getMessage());
    }
}
?>
