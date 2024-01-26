import { UserInfoResponse } from "./user";

export type FriendRequestResponse = {
    requesterUser: UserInfoResponse;
    requestTimeSent: Date;
}

export enum FriendRequestUpdateStatus {
    Accept, Reject
}

export type UpdateFriendStatusRequest = {
    userId: string;
    status: FriendRequestUpdateStatus
}