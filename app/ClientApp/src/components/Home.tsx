import React, { Component, useEffect, useState } from 'react';
import * as SignalR from '@microsoft/signalr';

export function Home() {
  const [connection, setConnection] = useState<SignalR.HubConnection|undefined>(undefined);
  const [time, setTime] = useState<number>(0);

  useEffect(() => {
    let newConnection = new SignalR.HubConnectionBuilder()
      .withUrl('/hubs/sim')
      // .withAutomaticReconnect()
      .build();
    setConnection(newConnection);
  }, [])

  useEffect(() => {
    if (!connection) return;
    connection.start()
      .then(()=>{
        connection.stream('Counter')
          .subscribe({
            next: (value) => {
              setTime(value);
            }, 
            complete() {},
            error() {}
          })
      })
  }, [connection])
  
  return (
    <div>
      <h1>Welcome to demonstration</h1>
      <p>SignalR server-to-client data streaming</p>
      <p>You is online at {time} seconds</p>
    </div>
  );
}

