import React from 'react';
import { Router, Route, Switch } from "react-router-dom";
import { createBrowserHistory } from "history";
import LoginPage from "./Pages/LoginPage";
import routes from './Routing/routes'
import PublicLayout from './shared/layout/PublicLayout'



const App = () => {
  
  const history = createBrowserHistory();
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
      )}
    </Switch>
  </Router>
  );
}

export default App;
