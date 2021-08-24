import React, { Component } from 'react';
import { Container, CssBaseline, makeStyles } from '@material-ui/core';
import { NavMenu } from './NavMenu';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex'
  },
  content: {
    flexGrow: 1,
    height: '100vh',
    overflow: 'auto',
  },
  appBarSpacer: theme.mixins.toolbar
}))

export function Layout(props: React.PropsWithChildren<{}>) {
  const classes = useStyles();
  return (
    <div className={classes.root}>
      <CssBaseline />
      <NavMenu />
      <main className={classes.content}>
        <div className={classes.appBarSpacer} />
        <Container>
          <div>
            {props.children}
          </div>
        </Container>
      </main>

    </div>
  );
}

