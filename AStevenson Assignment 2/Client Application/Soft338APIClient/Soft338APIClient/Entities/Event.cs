using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// An instance of an Event. 
/// The distributed API for this assignment writes out a stream that
/// is of the type "Results", which contains an array of Events. 
/// </summary>
/// <author> AStevenson; StudentID: 10164661 </author>
[DataContract]
public class Event
{
    private string _event_id;
    [DataMember]
    public string event_id
    {
        get { return _event_id; }
        set { _event_id = value; }
    }

    private string _name;
    [DataMember]
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }

    private string _date;
    [DataMember]
    public string date
    {
        get { return _date; }
        set { _date = value; }
    }

    private string _time;
    [DataMember]
    public string time
    {
        get { return _time; }
        set { _time = value; }
    }

    private string _address;
    [DataMember]
    public string address
    {
        get { return _address; }
        set { _address = value; }
    }

    private string _postcode;
    [DataMember]
    public string postcode
    {
        get { return _postcode; }
        set { _postcode = value; }
    }

    // First type of event
    public Event(string strEventRef, string strName, string strDate, string strTime)
    {
        this._event_id = strEventRef;
        this._name     = strName;
        this._date     = strDate;
        this._time     = strTime;
        this._address  = "unknown";  // Default
        this._postcode = "unknown"; // Default
    }

    // Second type of event
    public Event(string strEventRef, string strName, string strDate, string strTime, string strAddress, string strPostcode)
    {
        this._event_id = strEventRef;
        this._name     = strName;
        this._date     = strDate;
        this._time     = strTime;
        this._address  = strAddress;
        this._postcode = strPostcode;
    }

}