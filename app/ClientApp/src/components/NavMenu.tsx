import React, { Component } from 'react';
import { AppBar, Collapse, Container, Toolbar, Typography } from '@material-ui/core';
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
      <AppBar>
        <Toolbar>
          <Container>
            <Typography  component="h1" variant="h6" color="inherit" noWrap >TransportSim</Typography>
          </Container>
        </Toolbar>
      </AppBar>
    );
  }
}
