using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;

/// <summary>
/// The handler for the HTTP request. When a call is made and a resource is identified,
/// a specific handler is used. Once the path has been stripped the underpinning programmatic 
/// logic is carried out - assuming that no errors are encountered. 
/// </summary>
public class SOFT338HttpHandler : IHttpHandler
{

	public SOFT338HttpHandler()
	{
		// Default constructor
	}

    public bool IsReusable { get { return true; } }

    public void ProcessRequest(HttpContext context)
    {
        // Retrieve and shorten the path
        string strPath = context.Request.Path.Replace("/modules/soft338/astevenson/", "");
        
        switch (strPath.ToLower())
        {

            case "search_places":
            {
                APIOperations apiOperations = new APIOperations();
                apiOperations.SearchPlacesAPI(context); break;

            }

            case "events":

                switch (context.Request.HttpMethod.ToLower())
                {
                    // Accepted method: get/post (retrieving resource representation(s))
                    case "get":  ShowAllEvents(context); break;
                    case "post": ShowAllEvents(context); break;

                    // If not get or post, then return a 405 "method not allowed" response statuscode
                    // Although it should only be "get", "post" would not cause a huge problem for this type of query
                    default: MethodNotAllowedResponse(context, "get", context.Request.HttpMethod.ToLower()); break;
                }
                break;


            case "events/show":

                switch (context.Request.HttpMethod.ToLower())
                {
                    // Accepted methods: get and post
                    case "get":  ShowMatchingEvents(context); break;
                    case "post": ShowMatchingEvents(context); break;

                    // If not get or post, then return a 405 "method not allowed" response statuscode
                    // Although it should only be "get", "post" would not cause a huge problem for this type of query
                    default: MethodNotAllowedResponse(context, "get", context.Request.HttpMethod.ToLower()); break;
                }
                break;

            case "events/create":

                switch (context.Request.HttpMethod.ToLower())
                {
                    // Accepted method: post
                    case "post": CreateEvent(context); break;

                    // Anything besides accepted method, respond with a 405 status code
                    default: MethodNotAllowedResponse(context, "post", context.Request.HttpMethod.ToLower()); break;
                }

                break;

            case "events/delete":

                switch (context.Request.HttpMethod.ToLower())
                {
                    // Accepted method: delete
                    case "delete": DeleteEvent(context); break;

                    // Anything besides accepted method, respond with a 405 status code
                    default: MethodNotAllowedResponse(context, "delete", context.Request.HttpMethod.ToLower()); break;
                }

                break;


            case "events/update":

                switch (context.Request.HttpMethod.ToLower())
                {
                    // Accepted method: put - post would be okay, but in this context demonstrating put is required
                    case "put": UpdateEvent(context); break;

                    // Anything besides accepted method, respond with a 405 status code
                    default: MethodNotAllowedResponse(context, "put", context.Request.HttpMethod.ToLower()); break;

                }

                break;

            default: 
                
                // If the path entered is unrecognised, display the result to the user
                // and output the status code

                // Create the output stream
                Stream outputStream = context.Response.OutputStream;

                // Notify caller that the response resource is in JSON.
                context.Response.ContentType = "application/json";

                // Create the results object
                // The method "OutputResults" could be used instead, but it would require creating an event list that would not be used
                // anywhere else from within this specific context.
                Results results = new Results("INVALID_REQUEST", "The path to the resource is not correct. This is at path: '" + strPath + "'");

                // Create the new serializer object
                DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Results));

                // Write the results to the output stream
                jsonData.WriteObject(outputStream, results);

                break;
        }
    }

    /// <summary>
    /// In the case where the expected method for a particular request is incorrect,
    /// the client needs to be notified of the error and what can be corrected to resolve it.
    /// </summary>
    /// <param name="context">Contains all HTTP information about a specific request</param>
    /// <param name="expectedMethod">The expected method</param>
    /// <param name="foundMethod">The current method header</param>
    private void MethodNotAllowedResponse(HttpContext context, String expectedMethod, String foundMethod)
    {
        // Create output stream
        Stream outputStream = context.Response.OutputStream;

        // Notify caller that the response resource is in JSON.
        context.Response.ContentType = "application/json";

        // Create the results object
        // The method "OutputResults" could be used instead, but it would require creating an event list that would not be used
        // anywhere else from within this specific context.
        Results results = new Results("INVALID_REQUEST", "Method not allowed: expected '" + expectedMethod + "', but recieved: '" + foundMethod + "'"); 

        // Create the new serializer object
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Results));

        // Write the results to the output stream
        jsonData.WriteObject(outputStream, results);

    }

    //**********************************************************************//
    //                          Event Methods                               //
    //**********************************************************************//
    
    /// <summary>
    /// Uses the GET verb to retrive all events from the local database.
    /// </summary>
    /// <param name="context">Contains all HTTP information about a specific request</param>
    private void ShowAllEvents(HttpContext context)
    {
        Stream outputStream = context.Response.OutputStream;

        // Notify caller that the response resource is in JSON.
        context.Response.ContentType = "application/json";

        //Create the new serializer object - NOTE the type entered into the constructor!
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Results));

        //Get a list of Events
        IEnumerable<Event> events = DatabaseOperations.GetAllEvents();

        // Write the results to the output stream
        OutputResults(context, "OK", "Retrieved results successfully", events.ToList());

        // change the status code to OK
        context.Response.StatusCode = (Int32)HttpStatusCode.OK;

    }

    /// <summary>
    /// This method uses the GET verb to show Events that match either the specified event_id or name.
    /// that has been provided as parameters.
    /// </summary>
    /// <param name="context">Contains all HTTP information about a specific request</param>
    private void ShowMatchingEvents(HttpContext context)
    {
        // Change the content type
        context.Response.ContentType = "application/json";

        // Create the list of events
        List<Event> eventList = new List<Event>();

        // Collect the parameter information
        string reference = context.Request.QueryString["event_id"];
        string name = context.Request.QueryString["name"];

        if (reference != null || name != null)
        {
            //Get a list of Events
            IEnumerable<Event> events = DatabaseOperations.GetMatchingEvents(reference, name);

            // If we have results, populate the "events" portion of the results constructor that 
            // collects an array of events, with those that have been retrieved
            if (events.Count() > 0)
            {
                // Create a results object that will allow for storage of events and includes a status
                // result and message that provides extra feedback for the user
                OutputResults(context, "OK", "Events retrieved successfully", events.ToList());
            }
            // If we do not have any Events that match, then notify the client by way of a json status / message
            else
            {
                // Create a Results object that only has the status and message parts
                // Notify the user that no matches have been bound
                OutputResults(context, "NO_RESULTS_FOUND", "The process to find the matching Events has returned successfully, but with no results found", eventList);
            }

            // change the status code to OK
            context.Response.StatusCode = (Int32)HttpStatusCode.OK;
        }
        else
        {
            // If event_id or name is null, then notify the user 
            OutputResults(context, "INVALID_REQUEST", "Missing parameters: one or more search parameters are required (name or event_id).", eventList);


        }
    }

    /// <summary>
    /// Uses the POST verb to add a new event to the local database.
    /// </summary>
    /// <param name="context">Contains all HTTP information about a specific request</param>
    private void CreateEvent(HttpContext context)
    {
        // change the content type
        context.Response.ContentType = "application/json";

        // Create the list of events
        List<Event> eventList = new List<Event>();

        try
        {
            // retrieve the query strings from the url
            string name     = context.Request.QueryString["name"];
            string date     = context.Request.QueryString["date"];
            string time     = context.Request.QueryString["time"];
            string address  = context.Request.QueryString["address"];
            string postcode = context.Request.QueryString["postcode"];
            string useGooglePlaces = context.Request.QueryString["find_address"];

            if (name != null && date != null && time != null)
            {
                try
                {
                    // Create an instance of an event object
                    Event e;

                    // convert the date to the correct format
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime parsedDate = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    
                    // Create the reference
                    string reference = GenerateReference();

                    // The client can create two types of events, the first is
                    // 1) Name, date and time.
                    if (address == null && postcode == null)
                    {
                        e = new Event( reference, name, date, time);
                    }
                    // 2) and the second type is: name, date, time, address and postcode
                    else
                    {
                        //
                        // If the client decides to use Google Places to help in finding the address
                        // They must specify that the "useGooglePlaces" parameter be set to "yes"
                        // and in that case, Google Places will find an address based on the information provided
                        // (information from the address and postcode fields)
                        //
                        if (useGooglePlaces == null) useGooglePlaces = "no";// it has not been specified

                        if (useGooglePlaces.ToLower() == "yes")
                        {
                            // use the third party api search to "consume" the response
                            // first, store the url string that will be sent to and used by the methods contained
                            // within the "APIOperations" class.
                            string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + address + " " +
                                    postcode + "&key=AIzaSyDU3ZDN-wKRktsn7SgUWOrLFxe-GG1u1hc";

                            // Retrive the json response from the webclient request to Google Places
                            // It returns (in this case) the response value as a string, for the 
                            // tag "formatted_address". However, it can be changed to retrieve the value
                            // from any tag (as long as it is there in the json response).
                            APIOperations operations = new APIOperations();
                            address = operations.SearchAPIForTagValue(url, "formatted_address");

                            // Check to see if the postcode is contained in the response, if it is, then replace it
                            // with nothing
                            // first remove the spaces for the postcode
                            // and remember that strings are immutable in c#. So the left value assigning is required
                            postcode = postcode.Replace(" ", "");

                            // and make it upper case and add a space in the right place
                            postcode = postcode.ToUpper();

                            int length = postcode.Length;
                            string refinedPostcode = postcode.Substring(0, 4) + " " + postcode.Substring(4, length - 4);

                            // remove the postcode from the returned "formatted_address" tag response
                            if (address.ToUpper().Contains(refinedPostcode.ToUpper()))
                                address = address.Replace(refinedPostcode.ToUpper(), ""); // strings are immutable, so requires reassigning
                        }

                        // Change the date to make it more readable
                        date = parsedDate.Day + "/" + parsedDate.Month + "/" + parsedDate.Year;

                        // Create an instance of the type of event, which is:
                        e = new Event(reference, name, date, time, address, postcode);
                    }

                    // use the event object to write to the database
                    DatabaseOperations.PostNewEvent(e);

                    // Output the new event
                    // Create an array of events and make it consist of the
                    // Event that was added to the local database
                    eventList.Add(e);

                    // Write the results to the output stream
                    OutputResults(context, "OK", "Event has been created successfully", eventList);

                    // change the status code to OK
                    context.Response.StatusCode = (Int32)HttpStatusCode.OK;

                }
                catch (FormatException)
                {
                    // If the date is in the incorrect format then notify the client
                    OutputResults(context, "INVALID_REQUEST", "Please make sure the date is in the correct format of dd/m/yyyy", eventList);

                }
            }
            else
            {
                // If the user is missing one or more parameters, then notify them of such.
                // This does not add an event to the database, but is for notification purposes.
                OutputResults(context, "INVALID_REQUEST", "Missing one of three paramters: name, date or time", eventList);

            }
        }
        catch (SerializationException e)
        {
            // Notify the user of the serialisation error
            OutputResults(context, "SERVER_ERROR", "Serialisation error: " + e.Message.ToString(), eventList);

            // change the status code to 400
            context.Response.StatusCode = (Int32)HttpStatusCode.BadRequest;
        }
    }

    /// <summary>
    /// This method is used to delete an event from the database. The query header "event_id" needs to be
    /// used to determine the identifer for the event. Uses the DELETE verb.
    /// </summary>
    /// <param name="event_id">The identifer for the Event. Can be found by searching all of the events, or showing a specific one
    /// it can also be thought of as a json tag. </param>
    /// <param name="context">Encapulates all HTTP information relating to a request</param>
    private void DeleteEvent(HttpContext context)
    {
        // change the content type
        context.Response.ContentType = "application/json";

        // Create the list of events
        List<Event> eventList = new List<Event>();

        // Collect the parameter information
        string reference = context.Request.QueryString["event_id"];

        // Check the event_id parameter to make sure it is not null
        if (reference != null)
        {
            // Create a new event object. This will be the event that is deleted by the DBLayer;
            // the HttpResponse will output the variables of this specific event to the client.
            Event e;

            // Create a new DBLayer object and delete the event that matches the event_id
            // It is unlikely that a random 8 character string that consists of over 60 characters will
            // be the same as another, but the size of the random string can be increased in time to compensate
            // for this.
            e = DatabaseOperations.DeleteEvent(reference);

            if (e != null)
            {
                // If the event isnt null, then we can ascertain that the event has been deleted successfully.
                // Notify the client that the event has been deleted, and output the details of the event
                // that has been removed from the local database.
                OutputResults(context, "OK", "The event has been successfully deleted. The details were: name = '" + e.name + "'"
                    + ", date = '" + e.date + "', time = '" + e.time + "', address = '" + e.address + "', postcode = '" + e.postcode + "'", eventList);

                // Lastly, change the status code to OK
                context.Response.StatusCode = (Int32)HttpStatusCode.OK;
            }
            else
            {
                // The server has understood the query and has carried it out accordingly. There has been no problems with the
                // underlying server code, but the event_id that was provided did not match any Events that currently exist
                // in the local database, so the client should be informed of this.
                OutputResults(context, "NO_RESULTS_FOUND", "There is no Event that matches the event_id provided", eventList);
               
                context.Response.StatusCode = (Int32)HttpStatusCode.OK;
            }
        }
        else
        {
            // If event_id is null, then notify the client
            OutputResults(context, "INVALID_REQUEST", "Currently missing the required parameter 'event_id'.", eventList);
        }
    }

    /// <summary>
    /// This method uses the PUT verb to update an event. The event_id of the event is required and the
    /// other parameters that comprise an Event object are optional. Those that are specified are used to update
    /// those specific values for the object.
    /// </summary>
    /// <param name="context">Contains all HTTP information about a specific request</param>
    private void UpdateEvent(HttpContext context)
    {
        // Change the content type
        context.Response.ContentType = "application/json";

        // Create the list of events
        List<Event> eventList = new List<Event>();

        // Collect the required parameters information
        string reference = context.Request.QueryString["event_id"];

        // Collect the parameters that are optional
        string name     = context.Request.QueryString["name"];
        string time     = context.Request.QueryString["time"];
        string date     = context.Request.QueryString["date"];
        string address  = context.Request.QueryString["address"];
        string postcode = context.Request.QueryString["postcode"];

        if (reference != null)
        {
            try
            {
                // Create a new instance of an Event
                Event e;

                // Create a dictionary that is comprised of the optional parameters that
                // have been entered. 
                Dictionary<string, string> newEventDetails = new Dictionary<string, string>();

                if (name != null) newEventDetails.Add("Name", name);
                if (time != null) newEventDetails.Add("Time", time);
                if (date != null)
                {
                    // Try to convert the date into a DateTime Variable
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime parsedDate = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);

                    newEventDetails.Add("Date", "" + parsedDate.Day + "/" + parsedDate.Month + "/" + parsedDate.Year);
                }
                if (postcode != null) newEventDetails.Add("Postcode", postcode);
                if (address != null) newEventDetails.Add("Address", address); // Allow manual override of the address found by Googles Places API
                                                                              // by not repeating the same steps again.

                // If the count of the dictionary that contains the updateable variables has at least one entry, then
                // update the Event accordingly.
                if (newEventDetails.Count > 0)
                {
                    // Update the object
                    e = DatabaseOperations.UpdateEvent(newEventDetails, reference);

                    // if the returned event is null (has not been found) then notify the user
                    // and change the status code to not found
                    if (e == null)
                    {
                        OutputResults(context, "NO_RESULTS_FOUND", "No event has been found that matches the event_id or name provided", eventList);

                        // Throw a 422 error, as the server understands the request, but was unable to process the instructions
                        // that underlied it
                        context.Response.StatusCode = (Int32)HttpStatusCode.NotFound;
                    }
                    else
                    {
                        // The object has been found and changed, therefore output the results
                        eventList.Add(e);

                        OutputResults(context, "OK", "Event has been updated successfully", eventList);

                        // Change the status code
                        context.Response.StatusCode = (Int32)HttpStatusCode.OK;
                    }
                }
                else
                {
                    // If the client has not entered one or more optional parameters, notify them and change the status code
                    OutputResults(context, "INVALID_REQUEST", "Please provide at least one optional parameter", eventList);

                }
            }
            catch (FormatException)
            {
                // If we have a format exception (most likely because the date is not in the right format), then
                // notify the client
                OutputResults(context, "INVALID_REQUEST", "Please make sure the date is in the correct format of dd/m/yyyy.", eventList);

            }
        }
        else
        {
            // If event_id is null, then notify the client
            OutputResults(context, "INVALID_REQUEST", "Currently missing the required parameter 'event_id'.", eventList);

        }
    }

    /// <summary>
    /// This method creates an output stream which provides details to the client about the Events, status codes and messages
    /// for the Http Response.
    /// </summary>
    /// <param name="context">Contains all HTTP information about a specific request</param>
    /// <param name="statusCode">The status of the response. For example "OK".</param>
    /// <param name="message">A message to provide feedback to the client</param>
    /// <param name="eventList">The array of events that will be written to the ouput stream.</param>
    private void OutputResults(HttpContext context, string statusCode, string message, List<Event> eventList)
    {
        // Create a serialiser object of type "Resuts".
        // This is comprised of a list of Events, a status code (for example OK in cases where an operations has been
        // completed successfully) and finally a message for the client.
        DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(Results));

        // Create the output stream - which will provide the result in a json format
        Stream outputStream = context.Response.OutputStream;

        // Construct an instance of a Results Object (Status Code, Message, (optional) List<Event>)
        Results results;

        // Determine the type
        if (eventList.Count > 0)
            results = new Results(statusCode, message, eventList.ToArray());
        else
            results = new Results(statusCode, message);

        // Write the response to the output stream
        jsonData.WriteObject(outputStream, results);
    }

    /// <summary>
    /// Generate a random eight character long string, which will be used as the reference
    /// for the Event or Member
    /// </summary>
    /// <returns>An eight character long string that is the identifer for the event</returns>
    private string GenerateReference()
    {
        // Create the new identifer for the event/member ( a reference )
        // Source: h ttp://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        // The characters that will be used to form the key identifer
        var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // The container for the randomly generated string; the element size of the array defines the length
        char[] randomChar = new char[8];
        var random = new Random();

        for (int i = 0; i < randomChar.Length; i++)
        {
            // Until we reach the end of the length of stringChars,
            // append to the string a random character from the characters variable
            randomChar[i] = characters[random.Next(characters.Length)];
        }

        string reference = new String(randomChar);

        return reference;
    }

}