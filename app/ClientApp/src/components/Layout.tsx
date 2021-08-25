import React from 'react';
import { NavMenu } from './NavMenu';


export function Layout(props: React.PropsWithChildren<{}>) {
  return (
    <div>
      <NavMenu />
      <main>
        <div />
        <div>
          <div>
            {props.children}
          </div>
        </div>
      </main>

    </div>
  );
}

