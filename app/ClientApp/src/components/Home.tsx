import React, { useEffect, useState } from 'react';
import * as SignalR from '@microsoft/signalr';

// function useSimHubControllerFetchHandler<T extends string>(endpoint: T) {
//   return () => {
//     fetch(`/SimulationHubController/${endpoint}`)
//   }
// }

interface ScopeUpdatedNotificationArgs {
  transports: TransportScope[]
  allFinished: boolean
  trackDistance: number
}

interface TransportScope {
  id: string
  speed: number
  wheelPunctureProbability: number
  repairTimeSeconds: number
  type: string
  name: string
  [key: string]: any
}

interface TransportState {
  startedAt: Date
  started: boolean
  finishedAt: Date
  finished: boolean
  distanceTraveled: number
  wheelPunctured: boolean
  wheelPuncturedAt: Date
}

interface Transport extends TransportScope, TransportState {
}

interface SimulationState {
  status?: string
  message?: string
  trackDistance?: number
  transports?: Transport[]
}

interface TransportScopeRowProps {
  transport: TransportScope,
  editItem?: (item: TransportScope) => void
  deleteItem?: () => void
}
interface TransportEditorProps {
  transport: TransportScope,
  saveItem?: (item: TransportScope) => void
}

export function TransportEditor({ transport }: TransportEditorProps) {
  const [item, setItem] = useState(transport);
  const save = () => fetch(`/transport/${item.id}`, {
    body: JSON.stringify(item),
    method: "PUT",
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    }
  });
  return <table>
    <thead><tr><th><td colSpan={2}>Editor</td></th></tr></thead>
    <tbody>
      <tr>
        <td>Name</td>
        <td><input onChange={(e) => setItem({ ...item, name: e.target.value })} value={item.name} /></td></tr>
      {/*<tr>
       <td>Type</td>
        <td><input onChange={(e) => setItem({ ...item, type: e.target.value })} value={item.type} /></td></tr> */}
      <tr>
        <td>Speed</td>
        <td><input type="number" onChange={(e) => setItem({ ...item, speed: Number(e.target.value) })} value={item.speed} /></td></tr>
      <tr>
        <td>Repair Time</td>
        <td><input type="number" onChange={(e) => setItem({ ...item, repairTimeSeconds: Number(e.target.value) })} value={item.repairTimeSeconds} /></td></tr>
      <tr>
        <td>Wheel Puncture Probability</td>
        <td><input type="number" onChange={(e) => setItem({ ...item, wheelPunctureProbability: Number(e.target.value) })} value={item.wheelPunctureProbability} /></td></tr>
      <tr>
        <td>
          {item.type == "Automobile" && "Automobile"}
          {item.type == "Track" && "Track"}
          {item.type == "Motorbike" && "Motorbike"}
        </td>
        <td>
          {item.type == "Automobile" && <input type="number"
            onChange={(e) => setItem({ ...item, personCount: Number(e.target.value) })}
            value={item.personCount} />}
          {item.type == "Truck" && <input type="number"
            onChange={(e) => setItem({ ...item, cargoWeight: Number(e.target.value) })}
            value={item.cargoWeight} />}
          {item.type == "Motorbike" && <input type="checkbox"
            onChange={(e) => setItem({ ...item, hasSidecar: !item.hasSidecar })}
            value={item.hasSidecar} />}
        </td>
      </tr>
      <tr>
        <td colSpan={2}>
          <button onClick={() => save()}>Save</button>
        </td>
      </tr>
    </tbody>
  </table>
}

export function TransportHeadRow({ count = 0 }) {
  return <>
    <tr><th colSpan={6} style={{ textAlign: "center" }}>Transports [{count}]</th></tr>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Speed</th>
      <th>Repair time</th>
      <th>Wheel Puncture Probability</th>
      <th>Additional</th>
      <th>Actions</th>
    </tr>
  </>
}
export function TransportScopeRow({ transport, editItem, deleteItem }: TransportScopeRowProps) {
  const [details, setDetails] = useState(false);
  const deleteHandler = () => {
    fetch(`/transport/${transport.id}`, {
      method: "DELETE",
      body: JSON.stringify(transport),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then((res) => {
      if (res.ok && !!deleteItem) deleteItem();
    })
  }
  return <>
    <tr>
      <td>{transport.name}</td>
      <td>{transport.type}</td>
      <td>{transport.speed} km/h</td>
      <td>{transport.repairTimeSeconds}s</td>
      <td>{Math.round(transport.wheelPunctureProbability * 100)}%</td>
      <td>
        {transport.type == "Automobile" && <><span>Person</span> <span>{transport.personCount}</span></>}
        {transport.type == "Truck" && <><span>Cargo Weight</span> <span>{transport.cargoWeight}</span></>}
        {transport.type == "Motorbike" && <><span>Sidecar</span> <span>{transport.hasSidecar ? "yes" : "no"}</span></>}
      </td>
      <td>
        {!!editItem && <button style={{ margin: "3px" }} onClick={() => editItem(transport)}>Edit</button>}
        {!!deleteItem && <button style={{ margin: "3px" }} onClick={deleteHandler}>Delete</button>}
        <label>Details
          <input type="checkbox" checked={details} onChange={(e) => setDetails(!details)} />
        </label>
      </td>
    </tr>
    {details && <tr><td colSpan={6}><ul>
      {Object.entries(transport).map(([k, v]) => <li>
        {k}: {String(v)}
        {/*typeof(v)=="object" && v!=null && <ul>
    {Object.entries(v).map(([k2,v2])=><li>
      {k2}: {String(v2)}
      </li>)}  
    </ul>*/}
      </li>)}
    </ul></td></tr>}
  </>
}

export function Home() {
  const [connection, setConnection] = useState<SignalR.HubConnection | undefined>(undefined);
  const [time, setTime] = useState<number>(0);
  const [simState, setSimArgs] = useState<SimulationState | undefined>(undefined);
  const [editingItem, setEditingItem] = useState<TransportScope | undefined>(undefined);
  const [distanceEditing, setDistanceEditing] = useState(false);
  const [editionDistance, setEditionDistance] = useState(simState?.trackDistance ?? 0);

  const setSimArgsFetchHandler = (state?: SimulationState) => {
    setSimArgs({
      ...state,
      transports: state?.transports?.map<Transport>((t) => ({
        ...t,
        startedAt: new Date(t.startedAt),
        finishedAt: new Date(t.finishedAt),
        wheelPuncturedAt: new Date(t.wheelPuncturedAt)
      }))
    })
  }

  useEffect(() => {
    let newConnection = new SignalR.HubConnectionBuilder()
      .withUrl('/hubs/sim')
      // .withAutomaticReconnect()
      .build();
    setConnection(newConnection);
    fetch("/transport").then((res) => {
      if (res.ok) {
        var data = res.json();
        return data
      }
    }).then((transports: Transport[]) => {
      setSimArgsFetchHandler({ ...simState, transports })
    });
  }, [])

  useEffect(() => {
    if (!connection) return;
    connection.start()
      .then(() => {
        connection.on('ScopeUpdatedNotification',
          (scopeArgs: ScopeUpdatedNotificationArgs) => {
            const newSimState = { ...simState, trackDistance: scopeArgs.trackDistance };
            const transports: Transport[] = newSimState.transports ?? [];
            var dict: { [id: string]: Transport } = {};
            transports.forEach((t) => dict[t.id] = t);
            scopeArgs.transports.forEach((newTransport) => {
              const oldTransport = dict[newTransport.id];
              dict[newTransport.id] = { ...oldTransport, ...newTransport };
            })
            newSimState.transports = Object.values(dict);
            setSimArgsFetchHandler(newSimState);
          })
        connection.stream('Counter')
          .subscribe({
            next: (value) => {
              setTime(value);
            },
            complete() { },
            error() { }
          });
        connection.stream('SimulationUpdate')
          .subscribe({
            next: (value) => {
              setSimArgsFetchHandler(value);
            },
            complete() { },
            error() { }
          });
        connection.on('StateUpdatedNotification', (stateArgs) => {
          setSimArgsFetchHandler(stateArgs);
        })
      })
  }, [connection])

  const startHandler = () => fetch('simulation/start');
  const stopHandler = () => fetch('simulation/stop');
  const getTransportCreateHandler = (type: string) => () => fetch(`/transport/${type}`, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    }
  });
  const setDistanceHandler = () => fetch(`/simulation/distance/${editionDistance}`, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    }
  });

  const finishedTransport = simState?.transports
    ?.filter((t) => t.finished)
    .sort((a, b) => a.finishedAt.valueOf() - b.finishedAt.valueOf()) ?? []
  const runningTransport = simState?.transports
    ?.filter((t) => !t.finished && t.started)
    .sort((a, b) => b.distanceTraveled - a.distanceTraveled) ?? []

  const transports = simState?.transports ?? [];

  return (
    <div>
      <h1>Welcome to demonstration</h1>
      <p>SignalR server-to-client data streaming</p>
      <p>You is online at {time} seconds</p>
      <div>
        <div><span>Simulation</span></div>
        <div><button onClick={startHandler}>Start</button></div>
        <div><button onClick={stopHandler}>Stop</button></div>
        <div>{simState?.message}</div>
        <div>Simulation status: {simState?.status}</div>
        <div>Distance: {simState?.trackDistance} <button onClick={()=>{setDistanceEditing(!distanceEditing)}}>Edit</button></div>
        {distanceEditing && <div>new Distance: 
          <input type="number" value={editionDistance} 
            onChange={(e)=>setEditionDistance(Number(e.target.value))}/>
          <button onClick={setDistanceHandler}>Save</button>  
        </div>}
        <table>
          <thead>
            <TransportHeadRow count={transports.length} />
            <tr><td colSpan={6}>
              <button onClick={getTransportCreateHandler("Automobile")}>Add automobile</button>
              <button onClick={getTransportCreateHandler("Motorbike")}>Add motorbike</button>
              <button onClick={getTransportCreateHandler("Truck")}>Add truck</button>
            </td></tr>
          </thead>
          <tbody>
            {transports.map((t) => <TransportScopeRow key={t.id} transport={t}
              editItem={() => setEditingItem(t)}
              deleteItem={() => {
                var ts = transports.filter((t2) => t2.id != t.id);
                if (editingItem?.id == t.id) setEditingItem(undefined);
                setSimArgs({ ...simState, transports: ts });
              }} />)}
          </tbody>
        </table>
        {editingItem && <TransportEditor transport={editingItem} />}
        <div style={{ margin: "10px" }}>
          {finishedTransport.length > 0 && <table>
            <thead>
              <tr><th colSpan={4} style={{ textAlign: "center" }}>Finished</th></tr>
              {runningTransport.length == 0 && <tr><td colSpan={4} style={{ textAlign: "center" }}>
                <span>The simulation is finished, would you like to restart? </span>
                <button onClick={startHandler} >Yes</button>
              </td></tr>}
              <tr>
                <th>Rang</th>
                <th>Name</th>
                <th>Finished at</th>
                <th>Type</th>
              </tr>
            </thead>
            <tbody>
              {finishedTransport.map((t, rang) => <tr key={t.id}>
                <td>#{rang + 1}</td>
                <td>{t.name} {t.wheelPunctured && "[punctured]"}</td>
                <td>{t.finishedAt.toLocaleTimeString()}</td>
                <td>{t.type}</td>
              </tr>)}
            </tbody>
          </table>}
          {runningTransport.length > 0 && <table>
            <thead>
              <tr><th style={{ textAlign: "center" }}>Running</th></tr>
              <tr>
                <th>Rang</th>
                <th>Name</th>
                <th>Distance traveled</th>
                <th>Speed</th>
              </tr>
            </thead>
            <tbody>
              {runningTransport?.map((t, rang) => <tr key={t.id}>
                <td>#{rang + 1}</td>
                <td>{t.name} <span>[{t.wheelPunctured ? "punctured" : "running"}]</span></td>
                <td>
                  <progress value={t.distanceTraveled} max={simState?.trackDistance}></progress>
                  <span>{t.distanceTraveled}/{simState?.trackDistance} km</span>
                </td>
                <td>{t.speed} km/h</td>
              </tr>)}
            </tbody>
          </table>}
        </div>
      </div>
    </div>
  );
}

