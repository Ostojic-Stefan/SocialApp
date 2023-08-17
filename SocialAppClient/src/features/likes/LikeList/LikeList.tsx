import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../../store";
import { getLikesForPost } from "../likeSlice";
import LikeItem from "../LikeItem/LikeItem";
import style from "./LikeList.module.css";
import { LikeReaction } from "../../../api/likeService";
import { getEnumValues } from "../../../utils/mapEnum";
import { mapLikeReactionToEmoji } from "../../../utils/mapEmojiToReaction";

interface Props {
  postId: string;
}

const likeToEmojis = getEnumValues(LikeReaction).map((val) =>
  mapLikeReactionToEmoji(val)
);

function LikeList({ postId }: Props) {
  const dispatch = useAppDispatch();
  const likes = useAppSelector((store) =>
    store.like.likes.find((l) => l.postId === postId)
  );

  useEffect(() => {
    dispatch(getLikesForPost({ postId }));
  }, []);

  return (
    <div className={style.container}>
      <div className={style.stats}>
        <span className={style.statItem}>all</span>
        {likeToEmojis.map((val) => (
          <span className={style.statItem} key={val}>
            {val}
          </span>
        ))}
      </div>
      {likes?.likeInfo.map((like) => (
        <LikeItem key={like.id} like={like} />
      ))}
    </div>
  );
}

export default LikeList;
