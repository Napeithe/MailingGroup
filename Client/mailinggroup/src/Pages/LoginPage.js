import React from "react";
import { Card, Form, Input, Icon, Button } from "antd";
import style from './LoginPage.scss'

const LoginPage = props => {
  
  const { getFieldDecorator } = props.form;
    return (
      <>
        <Card
         bordered={false}
         style={{
           border: "1px solid #dcdcdc",
           boxShadow: "0px 15px 20px 5px #0000001a",
           width: 400
         }}>
            <h1 style={{ textAlign: "center" }}>Mailing groups</h1>
        <Form
          hideRequiredMark
          colon={false}
          onSubmit={()=>{}}
          layout="vertical"
        >
          <Form.Item>
            {getFieldDecorator("username", {
              rules: [{ required: true, message: "Please input your username" }]
            })(<Input size="large"
            prefix={<Icon type="user" style={{ color: 'rgba(0,0,0,.25)' }} />}
            placeholder="Username" />)}
          </Form.Item>
          <Form.Item>
            {getFieldDecorator("password", {
              rules: [{ required: true, message: "Please input your password" }]
            })(<Input size="large" placeholder="Password"
            type='password'
            prefix={<Icon type="lock" style={{ color: 'rgba(0,0,0,.25)' }} />}
            />)}
          </Form.Item>
          
          <Button type="primary" htmlType="submit" className="login-form-button">
            Log in
          </Button>
          Or <a href="">register now!</a>
          </Form>
        </Card>
      </>
    );
  };
  
  export default Form.create({ name: "Login" })(LoginPage);