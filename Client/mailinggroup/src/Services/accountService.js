import { endpoints } from './appconst'
import http from './httpService'
import jwt_decode from 'jwt-decode'
const itemKey = 'User'

export const registerUser = (userData) => {
  return http.post(endpoints.account.register, userData)
}

export const getDecodedToken = () => {
  const token = getToken()
  const decodedToken = jwt_decode(token)
  return decodedToken
}

export const loginService = (loginForm) => {
  return http.post(endpoints.account.login, loginForm)
    .then(response => {
      localStorage.setItem(itemKey, JSON.stringify(response.data))
      return Promise.resolve()
    })
    .catch(err => {
      if (err.response.status === 400) {
        return Promise.reject(new Error('Incorect username or password'))
      } else {
        return Promise.reject(new Error(`Cannot sign in right now. Status code: ${err.response.status}`))
      }
    })
}

export const getToken = () => {
  const userJson = localStorage.getItem(itemKey)
  if (!userJson) {
    return null
  }
  const user = JSON.parse(userJson)

  return user.accessToken
}

export const logout = () => {
  const user = localStorage.removeItem(itemKey)
  if (!user) {
    return null
  }
  return user.accessToken
}
