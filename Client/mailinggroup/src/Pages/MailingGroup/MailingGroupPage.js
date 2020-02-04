import React, { useEffect, useState } from 'react'
import { Card, Table, Modal } from 'antd'
import { getAllMailingGroups, createMailingGroup, removeMailingGroup } from '../../Services/mailGroupService'
import AddNewMailingGroupItemModal from './AddNewModalForm'
import style from './MailingGroup.scss'
import { ExtraButtons } from '../../Components/ExtraButton'
import routes from '../../Routing/routes'

const MailingGroupPage = props => {
  const [mailingGroups, setMailingGroups] = useState([])
  const [selectedMailingGroups, setSelectedMailingGroups] = useState([])
  const [addNewModal, setAddNewModal] = useState(false)
  const [confirmLoading, setConfirmLoading] = useState(false)
  const [removeButtonDisabled, setRemoveButtonDisabled] = useState(true)
  const [addNewGroupError, setAddNewGroupError] = useState('')
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
          .then(async () => {
            setSelectedMailingGroups([])
            setRemoveButtonDisabled(true)
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
