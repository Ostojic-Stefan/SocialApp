import { Divider, Listbox, ListboxItem, Spinner } from '@nextui-org/react';
import { useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import UserInfo from './UserInfo';
import { useAppDispatch, useAppSelector } from '../store';
import { getFriendsForUser } from '../store/friends-slice';

export default function FriendList() {
  const { user } = useAuth();
  const friends = useAppSelector((store) => store.friends.currentUserFriends);
  const dispatch = useAppDispatch();
  useEffect(() => {
    dispatch(getFriendsForUser({ userId: user.userInfo.userProfileId, isCurrentUser: true }));
  }, []);

  if (!friends) return <Spinner />;

  return (
    <div style={{ height: '70vh' }} className='shadow-gray-300 rounded-md'>
      <h1 className='text-secondary text-xl text-center p-5'>Friends</h1>
      <Divider />
      <div className='flex'>
        <Listbox aria-label='Actions'>
          {friends.map((friend) => (
            <ListboxItem className='bg-default-100' key={friend.userProfileId}>
              <UserInfo userInfo={friend} />
            </ListboxItem>
          ))}
        </Listbox>
      </div>
    </div>
  );
}
