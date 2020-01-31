import React from 'react'
import { Row, Layout } from 'antd'

const { Header, Content } = Layout

const AuthLayout = (props) => {
  return (
    <div className="auth-container">
      <div style={{ maxWidth: 1200, margin: 'auto' }}>
        <Row
          type="flex"
          justify="center"
          align="middle"
          style={{ paddingTop: 240 }}
        >
          {props.children}
        </Row>
      </div>
    </div>
  )
}

export default AuthLayout