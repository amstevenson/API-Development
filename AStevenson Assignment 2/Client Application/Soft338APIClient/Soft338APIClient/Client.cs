using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Soft338APIClient.APIOperations;

namespace Soft338APIClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            // Set the default of the combo boxes and other objects
            cboxVerbs.SelectedIndex = 0;
            cboxUrl.SelectedIndex = 0;
            txtQueryString.Text = "?";

            // When the client is launched, update the list of events so that all of the retrieved
            // events of the local database (on xserve) are added to the list view.
            string url = "http://xserve.uopnet.plymouth.ac.uk/modules/soft338/astevenson/events";
            string method = "get";

            // Create a json parser 
            JsonParser jsonParser = new JsonParser();

            // Retrieve the results
            Dictionary<string,object> returnDictionary = jsonParser.GetWebResponseDictionary(url, method);

            // Create a results object based on the response of the distributed API
            Results returnResults = jsonParser.ConvertDictionaryToResults(returnDictionary);

            // Change the view type of the list view
            lstAllEvents.View = View.Details;

            // Update the events overview listview
            UpdateEventOverview(returnResults);
        }

        /// <summary>
        /// This method is used to create rows of items for the list view
        /// that contains all of the events that are retrieved from the local database.
        /// This has been added to the application to show the changes that have been 
        /// made, based on certain server-side operations. 
        /// </summary>
        /// <param name="results"></param>
        private void UpdateEventOverview(Results results)
        {
            // First, clear the listview
            lstAllEvents.Items.Clear();

            // Create and assign the items
            try
            {
                int amountOfEvents = results.events.Count();

                ListViewItem[] eventItems = new ListViewItem[amountOfEvents];

                // Create an item for each event
                for (int i = 0; i < amountOfEvents; i++)
                {
                    // Get the event
                    Event newEvent = results.events[i];

                    // Assign the values to the list view
                    eventItems[i] = new ListViewItem(newEvent.event_id);
                    eventItems[i].SubItems.Add(newEvent.name);
                    eventItems[i].SubItems.Add(newEvent.address);
                    eventItems[i].SubItems.Add(newEvent.postcode);
                    eventItems[i].SubItems.Add(newEvent.date);
                    eventItems[i].SubItems.Add(newEvent.time);

                }

                // Assign the array list of items to the list view
                lstAllEvents.Items.AddRange(eventItems);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Argument null exception: " + ex.Message);

                txtResponseObjects.Text = ex.Message;
            }
            catch (SystemException ex)
            {
                Console.WriteLine("System Exception: " + ex.Message);

                txtResponseObjects.Text = ex.Message + "." + "Most likely event is that the server has"
                    + " not been started.";
            }

        }

        /// <summary>
        /// When the send button is clicked, the url and query string are combined, then a request
        /// is sent off to either the distributed API for the module, or an external API (which, for the
        /// examples provided, is Google Places). Based on the type of method used, a slightly different
        /// web client method is used to retrieve the results. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            // Reset the text currently in the response textboxes
            txtResponseCode.Text    = "";
            txtResponseMessage.Text = "";
            txtResponseObjects.Text = "";

            // Create a new Web Request based on the text boxes: txtUrl, cboxVerts, txtQueryString
            string url = "http://xserve.uopnet.plymouth.ac.uk/modules/soft338/astevenson/" 
                         + cboxUrl.GetItemText(cboxUrl.SelectedItem) + txtQueryString.Text;
            string method = cboxVerbs.GetItemText(cboxVerbs.SelectedItem);

            // Create a json parser 
            JsonParser jsonParser = new JsonParser();

            // Retrieve the results
            Dictionary<string,object> returnDictionary = jsonParser.GetWebResponseDictionary(url, method);

            // Create a results object based on the response of the distributed API
            Results returnResults = jsonParser.ConvertDictionaryToResults(returnDictionary);

            // Change the response section of the application to match the results collected
            // This could probably be made prettier admittedly
            txtResponseCode.Text    = returnResults.status;
            txtResponseMessage.Text = returnResults.message;
            txtResponseObjects.Text = jsonParser.GetWebResponseString();

            // For clarification purposes, update the list of events on the right hand side
            // to match all of the events currently stored in the local database for the soft338 module
            // However, only do this if the method isnt 'GET'.
            // Just because in all of the cases where that method is used, it is purely to select
            // a specific set or subset of the total amount of objects. 
            if (method.ToUpper() != "GET")
            {
                returnDictionary = jsonParser.GetWebResponseDictionary("http://xserve.uopnet.plymouth.ac.uk/modules/soft338/astevenson/events", "GET");
                returnResults = jsonParser.ConvertDictionaryToResults(returnDictionary);
            }

            // Then create and assign the items
            if (returnResults.events != null)
                UpdateEventOverview(returnResults);
            else
                lstAllEvents.Items.Clear();

        }

        /// <summary>
        /// This method is called when the selected item of the list view changes. 
        /// In this context it is used to simply copy a value to the clipboard - which essentially
        /// is the same functionality as using "ctrl and c".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstAllEvents_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // When the list view has been clicked on by the client, copy the event_id to the clipboard
            // in order to make it easier to construct query strings that involve this parameter
            if (lstAllEvents.Items.Count > 0)
                Clipboard.SetText(e.Item.Text);
        }
    }
}
