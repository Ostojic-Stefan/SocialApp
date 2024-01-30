import * as signalR from "@microsoft/signalr";

// const connectionUrl = "http://localhost:5000/hub";

const token = localStorage.getItem("auth")!;

// const connection = new signalR.HubConnectionBuilder()
//     .withUrl(connectionUrl, {
//         skipNegotiation: true,
//         transport: signalR.HttpTransportType.WebSockets,
//         accessTokenFactory: () => token
//     })
//     // .withAutomaticReconnect()
//     .build();


// connection.start()
//     .then(() => console.log("Connected"))
//     .catch(err => console.error(err));

const testConnectionUrl = "http://localhost:5000/hub";

const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(testConnectionUrl, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

connection.start()
    .then(() => console.log("Connected"))
    .catch(err => console.error(err));


export const notificationConnection = {
    close: () => connection.stop(),
    actions: {
        // newMessage: (message: string) => {
        //     connection.send("newMessage", "foo", message).then(_ => console.log("message sent"));
        // }
    },
    events: {
        onNotificationReceived: (cb: (message: string) => void) => {
            connection.on("ReceiveNotification", (message) => {
                console.log('message received');
                cb(message);
            });
        }
    }
}
