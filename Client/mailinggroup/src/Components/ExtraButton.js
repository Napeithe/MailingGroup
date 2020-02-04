import React from 'react'
import { Button } from 'antd'
import style from './ExtraButton.scss'

export const ExtraButtons = ({ addNewCallback, removeButtonCallback, removeButtonDisabled }) => {
  return (<>
    <Button onClick={addNewCallback}>Add new</Button>
    <Button onClick={removeButtonCallback} disabled={removeButtonDisabled} type='danger' className='removeButton'>Remove</Button></>)
}
