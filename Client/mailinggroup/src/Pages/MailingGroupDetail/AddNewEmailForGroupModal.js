import React from 'react'
import { Form, Modal, Input } from 'antd'
import Text from 'antd/lib/typography/Text'

const AddNewEmailForGroupModal = Form.create({ name: 'form_in_modal' })(
  // eslint-disable-next-line
    class extends React.Component {
    render () {
      const { visible, onCancel, onCreate, form, confirmLoading, errorMessage } = this.props
      const { getFieldDecorator } = form
      return (
        <Modal
          visible={visible}
          title="Create a new collection"
          okText="Create"
          onCancel={onCancel}
          confirmLoading={confirmLoading}
          onOk={onCreate}
        >
          <Form layout="vertical">
            <Form.Item label="Email">
              {getFieldDecorator('email', {
                rules: [
                  {
                    type: 'email',
                    message: 'The input is not valid E-mail!'
                  },
                  { required: true, message: 'Please input email!' }]
              })(<Input />)}
              <Text type='danger'>{errorMessage}</Text>
            </Form.Item>
          </Form>
        </Modal>
      )
    }
  }
)

export default AddNewEmailForGroupModal
