import { LikeReaction } from "../api/likeService";

const map = new Map<LikeReaction, string>();
map.set(LikeReaction.Like, "👍");
map.set(LikeReaction.Heart, "💖");
map.set(LikeReaction.Happy, "😀");
map.set(LikeReaction.Sad, "😢");
map.set(LikeReaction.TearsOfJoy, "😂");

export const mapLikeReactionToEmoji = (reaction: LikeReaction) => {
    return map.get(reaction);
}