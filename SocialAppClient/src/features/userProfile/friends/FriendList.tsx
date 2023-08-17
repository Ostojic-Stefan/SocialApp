import styles from "./FriendList.module.css";
import { useAppSelector } from "../../../store";

function FriendList() {
  const friends = useAppSelector((store) => store.user.friends);

  return (
    <div>
      <h3 className={styles.heading}>Friend List</h3>
      <div className={styles.container}>
        {friends.map((friend) => (
          <div className={styles.friendContainer} key={friend.userProfileId}>
            <img
              className={styles.imgStyle}
              src={friend.avatarUrl}
            />
            <span>{friend.username}</span>
          </div>
        ))}
      </div>
    </div>
  );
}

export default FriendList;
