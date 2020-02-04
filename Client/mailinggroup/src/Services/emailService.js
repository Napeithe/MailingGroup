import http from './httpService'
import { endpoints } from './appconst'

export const createEmailInGroup = (data) => {
  return http
    .post(endpoints.emails.create, data)
    .then(response => {
      return Promise.resolve(response.data)
    })
}
