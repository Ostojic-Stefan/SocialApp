import { useEffect, useState } from 'react';
import { userService } from '../api/userService';
import { UserInfoResponse } from '../api/dtos/user';
import ProfileImage from './ProfileImage';

interface UserProfileFriendListProps {
  userProfileId: string;
}

interface UserProfileFriendItemProps {
  userInfo: UserInfoResponse;
}

function UserProfileFriendItem({ userInfo }: UserProfileFriendItemProps) {
  return (
    <div className='p-4 bg-default-100 flex items-center gap-4 shadow-md'>
      <ProfileImage src={userInfo.profileImage.thumbnailImagePath} dimension={96} />
      <div className='flex flex-col gap-4'>
        <h3 className='text-xl font-semibold'>{userInfo.username}</h3>
        <div>
          <p>{userInfo.biography}</p>
        </div>
      </div>
    </div>
  );
}

export default function UserProfileFriendList({ userProfileId }: UserProfileFriendListProps) {
  const [friends, setFriends] = useState<UserInfoResponse[]>([]);
  useEffect(() => {
    async function getFriends() {
      const response = await userService.getFriends(userProfileId);
      if (response.hasError) {
        console.log(response.error);
        return;
      }
      setFriends(response.value);
    }

    getFriends();
  }, []);

  return (
    <div className='flex flex-col gap-4'>
      {friends.map((friend) => (
        <UserProfileFriendItem key={friend.userProfileId} userInfo={friend} />
      ))}
    </div>
  );
}
