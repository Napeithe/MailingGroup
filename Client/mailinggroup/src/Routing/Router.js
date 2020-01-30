import { Router, Route, Switch, Redirect } from "react-router-dom";
import LoginPage from './pages/LoginPage';

<Router>
  <Switch>
    <Route exact path="/login" component={LoginPage} />
  </Switch>
</Router>