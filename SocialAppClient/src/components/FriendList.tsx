// import { Listbox, ListboxItem, Spinner } from '@nextui-org/react';
// import { useEffect, useState } from 'react';
// import { FriendResponse, userService } from '../api/userService';
// import { useAuth } from '../context/AuthContext';
// import UserInfo from './UserInfo';

// export default function FriendList() {
//   const { user } = useAuth();
//   const [friends, setFriends] = useState<FriendResponse[]>();

//   useEffect(() => {
//     async function getFirends() {
//       if (!user) return;
//       const result = await userService.getFriends(user?.userInformation.userId);
//       if (result.hasError) {
//         console.log(result.error);
//         return;
//       }
//       setFriends(result.value);
//     }

//     getFirends();
//   }, [user]);

//   if (!user || !friends) return <Spinner />;

//   return (
//     <div style={{ height: '70vh' }} className='shadow-lg shadow-gray-300 rounded-md'>
//       <h1 className='text-secondary text-xl text-center p-5 bg-default-100'>Friends</h1>
//       <div className='flex'>
//         <Listbox aria-label='Actions'>
//           {friends!.map((friend) => (
//             <ListboxItem key={friend.userProfileId}>
//               <UserInfo
//                 userInfo={{
//                   userProfileId: friend.userProfileId,
//                   username: friend.username,
//                   avatarUrl: friend.avatarUrl,
//                 }}
//               />
//             </ListboxItem>
//           ))}
//         </Listbox>
//       </div>
//     </div>
//   );
// }
