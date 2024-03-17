import * as signalR from "@microsoft/signalr";

const connectionUrl = "https://localhost:5001/post-hub";

const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(connectionUrl)
    .withAutomaticReconnect()
    .build();

connection.start()
    .then(() => console.log("Post Hub Connected"))
    .catch(err => console.error(err));

export const postConnection = {
    close: () => connection.stop(),
    actions: {
        startTyping: (postId: string) => {
            connection.send("StartTyping", postId);
        },
        joinPost: (postId: string) => {
            connection.send("JoinPost", postId);
        },
        leavePost: (postId: string) => {
            connection.send("LeavePost", postId);
        }
    },
    events: {
        receiveTyping: (cb: (message: any) => void) => {
            connection.on("ReceiveTyping", message => {
                cb(message);
            })
        },
        receiveComment: (cb: (message: any) => void) => {
            connection.on("ReceiveComment", message => {
                cb(message);
            })
        }
    }
}