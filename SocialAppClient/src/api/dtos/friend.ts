import { ImageResponse } from "./image";

export type FriendRequestResponse = {
    requesterId: string;
    requesterImage: ImageResponse;
    requesterUsername: string;
    requestTimeSent: Date;
}