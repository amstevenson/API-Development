using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// This class provides a collection of methods that perform database related commands to perform
/// specified actions. There is at least one method for the verbs: get, put, post and delete.
/// </summary>
/// <author> AStevenson; StudentID: 10164661 </author>
public class DatabaseOperations
{
    // The connection string, found in the webConfig file
    private static string _ConnectionString = WebConfigurationManager.ConnectionStrings["SOFT338_ConnectionString"].ConnectionString;

	public DatabaseOperations()
	{
        // default constructor
	}

    /// <summary>
    /// Retrieves the Events from the local database that match either of the parameters provided.
    /// </summary>
    /// <param name="event_id">The 8 character long identifer for the Event.</param>
    /// <param name="name">The name of the Event.</param>
    /// <returns>Returns a list of Events that contains all of the retrieved objects.</returns>
    public static List<Event> GetMatchingEvents(string event_id, string name)
    {
        // Initialise the Event List
        List<Event> events = new List<Event>();

        // Create the connection to the database
        SqlConnection con = new SqlConnection(_ConnectionString);

        // Create the sql command that will collect all of the Events that match
        // either the reference_id or the name.
        SqlCommand cmd = new SqlCommand("SELECT * FROM Event WHERE Ref = '" + event_id + "' OR Name = '" + name + "';", con);

        using (con)
        {
            try
            {
                // Attempt to open the connection 
                con.Open();

                // Execute the command using the "cmd" object.
                SqlDataReader reader = cmd.ExecuteReader();

                // Retrieve all of the Events that match either the reference_id or name
                while (reader.Read())
                {
                    Event e = new Event((string)reader["Ref"], (string)reader["Name"], (string)reader["Date"], (string)reader["Time"], (string)reader["Address"], (string)reader["Postcode"]);
                    events.Add(e);
                }

            }
            catch (Exception ex)
            {
                // In the case of an error
                string LastError = ex.Message;
                Console.WriteLine(LastError);
            }

        }

        return events;
    }

    /// <summary>
    /// Collects all of the Events from the local database.
    /// </summary>
    /// <returns>All of the Events in a list.</returns>
    public static List<Event> GetAllEvents()
    {
        //We create a new list to add modules into
        List<Event> events = new List<Event>();

        //create a new connection using the connection string in the web.config
        SqlConnection con = new SqlConnection(_ConnectionString);

        //Create a new command object that holds the SQL statement we want to run.
        SqlCommand cmd = new SqlCommand("SELECT * FROM Event", con);
        using (con)
        {
            try
            {
                // Attempt to open the connection
                con.Open();

                //The reader is filled using the command object which goes over the connection.
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //this loops through all the variables that come together to comprise an event. Each event is added to a list.
                    Event e = new Event((string)reader["Ref"], (string)reader["Name"], (string)reader["Date"], (string)reader["Time"],
                        (string)reader["Address"], (string)reader["Postcode"]);
                    events.Add(e);
                }
            }
            catch (Exception ex)
            {
                // if it goes wrong - shouldn't do...hopefully!
                string LastError = ex.Message;
                Console.WriteLine(LastError);
            }
        }

        return events;
    }

    /// <summary>
    /// Creates a new Event row in the local database. This method's address parameter can optionally use
    /// an external API (Google Places) to provide the values representation (Which can be more detailed
    /// in some cases). Using these requires that the "find_address" value is set to "yes". 
    /// </summary>
    /// <param name="newEvent">The Event object for which the details will be used to add a new Event row to the local database.</param>
    /// <returns>The integer identifer representation of the new Event.</returns>
    public static Int32 PostNewEvent(Event newEvent)
    {
        // create a new connection using the connection string in web.config
        SqlConnection con = new SqlConnection(_ConnectionString);

        // create a new command object that holds the sql statement we want to run.
        SqlCommand cmd = new SqlCommand("INSERT into Event (Ref, Name, Date, Time, Address, Postcode) VALUES('" + newEvent.event_id + "', + '" + newEvent.name + "', '"
            + newEvent.date + "', '" + newEvent.time + "', '" + newEvent.address + "', '" + newEvent.postcode + "'); " 
            + "SELECT CAST(Scope_Identity() AS int)", con); 
        Int32 returnID = 0;

        using (con)
        {
            try
            {
                con.Open();

                // execute scalar gets the last ID back
                returnID = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // if it goes wrong - shouldn't do...hopefully!
                string LastError = ex.Message;
                Console.WriteLine(LastError);
            }
        }

        return returnID;
    }

    /// <summary>
    /// This method updates an Event based on the parameters provided.
    /// </summary>
    /// <param name="newEventDetails">A dictionary containing all of the parameters for the Event which will require changes.</param>
    /// <param name="event_id">The 8 characer long identifer for the Event.</param>
    /// <returns></returns>
    public static Event UpdateEvent(Dictionary<string, string> newEventDetails, string event_id)
    {
        // Create a blank event object
        Event e = new Event("unknown", "unknown", "unknown", "unknown"); // This will change to the Event that is updated, to output back to the user
                                                                         // for reference purposes.

        // Create a boolean to determine if the 
        Boolean foundMatch = false;

        // Create a new connection using the connection string found in web.config
        SqlConnection con = new SqlConnection(_ConnectionString);

        // create the string which will be appended to/concatonated multiple times.
        string sqlCommandString = "UPDATE Event SET ";
 
        // For each key, append the name of the key and the value to the end of the string.
        for (int i = 0; i < newEventDetails.Count; i++)
        {
            string keyName  = newEventDetails.Keys.ElementAt(i);
            string keyValue = newEventDetails[keyName]; 

            // Append the value accordingly
            sqlCommandString += keyName + "=" + "'" + keyValue + "'";

            // If we are not at the last element of the loop, add a comma to create a seperator
            // for the segment of the SqlCommand that focuses on updating specific attributes
            if (i != newEventDetails.Count - 1)
                sqlCommandString += ", "; // comma followed by a space
            else
                // if not, finish off the string
                sqlCommandString += " WHERE Ref = '" + event_id + "';";
        }

        // Create the sql command object that conprises the sqlCommandString and connection
        SqlCommand cmd = new SqlCommand(sqlCommandString, con);

        using (con)
        {
            try
            {
                // Open the connection
                con.Open();

                // Execute the query.
                SqlDataReader reader = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                // if it goes wrong - shouldn't do...hopefully!
                string LastError = ex.Message;
                Console.WriteLine(LastError);

                // If something goes wrong, then make the event null
                // Which will be checked after this method finishes in the code that proceeds this (in the DeleteEvent method).
                e = null;
            }

        }

        // reinitialise the connection property
        con = new SqlConnection(_ConnectionString);

        // Create a new command that will retrieve the Event 
        cmd = new SqlCommand("SELECT * FROM Event WHERE Ref = '" + event_id + "';", con); // create the new query

        using (con)
        {
            try
            {
                // Open the connection
                con.Open();

                // Execute the query
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Return the changed event object
                    e = new Event((string)reader["Ref"], (string)reader["Name"], (string)reader["Date"],
                        (string)reader["Time"], (string)reader["Address"], (string)reader["Postcode"]);

                    // Change the boolean to specify that the match has been found
                    foundMatch = true;
                }

            }
            catch (Exception ex)
            {
                // if it goes wrong - shouldn't do...hopefully!
                string LastError = ex.Message;
                Console.WriteLine(LastError);

                // If something goes wrong, then make the event null
                // Which will be checked after this method finishes in the code that proceeds this (in the DeleteEvent method).
                e = null;
            }
        }

        if (!foundMatch)
            e = null;

        return e;
    }

    /// <summary>
    /// This method deletes an Event from the local database.
    /// </summary>
    /// <param name="event_id">The 8 character long identifer for the Event which will be deleted.</param>
    /// <returns></returns>
    public static Event DeleteEvent(string event_id)
    {
        // Create a blank event object
        Event e = new Event("unknown", "unknown", "unknown", "unkown"); // It will change to either null, or another event dependant
                                                                        // on the SqlCommands retrieved row (if there is one).

        // Create a new connection using the connection string found in web.config
        SqlConnection con = new SqlConnection(_ConnectionString);

        // This boolean keeps track of whether an event has been found that matches the reference_id
        Boolean foundMatch = false;

        // Create a new sql command 
        // -- Retrive the event that matches the events reference identifer
        SqlCommand cmd = new SqlCommand("SELECT * FROM Event WHERE Ref = '" + event_id + "';", con);

        using (con)
        {
            try
            {
                // Attempt to open the connection
                con.Open();

                // Create the reader that will collect the information retrieved from
                // the sql command
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Return the event object
                    e = new Event((string)reader["Ref"], (string)reader["Name"], (string)reader["Date"],
                        (string)reader["Time"], (string)reader["Address"], (string)reader["Postcode"]);

                    // We have found an event so set the local boolean to true
                    foundMatch = true;
                }
            }
            catch (Exception ex)
            {
                // if it goes wrong - shouldn't do...hopefully!
                string LastError = ex.Message;
                Console.WriteLine(LastError);

                // If something goes wrong, then make the event null
                // Which will be checked after this method finishes in the code that proceeds this (in the DeleteEvent method).
                e = null;
            }

        }

        // If a match has not been found, make the event null. 
        if (!foundMatch)
            e = null;

        else  // if a match has been found. There is no point going through to the deletion stage for an event
              // in the case where a match has not been found. As nothing will happen.
        {

            // After getting the Event, it will (if there are no problems) be returned at the end of this method
            // and outputted for the clients reference as the Event that has been deleted, along with the details of
            // said event. Just to provide clarity into which one has been removed from the database.
            // So the next step is to change the SqlCommand so that it deletes the specific row from
            // the database:
            con = new SqlConnection(_ConnectionString);                                      // reinitialise the connection string

            cmd = new SqlCommand("DELETE FROM Event WHERE Ref = '" + event_id + "';", con); // create the new query

            using (con)
            {
                try
                {
                    // Open the connection and delete the event from the database
                    con.Open();

                    // Execute the query
                    SqlDataReader reader = cmd.ExecuteReader();

                }
                catch (Exception ex)
                {
                    // If something goes wrong
                    string lastError = ex.Message;
                    Console.WriteLine(lastError);
                }

            }
        }

        return e;
    }
}