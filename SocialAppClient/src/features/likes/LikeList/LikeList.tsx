import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../../store";
import { getLikesForPost } from "../likeSlice";

interface Props {
  postId: string;
}

function LikeList({ postId }: Props) {
  const dispatch = useAppDispatch();
  const likes = useAppSelector((store) =>
    store.like.likes.find((l) => l.postId === postId)
  );

  useEffect(() => {
    dispatch(getLikesForPost({ postId }));
  }, []);

  return (
    <div style={{ color: "#fff" }}>
      {likes?.likeInfo.map((like) => (
        <p key={like.id}>{like.likeReaction}</p>
      ))}
    </div>
  );
}

export default LikeList;
