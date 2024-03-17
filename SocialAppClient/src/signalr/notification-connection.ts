import * as signalR from "@microsoft/signalr";

const connectionUrl = "https://localhost:5001/notification-hub";

const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(connectionUrl)
    .withAutomaticReconnect()
    .build();

connection.start()
    .then(() => console.log("Notification Hub Connected"))
    .catch(err => console.error(err));

export const notificationConnection = {
    close: () => connection.stop(),
    actions: {
        // newMessage: (message: string) => {
        //     connection.send("newMessage", "foo", message).then(_ => console.log("message sent"));
        // }
    },
    events: {
        onCommentNotificationReceived: (cb: (message: any) => void) => {
            connection.on("ReceiveCommentNotification", (message) => {
                console.log('message received');
                cb(message);
            });
        },
        onLikeNotificationReceived: (cb: (message: any) => void) => {
            connection.on("ReceiveLikeNotification", message => {
                cb(message);
            });
        }
    }
}
