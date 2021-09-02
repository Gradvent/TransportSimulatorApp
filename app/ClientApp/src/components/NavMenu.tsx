import React, { Component } from 'react';
// import { Link } from 'react-router-dom';

interface INavMenuState {
  collapsed: boolean
}

export class NavMenu extends Component<any, INavMenuState> {
  static displayName = NavMenu.name;

  constructor(props: any) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <div>
        <div>
          <div>
            <span >TransportSim</span>
          </div>
        </div>
      </div>
    );
  }
}
