import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { UserProfileInformation, userService } from '../api/userService';
import { Spinner, Tab, Tabs } from '@nextui-org/react';
import ProfileImage from '../components/ProfileImage';
import { PostResponse, PostsForUserResponse, postService } from '../api/postService';
import PostList from '../components/PostList';

function mapData(postsForUser: PostsForUserResponse): PostResponse[] {
  return postsForUser.posts.map((postForUser) => {
    return {
      ...postForUser,
      userInfo: postsForUser.userInfo,
    };
  });
}

export default function UserProfile() {
  const { username } = useParams();
  const [userInformation, setUserInformation] = useState<UserProfileInformation | null>(null);
  const [userPosts, setUserPosts] = useState<PostsForUserResponse | null>(null);
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

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
    async function getPostsForUser() {
      if (!username) return;
      const response = await postService.getPostsForUser({ username });
      if (response.hasError) {
        console.log(response.error);
        return;
      }
      setUserPosts(response.value);
    }
    getPostsForUser();
  }, []);

  if (loading) {
    return <Spinner color='secondary' />;
  } else if (error) {
    return <h1 className='text-danger'>{error}</h1>;
  }

  return (
    <div className='flex flex-col gap-12'>
      <div className='flex gap-24 bg-default-50 p-10'>
        <ProfileImage dimension={240} src={userInformation?.avatarUrl!} />
        <div className='flex flex-col'>
          <h1 className='text-3xl'>{userInformation?.username}</h1>
          <div className='flex gap-7 my-4'>
            <p className='font-semibold'>1038 posts</p>
            <p className='font-semibold'>13.5k followers</p>
            <p className='font-semibold'>22 following</p>
          </div>
          <div className='bg-default-200 rounded p-2'>{userInformation?.biography}</div>
        </div>
      </div>
      <div className='flex flex-col items-center'>
        <Tabs fullWidth aria-label='Dynamic tabs'>
          <Tab title='Posts' key={'posts'}>
            {userPosts && <PostList posts={mapData(userPosts)} />}
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
