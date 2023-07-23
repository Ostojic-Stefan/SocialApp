export interface UserProfileInformation {
    userProfileId : string;
    username: string;
    biography: string;
    avatarUrl: string;
}

export interface PostsForUserResponse {
    id: string;
    imageUrl: string;
    contents: string;
    createdAt: Date;
    updatedAt: Date;
}