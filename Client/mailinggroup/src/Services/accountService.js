import { endpoints } from './appconst'
import http from './httpService'

export const registerUser = (userData) => {
  return http.post(endpoints.account.register, userData)
}
