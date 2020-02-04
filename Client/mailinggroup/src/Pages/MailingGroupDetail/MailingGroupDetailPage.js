import React, { useEffect, useState } from 'react'
import { Card, Table, Modal, Divider, Button } from 'antd'
import { getMailingGroupDetail } from '../../Services/mailGroupService'
import { ExtraButtons } from '../../Components/ExtraButton'
import AddNewEmailForGroupModal from './AddNewEmailForGroupModal'
import { useParams } from 'react-router-dom'
import { createEmailInGroup, removeEmails, updateEmailInGroup } from '../../Services/emailService'
import Text from 'antd/lib/typography/Text'
import EditComponentModal from '../../Components/EditComponentModal'

const MailingGroupDetailPage = props => {
  const { id } = useParams()

  const [emails, setEmails] = useState([])
  const [groupName, setGroupName] = useState([])
  const [selectedEmails, setSelectedEmails] = useState([])
  const [addNewModal, setAddNewModal] = useState(false)
  const [updateNameVisible, setUpdateNameVisible] = useState(false)
  const [confirmLoading, setConfirmLoading] = useState(false)
  const [removeButtonDisabled, setRemoveButtonDisabled] = useState(true)
  const [addNewError, setAddNewError] = useState('')
  const [updatedEmail, setUpdatedEmail] = useState({})
  let addNewModalFormRef = {}

  const { confirm } = Modal

  const fetchData = async (id) => {
    const result = await getMailingGroupDetail(id)
    setEmails(result.data.emails)
    setGroupName(result.data.name)
  }

  useEffect(() => {
    fetchData(id)
  }, [id])

  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      const selectedRowsIds = selectedRows.map((x) => x.id)
      if (selectedRowsIds.length > 0) {
        setRemoveButtonDisabled(false)
      } else {
        setRemoveButtonDisabled(true)
      }
      setSelectedEmails(selectedRowsIds)
    }
  }

  const columns = [
    {
      title: 'Email',
      dataIndex: 'name',
      key: 'name'
    },
    {
      title: '',
      key: 'action',
      width: '15%',
      // eslint-disable-next-line react/display-name
      render: (text, record) => (
        <span>
          <Button type='link'
            onClick={_ => {
              setUpdatedEmail(record)
              setUpdateNameVisible(true)
            }}>Edit</Button>
          <Divider type="vertical" />
          <Button type='link' onClick={_ => onRemoveItemClicked(record)}><Text type='danger'>Delete</Text></Button>
        </span>
      )
    }
  ]


  const onRemoveClicked = () => {
    confirm({
      title: 'Do you want to delete these items?',
      content: 'This operation is irreversible',
      onOk () {
        return removeEmails(selectedEmails, id)
          .then(async () => {
            setSelectedEmails([])
            setRemoveButtonDisabled(true)
            await fetchData(id)
          })
      },
      onCancel () {}
    })
  }

  const onRemoveItemClicked = (record) => {
    confirm({
      title: 'Do you want to delete these item?',
      content: 'This operation is irreversible',
      onOk () {
        return removeEmails([record.id], id)
          .then(async () => {
            await fetchData(id)
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

  const updateName = () => {
    const { form } = addNewModalFormRef.props
    form.validateFields(async (err, values) => {
      if (err) {
        return
      }

      setConfirmLoading(true)
      values.groupId = parseInt(id)
      values.emailId = updatedEmail.id
      updateEmailInGroup(values).then( async _ => {
        setUpdateNameVisible(false)
        setConfirmLoading(false)
        setAddNewError('')
        form.resetFields()
        await fetchData(id)
      }).catch(err => {
        setAddNewError(err.response.data)
        setConfirmLoading(false)
      })
    })
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
        <EditComponentModal
          wrappedComponentRef={saveFormRef}
          title="Update email"
          visible={updateNameVisible}
          onCancel={() => setUpdateNameVisible(false)}
          onCreate={updateName}
          confirmLoading={confirmLoading}
          errorMessage={addNewError}
          inputName="email"
          labelName="Email"
          defaultValue={updatedEmail.name}
          rules={
            [
              {
                type: 'email',
                message: 'The input is not valid E-mail!'
              },
              { required: true, message: 'Please input email!' }]
          }
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
