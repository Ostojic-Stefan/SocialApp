export interface UserInfo {
	username: string;
	biography: string;
	avatarUrl: string;
}
  
export interface Post {
	id: string;
	imageUrl: string;
	contents: string;
	createdAt: Date;
	updatedAt: Date;
	userProfileId: string;
	userInfo: UserInfo;
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