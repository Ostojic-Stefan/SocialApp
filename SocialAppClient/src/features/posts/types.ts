export interface UserInfo {
	userProfileId: string;
	username: string;
	avatarUrl: string;
}
  
export interface Post {
	id: string;
	imageUrl: string;
	contents: string;
	createdAt: Date;
	updatedAt: Date;
	userInfo: UserInfo;
	numLikes: number;
}

export interface GetAllPostsResponse {
	items: Post[];
	pageNumber: number;
	pageSize: number;
	totalCount: number;
	totalPages: number;
}

export interface UploadPost {
	contents: string;
	imageUrl: string;
}
