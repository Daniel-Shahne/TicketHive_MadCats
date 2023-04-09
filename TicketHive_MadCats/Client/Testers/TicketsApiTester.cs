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
            // For saving the posted tickets Id for deletion
            int createdTicketId = 0;

            // Sends the request
            var postTicketResponse = await httpClient.PostAsync($"api/Tickets/{userName}books1times2", null);
            if(postTicketResponse.StatusCode != System.Net.HttpStatusCode.OK) { return false; };

            // If no tests failed return true
            return true;
        }
    }
}
