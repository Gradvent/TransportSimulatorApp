import React, { useEffect, useState } from 'react';
import * as SignalR from '@microsoft/signalr';

// function useSimHubControllerFetchHandler<T extends string>(endpoint: T) {
//   return () => {
//     fetch(`/SimulationHubController/${endpoint}`)
//   }
// }

interface Transport {
  speed: number
  wheelPunctureProbability: number
  repairTime: any
  type: string
  name: string
  startedAt: any
  started: boolean
  finishedAt: any
  finished: boolean
  distanceTraveled: number 
  wheelPunctured: boolean
  wheelPuncturedAt: any
}

interface SimulationState {
  status?: string
  message?: string
  trackDistance?: number
  transports?: Transport[]
}

export function Home() {
  const [connection, setConnection] = useState<SignalR.HubConnection|undefined>(undefined);
  const [time, setTime] = useState<number>(0);
  const [simState, setSimArgs] = useState<SimulationState|undefined>(undefined);
  const [details, setDetails] = useState<boolean>(false);

  useEffect(() => {
    let newConnection = new SignalR.HubConnectionBuilder()
      .withUrl('/hubs/sim')
      // .withAutomaticReconnect()
      .build();
    setConnection(newConnection);
  }, [])

  const startHandler = () => fetch('simulation/start');
  const stopHandler = () => fetch('simulation/stop');

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
          });
        connection.stream('SimulationUpdate')
          .subscribe({
            next: (value) => {
              setSimArgs(value);
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
      <div>
        <div><span>Simulation</span></div>
        <div><button onClick={()=>startHandler()}>Start</button></div>
        <div><button onClick={()=>stopHandler()}>Stop</button></div>
        <div><label onClick={()=>setDetails(!details)}>Details<input type="checkbox" checked={details}/></label></div>
        <div>{simState?.message}</div>
        <div>Simulation status: {simState?.status}</div>
        <div>Distance: {simState?.trackDistance}</div>
        <div>Transports [{simState?.transports?.length}]:</div>
        <div style={{margin: "10px"}}>
          {simState?.transports?.map((t)=><div key={t.name}>
            <div>{t.name} <span>[{t.finished ? "finished" : t.wheelPunctured ? "punctured" : "running"}]</span></div>
            <div><progress value={t.distanceTraveled} max={simState.trackDistance}></progress>[{t.distanceTraveled}/{simState.trackDistance} km]</div>
            <div>
              <div>Speed: {t.speed} km/h</div>
              <div>Started at: {new Date(t.startedAt.totalMilliseconds).toLocaleTimeString()}</div>
              <div>Repair time: {t.repairTime.totalSeconds} seconds</div>
            </div>
            {details && <ul>
              {Object.entries(t).map(([k,v])=><li>
                {k}: {String(v)} 
                {/*typeof(v)=="object" && v!=null && <ul>
                  {Object.entries(v).map(([k2,v2])=><li>
                    {k2}: {String(v2)}
                  </li>)}  
                </ul>*/}
              </li>)}
            </ul>}
          </div>)}
        </div>

      </div>
    </div>
  );
}

