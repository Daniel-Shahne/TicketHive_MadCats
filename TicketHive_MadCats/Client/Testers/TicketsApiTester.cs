using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using TicketHive_MadCats.Shared.ViewModels;

namespace TicketHive_MadCats.Client.Testers
{
    public class TicketsApiTester
    {
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly HttpClient httpClient;

        public TicketsApiTester(AuthenticationStateProvider authStateProvider, HttpClient httpClient)
        {
            this.authStateProvider = authStateProvider;
            this.httpClient = httpClient;
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

            // Gets the username to send with request
            var userName = user.Identity.Name;

            // ---------------------- Get one ----------------------------
            var getOneResponse = await httpClient.GetAsync("api/Tickets/Ticket1");
            var getOneStatus = getOneResponse.StatusCode;
            if (getOneStatus != System.Net.HttpStatusCode.OK) { return false; }
            
            var getOneBody = await getOneResponse.Content.ReadAsStringAsync();
            TicketViewModel? oneTicketVM = JsonConvert.DeserializeObject<TicketViewModel>(getOneBody);
            if (oneTicketVM is null) { return false; }



            // --------------- Get one users tickets ---------------------
            var getAllResponse = await httpClient.GetAsync($"api/Tickets/Username{userName}");
            var getAllStatus = getAllResponse.StatusCode;
            if (getAllStatus != System.Net.HttpStatusCode.OK) { return false; }

            var getAllBody = await getAllResponse.Content.ReadAsStringAsync();
            List<TicketViewModel>? allTicketsVMs = JsonConvert.DeserializeObject<List<TicketViewModel>>(getAllBody);
            if(allTicketsVMs is null) { return false; }



            // -------------------- Post a ticket ---------------------
            // IS VERIFIED VIA MANUALLY CHECKING THE DATABASE WITH SSMS
            // THE RETURN VALUE ONLY INDICATES SUCCESS OR FAIL

            // Sends a valid booking request to book two tickets to event 1
            var postTicketResponse = await httpClient.PostAsync($"api/Tickets/{userName}books1times2", null);
            if(postTicketResponse.StatusCode != System.Net.HttpStatusCode.OK) { return false; };



            // ------------------- Delete a ticket --------------------
            // Cant automatically delete a ticket since cant automatically
            // retrieve the created ticket int's either. So to test deleting
            // a ticket you have to manually uncomment, hardcode an int, and
            // check SSMS for validation

            // Check SSMS after this line is run to check if entry is deleted
            var deleteTicketResponse = await httpClient.DeleteAsync("api/Tickets/3");

            // Check this status in debugger to see if it matches
            var deleteTicketStatus = deleteTicketResponse.StatusCode;

            // If no tests failed return true
            return true;
        }
    }
}
