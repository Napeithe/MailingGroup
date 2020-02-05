import React from 'react'
import { createBrowserHistory } from 'history'
import PropTypes from 'prop-types'
import MailingGroupRouter from './Routing/MailingGroupRouter'
import MailingGroupPagesRouter from './Routing/PagesRouter'
import './App.css'

const App = () => {
  App.propTypes = {
    history: PropTypes.instanceOf(History)
  }
  const history = createBrowserHistory()
  return (
    <MailingGroupRouter history={history} pages={MailingGroupPagesRouter} />
  )
}

export default App
