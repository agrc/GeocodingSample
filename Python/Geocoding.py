# -*- coding: utf-8 -*-

"""
geocoding.py
~~~~~~~~~~~~~

This module contains logic to geocode an address with agrc's API
"""
#: To get the requests package visit http://docs.python-requests.org/en/latest/user/install.html
import requests


class Geocoder(object):

    _api_key = None
    _url_template = "http://api.mapserv.utah.gov/api/v1/geocode/{}/{}"

    def __init__(self, api_key):
        """
        Create your api key at
        https://developer.mapserv.utah.gov/secure/KeyManagement
        """
        self._api_key = api_key

    def locate(self, street, zone, **kwargs):
        kwargs["apiKey"] = self._api_key

        r = requests.get(self._url_template.format(street, zone), params=kwargs)

        response = r.json()

        if r.status_code is not 200 or response["status"] is not 200:
            print "{} {} was not found. {}".format(street, zone, response["message"])
            return None

        result = response["result"]

        print "match: {} score [{}]".format(result["score"], result["matchAddress"])
        return result["location"]

if __name__ == "__main__":
    """
    Usage: Example usage is below. The dictionary passed in with ** can be any of the
    optional parameters for the api.
    """
    geocoder = Geocoder('insert your api key here')
    result = geocoder.locate("123 South Main Street", "SLC", **{"acceptScore": 90, "spatialReference": 4326})
    
    print result