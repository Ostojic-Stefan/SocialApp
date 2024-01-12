interface ProfileImageProps {
  src: string;
  dimension: number;
}

export default function ProfileImage({ src, dimension }: ProfileImageProps) {
  return (
    <div
      style={{ width: dimension, height: dimension }}
      className='flex justify-center items-center border-3 border-default
                 rounded-full overflow-hidden object-cover'
    >
      <img className='object-cover w-full h-full' src={src} />
    </div>
  );
}
