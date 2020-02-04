import { endpoints } from './appconst'
import http from './httpService'

export const getAllMailingGroups = () => {
  return http
    .get(endpoints.mailingGroup.getAll)
    .then(response => {
      return Promise.resolve(response.data)
    })
    .catch(err => {
      console.log(err)
    })
}

export const createMailingGroup = (data) => {
  return http
    .post(endpoints.mailingGroup.create, data)
    .then(response => {
      return Promise.resolve(response.data)
    })
}
