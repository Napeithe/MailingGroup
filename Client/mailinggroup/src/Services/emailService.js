import http from './httpService'
import { endpoints } from './appconst'

export const createEmailInGroup = (data) => {
  return http
    .post(endpoints.emails.create, data)
    .then(response => {
      return Promise.resolve(response.data)
    })
}

export const removeEmails = (emails, groupId) => {
  const emailId = `${emails.join('&emailId=')}`
  return http
    .delete(`${endpoints.emails.delete}?groupId=${groupId}&emailId=${emailId}`)
    .then(response => {
      return Promise.resolve(response.data)
    })
}
