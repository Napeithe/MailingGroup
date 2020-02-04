import React, { useEffect, useState } from 'react'
import { Card, Table, Button, Modal } from 'antd'
import { getAllMailingGroups, createMailingGroup, removeMailingGroup } from '../../Services/mailGroupService'
import AddNewMailingGroupItemModal from './AddNewModalForm'
import style from './MailingGroup.scss'
import { ExtraButtons } from '../../Components/ExtraButton'

const MailingGroupPage = props => {
  const [mailingGroups, setMailingGroups] = useState([])
  const [selectedMailingGroups, setSelectedMailingGroups] = useState([])
  const [addNewModal, setAddNewModal] = useState(false)
  const [confirmLoading, setConfirmLoading] = useState(false)
  const [removeButtonDisabled, setRemoveButtonDisabled] = useState(true)
  const [addNewGroupError, setAddNewGroupError] = useState('')
  let addNewModalFormRef = {}

  const { confirm } = Modal

  useEffect(() => {
    getAllMailingGroups().then(x => {
      setMailingGroups(x.data)
    })
  })

  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      if (selectedRowKeys.length > 0) {
        setRemoveButtonDisabled(false)
      } else {
        setRemoveButtonDisabled(true)
      }
      setSelectedMailingGroups(selectedRows.map((x) => x.id))
    },
    getCheckboxProps: record => ({
      disabled: record.name === 'Disabled User', // Column configuration not to be checked
      name: record.name
    })
  }

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name'
    },
    {
      title: 'Number of emails',
      dataIndex: 'numberOfEmails',
      key: 'numberOfEmails'
    }
  ]

  const onRemoveClicked = () => {
    confirm({
      title: 'Do you want to delete these items?',
      content: 'This operation is irreversible',
      onOk () {
        return removeMailingGroup(selectedMailingGroups)
          .then(() => {
            setSelectedMailingGroups([])
            setRemoveButtonDisabled(true)
          })
      },
      onCancel () {}
    })
  }

  const onGroupClicked = event => {
    console.log('row clice')
  }

  const handleOkForAddNewModal = () => {
    const { form } = addNewModalFormRef.props
    form.validateFields(async (err, values) => {
      if (err) {
        return
      }

      setConfirmLoading(true)

      createMailingGroup(values).then(response => {
        setMailingGroups(oldMailingGroups => [...oldMailingGroups, response.data])
        setAddNewModal(false)
        setConfirmLoading(false)
        setAddNewGroupError('')
        form.resetFields()
      }).catch(err => {
        setAddNewGroupError(err.response.data)
        setConfirmLoading(false)
      })
    })
  }

  const handleCancelForAddNewModal = () => {
    setAddNewModal(false)
  }

  const saveFormRef = formRef => {
    addNewModalFormRef = formRef
  }

  return (
    <>
      <Card title="Your mailing groups" extra={<ExtraButtons
        addNewCallback={() => setAddNewModal(true)}
        removeButtonCallback={onRemoveClicked}
        removeButtonDisabled={removeButtonDisabled}/>}>
        <AddNewMailingGroupItemModal
          wrappedComponentRef={saveFormRef}
          title="Title"
          visible={addNewModal}
          onCreate={handleOkForAddNewModal}
          confirmLoading={confirmLoading}
          onCancel={handleCancelForAddNewModal}
          errorMessage={addNewGroupError}
        />
        <Table
          rowSelection={rowSelection}
          dataSource={mailingGroups}
          columns={columns}
          rowKey='id'
          onRow={(record) => ({ onClick: () => onGroupClicked(record) })}
        />
      </Card>
    </>
  )
}

export default MailingGroupPage
