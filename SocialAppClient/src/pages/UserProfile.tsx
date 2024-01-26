import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { userService } from '../api/userService';
import { Button, Input, Spinner, Tab, Tabs, Textarea, useDisclosure } from '@nextui-org/react';
import ProfileImage from '../components/ProfileImage';
import { FriendStatus } from '../api/dtos/user';
import PostPreviewList from '../components/PostPreviewList';
import UserImages from '../components/UserImages';
import UploadUserImageModalForm from '../components/UploadUserImageModalForm';
import { useAuth } from '../context/AuthContext';
import { FriendRequestUpdateStatus } from '../api/dtos/friend';
import UserProfileFriendList from '../components/UserProfileFriendList';
import { useAppDispatch, useAppSelector } from '../store';
import { getUserProfileInformation } from '../store/user-profile-slice';

export default function UserProfile() {
  const { username } = useParams();
  const dispatch = useAppDispatch();
  const userInformation = useAppSelector((store) => store.userProfile.userProfile);
  const { onOpen, isOpen, onOpenChange } = useDisclosure();
  const { user } = useAuth();

  const [editMode, setEditMode] = useState<boolean>(false);

  const [newUserName, setNewUsername] = useState<string>(user.userInfo.username ?? '');
  const [newBiography, setNewBiography] = useState<string>(user.userInfo.biography ?? '');

  useEffect(() => {
    if (!username) return;
    dispatch(getUserProfileInformation({ username }));
  }, []);

  async function sendFriendRequestHandler(): Promise<void> {
    if (userInformation) {
      const result = await userService.sendFriendRequest({ userId: userInformation.userInfo.userProfileId });
      if (result.hasError) {
        console.log('Failed to add friend request');
        console.log(result.error);
        return;
      }
      console.log(result.value);
    }
  }

  async function acceptFriendRequestHandler(): Promise<void> {
    if (!userInformation) return;
    const response = await userService.updateFriendRequestStatus({
      userId: userInformation?.userInfo.userProfileId,
      status: FriendRequestUpdateStatus.Accept,
    });
    if (response.hasError) {
      console.log(response.error);
      return;
    }
    console.log(response.value);
  }

  async function rejectFriendRequestHandler(): Promise<void> {
    if (!userInformation) return;
    const response = await userService.updateFriendRequestStatus({
      userId: userInformation.userInfo.userProfileId,
      status: FriendRequestUpdateStatus.Reject,
    });
    if (response.hasError) {
      console.log(response.error);
      return;
    }
    console.log(response.value);
  }

  function renderFriendButton() {
    if (!userInformation) return null;
    const isCurrentUsersProfile = user.userInfo.userProfileId === userInformation.userInfo.userProfileId;
    if (isCurrentUsersProfile) {
      return false;
    }
    switch (userInformation.friendStatus) {
      case FriendStatus.Friend:
        return <div>Already Friends</div>;
      case FriendStatus.WaitingAcceptance:
        return (
          <div>
            <Button onClick={acceptFriendRequestHandler}>Accept Friend Request</Button>
            <Button onClick={rejectFriendRequestHandler}>Reject Friend Request</Button>
          </div>
        );
      case FriendStatus.WaitingApproval:
        return <div>Waiting For {userInformation.userInfo.username} to accept</div>;
      case FriendStatus.NotFriend:
      default:
        return <Button onClick={sendFriendRequestHandler}>Send Friend Requqest</Button>;
    }
  }

  if (!userInformation) return <Spinner />;

  const isCurrentUserProfile = user?.userInfo.userProfileId === userInformation.userInfo.userProfileId;

  async function handleUpdateProfile(): Promise<void> {
    if (!username) return;
    setEditMode(false);
    const result = await userService.updateUserProfile({ username: newUserName, biography: newBiography });
    if (result.hasError) {
      console.log(result.error);
      return;
    }
    console.log('Updated Successfully');
    dispatch(getUserProfileInformation({ username }));
  }

  return (
    <div className='flex flex-col gap-12'>
      <div className='flex gap-36 bg-default-50 p-10 '>
        <ProfileImage dimension={240} src={userInformation.userInfo.profileImage.thumbnailImagePath} />
        <div className='flex flex-col'>
          {!editMode ? (
            <h1 className='text-3xl'>{userInformation.userInfo.username}</h1>
          ) : (
            <Input type='text' value={newUserName} onChange={(e) => setNewUsername(e.target.value)} />
          )}
          <div className='flex gap-7 my-4'>
            <p className='font-semibold'>{userInformation.numPosts} posts</p>
            <p className='font-semibold'>{userInformation.numFriends} friends</p>
            <p className='font-semibold'>{userInformation.numLikes} likes</p>
          </div>
          {editMode ? (
            <Textarea value={newBiography} onChange={(e) => setNewBiography(e.target.value)} />
          ) : (
            userInformation.userInfo.biography && <p>{userInformation.userInfo.biography}</p>
          )}
          {isCurrentUserProfile &&
            (!editMode ? (
              <div>
                <Button onClick={() => setEditMode(true)}>Edit Profile</Button>
              </div>
            ) : (
              <Button onClick={handleUpdateProfile}>Save Changes</Button>
            ))}
        </div>

        {renderFriendButton()}
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
            <h1>
              <UserProfileFriendList userProfileId={userInformation.userInfo.userProfileId} />
            </h1>
          </Tab>
        </Tabs>
      </div>
    </div>
  );
}
