import React from 'react'
import { Card, Table } from 'antd'

const MailingGroupPage = props => {
  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows)
    },
    getCheckboxProps: record => ({
      disabled: record.name === 'Disabled User', // Column configuration not to be checked
      name: record.name
    })
  }

  const dataSource = [
    {
      key: '1',
      name: 'Mike',
      emailCount: 32
    },
    {
      key: '2',
      name: 'John',
      emailCount: 42
    }
  ]

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name'
    },
    {
      title: 'Number of emails',
      dataIndex: 'emailCount',
      key: 'emailCount'
    }
  ]
  const onGroupClicked = event => {
    console.log('row clice')
  }
  return (<>
    <Card title='Your mailing groups'>
      <Table rowSelection={rowSelection} dataSource={dataSource} columns={columns} onRowClick={onGroupClicked} />
    </Card>
  </>)
}

export default MailingGroupPage
