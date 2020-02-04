import React from 'react'
import { Form, Modal, Radio, Input } from 'antd'
import Text from 'antd/lib/typography/Text'

const AddNewMailingGroupItemModal = Form.create({ name: 'form_in_modal' })(
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
            <Form.Item label="Group name">
              {getFieldDecorator('name', {
                rules: [{ required: true, message: 'Please input the group name!' }]
              })(<Input />)}
              <Text type='danger'>{errorMessage}</Text>
            </Form.Item>
          </Form>
        </Modal>
      )
    }
  }
)

export default AddNewMailingGroupItemModal
