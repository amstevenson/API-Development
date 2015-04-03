using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// This class is identical to the structure of the server-side "Results.cs" 
/// code for this assignment. The retrieved json information from the distributed
/// API is in a particular format - for both the external API call and general
/// local database related operation responses. Based on this, the response
/// from the download and upload string for a particular WebClient request (with reference to
/// the methods in the JsonParser class) is always going to be in the same format.
/// Of course, this would not work in cases where the URL and query string are
/// directed at a different distributed API where the structure is different to this. 
/// 
/// @see In the JsonParser.cs class, there is a method that converts the dictionary of strings
/// and objects - which is the deserialised response of a WebClient request - to this
/// class's representation.
/// </summary>
/// <author> AStevenson; StudentID: 10164661 </author>
[DataContract]
public class Results
{
    
    // 
    // The status that is returned from the distributed API.
    // A few examples are:
    // OK:               No problems
    // NO_RESULTS_FOUND: Server understood and completed the request, but
    //                   no results were found.
    // INVALID_REQUEST:  Method not allowed; parameters missing.
    //
    private string _status;
    [DataMember]
    public string status
    {
        get { return _status; }
        set { _status = value; }
    }

    //
    // The message returned from the distributed API.
    //
    private string _message;
    [DataMember]
    public string message
    {
        get { return _message; }
        set { _message = value; }
    }

    private Event[] _events;
    [DataMember(IsRequired = false)]
    public Event[] events
    {
        get { return _events; }
        set { _events = value; }
    }

    // First constructor
    public Results(string status, string message)
    {
        this._status = status;
        this._message = message;
    }

    // Second constructor
    public Results(string status, string message, Event[] events)
    {
        this._status = status;
        this._message = message;
        this._events = events;
    }
}