// import * as signalR from "@microsoft/signalr";

// const testConnectionUrl = "http://localhost:5000/hub";

// const connection = new signalR.HubConnectionBuilder()
//     .withUrl(testConnectionUrl)
//     .withAutomaticReconnect()
//     .build();

// connection.start()
//     .then(() => console.log("Connected"))
//     .catch(err => console.error(err));

// export const testConnection = {
//     close: () => connection.stop(),
//     actions: {
//         newMessage: (message: string) => {
//             connection.send("newMessage", "foo", message).then(_ => console.log("message sent"));
//         }
//     },
//     events: {
//         onMessageReceived: (cb: (username: string, message: string) => void) => {
//             connection.on("messageReceived", (username, message) => {
//                 console.log('message received');
//                 cb(username, message);
//             });
//         }
//     }
// }
