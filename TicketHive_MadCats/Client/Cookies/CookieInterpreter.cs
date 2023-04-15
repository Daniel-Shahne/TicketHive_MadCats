using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TicketHive_MadCats.Client.Cookies
{
    public class CookieInterpreter
    {
        private IJSRuntime jS;
        public CookieInterpreter(IJSRuntime JS)
        {
            jS = JS;
        }

        // Just for testing. Otherwise useless
        public async Task<string> TestJsRuntime()
        {
            return await jS.InvokeAsync<string>("testPrintJs");
        }


        public async Task UpdateCookie(Dictionary<string, int> dict)
        {
            string serializedDict = JsonConvert.SerializeObject(dict);
            await jS.InvokeVoidAsync("updateCookie", serializedDict);
        }

        public async Task<Dictionary<string, int>?> ReadCookie()
        {
            // Returns all cookies like cookie1=value; cookie2=value; cookie3=value;
            // so the one named "cart" need to be found and its value deserialized
            string allCookies = await jS.InvokeAsync<string>("readCookies");

            // Splits the cookies
            List<string> splitCookies = allCookies.Split(";").ToList();

            // Finds the one named "cart=", which is our cookie
            string? cartCookie = null;
            foreach (string cookie in splitCookies)
            {
                if (cookie.Contains("cart=")) { cartCookie = cookie; }
            }

            // If no cookie could be found then print error to debug
            if(cartCookie == null)
            {
                Debug.WriteLine("No cookie for cart found");
                return null;
            }

            // Need to cut away the "cart=" part of the string
            string stringToDeserialize = cartCookie.Replace("cart=", "");

            // Deserializes into a Dictionary<string, int>
            Dictionary<string, int>? deserializedCookie = JsonConvert.DeserializeObject<Dictionary<string,int>>(stringToDeserialize);

            // If deserialization unsuccessfull, returns null and prints to debug
            if (deserializedCookie == null)
            {
                Debug.WriteLine("Could not deserialize cookie");
                return null;
            }

            // Otherwise returns the dictionary
            return deserializedCookie;
        }

        public async Task CreateCookie()
        {
            await jS.InvokeVoidAsync("createCookie");
        }

        public async Task<Dictionary<string, int>?> UpdateCookieForSingleEvent(string eventName, int quantity)
        {
            // Returns all cookies like cookie1=value; cookie2=value; cookie3=value;
            // so the one named "cart" need to be found and its value deserialized
            string allCookies = await jS.InvokeAsync<string>("readCookies");

            // Splits the cookies
            List<string> splitCookies = allCookies.Split(";").ToList();

            // Finds the one named "cart=", which is our cookie
            string? cartCookie = null;
            foreach (string cookie in splitCookies)
            {
                if (cookie.Contains("cart=")) { cartCookie = cookie; }
            }

            // If a cookie existed then can modify a single value using the other methods
            if(cartCookie != null)
            {
                // Need to cut away the "cart=" part of the string
                string stringToDeserialize = cartCookie.Replace("cart=", "");

                // Deserializes into a Dictionary<string, int>
                Dictionary<string, int>? deserializedCookie = JsonConvert.DeserializeObject<Dictionary<string, int>>(stringToDeserialize);

                // If for some reason couldnt deserialize then print to debug
                if(deserializedCookie == null)
                {
                    Debug.WriteLine("UpdateCookieForSingleEvent could not deserialize cookie");
                    return null;
                }

                // Checks if keys exists and then modifies the value depending on that
                if(deserializedCookie.ContainsKey(eventName))
                {
                    deserializedCookie[eventName] += quantity;
                    // If value goes below 1 then delete it
                    if (deserializedCookie[eventName] <= 0) deserializedCookie.Remove(eventName);
                }
                else
                {
                    deserializedCookie.Add(eventName, quantity);
                }

                await UpdateCookie(deserializedCookie);
                return deserializedCookie;
            }
            // If no cookie existed then need to create a new one using UpdateCookie
            // which should create a new one too
            else
            {
                Dictionary<string, int> newDict = new()
                {
                    { eventName, quantity },
                };

                await UpdateCookie(newDict);
                return newDict;
            }
        }
    }
}
