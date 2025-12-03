import React, { Suspense } from 'react';
import EventDetailsClient from '@/components/viewEventPage/EventDetailsClient';

export default function Page() {
  return (
    <Suspense fallback={<div />}> 
      <EventDetailsClient />
    </Suspense>
  );
}