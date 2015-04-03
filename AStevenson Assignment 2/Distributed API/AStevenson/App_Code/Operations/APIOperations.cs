using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Collections;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

/// <summary>
/// A class comprising of methods that allow for retrieving values from the third party API that was chosen
/// to be extracted and consume for this assignment. The response from the external API is consumed in the
/// PostNewEvent method (DatabaseOperations.cs) and comprises the address variable of the Event that is created.
/// Alternatively, the pure response of the Google Places API may be outputted.
/// </summary>
public class APIOperations
{
    public APIOperations()
    {
        // Default constructor
    }

    //*********************************************************************************************************************//
    //                                              Third party API Resource method                                        //
    //                                      (method that links with a resource in web.config)                              //
    //          Simply searches for a place, retrieving a specific tag - in order to Demonstrate a third party API call.   //
    //          Googles Places API is also used in specific "event" releated methods to consume json information retrieved //       
    //          which then gets used for database purposes. In that context, refer to the other methods that use the       //
    //          return value from the "searchAPIForTagValue" method (that is above), to see how it is used for database    //
    //          insertion purposes (the database allocated for the module).                                                //
    //*********************************************************************************************************************//

    //
    // Create a structure to be serialised, which returns the response in a json format
    //
    [DataContract]
    private struct GooglePlacesResponse
    {
        [DataMember]
        public string results;

        [DataMember]
        public string status;

        [DataMember]
        public string message;
    };

    /// <summary>
    /// Uses the third party API "Google Places" to extract information related to addresses. 
    /// In the context of this assignment, it is primarilly used to demonstrate a third party API call - "the extract part".
    /// It will be used to demonstrate that specific information can be retrieved.
    /// 
    /// However the method that is responsible for specifically extracting and "consuming" the response which subsequently contributes to
    /// the insertion of data to the local database can be found in the "PostNewEvent" method, that can be found within the EventOperations class,
    /// which makes use of the return value of SearchAPIForTagValue and "consumes" it as a result.
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="response"></param>
    public void SearchPlacesAPI(HttpContext context)
    {
        // Create the ouput stream
        Stream outputStream = context.Response.OutputStream;

        // Create the new serializer object
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(GooglePlacesResponse));
        
        // Create an empty GooglePlacesResult structure
        GooglePlacesResponse placesResponse = new GooglePlacesResponse();

        // change the content type
        context.Response.ContentType = "application/json";

        // retrieve parameter information
        string query = context.Request.QueryString["query"];
        string tagValue = context.Request.QueryString["tag"];

        if (query != null)
        {
            // Store the string that will comrpise the webclients download string that will retrieve
            // the json information from the request. 
            string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" +
                   query + "&key=AIzaSyDU3ZDN-wKRktsn7SgUWOrLFxe-GG1u1hc";

            // Create the string value that will collect the response from Google Places API
            string apiResponseValue = "";

            if (tagValue != null)
                // If a tag value is specified, search for the tag value and store it in a string.
                // This string will then be used as the json output for the 'results' structure variable.
                apiResponseValue = SearchAPIForTagValue(url, tagValue);
            else
            {
                //
                // Collect the response from Google Places API
                //
                var webClient = new System.Net.WebClient();
                apiResponseValue = webClient.DownloadString(url);
            }

            // If the result has been successful, provide the output for the
            // third party API call in the results string and inform the
            // client that the operation has been completed successfully.
            // Parametise the GooglePlacesResult structure with
            // the results of the third party call.
            placesResponse.results = apiResponseValue;
            placesResponse.status  = "OK";
            placesResponse.message = "Collected the result string for the external API call.";

            // Write the results to the output stream
            jsonData.WriteObject(outputStream, placesResponse);

            context.Response.StatusCode = (Int32)HttpStatusCode.OK;
        }
        else
        {
            // If the query parameter is missing, notify the client and output a not found response.
            // Create output stream

            // Parametise the GooglePlacesResult structure
            placesResponse.results    = "empty";
            placesResponse.status     = "INVALID_REQUEST";
            placesResponse.message    = "Missing the required paramter: 'query'";

            // Write the results to the output stream
            jsonData.WriteObject(outputStream, placesResponse);
        }
    }


    private string apiSearchResult = ""; // for searching using a third party API.

    ///<summary>
    /// This method is primarilly used to return the value of an external API call that corresponds to a specified tag. 
    /// For example, formatted_address may contain habitual information, and this method returns the first responses ArrayList 
    /// value for that specified tag. Admittely, this could be improved by returning an array of sorts that contains multiple 
    /// values, but validation can ensure that the correct address (in this example) can be found and used.
    ///</summary>
    /// <param name="url">The url that will return Json information. 
    /// This requires that the content-type of the current request is application/json</param>
    /// <param name="searchTag">The name of the JSON Tag that is being searched</param>
    /// <returns>The value of the searched tag</returns>
    public string SearchAPIForTagValue(string url, string searchTag)
    {
        apiSearchResult     = "";    // reset search variables - if this method is used more than once by an instance of this class
        string searchResult = "";

        //
        // Collect and parse the responce from the third party API
        //
        var webClient = new System.Net.WebClient();
        var jsonString = webClient.DownloadString(url);

        if (jsonString != null)
        {
            JavaScriptSerializer serialiser = new JavaScriptSerializer();
            Dictionary<string, object> jsonDictionary = serialiser.Deserialize<Dictionary<string, object>>(jsonString);

            //
            // Collect the result of the search
            //
            searchResult = FindJsonValue(jsonDictionary, searchTag);
        }

        return searchResult;
    }

    /// <summary>
    ///  Used in conjunction with SearchAPITagValue. This searches through an array of ArrayLists and Dictionaries, and finds the specific
    ///  value of a tag; this could admittedly be extended to return multiple values in an array.
    /// </summary>
    /// <param name="urlJson">The deserialised information of the url request to an API</param>
    /// <param name="searchTag">The  name of the JSON Tag that is being searched</param>
    /// <returns>The value of the searched tag</returns>
    private string FindJsonValue(Dictionary<string, object> urlJson, string searchTag)
    {
        object obj;

        //
        // Start the search
        //
        foreach (string startKey in urlJson.Keys)
        {
            // The key of the current dictionary
            obj = urlJson[startKey];

            if (obj is string)
            {
                // If the object is a string, we can check to see
                // if there is a match with the searchTag
                string keyValue = (string)obj;

                if (startKey.Equals(searchTag))
                {
                    // If the key that is being searched is found, retrieve
                    // the value of key and return it
                    return apiSearchResult = keyValue;
                }
            }
            else if (obj is Dictionary<string, object>)
            {
                // If we are faced with another dictionary, calling this method
                // will result in that specific dictionary being searched
                FindJsonValue((Dictionary<string, object>)obj, searchTag);
            }
            else if (obj is ArrayList)
            {
                // If the object is an ArrayList, we need to cast it to be a dictionary so that we
                // can repeat the above steps to find the string value.
                foreach (object o in (ArrayList)obj)
                {
                    if (o is Dictionary<string, object>)
                    {
                        FindJsonValue((Dictionary<string, object>)o, searchTag);
                    }
                    else if (o is string)
                    {
                        string keyValue = (string)o;

                        if (startKey.Equals(searchTag))
                        {
                            // If the key that is being searched is found, retrieve
                            // the value of key and return it
                            return apiSearchResult = keyValue;
                        }
                    }
                }
            }
            else
            {
                // if the object is of no discernable type(in the context of ArrayLists, Dictionaries and strings), or if we are at the end
                // of searching, then do nothing...
            }
        }

        // Return the results of the search, this can either be "" or an actual value. 
        // Note: The refresh in searchAPITagValue is global. If it was local in within the scope of this method, then each instance of this method
        //       would reset the variables that keep track of the search. Which for the most part, would result in "" being returned, even when a 
        //       value for a tag has been found.
        return apiSearchResult;
    }
}