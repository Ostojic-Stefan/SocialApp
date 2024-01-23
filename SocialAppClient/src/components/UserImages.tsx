import { useEffect, useState } from 'react';
import { userService } from '../api/userService';
import { ImageResponse } from '../api/dtos/image';
import { DropdownItem, DropdownMenu, DropdownTrigger, Spinner } from '@nextui-org/react';
import { Dropdown } from '@nextui-org/react';
import { FaEllipsisV } from 'react-icons/fa';

interface UserImagesProps {
  userProfileId: string;
}

function UserImages({ userProfileId }: UserImagesProps) {
  const [images, setImages] = useState<ImageResponse[]>();
  const [hoverImageId, setHoverImageId] = useState<string>();

  useEffect(() => {
    async function getUserImages() {
      const response = await userService.getUserImages({ userProfileId });
      if (response.hasError) {
        console.log(response.error);
        return;
      }
      setImages(response.value);
    }
    getUserImages();
  }, []);

  if (!images) {
    return <Spinner />;
  }

  async function handleSetProfileImage(imageId: string): Promise<void> {
    const response = await userService.setProfileImage({ imageId });
    if (response.hasError) {
      console.log(response.error);
      return;
    }
    console.log(response);
  }

  const darkenImage = {
    filter: 'brightness(0.3)',
  };

  async function handleDeleteImage(imageId: string): Promise<void> {
    console.log(`deleting image: ${imageId}`);
  }

  return (
    <div className='grid grid-cols-3 gap-4'>
      {images.map((img) => (
        <Dropdown key={img.imageId} placement='top-start'>
          <div
            onMouseEnter={() => setHoverImageId(img.imageId)}
            onMouseLeave={() => setHoverImageId('')}
            className='relative hover:cursor-pointer hover:border-secondary-300 w-full rounded-md border-2 border-default-500 flex items-center justify-center'
          >
            <DropdownTrigger>
              {hoverImageId === img.imageId ? (
                <div className='h-fit p-3 rounded-full bg-default-50 absolute bottom-10 right-5 top-5'>
                  <FaEllipsisV />
                </div>
              ) : (
                <></>
              )}
            </DropdownTrigger>
            <img
              style={hoverImageId === img.imageId ? darkenImage : {}}
              src={img.fullscreenImagePath}
              alt='Image 1'
              className='w-full h-auto'
            />
          </div>
          <DropdownMenu>
            <DropdownItem onClick={() => handleSetProfileImage(img.imageId)} color='secondary'>
              Set As Profile Image
            </DropdownItem>
            <DropdownItem onClick={() => handleDeleteImage(img.imageId)} className='text-danger' color='danger'>
              Delete An Image
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>
      ))}
    </div>
  );
}

export default UserImages;
