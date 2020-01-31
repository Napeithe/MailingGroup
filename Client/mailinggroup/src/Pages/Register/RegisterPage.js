import React, { useState } from 'react'
import { Card, Form, Input, Icon, Button } from 'antd'
import style from './RegisterPage.scss'
import { formShape } from 'rc-form'
import { registerUser } from '../../Services/accountService'
import routes from '../../Routing/routes'
import PropTypes from 'prop-types'

const RegisterPage = props => {
  RegisterPage.propTypes = {
    form: formShape,
    history: PropTypes.instanceOf(History)
  }

  const [rePasswordDirty, setRePasswordDirty] = useState(false)

  const compareToFirstPassword = (rule, value, callback) => {
    const { form } = props
    if (value && value !== form.getFieldValue('password')) {
      callback('Two passwords that you enter is inconsistent!')
    } else {
      callback()
    }
  }

  const handleSubmit = e => {
    e.preventDefault()
    props.form.validateFieldsAndScroll((err, values) => {
      if (!err) {
        registerUser(values)
          .then(() => {
            props.history.push(routes.registerSuccess)
          })
      }
    })
  }

  const handleConfirmBlur = e => {
    const { value } = e.target
    setRePasswordDirty(rePasswordDirty || !!value)
  }

  const validateToNextPassword = (rule, value, callback) => {
    const { form } = props
    if (value && rePasswordDirty) {
      form.validateFields(['repassword'], { force: true })
    }
    callback()
  }

  const { getFieldDecorator } = props.form
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
        <h1 style={{ textAlign: 'center' }}>Register</h1>
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
            {getFieldDecorator('email', {
              rules: [
                { required: true, message: 'Please input your email' },
                {
                  type: 'email',
                  message: 'The input is not valid E-mail!'
                }
              ]
            })(
              <Input
                size="large"
                type='email'
                prefix={
                  <Icon type="mail" style={{ color: 'rgba(0,0,0,.25)' }} />
                }
                placeholder="Email"
              />
            )}
          </Form.Item>
          <Form.Item>
            {getFieldDecorator('password', {
              rules: [
                { required: true, message: 'Please input your password' },
                { validator: validateToNextPassword }
              ]
            })(
              <Input.Password
                size="large"
                placeholder="Password"
                prefix={
                  <Icon type="lock" style={{ color: 'rgba(0,0,0,.25)' }} />
                }
              />
            )}
          </Form.Item>
          <Form.Item>
            {getFieldDecorator('repassword', {
              rules: [
                { required: true, message: 'Please repeat your password' },
                { validator: compareToFirstPassword }
              ]
            })(
              <Input.Password
                size="large"
                placeholder="Repeat password"
                onBlur={handleConfirmBlur}
                prefix={
                  <Icon type="lock" style={{ color: 'rgba(0,0,0,.25)' }} />
                }
              />
            )}
          </Form.Item>

          <Button htmlType="submit" className="login-form-button">
            Register
          </Button>
        </Form>
      </Card>
    </>
  )
}

export default Form.create({ name: 'Register' })(RegisterPage)
