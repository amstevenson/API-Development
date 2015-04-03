using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// A Results object is used to output an Array of Events, a
/// status to determine if the database operation was successful or not
/// and finally a message to provide additional information to the client.
/// </summary>
/// <author> AStevenson; StudentID: 10164661 </author>
[DataContract]
public class Results
{
    private string _status;
    [DataMember]
    public string status
    {
        get { return _status; }
        set { _status = value; }
    }

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
        this._status  = status;
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