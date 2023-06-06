//Create a WebSocket to perform duplex (back-and-forth) communication with server
const token = document.querySelector("#FinnhubToken").value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; //get symbol from input hidden

// Connection opened. Subscribe to a symbol
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

// Listen (ready to receive) for messages
socket.addEventListener('message', function (event) {

    //if error message is received from server
    if (event.data.type == "error") {
        $(".price").text(event.data.msg);
        return; //exit the function
    }

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            //get the updated price
            var updatedPrice = JSON.parse(event.data).data[0].p;
            //console.log(updatedPrice);

            //update the UI
            $(".price").text(updatedPrice.toFixed(2)); //price - big display
        }
    }
});

// Unsubscribe
var unsubscribe = function (symbol) {
    //disconnect from server
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

//when the page is being closed, unsubscribe from the WebSocket
window.onunload = function () {
    unsubscribe(stockSymbol);
};