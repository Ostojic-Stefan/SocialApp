import { FaGamepad } from 'react-icons/fa';

interface TopicItemProps {
  topic: string;
}

function TopicItem({ topic }: TopicItemProps) {
  return (
    <div className='hover:bg-default-200 p-3 flex gap-4 items-center'>
      <FaGamepad />
      {topic}
    </div>
  );
}

export default function TopicList() {
  const dummyTopics = ['Gaming', 'Sports', 'Business', 'Art', 'Music', 'Movies'];

  return (
    <div className='h-screen p-2 flex flex-col '>
      {dummyTopics.map((dt, idx) => (
        <TopicItem key={idx} topic={dt} />
      ))}
    </div>
  );
}
