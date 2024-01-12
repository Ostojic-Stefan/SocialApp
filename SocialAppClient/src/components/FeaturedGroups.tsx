import { Listbox, ListboxItem } from '@nextui-org/react';

export default function FeaturedGroups() {
  const tempGroups = ['webdev', 'js-sucks', '2Meirl4Meirl', 'overall messy'];

  const rednderedGroups = tempGroups.map((group, idx) => (
    <ListboxItem className='text-left my-1' color='secondary' key={idx}>
      <p className='text-base'>&rarr; {group}</p>
    </ListboxItem>
  ));

  return (
    <div className='h-fit shadow-lg shadow-gray-300 rounded-md border-r-8 border-secondary-400'>
      <h1 className='text-secondary text-xl text-center'>Featured Groups</h1>
      <div className='flex'>
        <Listbox aria-label='Actions'>{rednderedGroups}</Listbox>
      </div>
    </div>
  );
}
