import routes from './routes'
import LoginPage from '../Pages/Login/LoginPage'
import PublicLayout from '../shared/layout/PublicLayout'
import MailingGroupPage from '../Pages/MailingGroup/MailingGroupPage'
import AuthLayout from '../shared/layout/AuthLayout'
import RegisterPage from '../Pages/Register/RegisterPage'
import RegisterSuccessPage from '../Pages/Register/RegisterSuccessPage'
import { Route } from 'react-router-dom'
import { PrivateRoute } from '../Components/PrivateRoute'

const mailingGroupPagesRouter = [
  {
    exact: true,
    path: routes.login,
    component: LoginPage,
    layout: PublicLayout,
    route: Route
  },
  {
    exact: true,
    path: routes.register,
    component: RegisterPage,
    layout: PublicLayout,
    route: Route
  },
  {
    exact: true,
    path: routes.registerSuccess,
    component: RegisterSuccessPage,
    layout: PublicLayout,
    route: Route
  },
  {
    exact: false,
    path: routes.dashboard,
    component: MailingGroupPage,
    layout: AuthLayout,
    route: PrivateRoute
  }
]

export default mailingGroupPagesRouter
