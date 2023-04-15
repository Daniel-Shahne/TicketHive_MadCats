using Blazored.LocalStorage;

namespace TicketHive_MadCats.Client.Cookies
{
    /// <summary>
    /// Handles blazored.localstorage for the cart cookie
    /// thats a Dictionary<string, int>
    /// </summary>
    public class CookiesLocalStorage
    {
        private readonly ILocalStorageService localStorage;

        public CookiesLocalStorage(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }




        /// <summary>
        /// Updates the localstorage given the cart dictionary
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>

        public async Task UpdateCartLSAsync(Dictionary<string, int> cart) 
        {
            await localStorage.SetItemAsync("cartStorage", cart);
        }


        /// <summary>
        /// Reads the localstorage and returns it as a dictionary<string, int>
        /// </summary>
        /// <returns></returns>

        public async Task<Dictionary<string, int>?> ReadCartLSAsync()
        {
            return await localStorage.GetItemAsync<Dictionary<string, int>>("cartStorage");
        }


        /// <summary>
        /// Clears the localstorage, i.e clears the cart
        /// </summary>
        /// <returns></returns>

        public async Task DeleteCookie()
        {
            await localStorage.RemoveItemAsync("cartStorage");
        }
    }
}
