import React, { useEffect, useState } from 'react'
import { Card, Table, Modal } from 'antd'
import { removeMailingGroup, getMailingGroupDetail } from '../../Services/mailGroupService'
import { ExtraButtons } from '../../Components/ExtraButton'
import AddNewEmailForGroupModal from './AddNewEmailForGroupModal'
import { useParams } from 'react-router-dom'
import { createEmailInGroup } from '../../Services/emailService'

const MailingGroupDetailPage = props => {
  const { id } = useParams()

  const [emails, setEmails] = useState([])
  const [groupName, setGroupName] = useState([])
  const [selectedMailingGroups, setSelectedMailingGroups] = useState([])
  const [addNewModal, setAddNewModal] = useState(false)
  const [confirmLoading, setConfirmLoading] = useState(false)
  const [removeButtonDisabled, setRemoveButtonDisabled] = useState(true)
  const [addNewError, setAddNewError] = useState('')
  let addNewModalFormRef = {}

  const { confirm } = Modal

  useEffect(() => {
    getMailingGroupDetail(id).then(x => {
      setEmails(x.data.emails)
      setGroupName(x.data.name)
    })
  })

  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      const selectedRowsIds = selectedRows.map((x) => x.id)
      if (selectedRowsIds.length > 0) {
        setRemoveButtonDisabled(false)
      } else {
        setRemoveButtonDisabled(true)
      }
      setSelectedMailingGroups(selectedRowsIds)
    }
  }

  const columns = [
    {
      title: 'Email',
      dataIndex: 'name',
      key: 'name'
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

  const handleOkForAddNewModal = () => {
    const { form } = addNewModalFormRef.props
    form.validateFields(async (err, values) => {
      if (err) {
        return
      }

      setConfirmLoading(true)
      values.groupId = parseInt(id)
      createEmailInGroup(values).then(response => {
        setEmails(oldEmails => [...oldEmails, response.data])
        setAddNewModal(false)
        setConfirmLoading(false)
        setAddNewError('')
        form.resetFields()
      }).catch(err => {
        debugger
        setAddNewError(err.response.data)
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
      <h1>Group name: {groupName}</h1>
      <Card title="List of emails in group" extra={<ExtraButtons
        addNewCallback={() => setAddNewModal(true)}
        removeButtonCallback={onRemoveClicked}
        removeButtonDisabled={removeButtonDisabled}/>}>
        <AddNewEmailForGroupModal
          wrappedComponentRef={saveFormRef}
          title="Title"
          visible={addNewModal}
          onCreate={handleOkForAddNewModal}
          confirmLoading={confirmLoading}
          onCancel={handleCancelForAddNewModal}
          errorMessage={addNewError}
        />
        <Table
          rowSelection={rowSelection}
          dataSource={emails}
          columns={columns}
          rowKey='id'
        />
      </Card>
    </>
  )
}

export default MailingGroupDetailPage
