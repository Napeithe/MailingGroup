import React from 'react'
import { Row, Layout, Dropdown, Menu, Icon, Button } from 'antd'
import { logout } from '../../Services/accountService'
import PropTypes from 'prop-types'
import routes from '../../Routing/routes'

const { Header, Content } = Layout

const AuthLayout = (props) => {
  AuthLayout.propTypes = {
    history: PropTypes.object
  }

  const onLogout = () => {
    logout()

    props.history.push(routes.login)
  }

  return (
    <Layout>
      <Header
        style={{
          zIndex: 1,
          width: '100%',
          backgroundColor: '#672A2F'
        }}
      >
        <Row
          type="flex"
          align="middle"
          justify="end"
          style={{ height: '100%' }}
        >
          <Dropdown
            overlay={
              <Menu>
                <Menu.Item key="1" onClick={onLogout}>
                  <Icon type="logout" />
                    Logout
                </Menu.Item>
              </Menu>
            }
            trigger={['click']}
          >
            <Button ghost className="primary-btn">
                User@gmail.com <Icon type="down" />
            </Button>
          </Dropdown>
        </Row>
      </Header>
      <Content style={{ padding: '0 50px', marginTop: 64, height: '100vh' }}>
        <div style={{ padding: 24, minHeight: 280 }}>    {props.children}</div> </Content>
    </Layout>
  )
}

export default AuthLayout
