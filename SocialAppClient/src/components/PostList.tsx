import { PostResponse } from '../api/dtos/post';
import PostItem from './PostItem';

interface PostListProps {
  posts: PostResponse[];
}

export default function PostList({ posts }: PostListProps) {
  return (
    <div className='p-3 flex flex-col gap-9 items-center'>
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
    </div>
  );
}
