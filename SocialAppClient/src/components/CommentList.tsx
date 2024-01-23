import TimeAgo from 'timeago-react';
import UserInfo from './UserInfo';
import { CommentResponse } from '../api/dtos/comment';

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
          <UserInfo userInfo={comment.userInfo}>
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
