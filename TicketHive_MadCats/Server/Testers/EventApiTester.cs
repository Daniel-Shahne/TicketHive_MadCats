using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Testers
{
    public class EventApiTester
    {
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly HttpClient client;

        public EventApiTester(AuthenticationStateProvider authStateProvider, HttpClient client)
        {
            this.authStateProvider = authStateProvider;
            this.client = client;
        }


        public async Task<bool> testEndpoints()
        {
            // Gets the user
            var state = await authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            // If noone is logged in, so no authentication, fail immediately
            if (!user.Identity.IsAuthenticated) return false;

            // ---------------- EVENTS API TESTING ---------------------

            // Is used to save the id of the created event to then delete it
            int createdEventId = 0;
            // Tests posting a valid new event
            if (user.IsInRole("Admin"))
            {
                // New event to post. Has no tickets though for simplicity.
                EventModel newModel = new()
                {
                    Name = "TestEventFromIndexPage",
                    EventType = "event type test",
                    TicketPrice = 10,
                    Location = "location test",
                    Date = DateTime.Now,
                    ImageSrcs = "NOT VALID",
                };

                // Serializes and sends the event
                string serializedModel = JsonConvert.SerializeObject(newModel);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                var postResponse = await client.PostAsync("/api/Events", content);

                // If the post request failed then fail the test here
                if (!postResponse.IsSuccessStatusCode) 
                { 
                    return false; 
                }

                // Gets the response body and deserializes it to get the new id (or fails test)
                var responseBody = await postResponse.Content.ReadAsStringAsync();
                EventModel? responseModel = JsonConvert.DeserializeObject<EventModel>(responseBody);
                if (responseModel != null)
                {
                    createdEventId = responseModel.Id;
                }
                else return false;
            }
            // Deletes the newly created event
            if (user.IsInRole("Admin"))
            {
                // If for some reason the new event still has id 0
                // there is some issue with EFC probably.
                if (createdEventId == 0)
                {
                    return false;
                }
                
                // Checks the delete response and fails if unsuccesfull
                var deleteResponse = await client.DeleteAsync($"/api/Events/{createdEventId}");
                if (!deleteResponse.IsSuccessStatusCode)
                {
                    return false;
                }
            }
            
            // Tests deleting a nonexistent event, if user is admin
            // and if we dont have 9999 events for some ungodly reason
            // I.e this one should only fail the test if the deletion
            // operation is successfull
            if (user.IsInRole("Admin"))
            {
                var deleteResponse = await client.DeleteAsync("/api/Events/9999");
                if (deleteResponse.IsSuccessStatusCode) return false;
            }

            // Gets one event if user is logged in



            // If all ran tests succeeded, return true
            return true;
        }
    }
}
