import React, { useState } from 'react'
import { Card, Form, Input, Icon, Button, Typography } from 'antd'
import { formShape } from 'rc-form'
import style from './LoginPage.scss'
import routes from '../../Routing/routes'
import { loginService } from '../../Services/accountService'
import PropTypes from 'prop-types'

const LoginPage = props => {
  LoginPage.propTypes = {
    form: formShape,
    history: PropTypes.instanceOf(History)
  }
  const { getFieldDecorator } = props.form

  const [loginErrorMessage, setLoginErrorMessage] = useState('testmessage')
  const [showErrorMessage, setShowErrorMessage] = useState(false)

  const ErrorMessageBlock = () => {
    const { Text } = Typography
    const block = (
      <div className='error-message-block'>
        <Text type='danger'>{loginErrorMessage}</Text>
      </div>
    )
    if (showErrorMessage) {
      return block
    }
    return null
  }

  const handleSubmit = e => {
    e.preventDefault()
    props.form.validateFieldsAndScroll((err, values) => {
      if (!err) {
        loginService(values)
          .then(() => {
            props.history.push(routes.home)
          })
          .catch(err => {
            setShowErrorMessage(true)
            setLoginErrorMessage(err.message)
          })
      }
    })
  }

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
        <h1 style={{ textAlign: 'center' }}>Mailing groups</h1>
        <Form
          hideRequiredMark
          colon={false}
          onSubmit={handleSubmit}
          layout="vertical"
        >
          <Form.Item>
            {getFieldDecorator('username', {
              rules: [{ required: true, message: 'Please input your username' }]
            })(
              <Input
                size="large"
                prefix={
                  <Icon type="user" style={{ color: 'rgba(0,0,0,.25)' }} />
                }
                placeholder="Username"
              />
            )}
          </Form.Item>
          <Form.Item>
            {getFieldDecorator('password', {
              rules: [{ required: true, message: 'Please input your password' }]
            })(
              <Input
                size="large"
                placeholder="Password"
                type="password"
                prefix={
                  <Icon type="lock" style={{ color: 'rgba(0,0,0,.25)' }} />
                }
              />
            )}
          </Form.Item>
          <ErrorMessageBlock />
          <Button
            htmlType="submit"
            className="login-form-button"
          >
            Log in
          </Button>
          Or <a href={routes.register}>register now!</a>
        </Form>
      </Card>
    </>
  )
}

export default Form.create({ name: 'Login' })(LoginPage)
