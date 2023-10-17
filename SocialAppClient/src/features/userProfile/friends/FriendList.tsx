import styles from "./FriendList.module.css";
import { useAppSelector } from "../../../store";

function FriendList() {
  const friends = useAppSelector((store) => store.user.friends);

  return (
    <>
      <h3 className={styles.heading}>Friend List</h3>
      <div className={styles.container}>
        {/* {friends.map((friend) => (
          <div className={styles.friendContainer} key={friend.userProfileId}>
            <img
              className={styles.imgStyle}
              src={friend.avatarUrl}
            />
            <span>{friend.username}</span>
          </div>
        ))} */}

          {Array.from(Array(20).keys()).map(x =>(<div className={styles.friendContainer} key={x}>
            <img
              className={styles.imgStyle}
              src={"http://localhost:5000/Users/cc0f847e-8fb8-441d-a773-50fda7607064.png"}
            />
            <span>{"Lmfaodude"}</span>
          </div>))}
          
      </div>
    </>
  );
}

export default FriendList;
