import React from 'react'
import { Route, Redirect } from 'react-router-dom'
import { getToken } from '../Services/accountService'

import PropTypes from 'prop-types'
import routes from '../Routing/routes'

export const PrivateRoute = ({ component: Component, ...rest }) => {
  PrivateRoute.propTypes = {
    component: Component,
    location: PropTypes.string
  }

  return <Route {...rest} render={props => {
    const currentUser = getToken()
    debugger
    if (!currentUser) {
      return <Redirect to={{ pathname: routes.login, state: { from: props.location } }} />
    }
    return <Component {...props} />
  }} />
}
