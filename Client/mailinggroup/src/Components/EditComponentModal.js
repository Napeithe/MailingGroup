import React from 'react'
import { Form, Modal, Input } from 'antd'
import Text from 'antd/lib/typography/Text'

const EditComponentModal = Form.create({ name: 'editComponent' })(
  // eslint-disable-next-line
    class extends React.Component {
    render () {
      const { visible, onCancel, onCreate, form, confirmLoading, errorMessage, title, inputName, labelName, rules, defaultValue } = this.props
      const { getFieldDecorator } = form
      rules.initialValue = defaultValue
      return (
        <Modal
          visible={visible}
          title={title}
          okText="Update"
          onCancel={onCancel}
          confirmLoading={confirmLoading}
          onOk={onCreate}
        >
          <Form layout="vertical">
            <Form.Item label={labelName}>
              {getFieldDecorator(inputName, rules)(<Input />)}
              <Text type='danger'>{errorMessage}</Text>
            </Form.Item>
          </Form>
        </Modal>
      )
    }
  }
)

export default EditComponentModal
