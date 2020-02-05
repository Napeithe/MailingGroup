import React, { useEffect, useState } from 'react'
import { Card, Table, Modal, Button, Divider } from 'antd'
import { getAllMailingGroups, createMailingGroup, removeMailingGroup, updateMailingGroupName } from '../../Services/mailGroupService'
import AddNewMailingGroupItemModal from './AddNewModalForm'
import style from './MailingGroup.scss'
import { ExtraButtons } from '../../Components/ExtraButton'
import routes from '../../Routing/routes'
import Text from 'antd/lib/typography/Text'
import EditComponentModal from '../../Components/EditComponentModal'

const MailingGroupPage = props => {
  const [mailingGroups, setMailingGroups] = useState([])
  const [selectedMailingGroups, setSelectedMailingGroups] = useState([])
  const [addNewModal, setAddNewModal] = useState(false)
  const [confirmLoading, setConfirmLoading] = useState(false)
  const [removeButtonDisabled, setRemoveButtonDisabled] = useState(true)
  const [addNewGroupError, setAddNewGroupError] = useState('')
  const [updatedGroup, setUpdatedGroup] = useState({})
  const [updateNameVisible, setUpdateNameVisible] = useState(false)
  let addNewModalFormRef = {}

  const { confirm } = Modal

  const fetchData = async () => {
    const result = await getAllMailingGroups()
    setMailingGroups(result.data)
  }

  useEffect(() => {
    fetchData()
  }, [])

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
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      // eslint-disable-next-line react/display-name
      render: (text, record) => (<Button type='link' onClick={() => onGroupClicked(record)}>{text}</Button>),
      sorter: (a, b) => a.name.length - b.name.length,
      defaultSortOrder: 'ascend'
    },
    {
      title: 'Number of emails',
      dataIndex: 'numberOfEmails',
      key: 'numberOfEmails',
      sorter: (a, b) => a.numberOfEmails - b.numberOfEmails
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
              setUpdatedGroup(record)
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
        return removeMailingGroup(selectedMailingGroups)
          .then(async () => {
            setSelectedMailingGroups([])
            setRemoveButtonDisabled(true)
            await fetchData()
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
        return removeMailingGroup([record.id])
          .then(async () => {
            await fetchData()
          })
      },
      onCancel () {}
    })
  }

  const onGroupClicked = event => {
    props.history.push(`${routes.mailGroupDetail}/${event.id}`)
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

  const updateName = () => {
    const { form } = addNewModalFormRef.props
    form.validateFields(async (err, values) => {
      if (err) {
        return
      }

      setConfirmLoading(true)
      values.id = updatedGroup.id
      updateMailingGroupName(values).then(async _ => {
        setUpdateNameVisible(false)
        setConfirmLoading(false)
        setAddNewGroupError('')
        form.resetFields()
        await fetchData()
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
        <EditComponentModal
          wrappedComponentRef={saveFormRef}
          title="Update group"
          visible={updateNameVisible}
          onCancel={() => setUpdateNameVisible(false)}
          onCreate={updateName}
          confirmLoading={confirmLoading}
          errorMessage={addNewGroupError}
          inputName="name"
          labelName="Group name"
          defaultValue={updatedGroup.name}
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
          dataSource={mailingGroups}
          columns={columns}
          rowKey='id'
        />
      </Card>
    </>
  )
}

export default MailingGroupPage
