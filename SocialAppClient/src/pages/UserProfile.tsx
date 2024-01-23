import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { userService } from '../api/userService';
import { Button, Spinner, Tab, Tabs, useDisclosure } from '@nextui-org/react';
import ProfileImage from '../components/ProfileImage';
import { UserDetailsResponse } from '../api/dtos/user';
import PostPreviewList from '../components/PostPreviewList';
import UserImages from '../components/UserImages';
import UploadUserImageModalForm from '../components/UploadUserImageModalForm';

export default function UserProfile() {
  const { username } = useParams();
  const [userInformation, setUserInformation] = useState<UserDetailsResponse | null>(null);
  const { onOpen, isOpen, onOpenChange } = useDisclosure();

  useEffect(() => {
    async function getUserProfileInformation() {
      if (!username) return;
      const response = await userService.getUserProfileInformation({ username });
      if (response.hasError) {
        return;
      }
      setUserInformation(response.value);
    }
    getUserProfileInformation();
  }, []);

  // async function sendFriendRequestHandler(): Promise<void> {
  //   if (userInformation) {
  //     const result = await userService.sendFriendRequest({ userId: userInformation.userInfo.userProfileId });
  //     if (result.hasError) {
  //       console.log('Failed to add friend request');
  //       console.log(result.error);
  //       return;
  //     }
  //     console.log(result.value);
  //   }
  // }

  // function shouldRenderSendRequestButton() {
  //   const isCurrentUsersProfile = user?.userInformation.userId === userInformation?.userInfo.userProfileId;
  //   if (isCurrentUsersProfile) return false;

  //   return !userInformation?.isFriend;
  // }

  if (!userInformation) return <Spinner />;

  return (
    <div className='flex flex-col gap-12'>
      <div className='flex gap-36 bg-default-50 p-10 '>
        <ProfileImage dimension={240} src={userInformation.userInfo.profileImage.thumbnailImagePath} />
        <div className='flex flex-col'>
          <h1 className='text-3xl'>{userInformation.userInfo.username}</h1>
          <div className='flex gap-7 my-4'>
            <p className='font-semibold'>1038 posts</p>
            <p className='font-semibold'>13.5k friends</p>
            <p className='font-semibold'>133769 likes</p>
          </div>
          <div className='bg-default-200 rounded p-2'>{userInformation.userInfo.biography}</div>
        </div>

        {/* {shouldRenderSendRequestButton() && (
          <div>
            <Button onClick={sendFriendRequestHandler} color='primary' variant='flat' className='font-semibold'>
              Send Friend Request
            </Button>
          </div>
        )} */}
      </div>

      <div className='flex gap-4 bg-default-100 p-4 rounded'>
        <UploadUserImageModalForm isOpen={isOpen} onOpenChange={onOpenChange} />
        <Button onPress={onOpen} color='secondary' variant='flat' className='font-semibold'>
          upload a new image
        </Button>
      </div>

      <div className='flex flex-col items-center'>
        <Tabs fullWidth aria-label='Dynamic tabs'>
          <Tab className='w-full' title='Posts' key={'posts'}>
            <PostPreviewList userProfileId={userInformation.userInfo.userProfileId} />
          </Tab>
          <Tab title='Photos' key='photos'>
            <UserImages userProfileId={userInformation.userInfo.userProfileId} />
          </Tab>
          <Tab title='Friends' key='friends'>
            <h1>TODO: User Friends Here</h1>
          </Tab>
        </Tabs>
      </div>
    </div>
  );
}
