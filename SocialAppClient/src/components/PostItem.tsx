import {
  Button,
  Card,
  CardBody,
  CardFooter,
  CardHeader,
  Divider,
  Image,
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@nextui-org/react';

import TimeAgo from 'timeago-react';
import { Link } from 'react-router-dom';
import ProfileImage from './ProfileImage';
import { likeService } from '../api/likeService';
import { useAppDispatch } from '../store';
import { FaCommentAlt, FaThumbsUp } from 'react-icons/fa';
import { useState } from 'react';
import UserInfo from './UserInfo';
import { PostResponse } from '../api/dtos/post';
import { LikeReaction, LikesForPostResponse } from '../api/dtos/like';
import { addLikeToPost, removeLikeFromPost } from '../store/post-slice';

interface PostItemProps {
  post: PostResponse;
}

function PostItem({ post }: PostItemProps) {
  const dispatch = useAppDispatch();
  const [likesForAPost, setLikesForAPost] = useState<LikesForPostResponse | null>(null);

  const likeButtonStyles = !post.likeInfo?.likedByCurrentUser
    ? {
        color: 'gray',
        background: 'transparent',
      }
    : {};

  async function handleLikePost(): Promise<void> {
    if (post.likeInfo && post.likeInfo.likedByCurrentUser) {
      const response = await likeService.deleteLike({ postId: post.id, likeId: post.likeInfo.likeId });
      if (response.hasError) {
        console.log(response.error);
        return;
      }
      dispatch(removeLikeFromPost({ postId: post.id }));
    } else {
      const response = await likeService.addLikeToPost({ postId: post.id, likeReaction: LikeReaction.Like });
      if (response.hasError) {
        console.log(response.error);
        return;
      }
      dispatch(addLikeToPost({ likeId: response.value.likeId, postId: response.value.postId }));
    }
  }

  async function getAllLikesForAPostHanlder(): Promise<void> {
    const response = await likeService.getAllLikesForPost({ postId: post.id });
    if (!response.hasError) {
      setLikesForAPost(response.value);
      console.log(response);
    }
  }

  return (
    <Card className='rounded-sm w-full'>
      <CardHeader className='flex justify-between'>
        <div className='flex gap-3'>
          <ProfileImage dimension={50} src={post.userInfo.profileImage.thumbnailImagePath} />
          <div className='flex flex-col'>
            <Link to={`/profile/${post.userInfo.username}`}>
              <p className='hover:text-secondary-400 hover:font-bold text-md'>{post.userInfo.username}</p>
            </Link>
            <TimeAgo datetime={post.createdAt} locale='en_us' />
          </div>
        </div>
        <Button color='primary' variant='light' as={Link} to={`/post/${post.id}`}>
          Visit Post &rarr;
        </Button>
      </CardHeader>
      <Divider />
      <CardBody className='flex flex-col gap-4'>
        <div>
          <h2 className='text-2xl font-semibold'>{post.title}</h2>
          <p>{post.contents}</p>
        </div>
        <div className='border-1 border-default-400'>
          <img className='w-full rounded-none' src={post.images[0].fullscreenImagePath} />
        </div>
      </CardBody>
      <CardFooter className='flex justify-between'>
        <div className='flex gap-5'>
          <span className='text-md' color='secondary'>
            Comments ({post.numComments})
          </span>

          <Popover>
            <PopoverTrigger>
              <span onClick={getAllLikesForAPostHanlder} className='text-md' color='secondary'>
                Likes ({post.numLikes})
              </span>
            </PopoverTrigger>
            <PopoverContent>
              <div>
                <div className='flex flex-col gap-3 p-2'>
                  {likesForAPost?.likes.map((like) => (
                    <UserInfo key={like.id} dimension={25} userInfo={like.userInfo} />
                  ))}
                </div>
              </div>
            </PopoverContent>
          </Popover>
        </div>
      </CardFooter>
      <Divider />
      <CardFooter className='flex gap-5'>
        <Button
          onClick={handleLikePost}
          style={likeButtonStyles}
          className='text-md font-semibold'
          variant='flat'
          color='primary'
        >
          <FaThumbsUp />
          Like
        </Button>
        <Button className='text-md font-semibold' variant='flat' color='secondary'>
          <FaCommentAlt />
          Comment
        </Button>
      </CardFooter>
    </Card>
  );
}

export default PostItem;
