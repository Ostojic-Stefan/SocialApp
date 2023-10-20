import { PostResponse, PostsForUserResponse } from '../../api/postService';

export type UserInfo = {
  userProfileId: string;
  username: string;
  avatarUrl: string;
};

export type LikeInfo = {
  likeId: string;
  likedByCurrentUser: boolean;
};

export type Post = {
  id: string;
  imageUrl: string;
  contents: string;
  createdAt: Date;
  updatedAt: Date;
  numLikes: number;
  userInfo: UserInfo;
  numComments: number;
  likeInfo?: LikeInfo;
};

export type PostData = {
  id: string;
  imageUrl: string;
  contents: string;
  createdAt: Date;
  updatedAt: Date;
  numLikes: number;
  numComments: number;
  likeInfo?: LikeInfo;
};

export type PostsForUser = {
  userInfo: UserInfo;
  posts: PostData[];
};

export function mapPost(postResponse: PostResponse): Post {
  return {
    id: postResponse.id,
    imageUrl: postResponse.imageUrl,
    contents: postResponse.contents,
    createdAt: postResponse.createdAt,
    updatedAt: postResponse.updatedAt,
    numLikes: postResponse.numLikes,
    numComments: postResponse.numComments,
    userInfo: postResponse.userInfo,
    likeInfo: postResponse.likeInfo,
  };
}

export function mapPostsForUser(postsForUser: PostsForUserResponse): PostsForUser {
  return {
    userInfo: postsForUser.userInfo,
    posts: postsForUser.posts,
  };
}

export function mapPostFromPostForUser(postResponse: PostsForUser): Post[] {
  const userInfo = postResponse.userInfo;
  return postResponse.posts.map((pr) => {
    return { ...pr, userInfo };
  });
}
