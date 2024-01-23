import { UserInfoResponse } from "./user";

export type CommentResponse = {
    id: string;
    contents: string;
    userInfo: UserInfoResponse;
    createdAt: Date;
    updatedAt: Date;
}

export type CommentsOnPostResponse = {
    postId: string;
    comments: CommentResponse[];
}

export type CommentsForUserResponse = {
    comments: CommentResponse[];
}

export type CommentsForPostRequest = {
    postId: string;
}

export type CreateCommentRequest = {
    postId: string;
    contents: string;
}

export type GetCommentsForUserRequest = {
    username: string;
}