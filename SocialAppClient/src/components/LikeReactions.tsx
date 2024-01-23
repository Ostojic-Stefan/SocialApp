import { LikeReaction } from '../api/dtos/like';

interface LikeReactionsProps {
  handleLikeClick: (reaction: LikeReaction) => void;
}
const map = new Map<string, LikeReaction>();
map.set('👍', LikeReaction.Like);
map.set('💖', LikeReaction.Heart);
map.set('😀', LikeReaction.Happy);
map.set('😢', LikeReaction.Sad);
map.set('😂', LikeReaction.TearsOfJoy);

function LikeReactions({ handleLikeClick }: LikeReactionsProps) {
  const emojis = ['👍', '💖', '😀', '😢', '😂'];

  function handleClick(emoji: string) {
    const reaction = map.get(emoji);
    if (reaction !== undefined) handleLikeClick(reaction);
  }

  return (
    <div
      style={{
        display: 'flex',
        gap: '10px',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      {emojis.map((emoji, idx) => (
        <div style={{ cursor: 'pointer' }} key={idx} onClick={() => handleClick(emoji)}>
          {emoji}
        </div>
      ))}
    </div>
  );
}

export default LikeReactions;
