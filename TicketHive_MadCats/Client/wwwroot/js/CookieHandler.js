// Just to know if its active
console.log("Cookie script active")

// Incase no cookie exists, this creates one with an 
// empty cart
function createCookie() {
    document.cookie = "cart="
}

// Updates the one cookie named cart with any value
// but is intended for a serialized Dictionary<string, int>
// representing the event name and how many of its tickets
// are in the cart
function updateCookie(value){
    // Sets an expiration date of 120 minutes
    //let date = new Date();
    //date.setTime(date.getTime() + (7200000));
    //expires = "; expires=" + date.toUTCString();

    // Creates the cookie with the value and expiration date
    /*document.cookie = `cart=${value}${expires}`*/
    document.cookie = `cart=${value}`
}

// Returns ALL cookies and needs to be filtered
function readCookies(){
    return document.cookie
}

// Just to test if IJSRuntime can reach the method
function testPrintJs() {
    return "Could reach js method!"
}
