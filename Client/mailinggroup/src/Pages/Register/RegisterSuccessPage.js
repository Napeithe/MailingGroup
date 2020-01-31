import React from 'react'
import { Card } from 'antd'
import style from './RegisterPage.scss'
import routes from '../../Routing/routes'

const RegisterPage = props => {
  return (
    <>
      <Card
        bordered={false}
        style={{
          border: '1px solid #dcdcdc',
          boxShadow: '0px 15px 20px 5px #0000001a',
          width: 400
        }}
      >
        <h1 style={{ textAlign: 'center' }}>Thank you for signing up</h1>
        <h3 style={{ textAlign: 'center' }}>Now you can <a href={routes.login}>sign in </a></h3>

      </Card>
    </>
  )
}

export default (RegisterPage)
