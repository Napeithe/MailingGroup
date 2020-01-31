import React from 'react'
import { Route, Redirect } from 'react-router-dom'
import { getToken } from '../Services/accountService'

import PropTypes from 'prop-types'
import routes from '../Routing/routes'

export const PrivateRoute = ({ render: Render, ...rest }) => {
  PrivateRoute.propTypes = {
    render: PropTypes.func,
    location: PropTypes.object,
    children: PropTypes.object
  }

  return <Route {...rest} render={props => {
    const currentUser = getToken()
    if (!currentUser) {
      return <Redirect to={{ pathname: routes.login, state: { from: props.location } }} />
    }
    return <Render {...props}></Render>
  }} />
}
