import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../store';
import { getPostsForUser } from '../store/post-slice';
import PostPreview from './PostPreview';

interface PostPreviewListProps {
  userProfileId: string;
}

function PostPreviewList({ userProfileId }: PostPreviewListProps) {
  const dispatch = useAppDispatch();
  const postsForUser = useAppSelector((store) =>
    store.post.postsForUser.find((pfu) => pfu.userInfo.userProfileId === userProfileId)
  );

  useEffect(() => {
    dispatch(getPostsForUser({ userProfileId }));
  }, []);

  return (
    <div className='flex flex-col items-center gap-5'>
      {postsForUser?.posts.map((post) => (
        <PostPreview key={post.id} postData={post} />
      ))}
    </div>
  );
}

export default PostPreviewList;
