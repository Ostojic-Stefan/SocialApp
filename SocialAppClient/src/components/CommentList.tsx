import TimeAgo from 'timeago-react';
import { CommentResponse } from '../api/commentService';
import UserInfo from './UserInfo';

interface CommentListProps {
  comments: CommentResponse[];
}

export default function CommentList({ comments }: CommentListProps) {
  const renderedComments = comments.map((comment) => {
    return (
      <div
        key={comment.id}
        style={{ flexBasis: 'auto' }}
        className='border shadow p-3 rounded-md border-t border-gray-200'
      >
        <div className='flex space-x-2 items-center'>
          {/* TODO: FIX AFTER CHANGIN THE API TO RETURN THE UserInfo OBJECT */}
          <UserInfo
            userInfo={{
              avatarUrl: comment.avatarUrl,
              username: comment.username,
              userProfileId: comment.userProfileId,
            }}
          >
            <p className='text-gray-500 text-sm'>
              <TimeAgo datetime={comment.createdAt} />
            </p>
          </UserInfo>
        </div>
        <p className='mt-2'>{comment.contents}</p>
      </div>
    );
  });

  return <div className='h-full space-y-6 p-5'>{renderedComments}</div>;
}
