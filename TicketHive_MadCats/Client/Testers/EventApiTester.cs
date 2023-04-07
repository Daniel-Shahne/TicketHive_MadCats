using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Json;
using System.Text;
using TicketHive_MadCats.Shared.Models;
using TicketHive_MadCats.Shared.ViewModels;

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
            // ---------------- PREPARING FOR TESTS ------------------------
            
            // Gets the user
            var state = await authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            // Apparently user can be null so need to fail if thats the case too
            // even though i tested with albin and somewhat sure user is allways
            // an object, even if its an empty one
            if (user is null) return false;
            
            // If user is not authenticated: fail immediately
            if (!user.Identity.IsAuthenticated) return false;



            // ---------------- POSTING AND DELETING ---------------------

            // Is used to save the id of the created event to then delete it
            int createdEventId = 0;
            // Tests posting a valid new event
            if (user.IsInRole("Admin"))
            {
                // New event to post
                EventModel newModel = new()
                {
                    Name = "TestValidEvent",
                    EventType = "event type test",
                    TicketPrice = 10,
                    Location = "location test",
                    Date = DateTime.Now,
                    ImageSrcs = "NOT VALID",
                    MaxTickets = 10,
                    Tickets = new()
                    {
                        new TicketModel
                        {
                            UserId = 1,
                            EventModelId = 1
                        },
                        new TicketModel
                        {
                            UserId = 1,
                            EventModelId = 1
                        }
                    }
                };

                // Serializes and sends the event
                //string serializedModel = JsonConvert.SerializeObject(newModel);
                //var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                //var postResponse = await client.PostAsync("/api/Events/", content);

                // Another approach. Mutually exclusive with above.
                var postResponse = await client.PostAsJsonAsync<EventModel>("/api/Events/Post", newModel);

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
            // operation is successfull even though it SHOULD not happen
            if (user.IsInRole("Admin"))
            {
                var deleteResponse = await client.DeleteAsync("/api/Events/9999");
                if (deleteResponse.IsSuccessStatusCode) return false;
            }

            // Tries posting an invalid event, which is one with a conflicting
            // (already existing) primary key. Should result in bad request, fail
            // otherwise
            if (user.IsInRole("Admin"))
            {
                // New INVALID event to post. Has no tickets though for simplicity.
                EventModel newModel = new()
                {
                    Id = 1,
                    Name = "TestINVALIDEvent",
                    EventType = "event type test",
                    TicketPrice = 10,
                    Location = "location test",
                    Date = DateTime.Now,
                    ImageSrcs = "NOT VALID",
                    MaxTickets = 10,
                    Tickets = new()
                    {
                        new TicketModel
                        {
                            UserId = 1,
                            EventModelId = 1
                        },
                        new TicketModel
                        {
                            UserId = 1,
                            EventModelId = 1
                        }
                    }
                };

                string serializedModel = JsonConvert.SerializeObject(newModel);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                var postResponse = await client.PostAsync("/api/Events", content);

                // If the post request is anything other than bad request, fail here
                if (postResponse.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    return false;
                }
            }

            // --------------------- GETTING -------------------------

            // Gets one event (id 1) if anyone is logged in
            // Fails if no eventviewmodel could be retrieved 
            if (user.Identity.IsAuthenticated)
            {
                // Checks if response is ok
                var getOneResponse = await client.GetAsync("api/Events/1");
                if (!getOneResponse.IsSuccessStatusCode) { return false; }
                
                // Checks if deserialisation was ok
                var getOneJson = await getOneResponse.Content.ReadAsStringAsync();
                EventViewModel? model = JsonConvert.DeserializeObject<EventViewModel>(getOneJson);
                if (model == null) { return false; }
            }

            // Tries to get an event that does not exist (id 9999)
            // which should not result in anything other than bad request status
            if (user.Identity.IsAuthenticated)
            {
                // Fails if status code is anything other than bad request
                var getOneResponse = await client.GetAsync("api/Events/9999");
                if (getOneResponse.StatusCode != System.Net.HttpStatusCode.BadRequest) { return false; }
            }

            // Gets all events if anyone is logged in
            if (user.Identity.IsAuthenticated)
            {
                // Checks if response status is ok
                var getAllResponse = await client.GetAsync("api/Events");
                if (!getAllResponse.IsSuccessStatusCode) { return false; }
                
                // Tries deserialising the body
                var getAllJson = await getAllResponse.Content.ReadAsStringAsync();
                List<EventViewModel>? listOfViewModels = JsonConvert.DeserializeObject<List<EventViewModel>>(getAllJson);
                if (listOfViewModels == null) { return false; }
            }

            // If all ran tests succeeded, return true
            return true;
        }
    }
}
