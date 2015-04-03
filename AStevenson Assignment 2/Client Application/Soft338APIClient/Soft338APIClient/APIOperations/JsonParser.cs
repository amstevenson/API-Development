using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Collections;

namespace Soft338APIClient.APIOperations
{
    /// <summary>
    /// This class was created to provide a modular way of converting the information
    /// retrieved from the distributed api for the soft338 module and the external (third party) API
    /// into representations that can be used for the context of demonstrating the
    /// functionality of the verbs: get, post, update and delete. 
    /// </summary>
    /// <author>AStevenson; StudentID = 10164661 </author>
    class JsonParser
    {
        private string webResponseString = ""; // Used for unedited response (json) 

        public JsonParser()
        {
            // Default constructor
        }

        /// <summary>
        /// This method connects with an API and collects the response inside of a 
        /// dictionary of objects thare are identified by a string tag. This dictionary
        /// can then be used to construct objects of a specific type, or to simply 
        /// extract certain values.
        /// </summary>
        /// <param name="url"> The url to the API. The parameters must be added to the end of this url string 
        ///  in a restful manner if the method is anything but GET. </param>
        /// <param name="method"> The method: GET, POST, PUT or DELETE </param>
        /// <returns></returns>
        public Dictionary<string, object> GetWebResponseDictionary(string url, string method)
        {
            // Make sure the method is in upper case
            method = method.ToUpper();

            // Create the webclient
            var webClient = new System.Net.WebClient();

            // Create a blank dictionary
            Dictionary<string, object> returnDictionary;

            // Create the serialiser
            JavaScriptSerializer serialiser = new JavaScriptSerializer();

            // Create the string that will collect the response from the distributed api
            string returnString = "";

            // There are four selections available for the list of verbs. The GET
            // verb requires a different method of retrieving the results compared to the other three.
            try
            {
                if (method.ToUpper() == "GET")
                {
                    returnString = webClient.DownloadString(url);

                }
                else
                {
                    // The url in this context must have the query parameters added. 
                    // For example: protocol//example.com/create?name=example
                    returnString = webClient.UploadString(url, method, "");
                }
            }
            catch (WebException ex)
            {
                // One of the common ways in which this exception is triggered, is when
                // the status code for the server that (in this case the distributed API) changes to
                // a 4XX result (replace XX with numbers). 
                // For example: 405 (method not found) would not return any dictionary for the request
                // but would instead trigger this exception.
                Console.WriteLine("Web exception encountered: " + ex.Message);
            }
            catch (SystemException ex)
            {
                Console.WriteLine("System exception: " + ex.Message);
            }

            // initialise...and deserialise the return dictionary
            returnDictionary = serialiser.Deserialize<Dictionary<string, object>>(returnString);

            // Parametise the web response string that is used in the application to output
            // the pure json response for the web request
            SetWebResponseString(returnString);

            return returnDictionary;
        }

        /// <summary>
        /// This method creates and returns an instance of a 'Results' object based on a 
        /// deserialised dictionary of strings and objects. 
        /// </summary>
        /// <param name="dictionary">The deserialised dictionary that is returned from 
        /// the "GetWebResponseDictionary" method. </param>
        /// <returns></returns>
        public Results ConvertDictionaryToResults(Dictionary<string, object> dictionary)
        {
            // Create a blank results object
            Results results = new Results("", "");

            // Create a blank event array
            List<Event> eventList;

            // Create a blank object
            object obj;

            // For each key we have, determine what to do with the values
            // based on the type of object for each instance.
            try
            {
                foreach (string startKey in dictionary.Keys)
                {
                    // The key of the current dictionary
                    obj = dictionary[startKey];

                    if (obj is ArrayList)
                    {
                        // We have the ArrayList of events
                        ArrayList eventArrayList = (ArrayList)obj;

                        // initialise the event list
                        eventList = new List<Event>(eventArrayList.Count);

                        // In the scope of the results collected from the 
                        // distributed API. The events are split up into
                        // individual dictionaries of <string, string>
                        for (int i = 0; i < eventArrayList.Count; i++)
                        {
                            // Get the specific dictionary with the event information inside of it
                            Dictionary<string, object> eventDictionary = (Dictionary<string, object>)eventArrayList[i];

                            // Create a blank event object
                            Event e = new Event("Unknown", "Unknown", "Unknown", "Unknown");

                            // For each key we have for this dictionary, retrieve the key name and the corresponding value
                            foreach (string eventKey in eventDictionary.Keys)
                            {
                                // Store the corresponding value for the key
                                string strKeyValue = (string)eventDictionary[eventKey];

                                switch (eventKey)
                                {
                                    case "address": e.address = strKeyValue; break;
                                    case "date": e.date = strKeyValue; break;
                                    case "time": e.time = strKeyValue; break;
                                    case "name": e.name = strKeyValue; break;
                                    case "event_id": e.event_id = strKeyValue; break;
                                    case "postcode": e.postcode = strKeyValue; break;

                                    default: break;
                                }

                            }

                            // Add the new event to the list
                            eventList.Add(e);
                        }

                        // Finally, if the events list's size is more than zero, set the array of Events
                        // for the Results object.
                        if (eventList.Count > 0)
                            results.events = eventList.ToArray();

                    }
                    else if (obj is string)
                    {
                        // We have either the success or message string
                        string strKeyValue = (string)obj;

                        switch (startKey)
                        {
                            case "message": results.message = strKeyValue; break;

                            case "status": results.status = strKeyValue; break;

                            // Do nothing
                            default: break;
                        }
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine("Invalid cast exception: " + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Null reference exception: " + ex.Message);
            }

            return results;
        }

        /// <summary>
        /// Setter for the json response of a download/upload string method
        /// for a WebClient request. 
        /// </summary>
        /// <param name="webResponse">The bare response for the API call</param>
        private void SetWebResponseString(string webResponse)
        {
            this.webResponseString = webResponse;
        }
        
        /// <summary>
        /// Getter for the json response of a download/upload string method 
        /// for a WebClient request. 
        /// </summary>
        /// <returns>The bare response for the API call</returns>
        public string GetWebResponseString()
        {
            return webResponseString;
        }
    }
}
