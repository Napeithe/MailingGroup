import React from 'react'
import { Router, Route, Switch, Redirect } from 'react-router-dom'
import { createBrowserHistory } from 'history'
import LoginPage from './Pages/Login/LoginPage'
import RegisterPage from './Pages/Register/RegisterPage'
import MailingGroupPage from './Pages/MailingGroup/MailingGroupPage'
import routes from './Routing/routes'
import PublicLayout from './shared/layout/PublicLayout'
import RegisterSuccessPage from './Pages/Register/RegisterSuccessPage'
import PropTypes from 'prop-types'
import AuthLayout from './shared/layout/AuthLayout'
import { PrivateRoute } from './Components/PrivateRoute'

const App = () => {
  App.propTypes = {
    history: PropTypes.instanceOf(History)
  }
  const history = createBrowserHistory()
  return (
    <Router history={history}>
      <Switch>
        <Route
          key={1}
          exact={true}
          path={routes.login}
          render={props => (
            <PublicLayout history={props.history}>
              <LoginPage {...props} />
            </PublicLayout>
          )}
        />
        <Route
          key={2}
          exact={true}
          path={routes.register}
          render={props => (
            <PublicLayout history={props.history}>
              <RegisterPage {...props} />
            </PublicLayout>
          )}
        />
        <Route
          key={3}
          exact={true}
          path={routes.registerSuccess}
          render={props => (
            <PublicLayout history={props.history}>
              <RegisterSuccessPage {...props} />
            </PublicLayout>
          )}
        />
        <PrivateRoute
          key={4}
          exact={false}
          path={routes.home}
          render={props => (
            <AuthLayout history={props.history}>
              <MailingGroupPage {...props} />
            </AuthLayout>
          )}
        />
        <Redirect to={routes.home} />
      </Switch>
    </Router>
  )
}

export default App
