using Blazored.LocalStorage;

namespace TicketHive_MadCats.Client.Cookies
{
    public class CookiesLocalStorage
    {
        private readonly ILocalStorageService localStorage;

        public CookiesLocalStorage(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }
        public async Task UpdateCartLSAsync(Dictionary<string, int> cart) 
        {
            await localStorage.SetItemAsync("cartStorage", cart);
        }
        public async Task<Dictionary<string, int>?> ReadCartLSAsync()
        {
            return await localStorage.GetItemAsync<Dictionary<string, int>>("cartStorage");
        }
        public async Task DeleteCookie()
        {
            await localStorage.RemoveItemAsync("cartStorage");
        }
    }
}
