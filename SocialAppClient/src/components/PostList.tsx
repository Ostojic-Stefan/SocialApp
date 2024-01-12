import PostItem from './PostItem';
import { PostResponse } from '../api/postService';

interface PostListProps {
  posts: PostResponse[];
}

export default function PostList({ posts }: PostListProps) {
  return (
    <div className='flex flex-col gap-9'>
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
    </div>
  );
}
