import { LikeReaction } from "../../../api/likeService";
import { UserInfo } from "../../../api/postService";
import style from "./LikeItem.module.css";

interface Props {
  like: {
    likeReaction: LikeReaction;
    id: string;
    userInformation: UserInfo;
  };
}

function LikeItem({ like }: Props) {
  return (
    <div className={style.container}>
      <div className={style.userinfo}>
        <img className={style.img} src={like.userInformation.avatarUrl} />
        <span>{like.userInformation.username}</span>
      </div>
      <div>
        <button>Add Friend</button>
      </div>
    </div>
  );
}

export default LikeItem;
