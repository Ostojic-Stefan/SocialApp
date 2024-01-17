import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { UserProfileInformation, userService } from '../api/userService';
import { Button, Spinner, Tab, Tabs, useDisclosure } from '@nextui-org/react';
import ProfileImage from '../components/ProfileImage';
import PostPreview from '../components/PostPreview';
import CreatePostModal from '../components/CreatePostModal';
import { useAppDispatch, useAppSelector } from '../store';
import { getPostsForUser } from '../store/post-slice';
import { useAuth } from '../context/AuthContext';

export default function UserProfile() {
  const { username } = useParams();
  const [userInformation, setUserInformation] = useState<UserProfileInformation | null>(null);

  const postsForUser = useAppSelector((store) =>
    store.post.postsForUser.find((pfu) => pfu.userInfo.username === username)
  );
  const dispatch = useAppDispatch();

  const { user } = useAuth();

  // const [userPosts, setUserPosts] = useState<PostsForUserResponse | null>(null);
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const { onOpen, isOpen, onOpenChange } = useDisclosure();

  useEffect(() => {
    async function getUserProfileInformation() {
      setLoading(true);
      if (!username) return;
      const response = await userService.getUserProfileInformation({ username });
      if (response.hasError) {
        setError(response.error.errorMessages.join(', '));
        return;
      }
      setUserInformation(response.value);
      setLoading(false);
    }
    getUserProfileInformation();
  }, []);

  useEffect(() => {
    if (!username) return;
    dispatch(getPostsForUser({ username: username }));
  }, []);

  if (loading) {
    return <Spinner color='secondary' />;
  } else if (error) {
    return <h1 className='text-danger'>{error}</h1>;
  }

  async function sendFriendRequestHandler(): Promise<void> {
    if (userInformation) {
      const result = await userService.sendFriendRequest({ userId: userInformation.userProfileId });
      if (result.hasError) {
        console.log('Failed to add friend request');
        console.log(result.error);
        return;
      }
      console.log(result.value);
    }
  }

  function shouldRenderSendRequestButton() {
    const isCurrentUsersProfile = user?.userInformation.userId === userInformation?.userProfileId;
    if (isCurrentUsersProfile) return false;

    return !userInformation?.isFriend;
  }

  return (
    <div className='flex flex-col gap-12'>
      <div className='flex gap-36 bg-default-50 p-10 '>
        <ProfileImage dimension={240} src={userInformation?.avatarUrl!} />
        <div className='flex flex-col'>
          <h1 className='text-3xl'>{userInformation?.username}</h1>
          <div className='flex gap-7 my-4'>
            <p className='font-semibold'>1038 posts</p>
            <p className='font-semibold'>13.5k friends</p>
            <p className='font-semibold'>133769 likes</p>
          </div>
          <div className='bg-default-200 rounded p-2'>{userInformation?.biography}</div>
        </div>

        {shouldRenderSendRequestButton() && (
          <div>
            <Button onClick={sendFriendRequestHandler} color='primary' variant='flat' className='font-semibold'>
              Send Friend Request
            </Button>
          </div>
        )}

        {/* {!isCurrentUsersProfile && userInformation?.isFriend ? (
          <div>Already Friends</div>
        ) : (
          <div>
            <Button onClick={sendFriendRequestHandler} color='primary' variant='flat' className='font-semibold'>
              Send Friend Request
            </Button>
          </div>
        )} */}
      </div>

      <div className='flex gap-4 bg-default-100 p-4 rounded'>
        <CreatePostModal isOpen={isOpen} onOpenChange={onOpenChange} />
        <Button onPress={onOpen} color='primary' variant='flat' className='font-semibold'>
          create a new post
        </Button>
        <Button color='secondary' variant='flat' className='font-semibold'>
          upload a new image
        </Button>
      </div>

      <div className='flex flex-col items-center'>
        <Tabs fullWidth aria-label='Dynamic tabs'>
          <Tab className='w-full' title='Posts' key={'posts'}>
            <div className='flex flex-col items-center gap-5'>
              {postsForUser?.posts.map((post) => (
                <PostPreview key={post.id} postData={post} />
              ))}
            </div>
          </Tab>
          <Tab title='Photos' key='photos'>
            <h1>TODO: User Images Here</h1>
          </Tab>
          <Tab title='Friends' key='friends'>
            <h1>TODO: User Friends Here</h1>
          </Tab>
        </Tabs>
      </div>
    </div>
  );
}
