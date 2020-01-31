import React from 'react'
import { Router, Switch, Redirect } from 'react-router-dom'
import routes from './routes'

const MailingGroupRouter = ({ history, pages }) => {
  return (
    <Router history={history}>
      <Switch>
        {pages.map(
          ({ exact, path, component: Component, layout: Layout, route: DefinedRoute }, index) => (
            <DefinedRoute
              key={index}
              exact={exact}
              path={path}
              render={props => (
                <Layout history={props.history}>
                  <Component {...props} />
                </Layout>
              )}
            />
          )
        )}
        <Redirect to={routes.home} />
        {/* Or Uncomment below to use a custom 404 page */}
        {/* <Route component={NotFoundPage} /> */}
      </Switch>
    </Router>
  )
}

export default MailingGroupRouter
