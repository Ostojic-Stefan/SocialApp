export interface AddCommentRequest {
    postId: string;
    contents: string;
}

export interface AddCommentResponse {
    id: string;
    contents: string;
    postId: string;
    userProfileId: string;
    createdAt: Date;
    updatedAt: Date;
}

export interface CommentResponse {
    id: string;
    contents: string;
    // postId: string;
    userProfileId: string;
    createdAt: Date;
    updatedAt: Date;
}

export interface CommentsFromPostResponse {
    postId: string;
    comments: CommentResponse[]
}